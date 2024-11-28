using UnityEngine;

public class MovementBlocker : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == Player.player.playerCollider)
        {
            Player.player.direction = Vector3.zero;
            Player.player.isMoving = false;
        }
    }
}
