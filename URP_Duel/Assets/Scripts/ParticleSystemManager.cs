using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    public static ParticleSystemManager Instance;

    public enum Particle
    {
        pfShadowParticles,
        pfTearParticles
    }

    private Dictionary<Particle, ParticleSystem> particleParticleSystemDictionary;

    private void Awake()
    {
        Instance = this;

        particleParticleSystemDictionary = new Dictionary<Particle, ParticleSystem>();
        foreach (Particle particle in System.Enum.GetValues(typeof(Particle)))
        {
            particleParticleSystemDictionary[particle] = Resources.Load<ParticleSystem>(particle.ToString());
        }
    }

    public void CreateParticle(Particle particle, Vector3 position)
    {
        transform.position = position;

        Instantiate(particleParticleSystemDictionary[particle], transform);
    }
}