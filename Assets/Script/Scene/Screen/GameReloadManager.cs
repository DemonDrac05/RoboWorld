using UnityEngine;

public class GameReloadManager : MonoBehaviour
{
    [Header("=== GUI Assets ==========")]
    [SerializeField] private GameObject reloadSceneCanvas;

    private void Update()
    {
        if (PlayerHealth.player.currentHealth <= 0f)
        {
            reloadSceneCanvas.SetActive(true);
        }
    }
}
