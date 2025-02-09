using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatKing : Enemy
{
    Collider2D hit;
    public Transform Muzzle;
    float AttackTime;
    float FireTime;
    float AniItme;


    //Pattern함수는 이 몬스터가 살아있는동안 매 시간 호출이 됨
    public override void Pattern()
    {
        AttackTime += Time.deltaTime;
    }

    void FixedUpdate()
    {

        hit = Physics2D.OverlapCircle(transform.position, 7f);
        if (hit != null && hit.gameObject.name == "Player" && state == STATE.NORMAL)
        {
            Vector2 heading = hit.transform.position - transform.position;
            float dis = heading.magnitude;
            Vector2 dir = heading / dis;


            if (dir.x > 0)
            {
                TransEnemy.localScale = new Vector3(-1, 1, 1);
                HealthBar.transform.localScale = new Vector3(TransEnemy.localScale.x * 0.08f, 0.08f, 1);
            }
            else
            {
                TransEnemy.localScale = new Vector3(1, 1, 1);
                HealthBar.transform.localScale = new Vector3(TransEnemy.localScale.x * 0.08f, 0.08f, 1);
            }

            FireBuller();
        }
        
       
    }

    public override void Dead()
    {
        if (fHp <= 0)
        {
            KillHeal();
            EffectMng.Ins.CreateParticle("Leave", transform.position);
            GameMng.ins.CreateGold(transform.position, Random.Range(50, 80));
            Player.Ins.AddExp(Random.Range(100, 160));

            if (Random.Range(0, 100) >= 90)
                EntityMng.ins.CreateEntity_Skill(Player.Ins.transform.position - new Vector3(0.8f, 0, 0), GameMng.ins.SkillList[Random.Range(0, GameMng.ins.SkillList.Count)]);
            gameObject.SetActive(false);
            bUse = false;
            GameMng.ins.fEnemyTotalKill++;
        }

    }
    void FireBuller()
    {
         FireTime += Time.deltaTime;
        AniItme += Time.deltaTime;
        if (FireTime > 2.5)
        {
            AniEnemy.ResetTrigger("Idle");
            AniEnemy.SetTrigger("Attack");
            AniItme = 0;
        } 
    }

    void BulletFire()
    {
        Vector2 heading = transform.position - Player.Ins.transform.position;
        float dis = heading.magnitude;
        Vector2 dir = heading / dis;
        FireTime = 0;
        BulletMng.ins.CreateBullet("Wave", null, Muzzle.position, Mathf.Atan2(transform.position.x - Player.Ins.transform.position.x, Player.Ins.transform.position.y - transform.position.y) * 180 / Mathf.PI + 90, 6, 5, 20, false, false, false);
    }

    void ChangeAni()
    {
        AniEnemy.SetTrigger("Idle");
        AniEnemy.ResetTrigger("Attack");
    }

    public void SetFalse()
    {
        gameObject.SetActive(false);
    }

    public void BatSnd()
    {
        SoundMng.Ins.Play("Bat");
    }
}
