using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class MissileFlight : MonoBehaviour
{
    public float flightDuration;
    public Vector3 controlPoint;

    [HideInInspector] public Transform movingTarget = null;

    private void Start()
    {
        if (movingTarget != null)
        {
            float offsetX = Random.Range(-controlPoint.x, controlPoint.y);
            float offsetY = Random.Range(-controlPoint.x, controlPoint.y);
            float offsetZ = Random.Range(-controlPoint.x, controlPoint.y);

            controlPoint = new Vector3(offsetX, offsetY, offsetZ);
            controlPoint += (transform.position + movingTarget.position) / 2;

            StartCoroutine(FlyAlongCurve(this.transform.position, movingTarget, controlPoint, flightDuration));
        }
    }

    Vector3 BezierCurve(Vector3 a, Vector3 b, Vector3 control, float t)
    {
        return (1 - t) * (1 - t) * a + 2 * (1 - t) * t * control + t * t * b;
    }

    IEnumerator FlyAlongCurve(Vector3 startPoint, Transform movingTarget, Vector3 controlPoint, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector3 endPoint = movingTarget.position;

            float t = elapsedTime / duration;
            Vector3 curvePosition = BezierCurve(startPoint, endPoint, controlPoint, t);

            float lookAheadT = Mathf.Clamp01(t + 0.01f);
            Vector3 futurePosition = BezierCurve(startPoint, endPoint, controlPoint, lookAheadT);
            Vector3 direction = (futurePosition - curvePosition).normalized;
            transform.rotation = Quaternion.LookRotation(direction);

            transform.position = curvePosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = movingTarget.position;

        CombatManager.instance.ApplyMissileDamage(PlayerStat.playerStat, movingTarget.GetComponent<EnemyStat>());

        MissileLaunch.missileManager.targetList.Remove(this.gameObject);

        Destroy(this.gameObject);
    }
}
