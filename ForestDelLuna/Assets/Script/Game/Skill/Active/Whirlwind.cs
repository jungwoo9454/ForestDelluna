using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : Skill
{
    new Collider2D collider2D;
    //스킬은 처음 시전하였을때(키보드에서 누를시) 딱 1번 들어오는곳
    public override void SkillOn()
    {
        Player.Ins.nSwordAttack = 5;
    }
}
