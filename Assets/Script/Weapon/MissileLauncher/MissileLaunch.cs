using UnityEngine;

public class MissileLaunch : MonoBehaviour
{
    [Header("=== Missile Properties ==========")]
    public GameObject missilePrefab;
    public int maxMissileQuantity;
    public int currentMissileQuantity;

    public Transform movingTarget;

    private bool isPreLaunching = true;
    private float preLaunchTimer = 0f;

    public static MissileLaunch missileManager;

    private void Awake() => missileManager = this;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && isPreLaunching)
        {
            preLaunchTimer += Time.deltaTime;
            if (preLaunchTimer > 0.1f)
            {
                preLaunchTimer = 0f;
                currentMissileQuantity++;
            }
            if (currentMissileQuantity > maxMissileQuantity)
            {
                currentMissileQuantity = maxMissileQuantity;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            isPreLaunching = false;
            preLaunchTimer = -1f;
        }
        if (currentMissileQuantity > 0 && !isPreLaunching && preLaunchTimer == -1f)
        {
            for (int i = 0; i < currentMissileQuantity; i++)
            {
                GameObject missile = Instantiate(missilePrefab, transform);
                currentMissileQuantity--;
                if (currentMissileQuantity == 0)
                {
                    isPreLaunching = true;
                    break;
                }
            }
        }
    }
}
