using UnityEngine;

public class DemoTesting : MonoBehaviour
{
    private CharacterController characterController;
    public bool isGrounded = false;
    public float groundCheckDistance;
    private float bufferCheckDistance = 0.1f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {

        //groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
        //{
        //    isGrounded = true;
        //}
        //else
        //{
        //    isGrounded = false;
        //}
        //Debug.Log(isGrounded);
        Debug.Log(characterController.isGrounded);
    }
}