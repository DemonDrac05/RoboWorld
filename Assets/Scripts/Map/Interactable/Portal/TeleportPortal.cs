using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Cysharp.Threading.Tasks;

public class TeleportPortal : MonoBehaviour
{
    [Header("=== Portal Properties ==========")]
    // --- SIGNAL OBJECT ----------
    [SerializeField] private GameObject portalSignalsGroup;
    [SerializeField] private GameObject portalSignalPrefab;
    private GameObject _currentPortalSignal = null;

    [HideInInspector] public bool IsResumeButtonPressed = false;

    private float _heightOfSignal = 2f;
    private const string PortalSignalLabel = "[Portal Signal]";

    // --- VARIABLES ----------
    public string portalName;
    public bool isOnPortalBase;

    private bool _portalActivate = true;
    private float _distanceOperating = 5f;
    private Vector3 _relativePlayerPosition;
    private Vector3 _offsetPortalPosition;

    [Header("=== Components' Meshes ==========")]
    [SerializeField] private MeshRenderer _upperSkinnedMesh;
    [SerializeField] private MeshRenderer _lowerSkinnedMesh;

    [Header("=== Player Skin Meshes ==========")]
    [SerializeField] private SkinnedMeshRenderer _skinnedSurfaceMesh;
    [SerializeField] private SkinnedMeshRenderer _skinnedJointMesh;

    [Header("=== VFX Effect ==========")]
    [SerializeField] private GameObject _splashEffect;
    [SerializeField] private GameObject _circleEffect;
    [SerializeField] private VisualEffect _vfxgraphDissolve;

    [Header("=== Portal Menu ==========")]
    public GameObject portalCanvas;

    [Header("=== Player Teleport ==========")]
    [SerializeField] private CheckpointSO _checkpoint;

    // --- MATERIALS ----------
    private Material[] _upperSkinnedMaterials;
    private Material[] _lowerSkinnedMaterials;

    // --- CLASSES' DELEGATION ----------
    private DissolveController _dissolveController;
    private PlayerInteract _playerInteract;
    private PlayerMovement _playerMovement;
    private Player _player;

    // --- EMISSION SETTINGS ----------
    private enum EmissionState { Activate, Deactivate }
    private Dictionary<EmissionState, Vector2> _emissionSettings = new()
    {
        { EmissionState.Activate, new Vector2(5f, 5f) },
        { EmissionState.Deactivate, new Vector2(1f, 5f) },
    };

    private void OnEnable()
    {
        InitializeClasses();
        InitializeMaterials();
        InitializeEmission();
    }

    private void InitializeMaterials()
    {
        _upperSkinnedMaterials = _upperSkinnedMesh.materials;
        _lowerSkinnedMaterials = _lowerSkinnedMesh.materials;

        _dissolveController._skinnedSurfaceMaterials = _skinnedSurfaceMesh.materials;
        _dissolveController._skinnedJointMaterials = _skinnedJointMesh.materials;
    }

    private void InitializeEmission()
    {
        if (_checkpoint.checkpoints.Contains(transform.position))
        {
            SetMaterialEmission(EmissionState.Activate);
        }
    }

    private void InitializeClasses()
    {
        _player = Player.player;
        _dissolveController = new DissolveController();
        _playerInteract = _player.GetComponent<PlayerInteract>();
        _playerMovement = _player.GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _dissolveController.VFXGraph = _vfxgraphDissolve;
        _offsetPortalPosition = new(transform.position.x, _heightOfSignal, transform.position.z);

        PortalActiveStatus();
        StartRespawnProcess();
    }

    private void StartRespawnProcess()
    {
        if (_player.transform.position.x == transform.position.x 
            && _player.transform.position.z == transform.position.z)
        {
            StartCoroutine(TeleportProcess());
        }
    }

    private IEnumerator TeleportProcess()
    {
        Player.player.SetMobility(false);

        GameObject circle = Instantiate(_circleEffect, _player.transform.position, _circleEffect.transform.rotation);

        yield return DissolveProcess(false);

        GameObject splash = Instantiate(_splashEffect, this.transform.position, _splashEffect.transform.rotation);

        yield return new WaitUntil(() => splash.GetComponent<ParticleSystem>().isStopped);

        Destroy(splash); 
        Destroy(circle);

        yield return PortalClosing();
    }

    private IEnumerator DissolveProcess(bool dissolve)
    {
        _player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        yield return _dissolveController.DissolveProcess(dissolve);

        _player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (IsResumeButtonPressed)
        {
            StartCoroutine(PortalClosing());
            IsResumeButtonPressed = false;
        }

        if (!_portalActivate)
        {
            CreatePortalSignal();
        }
        else
        {
            DestroyPortalSignal();
        }
    }

    private void PortalActiveStatus()
    {
        PlayerData playerData = SaveSystem.LoadGame();
        Vector3 XZPosition = new(playerData.checkpoint[0], _heightOfSignal, playerData.checkpoint[2]);
        if (XZPosition != _offsetPortalPosition)
        {
            _portalActivate = false;

            Vector3 currentPos = transform.position;
            transform.position = new(currentPos.x, currentPos.y - _distanceOperating, currentPos.z);
        }
        else
        {
            _portalActivate = true;
        }
    }

    private void CreatePortalSignal()
    {
        if (_currentPortalSignal == null)
        {
            _currentPortalSignal = Instantiate(portalSignalPrefab, portalSignalsGroup.transform);
            _currentPortalSignal.transform.position = _offsetPortalPosition;
            _currentPortalSignal.name = PortalSignalLabel + name;

            var portalSignalCollider = _currentPortalSignal.GetComponentInChildren<CheckPlayerOnPortalSignal>();
            portalSignalCollider.SetPortal(this);
        }
    }

    private void DestroyPortalSignal()
    {
        if (_currentPortalSignal != null)
        {
            if (portalSignalsGroup.transform.Find(_currentPortalSignal.name).gameObject != null)
            {
                GameObject newObj = _currentPortalSignal;
                Destroy(newObj);
            }
            _currentPortalSignal = null;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider == Player.player.Collider)
        {
            _playerInteract.isTriggerPortal = true;
            _playerInteract.SetCurrentPortal(this);

            if (isOnPortalBase)
            {
                _relativePlayerPosition = _player.transform.position - transform.position;
            }

            SetMaterialEmission(EmissionState.Activate);

            if (!_checkpoint.checkpoints.Contains(this.transform.position))
            {
                _checkpoint.checkpoints.Add(this.transform.position);
                //StartCoroutine(UnlockPortalProcess());
            }
        }
    }

    private void SetPlayerPosInCenter(Vector3 teleportPos, float posY) 
        => _player.transform.position = new(teleportPos.x, posY, teleportPos.z);

    private IEnumerator UnlockPortalProcess()
    {
        _playerMovement.SetMobility(false);

        SetPlayerPosInCenter(transform.position, -10f);

        GameObject splash = Instantiate(_splashEffect, transform.position, _splashEffect.transform.rotation);

        float duration = splash.GetComponent<ParticleSystem>().main.duration;

        yield return new WaitForSeconds(duration);

        Destroy(splash);

        _playerMovement.SetMobility(true);
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider == Player.player.Collider)
        {
            _playerInteract.isTriggerPortal = false;
            _playerInteract.SetCurrentPortal(null);
        }
    }

    public IEnumerator PortalOpening()
    {
        _playerMovement.SetMobility(false);
        SetPlayerPosInCenter(transform.position, 0f);

        float elapsedTimer = 0f;
        float animationDuration = 5f;

        Vector3 offsetPos = transform.position;
        Vector3 targetPos = new Vector3(offsetPos.x, offsetPos.y + _distanceOperating, offsetPos.z);

        while (elapsedTimer < animationDuration)
        {
            float t = elapsedTimer / animationDuration;

            transform.position = Vector3.Lerp(offsetPos, targetPos, t);
            if (_playerInteract.isTriggerPortal)
            {
                _player.transform.position = transform.position + _relativePlayerPosition;
            }

            elapsedTimer += Time.deltaTime;

            yield return null;
        }

        _portalActivate = true;
        transform.position = targetPos;
    }

    public IEnumerator PortalClosing()
    {
        float elapsedTimer = 0f;
        float animationDuration = 5f;

        Vector3 offsetPos = transform.position;
        Vector3 targetPos = new Vector3(offsetPos.x, offsetPos.y - _distanceOperating, offsetPos.z);

        while (elapsedTimer < animationDuration)
        {
            float t = elapsedTimer / animationDuration;

            transform.position = Vector3.Lerp(offsetPos, targetPos, t);
            if (!_player.IsGrounded)
            {
                _player.transform.position = transform.position + _relativePlayerPosition;
            }
            else
            {
                if (!_playerMovement.canMove)
                {
                    _playerMovement.SetMobility(true);
                }
            }

            elapsedTimer += Time.deltaTime;
            yield return null;
        }

        _portalActivate = false;
        transform.position = targetPos;
    }

    private void SetMaterialEmission(EmissionState emissionState)
    {
        Vector2 intensity = _emissionSettings[emissionState];
        var targetMaterials = new[] { _upperSkinnedMaterials, _lowerSkinnedMaterials };
        foreach (var materials in targetMaterials)
        {
            if (materials == null) continue;
            foreach (var mat in materials)
            {
                if (mat.HasProperty("_ActivateColor"))
                    mat.SetVector("_ActivateColor", intensity);
            }
        }
    }
}
