using System.Linq;
using UnityEngine;

public class LoadScreenManager : MonoBehaviour
{
    [SerializeField] private Transform _contentTransform;
    [SerializeField] private GameObject _saveBlockObject;


    public SaveBlockDataSO saveBlockDataSO;

    private SaveBlockData currentBlockCreated;
    private const float _distanceBetweenBlocks = 175f;

    public static LoadScreenManager instance {  get; private set; }

    private void Awake() => instance = this;

    private void OnEnable()
    {
        if (saveBlockDataSO != null)
        {
            foreach (Transform child in _contentTransform)
            {
                Destroy(child);
            }
            if (saveBlockDataSO.saveBlockDatas.Count > 0)
            {
                foreach (var dataBlock in saveBlockDataSO.saveBlockDatas)
                {
                    LoadDataBlockUI(dataBlock);
                }
            }
        }
    }

    public void CreateNewSavePath(out string savePath, int index)
    {
        savePath = System.IO.Path.Combine(Application.persistentDataPath, $"playerData{index}.json");
        CreateNewSaveBlock(savePath, index);

    }

    public void CreateNewSaveBlock(string savePath, int index)
    {
        GameObject newButton = Instantiate(_saveBlockObject, _contentTransform);
        var rectNewButton = newButton.GetComponent<RectTransform>();

        Vector3 newPosition = saveBlockDataSO.saveBlockDatas.Count == 0
            ? new Vector3(0f, _distanceBetweenBlocks)
            : new Vector3(saveBlockDataSO.saveBlockDatas.Last().blockPosition.x,
                            saveBlockDataSO.saveBlockDatas.Last().blockPosition.y - _distanceBetweenBlocks);

        rectNewButton.localPosition = newPosition;

        Debug.Log($"New Position: {newPosition} && New Button Position: {rectNewButton.localPosition}");

        SaveBlockData newBlockData = new SaveBlockData()
        {
            savePath = savePath,
            blockName = $"[Button] SavePath{index}",
            blockPosition = rectNewButton.localPosition
        };

        newButton.GetComponent<LoadBlock>().saveBlockData = currentBlockCreated = newBlockData;
        saveBlockDataSO.saveBlockDatas.Add(newBlockData);
    }

    public bool NewBlockCreated() => saveBlockDataSO.saveBlockDatas.Contains(currentBlockCreated);

    public void LoadDataBlockUI(SaveBlockData dataBlock)
    {
        GameObject savedBlock = Instantiate(_saveBlockObject, _contentTransform);
        savedBlock.name = dataBlock.blockName;

        RectTransform rectSavedBlock = savedBlock.GetComponent<RectTransform>();
        rectSavedBlock.localPosition = dataBlock.blockPosition;

        LoadBlock loadBlock = savedBlock.GetComponent<LoadBlock>();
        loadBlock.saveBlockData.savePath = dataBlock.savePath;
    }
}
