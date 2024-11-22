using System.Collections.Generic;
using UnityEngine;

public class ParticleStopChecker : MonoBehaviour
{
    private List<ParticleSystem> _particles = new List<ParticleSystem>();

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            _particles.Add(child.GetComponent<ParticleSystem>());
        }
    }

    private void Update()
    {
        for (int i = _particles.Count - 1; i >= 0; i--)
        {
            var particle = _particles[i];
            if (particle.isStopped)
            {
                _particles.RemoveAt(i); // Safely remove the particle
            }
        }

        // Destroy the GameObject if the list is empty
        if (_particles.Count == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
