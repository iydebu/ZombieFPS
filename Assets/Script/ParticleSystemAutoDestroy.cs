using UnityEngine;

public class ParticleSystemAutoDestroy : MonoBehaviour
{
    private ParticleSystem particle_System;

    private void Start()
    {
        particle_System = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particle_System != null && !particle_System.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
