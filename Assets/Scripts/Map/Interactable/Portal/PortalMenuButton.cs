using UnityEngine;
using UnityEngine.Animations;

public class PortalMenuButton : PauseMenuButton
{
    [Header("=== Teleport Canvas ==========")]
    private GameObject portalMenuCanvas;
    private TeleportPortal currentPortal;

    protected const string Menu_Save = Menu_ButtonName + " Save";
    protected const string Menu_Map = Menu_ButtonName + " Map";

    public override void OnEnable()
    {
        base.OnEnable();
        GamePauseManager.instance.PauseGame();

        currentPortal = GetComponentInParent<TeleportPortal>();
        portalMenuCanvas = currentPortal.transform.Find("PortalCanvas").gameObject;
    }

    public override void FunctionButtons()
    {
        switch (this.gameObject.name)
        {
            case Menu_Resume:
                currentPortal.IsResumeButtonPressed = true;
                GamePauseManager.instance.ResumeGame();
                portalMenuCanvas.SetActive(false);
                break;
            case Menu_Save:
                SaveSystem.SaveGame(PlayerStat.playerStat.CreatePlayerData(currentPortal.transform.position));
                break;
            case Menu_Map:
                break;
        }
    }
}
