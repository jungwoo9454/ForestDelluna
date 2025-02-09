using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GwangBun : Skill
{
    public ParticleSystem Electro;
    public override void SkillOn()
    {
        float nConsumeHp = Player.Ins.GetMaxHp() - Player.Ins.GetHp();
        float AddSpeed = (nConsumeHp / (Player.Ins.GetMaxHp() * 0.1f));
        float AttackSpeedPercent = ((AddSpeed + 1.0f) * 5.0f) * 0.01f - 0.05f;

        if (Player.Ins.GetMaxHp() == Player.Ins.GetHp())
        {
            AttackSpeedPercent = 0;
        }

        if (AttackSpeedPercent != 0)
            Electro.Play();
        else
            Electro.Stop();

        Player.Ins.AddPctSpeed(AttackSpeedPercent);
    }
}