using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("=== GUI Properties =========")]
    [SerializeField] private Color enterIconColor;
    [SerializeField] private Color enterFrameColor;
    [SerializeField] private Color clickIconColor;
    [SerializeField] private Color clickFrameColor;

    [Header("=== Animation Properties ==========")]
    [SerializeField] private float duration;
    [SerializeField] private float distanceToOutScreen;

    // === Original Colors ==========
    private Color original_iconColor;
    private Color original_frameColor;

    // === GUI Components ==========
    private Image iconObject;
    private Image textObject;
    private Image icon;
    private TextMeshProUGUI text;

    // === Constant name variables =========
    private const string Menu_BackgroundIconName = "[BackgroundIcon]";
    private const string Menu_BackgroundTextName = "[BackgroundText]";
    private const string Menu_ButtonName = "[Button]";
    private const string Menu_IconName = "[Icon]";
    private const string Menu_TextName = "[TMP]";

    private const string Menu_Options = Menu_ButtonName + " Options";
    private const string Menu_Resume = Menu_ButtonName + " Resume";
    private const string Menu_Exit = Menu_ButtonName + " Exit";

    private void Awake()
    {
        iconObject = transform.Find(Menu_BackgroundIconName).gameObject.GetComponent<Image>();
        textObject = transform.Find(Menu_BackgroundTextName).gameObject.GetComponent<Image>();

        icon = iconObject.transform.Find(Menu_IconName).gameObject.GetComponent<Image>();
        text = textObject.transform.Find(Menu_TextName).gameObject.GetComponent<TextMeshProUGUI>();

        original_iconColor = iconObject.color;
        original_frameColor = textObject.color;
    }

    private Vector2 iconObjectOffset = Vector2.zero;
    private Vector2 textObjectOffset = Vector2.zero;

    private void OnEnable()
    {
        iconObjectOffset = new Vector2(-200f, iconObject.transform.localPosition.y);
        textObjectOffset = new Vector2(+150f, textObject.transform.localPosition.y);

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