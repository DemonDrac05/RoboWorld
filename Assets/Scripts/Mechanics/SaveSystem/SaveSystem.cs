using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    public static string savePath;
    public static GameObject newWaitScreen;

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

    public static void DeleteSaveFile(string savePath)
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }

    public static bool IsNewGame() => !File.Exists(savePath);

    public static async UniTask LoadGameProcess()
    {
        if (!IsNewGame()) LoadGame();
        await UniTask.WaitUntil(() => !IsNewGame());
    }
    public static async UniTask LoadGameSuccess()
    {
        await LoadMenuSceneAsync();
        await LoadGameSceneAsync();
    }

    public static async UniTask LoadMenuSceneAsync()
    {
        WaitScreenManager.instance.LoadWaitScreen();
        await WaitScreenManager.instance.AnimateWaitScreen();
    }

    public static async UniTask LoadGameSceneAsync()
    {
        var asyncOp = SceneManager.LoadSceneAsync("TestingGameplayScene");
        asyncOp.allowSceneActivation = false;

        while (asyncOp.progress < 0.9f)
        {
            await UniTask.Yield();
        }

        asyncOp.allowSceneActivation = true;
        await UniTask.WaitUntil(() => asyncOp.isDone);

        WaitScreenManager.instance.LoadWaitScreen();

        GamePauseManager.instance.PauseGame();

        if (PlayerStat.playerStat != null)
        {
            PlayerData loadedData = SaveSystem.LoadGame();
            if (loadedData != null)
            {
                await PlayerStat.playerStat.ApplyPlayerData(loadedData);
            }
        }

        await WaitScreenManager.instance.AnimateWaitScreen();

        GamePauseManager.instance.ResumeGame();
    }

}
