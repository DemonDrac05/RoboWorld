using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Cysharp.Threading.Tasks;

public class TeleportPortal : MonoBehaviour
{
    [Header("=== Portal Properties ==========")]
    public string portalName;

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
    public GameObject teleportCanvas;

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
    }

    private IEnumerator DissolveProcess(bool dissolve)
    {
        _player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        yield return _dissolveController.DissolveProcess(dissolve);

        _player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider == Player.player.Collider)
        {
            _playerInteract.isTriggerPortal = true;
            _playerInteract.SetCurrentPortal(this);

            SetMaterialEmission(EmissionState.Activate);

            if (!_checkpoint.checkpoints.Contains(this.transform.position))
            {
                _checkpoint.checkpoints.Add(this.transform.position);
                StartCoroutine(UnlockPortalProcess());
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
