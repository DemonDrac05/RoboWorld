using UnityEngine;

public interface IComponents
{
    Rigidbody Rigidbody { get; }
    Animator Animator { get; }
    Collider Collider { get; }
}
