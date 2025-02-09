using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelKnight : Enemy
{
    public Transform SKelDdaGgari;
    float AttackTime;
    bool bMove = true;

    //Pattern함수는 이 몬스터가 살아있는동안 매 시간 호출이 됨
    void onAwake()
    {
        SKelDdaGgari = transform.GetChild(0).transform;
    }
    public override void Pattern()
    {
        AttackTime += Time.deltaTime;
    }

    void FixedUpdate()
    {

        if (Physics2D.OverlapCircle(transform.position, 8, GameMng.ins.PlayerMask.value) != null && bMove && bUse && state == STATE.NORMAL)
        {
            AniEnemy.ResetTrigger("Idle");
            AniEnemy.SetTrigger("Run");

            ///
            /// 방향벡터 구하여 그 방향에 따라 이동하는 공식임
            /// URL참고 : https://docs.unity3d.com/kr/530/Manual/DirectionDistanceFromOneObjectToAnother.html
            ///\
            if (fHp > 0)
            {
                Vector2 heading = Player.Ins.transform.position - transform.position;
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
            if (Physics2D.OverlapCircle(transform.position, 8, GameMng.ins.PlayerMask.value) != null)
            {
                if (fAttackSpeed <= AttackTime)
                {
                    AttackTime = 0;
                    AniEnemy.ResetTrigger("Run");
                    AniEnemy.ResetTrigger("Idle");
                    AniEnemy.SetTrigger("Attack");

                }

                if (GameMng.ins.GetAniState(AniEnemy, "Attack"))
                    RigidEnemy.velocity = new Vector2(0, RigidEnemy.velocity.y);
            }
        }
        else if(state != STATE.KNOCKBACK || state != STATE.BLACKOUT)
        {
            RigidEnemy.velocity = new Vector2(0, RigidEnemy.velocity.y);
            AniEnemy.ResetTrigger("Run");
            AniEnemy.SetTrigger("Idle");
        }
    }

    public override void Dead()
    {
        if (fHp <= 0)
        {
            KillHeal();
            bUse = false;
            HealthBar.gameObject.SetActive(false);
            SKelDdaGgari.parent = null;
            gameObject.SetActive(false);
            EffectMng.Ins.CreateParticle("Leave", transform.position);
            GameMng.ins.CreateGold(transform.position, Random.Range(200, 500));
            Player.Ins.AddExp(Random.Range(400, 650));
            GameMng.ins.fEnemyTotalKill++;
            SKelDDaRespawn();
            if (Random.Range(0, 100) >= 90)
                EntityMng.ins.CreateEntity_Skill(Player.Ins.transform.position - new Vector3(0.8f, 0, 0), GameMng.ins.SkillList[Random.Range(0, GameMng.ins.SkillList.Count)]);
        }
    }

    void SKelDDaRespawn()
    {
        SKelDdaGgari.gameObject.SetActive(true);
    }

    public void SpearSnd()
    {
        SoundMng.Ins.Play("Spear");
    }
}
