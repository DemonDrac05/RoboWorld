using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadBlock : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public SaveBlockData saveBlockData;

    public async void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SaveSystem.savePath = saveBlockData?.savePath;
            await SaveSystem.LoadGameProcess();
            SceneManager.LoadScene("TerrainScene");
        }
    }
}
