using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using Unity.VisualScripting;

public class WaitScreenManager : MonoBehaviour
{
    public static WaitScreenManager instance { get; private set; }
    public GameObject waitScreenPrefab; 

    private GameObject _waitScreenInstance;
    private bool _isAnimating = true;

    private bool _completeGameLoadProcess = false;

    private float textAnimationSpeed = 0.1f;
    private float sliderAnimatonSpeed = 0.001f;

    private Slider _loadingBar;
    private TextMeshProUGUI _loadingText;

    private const string LoadingAssets = "Loading Assets";
    private const string LoadingGame = "Loading Game";
    private const string LoadingDefault = "Loading";

    private const string Menu = "MenuScene";
    private const string Game = "TerrainScene";

    private void Awake()
    {
        instance = this;
    }

    public void LoadWaitScreen()
    {
        waitScreenPrefab = Resources.Load("[Canvas] WaitScreen").GameObject();
        _waitScreenInstance = Instantiate(waitScreenPrefab);

        _loadingBar = _waitScreenInstance.GetComponentInChildren<Slider>();
        _loadingText = _waitScreenInstance.GetComponentInChildren<TextMeshProUGUI>();
    }

    public async UniTask AnimateWaitScreen()
    {
        await UniTask.WaitUntil(() => _waitScreenInstance != null);
        {
            while (_isAnimating)
            {
                await UniTask.WhenAll(AnimateText(), AnimateSlider());
            }
            if (!_isAnimating)
            {
                Destroy(_waitScreenInstance);
            }
        }
    }

    private async UniTask AnimateText()
    {
        while (_isAnimating)
        {
            for (int i = 0; i <= 3; i++)
            {
                string dotString = string.Empty;
                for (int j = 0; j < i; j++)
                {
                    dotString += ".";
                }
                var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                _loadingText.text = currentScene.name switch
                {
                    Menu => LoadingAssets,
                    Game => LoadingGame,
                    _ => LoadingDefault
                } + dotString;
                await UniTask.Delay(TimeSpan.FromSeconds(textAnimationSpeed), true);
            }
        }
    }

    private async UniTask AnimateSlider()
    {
        while (_isAnimating)
        {
            for (int i = (int)_loadingBar.value; i <= (int)_loadingBar.maxValue; i++)
            {
                if (_loadingBar.value <= 90 || _completeGameLoadProcess)
                {
                    _loadingBar.value = i;
                }
                else if (_loadingBar.value >= 90 && !_completeGameLoadProcess)
                {
                    _loadingBar.value = 90;

                    await SaveSystem.LoadGameProcess();
                    _completeGameLoadProcess = true;
                    break;
                }

                if (_loadingBar.value == 100 && _completeGameLoadProcess)
                {
                    _isAnimating = false; 
                    break;
                }
                await UniTask.Delay(TimeSpan.FromSeconds(sliderAnimatonSpeed), true);
            }
        }
    }

}