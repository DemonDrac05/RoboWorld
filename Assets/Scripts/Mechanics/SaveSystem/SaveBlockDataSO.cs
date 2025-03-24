using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SaveData/SaveDataBlock")]
public class SaveBlockDataSO : ScriptableObject
{
    public List<SaveBlockData> saveBlockDatas = new List<SaveBlockData>();
}

[System.Serializable]
public class SaveBlockData
{
    public string savePath;
    public string blockName;
    public Vector3 blockPosition;
}