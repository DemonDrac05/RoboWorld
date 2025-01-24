using UnityEngine;

public class CheckPlayerOnPortalSignal : MonoBehaviour
{
    private PlayerInteract _playerInteract;
    private TeleportPortal _teleportPortal;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && _teleportPortal != null)
        {
            _playerInteract = collider.GetComponent<PlayerInteract>();
            _playerInteract.SetCurrentPortal(_teleportPortal);
            _playerInteract.isTriggerPortal = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && _teleportPortal != null)
        {
            _playerInteract = collider.GetComponent<PlayerInteract>();
            _playerInteract.SetCurrentPortal(null);
            _playerInteract.isTriggerPortal = false;
        }
    }

    public void SetPortal(TeleportPortal portal)
    {
        _teleportPortal = portal;
    }
}
