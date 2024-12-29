using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // --- COMPONENTS OFFSET ----------
    protected Vector2 iconObjectOffset = Vector2.zero;
    protected Vector2 textObjectOffset = Vector2.zero;

    [Header("=== COMPONENTS OFFSET X ==========")]
    [SerializeField] protected float offset_IconPosX;
    [SerializeField] protected float offset_TextPosX;

    [Header("=== GUI Properties =========")]
    // --- CHILD CANVAS ----------
    [SerializeField] private GameObject _loadScreenScrollView;
    [SerializeField] private GameObject _mainMenuCanvas;
    // --- ADJUSTED COLORS ----------
    [SerializeField] protected Color32 enterIconColor = new Color32(0, 222, 255, 255);
    [SerializeField] protected Color32 enterFrameColor = new Color32(0, 63, 116, 125);
    [SerializeField] protected Color32 clickIconColor = new Color32(0, 125, 125, 255);
    [SerializeField] protected Color32 clickFrameColor = new Color32(0, 63, 63, 125);

    [Header("=== Animation Properties ==========")]
    [SerializeField] protected float duration;
    [SerializeField] protected float distanceToOutScreen;

    // --- ORIGINAL COLOR -----------
    protected Color original_iconColor;
    protected Color original_frameColor;

    // --- GUI CHILDREN COMPONENTS ---------- 
    protected Image iconObject;
    protected Image textObject;
    protected Image icon;
    protected TextMeshProUGUI text;

    // --- CONSTANT COMPONENTS NAME ----------
    protected const string Menu_BackgroundIconName = "[BackgroundIcon]";
    protected const string Menu_BackgroundTextName = "[BackgroundText]";
    protected const string Menu_ButtonName = "[Button]";
    protected const string Menu_IconName = "[Icon]";
    protected const string Menu_TextName = "[TMP]";


    protected const string Menu_NewGame = Menu_ButtonName + " NewGame";
    protected const string Menu_LoadGame = Menu_ButtonName + " LoadGame";
    protected const string Menu_Settings = Menu_ButtonName + " Settings";
    protected const string Menu_ExitGame = Menu_ButtonName + " ExitGame";

    private void Awake()
    {
        iconObject = transform.Find(Menu_BackgroundIconName).gameObject.GetComponent<Image>();
        textObject = transform.Find(Menu_BackgroundTextName).gameObject.GetComponent<Image>();

        icon = iconObject.transform.Find(Menu_IconName).gameObject.GetComponent<Image>();
        text = textObject.transform.Find(Menu_TextName).gameObject.GetComponent<TextMeshProUGUI>();

        original_iconColor = iconObject.color;
        original_frameColor = textObject.color;
    }

    public virtual void OnEnable()
    {
        iconObjectOffset = new Vector2(offset_IconPosX, iconObject.transform.localPosition.y);
        textObjectOffset = new Vector2(offset_TextPosX, textObject.transform.localPosition.y);

        iconObject.GetComponent<RectTransform>().anchoredPosition = new(-distanceToOutScreen, iconObjectOffset.y);
        textObject.GetComponent<RectTransform>().anchoredPosition = new(+distanceToOutScreen, textObjectOffset.y);

        AnimateIn(iconObject.GetComponent<RectTransform>(), iconObjectOffset);
        AnimateIn(textObject.GetComponent<RectTransform>(), textObjectOffset);
    }

    private void OnDisable()
        => PropertiesColorChange(original_iconColor, original_frameColor);

    public void OnPointerEnter(PointerEventData eventData)
        => PropertiesColorChange(enterIconColor, enterFrameColor);

    public void OnPointerExit(PointerEventData eventData)
        => PropertiesColorChange(original_iconColor, original_frameColor);

    public void OnPointerClick(PointerEventData eventData)
        => StartCoroutine(ProcessPauseMenuButtons());

    public IEnumerator ProcessPauseMenuButtons()
    {
        PropertiesColorChange(clickFrameColor, clickFrameColor);
        yield return new WaitForSecondsRealtime(0.1f);
        PropertiesColorChange(enterIconColor, enterFrameColor);

        AnimateOut(iconObject.GetComponent<RectTransform>(), new(-distanceToOutScreen, iconObjectOffset.y));
        AnimateOut(textObject.GetComponent<RectTransform>(), new(+distanceToOutScreen, textObjectOffset.y));

        yield return new WaitForSecondsRealtime(duration);

        FunctionButtons();
    }

    public async virtual void FunctionButtons()
    {
        switch (this.gameObject.name)
        {
            case Menu_NewGame:
                SaveSystem.NewGame();
                await SaveSystem.LoadGameProcess();
                SceneManager.LoadScene("TerrainScene");
                break;
            case Menu_LoadGame:
                _mainMenuCanvas.SetActive(false);
                _loadScreenScrollView.SetActive(true);
                break;
            case Menu_Settings:
                break;
            case Menu_ExitGame:
                Application.Quit();
                break;
        }
    }

    private void PropertiesColorChange(Color color1, Color color2)
    {
        iconObject.color = icon.color = text.color = color1;
        textObject.color = color2;
    }

    private void AnimateIn(RectTransform rect, Vector2 targetPosition)
        => rect.DOAnchorPos(targetPosition, duration).SetEase(Ease.OutBack).SetUpdate(true);

    private void AnimateOut(RectTransform rect, Vector2 targetPosition)
        => rect.DOAnchorPos(targetPosition, duration).SetEase(Ease.InBack).SetUpdate(true);
}