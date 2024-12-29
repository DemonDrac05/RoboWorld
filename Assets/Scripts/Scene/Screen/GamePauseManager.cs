using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseManager : MonoBehaviour
{
    [Header("=== GUI Assets ==========")]
    public GameObject pauseMenuCanvas;

    private GameObject portalMenuCanvas;

    [HideInInspector] public bool isPaused = false;
    public static GamePauseManager instance { get; private set; } 

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
            if (CheckpointManager.instance.IsUsingPortal())
            {
                StartCoroutine(ResumePortalMenu());
            }
            else if (pauseMenuCanvas.activeSelf)
            {
                StartCoroutine(ResumePauseMenu());
            }
        }
        else
        {
            PauseGame();
            if (!CheckpointManager.instance.IsUsingPortal()) pauseMenuCanvas.SetActive(true);
        }
    }

    private IEnumerator ResumePauseMenu()
    {
        var resumeButton = pauseMenuCanvas.transform.Find("[Button] Resume").gameObject;
        yield return resumeButton.GetComponent<PauseMenuButton>().ProcessPauseMenuButtons();
    }

    private IEnumerator ResumePortalMenu()
    {
        portalMenuCanvas = CheckpointManager.instance.currentPortal;
        var portalResumeButton = portalMenuCanvas?.transform.Find("[Button] Resume").gameObject;
        yield return portalResumeButton.GetComponent<TeleportMenuButton>().ProcessPauseMenuButtons();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        playerMovement.SetMobility(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        playerMovement.SetMobility(true);
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

