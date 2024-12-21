using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance { get; private set; }

    private void Awake() => instance = this;

    public void SetImageAlphaColor(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha / 255f;
        image.color = color;
    }
}
