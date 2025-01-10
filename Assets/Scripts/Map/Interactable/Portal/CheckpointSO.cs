using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Checkpoint")]
public class CheckpointSO : ScriptableObject
{
    public List<Vector3> checkpoints = new List<Vector3>();
    private Vector3 _defaultCheckpoint = new Vector3(0f, -11.59f, 0f);

    public void LoadData(string path)
    {
        CheckpointData loadedData = JsonDataSerializer.Deserialize<CheckpointData>(path);
        if (loadedData == null) return;
        checkpoints = loadedData.checkpoints;
    }

    public void SaveData(string path)
    {
        CheckpointData dataToSave = new CheckpointData();
        if (checkpoints.Count > 0)
        {
            if (!checkpoints.Contains(_defaultCheckpoint))
            {
                checkpoints.Insert(0, _defaultCheckpoint);
            }
        }
        else
        {
            checkpoints.Add(_defaultCheckpoint);
        }
        dataToSave.checkpoints = checkpoints;
        JsonDataSerializer.Serialize(dataToSave, path);
    }
}

[System.Serializable]
public class CheckpointData
{
    public List<Vector3> checkpoints = new List<Vector3>();
}
