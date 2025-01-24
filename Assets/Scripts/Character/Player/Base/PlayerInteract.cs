using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [Header("=== Interact Board Properties ===========")]
    [SerializeField] private GameObject interactCanvas;

    // --- TRIGGER VARIABLES ----------
    [HideInInspector] public bool isTriggerPortal = false;
    private bool _isActivating = false;

    // --- IMAGES ----------
    private Image interact_BackgroundImage;
    private Image interact_FrameImage;

    // --- CONSTANT COMPONENTS NAME ----------
    private const string InteractBackground = "[Image] Background";
    private const string InteractFrame = "[Image] Frame";
    private const string InteractText = "[TMP] Text";

    // --- SINGLETON CLASS ----------
    private GameInputActions controls;
    private TeleportPortal currentPortal;

    private void Awake()
    {
        interact_BackgroundImage = interactCanvas?.transform.Find(InteractBackground).gameObject.GetComponent<Image>();
        interact_FrameImage = interactCanvas?.transform.Find(InteractFrame).gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        controls = new GameInputActions();
        controls.Gameplay.Enable();
        controls.Gameplay.Interact.performed += OnInteract;
    }

    private async void OnInteract(InputAction.CallbackContext context)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTriggerPortal && !_isActivating)
            {
                _isActivating = true;
                await ActivateProcess();
                _isActivating = false;
            }
        }
    }

    private void Update()
    {
        if (isTriggerPortal)
        {
            interactCanvas?.SetActive(!currentPortal.portalCanvas.activeSelf);
        }
        else
        {
            interactCanvas?.SetActive(false);
        }
    }

    public void SetCurrentPortal(TeleportPortal portal) => currentPortal = portal;

    private async UniTask ActivateProcess()
    {
        GUIManager.instance.SetImageAlphaColor(interact_BackgroundImage, 255f);
        GUIManager.instance.SetImageAlphaColor(interact_FrameImage, 255f);

        await UniTask.Delay(250);


        GUIManager.instance.SetImageAlphaColor(interact_BackgroundImage, 25f);
        GUIManager.instance.SetImageAlphaColor(interact_FrameImage, 25f);

        interactCanvas.SetActive(false);

        await currentPortal.PortalOpening();

        if (currentPortal != null) currentPortal.portalCanvas.SetActive(true);
    }
}