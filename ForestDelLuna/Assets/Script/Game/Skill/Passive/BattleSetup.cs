using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSetup : Skill
{
    public ParticleSystem Electro;
    List<float> fAttackTimeList = new List<float>();
    public override void SkillOn()
    {
        if(GameMng.ins.nAttackSuccess >= 3)
        {
            GameMng.ins.nAttackSuccess = 0;
            fAttackTimeList.Add(0f);
        }

        for (int i = 0; i < fAttackTimeList.Count; i++)
        {
            fAttackTimeList[i] += Time.deltaTime;

            if(fAttackTimeList[i] >= 5.0f)
            {
                fAttackTimeList.RemoveAt(i);
            }
        }

        if(fAttackTimeList.Count >= 1)
        {
            Electro.Play();
        }else
        {
            Electro.Stop();
        }

        Player.Ins.AddPctAttack(fAttackTimeList.Count * 0.1f);

    }
}
