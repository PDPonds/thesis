using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    Animator anim;
    ParticleSystem particle;

    private void Awake()
    {
        TryGetComponent<Animator>(out anim);
        TryGetComponent<ParticleSystem>(out particle);
    }

    [System.Obsolete]
    void Update()
    {
        if (anim != null)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                Destroy(gameObject);
            }
        }

        if(particle != null)
        {
            Destroy(gameObject, particle.duration);
        }

    }
}
