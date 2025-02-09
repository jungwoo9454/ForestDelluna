using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMng : MonoBehaviour
{
    public List<Particle> PsPrefabList = new List<Particle>();

    public GameObject OriginalAni;
    public GameObject OriginalParticle;

    public Transform AnimngTrans;
    public Transform PsmngTrans;

    List<Particle> ParticleObjList = new List<Particle>();
    List<Animator> AniObjList = new List<Animator>();

    private static EffectMng effectMng;
    public static EffectMng Ins
    {
        get
        {
            if (effectMng == null)
            {
                effectMng = FindObjectOfType<EffectMng>();

                if (effectMng == null)
                {
                    GameObject effectObj = new GameObject();
                    effectObj.name = "EffectMng";
                    effectMng = effectObj.AddComponent<EffectMng>();
                }
            }
            return effectMng;
        }
    }


    public void CreateEffect(string Effname, Vector3 spawnpos)
    {
        bool FindEffect = false;
        for (int i = 0; i < AniObjList.Count; i++)
        {
            if (!AniObjList[i].gameObject.activeSelf)        
            {
                AniObjList[i].transform.position = spawnpos;
                AniObjList[i].gameObject.SetActive(true);
                FindEffect = true;          
                AniObjList[i].Play(Effname);
                break;
            }
        }

        if (!FindEffect)
        {
            Animator effect = Instantiate(OriginalAni).GetComponent<Animator>();   
            AniObjList.Add(effect);                                               
            effect.transform.parent = AnimngTrans;
            effect.gameObject.SetActive(true);
            effect.transform.position = spawnpos;
            effect.Play(Effname);
        }
    }

    public void CreateParticle(string Effname, Vector3 spawnpos, ref Transform pos)
    {
        bool FindParticle = false;
        for (int i = 0; i < ParticleObjList.Count; i++)     
        {
            if (!ParticleObjList[i].GetUse() && ParticleObjList[i].gameObject.name == Effname)
            {
                ParticleObjList[i].CreateParticle(spawnpos, ref pos);
                FindParticle = true;
                break;
            }
        }

        if (!FindParticle)
        {
            Particle effect = Instantiate(GetPs(Effname)).GetComponent<Particle>();
            ParticleObjList.Add(effect);
            effect.gameObject.name = Effname;
            effect.transform.parent = PsmngTrans;                                 
            effect.CreateParticle(spawnpos, ref pos);
        }
    }

    public void CreateParticle(string Effname, Vector3 spawnpos)
    {
        bool FindParticle = false;
        for (int i = 0; i < ParticleObjList.Count; i++)
        {
            if (!ParticleObjList[i].GetUse() && ParticleObjList[i].gameObject.name == Effname)
            {
                ParticleObjList[i].CreateParticle(spawnpos);
                FindParticle = true;
                break;
            }
        }

        if (!FindParticle)
        {
            Particle effect = Instantiate(GetPs(Effname)).GetComponent<Particle>();
            effect.PsParticle = GetComponent<ParticleSystem>();
            ParticleObjList.Add(effect);
            effect.gameObject.name = Effname;
            effect.transform.parent = PsmngTrans;
            effect.CreateParticle(spawnpos);
        }
    }

    ParticleSystem GetPs(string Effname)
    {
        for (int i = 0; i < PsPrefabList.Count; i++)
        {
            if (Effname == PsPrefabList[i].gameObject.name)
            {
                ParticleSystem ani = PsPrefabList[i].GetComponent<ParticleSystem>();
                return ani;
            }
        }
        return null;
    }
}
