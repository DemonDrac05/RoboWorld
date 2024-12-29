public class PauseMenuButton : MainMenuButton
{
    protected const string Menu_MainMenu = Menu_ButtonName + " MainMenu";
    protected const string Menu_Resume = Menu_ButtonName + " Resume";

    public override void FunctionButtons()
    {
        switch (this.gameObject.name)
        {
            case Menu_Resume:
                GamePauseManager.instance.ResumeGame();
                GamePauseManager.instance.pauseMenuCanvas.SetActive(false);
                break;
            case Menu_Settings:
                break;
            case Menu_MainMenu:
                GamePauseManager.instance.LoadScene(true, "MenuScene");
                break;
        }
    }
}