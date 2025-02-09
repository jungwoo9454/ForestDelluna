using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverDrive : Skill
{
    public override void SkillOn()
    {
        Player.Ins.PlayerAni.Play("OverDrive");
    }
}
