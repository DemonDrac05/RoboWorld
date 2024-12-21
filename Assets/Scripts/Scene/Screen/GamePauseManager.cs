using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePauseManager : MonoBehaviour
{
    [Header("=== GUI Assets ==========")]
    public GameObject pauseMenuCanvas;
    public PauseMenuButton resumeButton;

    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public static GamePauseManager instance;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        instance = this;
        playerMovement = FindObjectOfType<PlayerMovement>(); // Find PlayerMovement script in the scene.
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            StartCoroutine(resumeButton.ProcessPauseMenuButtons());
        }
        else
        {
            PauseGame();
            pauseMenuCanvas.SetActive(true);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        playerMovement.SetMobility(false); // Disable player movement on pause.
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        playerMovement.SetMobility(true); // Re-enable player movement on resume.
    }

    public void LoadScene(bool gamePaused, string sceneName)
    {
        Convert.ToInt32(gamePaused);
        Time.timeScale = gamePaused ? 1 : 0;
        SceneManager.LoadScene(sceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}

