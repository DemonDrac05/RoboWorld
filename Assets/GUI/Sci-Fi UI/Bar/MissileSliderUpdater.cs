using UnityEngine;
using UnityEngine.UI;

public class MissileSliderUpdater : MonoBehaviour
{
    [Header("=== Slider Settings ===")]
    public Slider missileSlider; // Slider referansý
    public MissileLaunch missileLaunch; // MissileLaunch referansý

    private void Start()
    {
        if (missileSlider != null && missileLaunch != null)
        {
            missileSlider.minValue = 0f;
            missileSlider.maxValue = missileLaunch.maxMissileQuantity;
            missileSlider.value = missileLaunch.currentMissileQuantity;
        }
        else
        {
            Debug.LogWarning("Slider veya MissileLaunch referansý eksik!");
        }
    }

    private void Update()
    {
        if (missileSlider != null && missileLaunch != null)
        {
            missileSlider.value = missileLaunch.currentMissileQuantity;
        }
    }
}
