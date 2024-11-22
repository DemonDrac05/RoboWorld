using UnityEngine;

public class GameReloadManager : MonoBehaviour
{
    [Header("=== GUI Assets ==========")]
    [SerializeField] private GameObject reloadSceneCanvas;

    private void Update()
    {
        if (PlayerStat.playerStat.Health <= 0f)
        {
            reloadSceneCanvas.SetActive(true);
        }
    }
}
