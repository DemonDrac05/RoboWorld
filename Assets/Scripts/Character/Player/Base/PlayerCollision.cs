using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Rigidbody playerRigidbody;

    private PlayerMovement playerMovement;
    private Player player;

    private bool isTouchingWall = false;
    private float additionalGravity = 20f;
    private float maxSlideSpeed = -10f;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;

            Vector3 normal = collision.contacts[0].normal;

            if (playerRigidbody.linearVelocity.y > 0)
            {
                playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0, playerRigidbody.linearVelocity.z);
            }

            Vector3 downwardForce = Vector3.down * additionalGravity;
            playerRigidbody.AddForce(downwardForce, ForceMode.Acceleration);

            if (playerRigidbody.linearVelocity.y < maxSlideSpeed)
            {
                playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, maxSlideSpeed, playerRigidbody.linearVelocity.z);
            }
        }
        //if (collision.gameObject.CompareTag("Floor"))
        //{
        //    playerMovement.SetMobility(true);
        //    Debug.Log("On Floor");
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
        }
    }
}
