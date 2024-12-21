using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // --- COMPONENTS OFFSET ----------
    private Vector2 iconObjectOffset = Vector2.zero;
    private Vector2 textObjectOffset = Vector2.zero;

    [Header("=== COMPONENTS OFFSET X ==========")]
    [SerializeField] protected float offset_IconPosX;
    [SerializeField] protected float offset_TextPosX;

    [Header("=== GUI Properties =========")]
    [SerializeField] protected Color enterIconColor = new Color(0, 222, 255, 255);
    [SerializeField] protected Color enterFrameColor = new Color(0, 63, 116, 125);
    [SerializeField] protected Color clickIconColor = new Color(0, 125, 125, 255);
    [SerializeField] protected Color clickFrameColor = new Color(0, 63,  63, 125);

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

    protected const string Menu_Options = Menu_ButtonName + " Options";
    protected const string Menu_Resume = Menu_ButtonName + " Resume";
    protected const string Menu_Exit = Menu_ButtonName + " Exit";

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

    public virtual void FunctionButtons()
    {
        switch (this.gameObject.name)
        {
            case Menu_Resume:
                GamePauseManager.instance.ResumeGame();
                GamePauseManager.instance.pauseMenuCanvas.SetActive(false);
                break;
                //case "Options":
                //    GamePauseManager.instance.LoadScene(true, "");
                //    break;
                //case "Exit":
                //    GamePauseManager.instance.LoadScene(true, "");
                //    break;
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