using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    float AttackTime;
    float WaitTime;
    float BoomTime;
    
    bool IsBehind = false;
    bool BackOk = false;

    //Pattern함수는 이 몬스터가 살아있는동안 매 시간 호출이 됨
    public override void Pattern()
    {
        AttackTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (WayInfo.collider != null && bUse && state == STATE.NORMAL)
        {
            GoBehind();


            if (IsBehind == true)
            {
                WaitTime += Time.deltaTime;
            }

            if (IsBehind == true && WaitTime >= 3)

            {
                BackOk = true;
                if (WayInfo.collider != null)
                {


                    BoomTime += Time.deltaTime;
                    AniEnemy.ResetTrigger("Idle");
                    AniEnemy.SetTrigger("Run");

                    ///
                    /// 방향벡터 구하여 그 방향에 따라 이동하는 공식임
                    /// URL참고 : https://docs.unity3d.com/kr/530/Manual/DirectionDistanceFromOneObjectToAnother.html
                    ///\
                    if (fHp > 0)
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
                    //HitInfo는 Fixed에서 처리 바람'

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
                if(BoomTime>=3&&BackOk)
                {
                    if(!GameMng.ins.GetAniState(AniEnemy,"Boom"))
                    {
                        BackOk = false;

                        AniEnemy.ResetTrigger("Attack");
                        AniEnemy.ResetTrigger("Run");
                        AniEnemy.ResetTrigger("Idle");
                        AniEnemy.SetTrigger("Boom");
                    }
                    
                }
            }
        }

    }

    public override void EnemyMoveRot()
    {
        if (RigidEnemy.velocity.x > 0.01f)
        {
            TransEnemy.localScale = new Vector3(-1, 1, 1);
            HealthBar.transform.localScale = new Vector3(TransEnemy.localScale.x * 0.1f, 0.1f, 1);
        }
        else if (RigidEnemy.velocity.x < -0.01f)
        {
            TransEnemy.localScale = new Vector3(1, 1, 1);
            HealthBar.transform.localScale = new Vector3(TransEnemy.localScale.x * 0.1f, 0.1f, 1);
        }
    }

    public override void Dead()
    {
        if (fHp <= 0)
        {
            GameMng.ins.fEnemyTotalKill++;
            KillHeal();
            bUse = false;
            HealthBar.gameObject.SetActive(false);
            gameObject.SetActive(false);
            EffectMng.Ins.CreateParticle("Smoke", transform.position);
            GameMng.ins.CreateGold(transform.position, Random.Range(50, 100));
            Player.Ins.AddExp(Random.Range(100, 150));

            if (Random.Range(0, 100) >= 90)
                EntityMng.ins.CreateEntity_Skill(Player.Ins.transform.position - new Vector3(0.8f, 0, 0), GameMng.ins.SkillList[Random.Range(0, GameMng.ins.SkillList.Count)]);
        }

    }

    void GoBehind()
    {
        if (IsBehind == false)
        {
            GameObject go = GameObject.Find("BackCenser");
            transform.position = go.transform.position;
            RigidEnemy.velocity = new Vector2(0, RigidEnemy.velocity.y);
            IsBehind = true;

        }
    }

    public void Boom()
    {
        bUse = false;
        if (Physics2D.OverlapCircle(transform.position, 3.5f, GameMng.ins.PlayerMask.value))
        {
            Player.Ins.TakeDmg(10);
            GameMng.ins.Shake(0.3f, 1f);
        }
        EffectMng.Ins.CreateParticle("boom", transform.position);
        HealthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void BoomSnd()
    {
        SoundMng.Ins.Play("Boom");
    }
}

