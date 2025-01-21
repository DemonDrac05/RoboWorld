using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadBlock : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public SaveBlockData saveBlockData;
    private SaveBlockDataSO saveBlockDataSO;

    private GameObject _confirmationCanvas;

    private bool _isDeleteButtonClicked = false;

    // --- UI PROPERTIES ----------
    private Button _confirmButton;
    private Button _deleteButton;

    private Image _backkgroundImage;
    private TextMeshProUGUI _progressText;
    private TextMeshProUGUI _levelText;

    // --- CONSTANT LABEL ----------
    private const string ConfirmationCanvasName = "[Canvas] RequestScreen";
    private const string ConfirmButtonName = "[Button] Confirm";

    private const string BackgroundImage = "[Image] LevelBackground";
    private const string ProgressText = "[TMP] Progress";
    private const string LevelText = "[TMP] LevelText";

    private void OnEnable()
    {
        saveBlockDataSO = GetComponentInParent<LoadScreenManager>().saveBlockDataSO;

        var canvas = GetComponentInParent<Canvas>();
        _confirmationCanvas = canvas.transform.Find(ConfirmationCanvasName).gameObject;
        _confirmButton = _confirmationCanvas.transform.Find(ConfirmButtonName).GetComponent<Button>();
        _deleteButton = GetComponentInChildren<Button>();

        _confirmButton?.onClick.AddListener(OnConfirmDeleteButtonClicked);
        _deleteButton?.onClick.AddListener(OnDeleteButtonClicked);

        //_backkgroundImage = canvas.transform.Find(BackgroundImage).GetComponent<Image>();
        //_progressText = canvas.transform.Find(ProgressText).GetComponent<TextMeshProUGUI>();
        //_levelText = canvas.transform.Find(LevelText).GetComponent<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        _confirmButton?.onClick.RemoveListener(OnConfirmDeleteButtonClicked);
        _deleteButton?.onClick.RemoveListener(OnDeleteButtonClicked);
    }

    public void OnDeleteButtonClicked()
    {
        _confirmationCanvas?.SetActive(true);
        _isDeleteButtonClicked = true;
    }
    public void OnConfirmDeleteButtonClicked()
    {
        if (_isDeleteButtonClicked)
        {
            int index = saveBlockDataSO.saveBlockDatas.FindIndex(x => x.savePath == this.saveBlockData.savePath);

            SaveSystem.DeleteSaveFile(saveBlockDataSO.saveBlockDatas[index].savePath);
            saveBlockDataSO.saveBlockDatas.RemoveAt(index);
            Destroy(this.gameObject);

            GetComponentInParent<LoadScreenManager>().ReloadDataBlockUI();
        }
    }

    public async void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SaveSystem.savePath = saveBlockData?.savePath;
            await SaveSystem.LoadGameSuccess();
        }
    }
}
