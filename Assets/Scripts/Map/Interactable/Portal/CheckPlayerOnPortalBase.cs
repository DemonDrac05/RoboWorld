using UnityEngine;

public class CheckPlayerOnPortalBase : MonoBehaviour
{
    private TeleportPortal _portal;

    private void OnEnable()
    {
        _portal = GetComponentInParent<TeleportPortal>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _portal.isOnPortalBase = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _portal.isOnPortalBase = false;
        }
    }
}
