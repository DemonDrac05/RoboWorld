using UnityEngine;

public class TeleportMenuButton : PauseMenuButton
{
    [Header("=== Teleport Canvas ==========")]
    [SerializeField] protected GameObject teleportMenuCanvas;

    protected const string Menu_AreaMap = Menu_ButtonName + " AreaMap";

    public override void OnEnable()
    {
        base.OnEnable();
        GamePauseManager.instance.PauseGame();
    }

    public override void FunctionButtons()
    {
        switch (this.gameObject.name)
        {
            case Menu_Resume:
            case Menu_AreaMap:
                GamePauseManager.instance.ResumeGame();
                teleportMenuCanvas.SetActive(false);
                break;
                //case "Options":
                //    GamePauseManager.instance.LoadScene(true, "");
                //    break;
                //case "Exit":
                //    GamePauseManager.instance.LoadScene(true, "");
                //    break;
        }
    }
}
