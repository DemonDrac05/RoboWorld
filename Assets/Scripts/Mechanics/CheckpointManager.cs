using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private CheckpointSO _checkpointList;

    [HideInInspector] public Dictionary<Vector3, string> portals = new Dictionary<Vector3, string>();
    [HideInInspector] public List<GameObject> portalCanvas = new List<GameObject>();
    [HideInInspector] public GameObject currentPortal = null;

    private const string _portalTag = "Checkpoint";
    private const string _portalCanvas = "PortalCanvas";

    private const string _portalDefalutName = "Corridor 1";

    public static CheckpointManager instance { get; private set; }

    private void Awake()
    {
        instance = this;

        portals.Add(_checkpointList.checkpoints[0], _portalDefalutName);

        GameObject[] portalObjects = GameObject.FindGameObjectsWithTag(_portalTag);
        foreach (GameObject portalObject in portalObjects)
        {
            portalCanvas.Add(portalObject.transform.Find(_portalCanvas).gameObject);

            var portal = portalObject.GetComponent<TeleportPortal>();
            if (!portals.ContainsKey(portal.transform.position))
            {
                portals.Add(portal.transform.position, portal.portalName);
            }
        }
    }

    private void Start()
    {
        SortPortalPositions();
    }

    public int ProgressPercentage() 
    { 
        if (_checkpointList.checkpoints.Count > 1)
        {
            return (portals.Count - 1) * 100 / (_checkpointList.checkpoints.Count - 1);
        }
        return 100;
    }

    private void SortPortalPositions()
    {
        if (portals.Count <= 1) return;

        Vector3 origin = portals.First().Key;

        var sortedDictionary = portals.OrderBy(pair => CalculateDistanceXZ(pair.Key, origin)).ToDictionary(pair => pair.Key, pair => pair.Value);

        portals = sortedDictionary;
    }

    private float CalculateDistanceXZ(Vector3 position, Vector3 origin)
    {
        return Mathf.Sqrt(Mathf.Pow(position.x - origin.x, 2)) + Mathf.Sqrt(Mathf.Pow(position.z - origin.z, 2));
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
