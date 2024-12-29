using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private CheckpointSO _checkpointList;

    [HideInInspector] public List<GameObject> portalCanvas = new List<GameObject>();
    [HideInInspector] public GameObject currentPortal = null;

    private const string _portalTag = "Checkpoint";
    private const string _portalCanvas = "PortalCanvas";

    public static CheckpointManager instance { get; private set; }

    private void Awake()
    {
        instance = this;

        GameObject[] portalObjects = GameObject.FindGameObjectsWithTag(_portalTag);
        foreach (GameObject portalObject in portalObjects)
        {
            portalCanvas.Add(portalObject.transform.Find(_portalCanvas).gameObject);
        }
    }

    public bool IsUsingPortal()
    {
        foreach (GameObject portalObject in portalCanvas)
        {
            if (portalObject.activeSelf)
            {
                currentPortal = portalObject;
                return true;
            }
        }
        return false;
    }
}
