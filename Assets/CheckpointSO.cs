using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Checkpoint")]
public class CheckpointSO : ScriptableObject
{
    public List<Vector3> checkpoints;
}
