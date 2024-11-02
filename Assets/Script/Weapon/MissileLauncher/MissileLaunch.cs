using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissileLaunch : MonoBehaviour
{
    [Header("=== Missile Properties ==========")]
    public GameObject missilePrefab;
    public int maxMissileQuantity;
    public int maxMissileQuantityForSingleTarget;
    public int currentMissileQuantity;

    [HideInInspector] public Dictionary<GameObject, float> enemyList = new();
    [HideInInspector] public List<GameObject> targetList = new();
    [HideInInspector] public bool isPreLaunching = true;
    [HideInInspector] public float preLaunchTimer = 0f;

    public static MissileLaunch missileManager;

    private void Awake() => missileManager = this;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && isPreLaunching)
        {
            preLaunchTimer += Time.deltaTime;
            if (preLaunchTimer > 0.25f)
            {
                preLaunchTimer = 0f;
                currentMissileQuantity++;
            }
            if (currentMissileQuantity > maxMissileQuantity)
            {
                currentMissileQuantity = maxMissileQuantity;
            }
            if (currentMissileQuantity > enemyList.Count * maxMissileQuantityForSingleTarget)
            {
                currentMissileQuantity = enemyList.Count * maxMissileQuantityForSingleTarget;
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
                GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
                targetList.Add(missile);
                currentMissileQuantity--;

                if (currentMissileQuantity == 0)
                {
                    isPreLaunching = true;
                    break;
                }
            }
        }

        if (enemyList.Count > 0)
        {
            foreach (var enemy in enemyList.Keys.ToList())
            {
                float distance
                    = Vector3.Distance(Player.player.transform.position, enemy.transform.position);
                enemyList[enemy] = distance;
            }
            enemyList = enemyList.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        if (targetList.Count > 0 && enemyList.Count > 0)
        {
            var enemyIndex = enemyList.ToList();
            var targetIndex = targetList.ToList();

            int minCount = Mathf.Min(targetIndex.Count, enemyIndex.Count);

            for (int i = 0; i < minCount; i++)
            {
                targetIndex[i].GetComponent<MissileFlight>().movingTarget = enemyIndex[i].Key.transform;
            }
            for (int i = minCount; i < targetIndex.Count; i++)
            {
                targetIndex[i].GetComponent<MissileFlight>().movingTarget = enemyIndex[i % enemyIndex.Count].Key.transform;
            }
        }
    }
}
