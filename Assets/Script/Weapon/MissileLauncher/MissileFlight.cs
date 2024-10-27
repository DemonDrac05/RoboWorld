using UnityEngine;
using System.Collections;

public class MissileFlight : MonoBehaviour
{
    public float flightDuration;
    public Vector3 controlPoint;
    public GameObject explosionEffect;

    private Transform movingTarget;

    private void Awake()
        => movingTarget = MissileLaunch.missileManager.movingTarget;

    private void OnEnable()
    {
        float offsetX = Random.Range(-controlPoint.x, controlPoint.y);
        float offsetY = Random.Range(-controlPoint.x, controlPoint.y);
        float offsetZ = Random.Range(-controlPoint.x, controlPoint.y);

        controlPoint = new Vector3(offsetX, offsetY, offsetZ);
        controlPoint += (transform.position + movingTarget.position) / 2;
    }

    private void Start()
        => StartCoroutine(FlyAlongCurve(this.transform.position, movingTarget, controlPoint, flightDuration));

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
        GameObject explosion = Instantiate(explosionEffect, transform);
        var effect = explosion.GetComponent<ParticleSystem>();
        effect.Play();

        transform.position = movingTarget.position;

        yield return new WaitUntil(() => effect.isStopped && effect.particleCount == 0);

        Destroy(explosion.gameObject);
        Destroy(this.gameObject);
    }
}
