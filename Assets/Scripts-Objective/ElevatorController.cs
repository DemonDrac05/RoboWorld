using System.Collections;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float distance = 10.0f;
    public float speed = 2.0f;
    public float waitTime = 1.0f;

    [SerializeField] private MovementDirection direction = MovementDirection.Vertical;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        Vector3 movementVector = GetMovementVector(direction);
        startPosition = transform.position;
        targetPosition = startPosition + movementVector * distance;
        StartCoroutine(MoveElevator());
    }

    IEnumerator MoveElevator()
    {
        while (true)
        {
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            targetPosition = targetPosition == startPosition ? startPosition + GetMovementVector(direction) * distance : startPosition;
        }
    }

    public enum MovementDirection
    {
        Vertical,
        Horizontal,
        SideToSide
    }

    private Vector3 GetMovementVector(MovementDirection direction)
    {
        switch (direction)
        {
            case MovementDirection.Vertical:
                return Vector3.up;
            case MovementDirection.Horizontal:
                return Vector3.forward;
            case MovementDirection.SideToSide:
                return Vector3.left; // Adjusted to move backward first (negative X direction)
            default:
                return Vector3.zero;
        }
    }
}
