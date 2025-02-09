using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelWorrier : Enemy
{
    float AttackTime;
    bool bRespown = true;
    bool bMove = true;

    public void RespawnEff()
    {
        EffectMng.Ins.CreateParticle("Respawn", transform.position);
    }

    //Pattern함수는 이 몬스터가 살아있는동안 매 시간 호출이 됨
    public override void Pattern()
    {
        AttackTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (WayInfo.collider != null && bMove && bUse && state == STATE.NORMAL)
        {

            AniEnemy.ResetTrigger("Idle");
            AniEnemy.SetTrigger("Run");

            ///
            /// 방향벡터 구하여 그 방향에 따라 이동하는 공식임
            /// URL참고 : https://docs.unity3d.com/kr/530/Manual/DirectionDistanceFromOneObjectToAnother.html
            ///
            if (fHp > 0 || bRespown)
            {
                Vector2 heading = WayInfo.transform.position - transform.position;
                float dis = heading.magnitude;
                Vector2 dir = heading / dis;
                RigidEnemy.velocity = new Vector2(dir.x * fMoveSpeed, RigidEnemy.velocity.y);
            }

            if (!bIsObstacle)
            {
                RigidEnemy.velocity = new Vector2(0, RigidEnemy.velocity.y);
            }

            //현재 레이저를 쐈을 떄 충돌체의 정보가 있다면(충돌 했을 떄)
            //HitInfo는 Fixed에서 처리 바람
            if (AttackInfo.collider != null)
            {
                if (fAttackSpeed <= AttackTime)
                {
                    AttackTime = 0;
                    AniEnemy.ResetTrigger("Run");
                    AniEnemy.ResetTrigger("Idle");
                    AniEnemy.SetTrigger("Attack");
                   
                }
            }
        }
        else
        {
            RigidEnemy.velocity = new Vector2(0, RigidEnemy.velocity.y);
            AniEnemy.ResetTrigger("Run");
            AniEnemy.SetTrigger("Idle");
        }

        if (GameMng.ins.GetAniState(AniEnemy, "Attack"))
        {
            RigidEnemy.velocity = new Vector2(0, RigidEnemy.velocity.y);
        }
    }

    public override void Dead()
    {
        if (fHp <= 0)
        {
            if (bRespown)
            {
                if (!GameMng.ins.GetAniState(AniEnemy, "Respown"))
                {
                    bMove = false;
                    AniEnemy.ResetTrigger("Run");
                    AniEnemy.ResetTrigger("Idle");
                    AniEnemy.ResetTrigger("Attack");
                    AniEnemy.SetTrigger("Respown");
                }
            }
            else
            {
                AniEnemy.ResetTrigger("Run");
                AniEnemy.ResetTrigger("Idle");
                AniEnemy.ResetTrigger("Attack");
                AniEnemy.ResetTrigger("Respown");
                AniEnemy.SetTrigger("Dead");
                KillHeal();
                bUse = false;
                gameObject.SetActive(false);
                GameMng.ins.CreateGold(transform.position, Random.Range(30, 40));
                EffectMng.Ins.CreateParticle("Leave", transform.position);
                GameMng.ins.fEnemyTotalKill++;
                Player.Ins.AddExp(Random.Range(60, 100));
                if (Random.Range(0,100) > 95)
                    EntityMng.ins.CreateEntity_Skill(Player.Ins.transform.position - new Vector3(0.8f, 0, 0), GameMng.ins.SkillList[Random.Range(0, GameMng.ins.SkillList.Count)]);
            }
        }
    }

    void HpFull()
    {
        fHp = fMaxHp;
        bRespown = false;
        bMove = true;
    }

    public void SetFalse()
    {
        gameObject.SetActive(false);
    }

    public void AttackSnd()
    {
        SoundMng.Ins.Play("Knife");
    }
}
