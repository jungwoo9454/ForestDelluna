using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatKid : Enemy
{
    float MoveTime;
    public bool bSpawn;
    bool bCol;

    public override void Pattern()
    {
        MoveTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if(bUse && state == STATE.NORMAL)
        {
            if (MoveTime > 8)
                MoveTime = 0;

            if (MoveTime < 4)
            {
                RigidEnemy.velocity = Vector2.right * fMoveSpeed;
            }
            if (MoveTime > 4)
            {
                RigidEnemy.velocity = Vector2.left * fMoveSpeed;
            }

            if (Physics2D.OverlapCircle(transform.position, 1, GameMng.ins.GetPlayerMask().value) == null)
            {
                bCol = false;
            }
            else
            {
                if (bCol == false)
                {
                    bCol = true;
                    Player.Ins.TakeDmg(fAttack);
                }
            }
        }
    }

    public override void Dead()
    {
        if (fHp <= 0)
        {
            GameMng.ins.fEnemyTotalKill++;
            gameObject.SetActive(false);
            HealthBar.gameObject.SetActive(false);
            bUse = false;
            KillHeal();
            EffectMng.Ins.CreateParticle("Leave", transform.position);
            GameMng.ins.CreateGold(transform.position, Random.Range(10, 20));
            Player.Ins.AddExp(Random.Range(80, 110));

            if(bSpawn)
            {
                EntityMng.ins.CreateEntity_Skill(Player.Ins.transform.position - new Vector3(0.8f, 0, 0), GameMng.ins.SkillList[Random.Range(0, GameMng.ins.SkillList.Count)]);
            }
            else
            {
                if (Random.Range(0, 100) >= 90)
                    EntityMng.ins.CreateEntity_Skill(Player.Ins.transform.position - new Vector3(0.8f, 0, 0), GameMng.ins.SkillList[Random.Range(0, GameMng.ins.SkillList.Count)]);
            }
        }

    }
}
