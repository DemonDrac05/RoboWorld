using UnityEngine;
using UnityEngine.Animations;

public class TeleportMenuButton : PauseMenuButton
{
    [Header("=== Teleport Canvas ==========")]
    private GameObject portalMenuCanvas;
    private GameObject currentPortal;

    protected const string Menu_Save = Menu_ButtonName + " Save";
    protected const string Menu_Map = Menu_ButtonName + " Map";

    public override void OnEnable()
    {
        base.OnEnable();
        GamePauseManager.instance.PauseGame();

        currentPortal = GetComponentInParent<TeleportPortal>().gameObject;
        portalMenuCanvas = currentPortal.transform.Find("PortalCanvas").gameObject;
    }

    public override void FunctionButtons()
    {
        switch (this.gameObject.name)
        {
            case Menu_Resume:
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
