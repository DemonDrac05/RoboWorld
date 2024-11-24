using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("TerrainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnMenu() // use for death menu button
    {
        SceneManager.LoadScene("Menu");
    }
}
