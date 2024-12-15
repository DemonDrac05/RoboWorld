using UnityEngine;

public class MovementBlocker : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == Player.player.Collider)
        {
            Player.player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            Player.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider == Player.player.Collider)
        {
            Player.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Player.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider == Player.player.Collider)
        {
            Debug.Log("OnWall");
        }
    }
}
