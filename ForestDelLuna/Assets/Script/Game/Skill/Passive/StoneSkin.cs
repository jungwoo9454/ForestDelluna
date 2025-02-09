using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSkin : Skill
{
    public ParticleSystem Shield;
    int nEnemy = 0;

    public override void SkillOn()
    {
        Mathf.Clamp(nEnemy, 0, 5);
        Player.Ins.AddPctDeffensive(nEnemy * 0.1f);

        Debug.Log(nEnemy);

        if(nEnemy >= 4)
        {
            Shield.gameObject.SetActive(true);
            Shield.Play();
        }else
        {
            Shield.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Enemy")
        {
            nEnemy++;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            nEnemy--;
        }
    }
}