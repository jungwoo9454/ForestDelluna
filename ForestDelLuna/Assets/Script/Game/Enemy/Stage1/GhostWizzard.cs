using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GhostWizzard : Enemy
{
    float AttackTime;
    float StateTime;

    float fPadTime;

    int nAttackState;
    public Transform Muzzle;
    public Transform FeetPos;

    public GameObject FirePad;
    public ParticleSystem MagicPad;
    public BossPad pad;
    public Text hpText;

    public AudioSource particlesound;

    public ParticleSystem DeadParticle;
    public ParticleSystem MagicBoard;

    bool bMagicBall;

    public override void Pattern()
    {
        AttackTime += Time.deltaTime;
        StateTime += Time.deltaTime;
        if (StateTime >= 2.2f)
        {
            StateTime = 0;

            if(Random.Range(0,10) > 3)
            {
                nState = 1;
            }
            else
            {
                nState = 0;
            }

            if (nState == 1)
            {
                nAttackState = Random.Range(0, 3);
                if (nAttackState == 0 && bMagicBall)
                {
                    nAttackState = Random.Range(1, 3);
                }

            }
        }


        if(MagicPad.isPlaying)
        {
            fPadTime += Time.deltaTime;
            if (fPadTime > 5.0f)
            {
                fPadTime = 0;
                SoundMng.Ins.Stop("FireWork");
                MagicPad.Stop();
                pad.IsActive = false;
            }
        }

        if (fHp <= 0)
        {
            FireEff.gameObject.SetActive(false);
            FireLight.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        hpText.text = "남은 체력 : " + fHp.ToString() + " / " + fMaxHp.ToString();
        switch (nState)
        {
        case 0:
            {
                Move();
            }
            break;
        case 1:
            {
                AttackState();
            }
            break;
        }
    }

    public override void Dead()
    {
        if (fHp <= 0)
        {
            AniEnemy.ResetTrigger("Idle");
            AniEnemy.ResetTrigger("Run");
            AniEnemy.ResetTrigger("StoneDown");
            AniEnemy.ResetTrigger("MagicBall");
            AniEnemy.ResetTrigger("Pad");
            AniEnemy.SetTrigger("Dead");
            RigidEnemy.velocity = Vector3.zero;
            UiMng.Ins.Clear();
            StartCoroutine(Deadparticle());
            DeadParticle.transform.parent = null;
            HealthBar.gameObject.SetActive(false);
            bUse = false;
            Debug.Log("AAAA");
        }
    }

    IEnumerator Deadparticle()
    {
        yield return new WaitForSeconds(1.5f);
        DeadParticle.Play();
        StartCoroutine(ParticleSodund(18, 0));
        GameMng.ins.Shake(3f, 0.2f);
    }

    IEnumerator ParticleSodund(int n, float time)
    {
        if(n > 0)
        {
            yield return new WaitForSeconds(time);
            particlesound.PlayOneShot(particlesound.clip);
            StartCoroutine(ParticleSodund(n - 1, Random.Range(0.1f, 0.2f)));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(FeetPos.position - new Vector3(fLength / 2, 0, 0), FeetPos.position + new Vector3(fLength / 2, 0, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(FeetPos.position - new Vector3(fAttackLength / 2, 0, 0), FeetPos.position + new Vector3(fAttackLength / 2, 0, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(LeftWay.position, fPhysicRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(RightWay.position, fPhysicRadius);
    }

    void Move()
    {
        WayInfo = Physics2D.Raycast(FeetPos.position - new Vector3(fLength * 0.5f, 0, 0), Vector2.right, fLength, PlayerLayer.value);

        if (WayInfo.collider != null && state == STATE.NORMAL)
        {
            if (CircleCol(LeftWay) && CircleCol(RightWay))
            {
                AniEnemy.ResetTrigger("Idle");
                AniEnemy.SetTrigger("Run");
            }
            else
                AniEnemy.ResetTrigger("Run");

            Vector2 heading = WayInfo.transform.position - transform.position;
            float dis = heading.magnitude;
            Vector2 dir = heading / dis;

            RigidEnemy.velocity = new Vector2((dir.x * fMoveSpeed), RigidEnemy.velocity.y);
        }
        else
        {
            if (state == STATE.NORMAL)
            {
                RigidEnemy.velocity = new Vector2(0, RigidEnemy.velocity.y);
                AniEnemy.ResetTrigger("Run");
                AniEnemy.SetTrigger("Idle");
            }
        }
    }

    void AttackState()
    {
        AttackInfo = Physics2D.Raycast(FeetPos.position - new Vector3(fAttackLength * 0.5f, 0, 0), Vector2.right, fAttackLength, PlayerLayer.value);
        if (AttackInfo.collider != null && state == STATE.NORMAL)
        {
            switch (nAttackState)
            {
                //마법 총알 뿌리는 패턴
                case 0:
                    {
                        if (fAttackSpeed <= AttackTime)
                        {
                            AttackTime = 0;
                            AniEnemy.ResetTrigger("Run");
                            AniEnemy.ResetTrigger("Idle");
                            AniEnemy.ResetTrigger("StoneDown");
                            AniEnemy.ResetTrigger("Pad");
                            AniEnemy.SetTrigger("MagicBall");
                        }

                        Vector2 heading = AttackInfo.transform.position - transform.position;
                        float dis = heading.magnitude;
                        Vector2 dir = heading / dis;

                        if (dir.x > 0.01f)
                        {
                            TransEnemy.localScale = new Vector3(1, 1, 1);
                            //RendereEnemy.flipX = false;
                            //if (AttackCollider != null)
                            //    AttackCollider.localScale = new Vector3(1, AttackCollider.localScale.y, AttackCollider.localScale.z);
                        }
                        else if (dir.x < -0.01f)
                        {
                            TransEnemy.localScale = new Vector3(-1, 1, 1);
                            //RendereEnemy.flipX = true;
                            //if (AttackCollider != null)
                            //    AttackCollider.localScale = new Vector3(-1, AttackCollider.localScale.y, AttackCollider.localScale.z);
                        }
                    }
                    break;

                //운석 떨어지는 패턴
                case 1:
                    {
                        if (fAttackSpeed <= AttackTime)
                        {
                            AttackTime = 0;
                            AniEnemy.ResetTrigger("Run");
                            AniEnemy.ResetTrigger("Idle");
                            AniEnemy.ResetTrigger("MagicBall");
                            AniEnemy.ResetTrigger("Pad");
                            AniEnemy.SetTrigger("StoneDown");
                        }
                    }
                    break;

                //장판 까는 패턴
                case 2:
                    {
                        if (fAttackSpeed <= AttackTime)
                        {
                            AttackTime = 0;
                            AniEnemy.ResetTrigger("Run");
                            AniEnemy.ResetTrigger("Idle");
                            AniEnemy.ResetTrigger("StoneDown");
                            AniEnemy.ResetTrigger("MagicBall");
                            AniEnemy.SetTrigger("Pad");
                        }
                    }
                    break;
            }
        }
    }

    public override void DisableFunc()
    {
        if(TransEnemy.localScale.x > 0)
        {
            HealthBar.transform.localScale = new Vector3(TransEnemy.localScale.x * 1f, 1f, 1);
        }
        else
        {
            HealthBar.transform.localScale = new Vector3(-TransEnemy.localScale.x * 1f, 1f, 1);
        }

        if(UiMng.Ins.ResultWindowObj.activeSelf)
        {
            HealthBar.gameObject.SetActive(false);
        }
    }

    public void MagicBallAttack()
    {
        bMagicBall = true;
        StartCoroutine(MagicBall(5));
    }

    public void StondDownAttack()
    {
        BulletMng.ins.CreateBullet("Stone", null, Player.Ins.transform.position + new Vector3(Random.Range(-2, 2), 15, 0), -90, 7, 0, 30, false, false, false);
        StartCoroutine(StoneCo());
    }

    IEnumerator StoneCo()
    {
        yield return new WaitForSeconds(1.4f);
        BulletMng.ins.CreateBullet("Stone", null, Player.Ins.transform.position + new Vector3(Random.Range(-2, 2), 15, 0), -90, 7, 0, 30, false, false, false);
    }

    public void PadCreate()
    {
        fPadTime = 0;
        if(!SoundMng.Ins.IsPlay("FireWork"))
        {
            SoundMng.Ins.Play("FireWork");
        }
        FirePad.transform.position = new Vector3(Player.Ins.transform.position.x, FirePad.transform.position.y, 0);
        MagicPad.Play();
        pad.IsActive = true;
    }

    IEnumerator MagicBall(int n)
    {
        yield return new WaitForSeconds(1f);

        if (n > 0)
        {
            SoundMng.Ins.Play("MagicBall");
            BulletMng.ins.CreateBullet("MagicBall", MagicBallDead, Muzzle.position, 0, 4, Random.Range(1.5f,2.5f), 12, false, true, false);
            StartCoroutine(MagicBall(n - 1));
        }else
        {
            bMagicBall = false;
        }
    }

    void MagicBallDead(Collider2D col)
    {
        EffectMng.Ins.CreateParticle("MagicBallHit", Player.Ins.transform.position);
    }

    public void MagicBoardPlay()
    {
        MagicBoard.Play();
    }
}
