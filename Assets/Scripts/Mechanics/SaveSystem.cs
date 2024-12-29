using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;

public static class SaveSystem
{
    public static string savePath;

    public static void NewGame()
    {
        var saveBlockDataSO = LoadScreenManager.instance.saveBlockDataSO;
        int newIndex = saveBlockDataSO.saveBlockDatas.Count + 1;
        LoadScreenManager.instance.CreateNewSavePath(out savePath, newIndex);

        if (LoadScreenManager.instance.NewBlockCreated())
        {
            SaveGame(new PlayerData()); LoadGame();
        }
    }

    public static void SaveGame(PlayerData data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(savePath, json);
    }

    public static PlayerData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }
        return null;
    }

    public static bool IsNewGame() => !File.Exists(savePath);

    public static async UniTask LoadGameProcess()
    {
        if (!IsNewGame()) LoadGame();
        await UniTask.WaitUntil(() => !IsNewGame());
    }
}
