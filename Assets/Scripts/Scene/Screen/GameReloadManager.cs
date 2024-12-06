using UnityEngine;

public class GameReloadManager : MonoBehaviour
{
    [Header("=== GUI Assets ==========")]
    [SerializeField] private GameObject reloadSceneCanvas;

    public static GameReloadManager instance;

    private void Awake() => instance = this;

    public void ReloadScene()
    {
        GamePauseManager.instance.PauseGame();
        reloadSceneCanvas.SetActive(true);
    }
}
