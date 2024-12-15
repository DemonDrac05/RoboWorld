using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameReloadManager : MonoBehaviour
{
    [Header("=== GUI Assets ==========")]
    [SerializeField] private GameObject reloadSceneCanvas;
    [SerializeField] private Button agreeButton;
    [SerializeField] private Button disagreeButton;

    public static GameReloadManager instance;

    private void Awake() => instance = this;

    public void ReloadScene()
    {
        GamePauseManager.instance.PauseGame();
        reloadSceneCanvas.SetActive(true);
    }

    //private void Update()
    //{
    //    agreeButton?.onClick.AddListener(OnAgreeButtonClick);
    //    disagreeButton?.onClick.AddListener(OnDisagreeButtonClick);
    //}

    //private void OnAgreeButtonClick()
    //{
    //    StartCoroutine(ReloadProcess());
    //}

    //private void OnDisagreeButtonClick()
    //{

    //}

    //IEnumerator ReloadProcess()
    //{
    //    var effectController = Player.player.GetComponentInParent<DissolveController>();

    //    PlayerStat.playerStat.GetCheckpoint(Player.player.transform.position);

    //    Player.player.stateMachine.ChangeState(Player.player.movementState);
    //}
}
