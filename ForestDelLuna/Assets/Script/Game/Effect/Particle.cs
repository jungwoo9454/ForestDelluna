using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    Transform TransParticle;
    Transform ParentsPos;
    public ParticleSystem PsParticle;
    public bool bUse;

    void Update()
    {
        if (PsParticle.isPlaying)
        {
            if (ParentsPos != null)
                transform.position = ParentsPos.position;
        }
        else
        {
            bUse = false;
            gameObject.SetActive(false);
        }
    }

    public bool GetUse()
    {
        return bUse;
    }

    public void CreateParticle(Vector3 spawnpos, ref Transform pos)
    {
        if (PsParticle == null || TransParticle == null)
        {
            PsParticle = GetComponent<ParticleSystem>();
            TransParticle = GetComponent<Transform>();
        }

        ParentsPos = pos;
        TransParticle.position = spawnpos;
        bUse = true;
        gameObject.SetActive(true);
        PsParticle.Play();
    }

    public void CreateParticle(Vector3 spawnpos)
    {
        if (PsParticle == null || TransParticle == null)
        {
            PsParticle = GetComponent<ParticleSystem>();
            TransParticle = GetComponent<Transform>();
        }

        ParentsPos = null;
        TransParticle.position = spawnpos;
        bUse = true;
        gameObject.SetActive(true);
        PsParticle.Play();
    }
}
