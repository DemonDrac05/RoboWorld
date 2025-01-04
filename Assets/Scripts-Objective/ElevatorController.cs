using System.Collections;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float distance = 10.0f;
    public float speed = 2.0f;
    public float waitTime = 1.0f;

    [SerializeField] private MovementDirection direction = MovementDirection.Vertical; // Direction selector

    private Vector3 bottomPosition;
    private Vector3 topPosition;
    private Vector3 targetPosition;

    void Start()
    {
        // Set movement direction
        Vector3 movementVector = direction == MovementDirection.Vertical ? Vector3.up : Vector3.forward;

        bottomPosition = transform.position;
        topPosition = bottomPosition + movementVector * distance;
        targetPosition = topPosition;

        StartCoroutine(MoveElevator());
    }

    IEnumerator MoveElevator()
    {
        while (true)
        {
            // Move towards the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            // Wait at the target position
            yield return new WaitForSeconds(waitTime);

            // Switch target position
            targetPosition = targetPosition == bottomPosition ? topPosition : bottomPosition;
        }
    }

    // Enum for movement direction
    public enum MovementDirection
    {
        Vertical,
        Horizontal
    }
}
