using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    // --- LayerMask ----------
    private readonly string currentMask = "Player";
    private readonly string dodgingMask = "Ignore Raycast";

    // --- 
    private Player player;

    private void Awake() => player = GetComponent<Player>();

    private void FixedUpdate()
    {
        SetLayerMaskOnDodging();
    }

    private void Update()
    {
        SetBodyOnGround();

        player.SetGrounded(CheckIsGrounded());
    }

    public bool CheckIsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, GetComponent<PlayerMovement>().clickableLayer);
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
