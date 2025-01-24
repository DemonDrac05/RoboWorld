using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    // --- LayerMask ----------
    private readonly string currentMask = "Player";
    private readonly string dodgingMask = "Ignore Raycast";

    // ---
    private Player player;
    private PlayerMovement playerMovement; // Add reference to PlayerMovement
    private float groundCheckDistance = 0.2f; // Increased Ray Length

    private void Awake()
    {
        player = GetComponent<Player>();
        playerMovement = GetComponent<PlayerMovement>(); // Assign PlayerMovement component
    }

    private void FixedUpdate()
    {
        SetLayerMaskOnDodging();
    }

    private void Update()
    {
        SetBodyOnGround();

        player.SetGrounded(CheckIsGrounded());

        if (CheckIsGrounded()) Debug.Log("On Ground");
    }

    public bool CheckIsGrounded()
    {
        // Use a more appropriate origin, starting a bit above the player's center
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f; // Add small offset above the center

        // Ensure playerMovement.clickableLayer is valid
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement reference not initialized.");
            return false;
        }
        if (playerMovement.clickableLayer == 0)
        {
            Debug.LogError("Clickable Layer is not initialized in PlayerMovement");
            return false;
        }

        return Physics.Raycast(raycastOrigin, Vector3.down, groundCheckDistance, playerMovement.clickableLayer);
    }

    protected void SetLayerMaskOnDodging() => gameObject.layer = LayerMask.NameToLayer(player.IsVulnerable ? currentMask : dodgingMask);

    protected void SetBodyOnGround()
    {
        if (CheckIsGrounded())
        {
            player.Rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
    }
}
