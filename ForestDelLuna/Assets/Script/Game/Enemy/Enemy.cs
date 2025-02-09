using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

public enum STATE { NORMAL,KNOCKBACK,BLACKOUT}

public class Enemy : MonoBehaviour
{
    //몬스터를 조작하기 위해 기본적으로 넣어둔 변수들
    [Header("EnemyInfo File Only")]
    public EnemyInfo info;              //프로젝트에서 만든 몬스터 정보

    [Header("Only Layer Ground,Player")]
    public LayerMask WhatIsGround;
    public LayerMask PlayerLayer;

    [Header("Physics Value")]
    public Transform LeftWay;
    public Transform RightWay;
    public Transform AttackCollider;
    public float fAttackLength;
    public float fLength;
    public float fPhysicRadius;

    [Header("Health Power bar")]
    public Slider HealthBar;

    [Header("Enemy Status - no Setting")]
    public float fMoveSpeed;
    public float fMaxHp;
    public float fAttack;
    public float fAttackSpeed;
    public float fHp;

    [Header("Enemy State")]
    public STATE state;
    public ParticleSystem FireEff;
    public FireLighting FireLight;

    //protected float Rot;
    protected int nState;
    protected Transform TransEnemy;
    protected Rigidbody2D RigidEnemy;
    protected SpriteRenderer RendereEnemy;
    protected Animator AniEnemy;
    protected RaycastHit2D WayInfo;
    protected RaycastHit2D AttackInfo;

    protected bool bIsObstacle;
    protected bool bUse = true;

    public float fTotalBurnTime;
    private float fBurnTime;
    public bool bBurn;



    void Start()
    {
        TransEnemy = GetComponent<Transform>();
        RigidEnemy = GetComponent<Rigidbody2D>();
        RendereEnemy = GetComponent<SpriteRenderer>();
        AniEnemy = GetComponent<Animator>();

        fMoveSpeed = info.fMoveSpeed;
        fMaxHp = info.fMaxHp;
        fHp = info.fMaxHp;
        fAttack = info.fAttack;
        fAttackSpeed = info.fAttackSpeed;
        bUse = true;
    }
    
    void Update()
    {
        if(bUse)
        {
            Mathf.Clamp(fHp, 0, fMaxHp);

            bIsObstacle = Physics2D.OverlapCircle(LeftWay.position, fPhysicRadius, WhatIsGround) && 
                Physics2D.OverlapCircle(RightWay.position, fPhysicRadius, WhatIsGround);
            WayInfo = Physics2D.Raycast(transform.position - new Vector3(fLength * 0.5f, 0, 0), Vector2.right, fLength, PlayerLayer.value);
            AttackInfo = Physics2D.Raycast(transform.position - new Vector3(fAttackLength * 0.5f, 0, 0), transform.right, fAttackLength, PlayerLayer.value);

            if (state != STATE.BLACKOUT)
            {
                Pattern();
                EnemyMoveRot();
            }
            else if (state == STATE.BLACKOUT)
            {
                AniEnemy.ResetTrigger("Attack");
                AniEnemy.ResetTrigger("Run");
                AniEnemy.SetTrigger("Idle");
                RigidEnemy.velocity = Vector2.zero;
            }

            Burn();
            ScrollCount();
            Dead();
        }
        DisableFunc();
    }

    void Burn()
    {
        if (bBurn)
        {
            if (!FireEff.isPlaying)
            {
                FireEff.Play();
                FireLight.SetActive(true);
            }
            fTotalBurnTime += Time.deltaTime;
            if (fTotalBurnTime <= 3f)
            {
                fBurnTime += Time.deltaTime;
                if (fBurnTime >= 1f)
                {
                    TakeDmg(5);
                    fBurnTime = 0;
                }
            }
            else
            {
                fTotalBurnTime = 0;
                bBurn = false;
            }
        }
        else
        {
            if (FireEff.isPlaying)
            {
                FireLight.SetActive(false);
                FireEff.Stop();
            }
        }
    }

    public virtual void EnemyMoveRot()
    {
        if (RigidEnemy.velocity.x > 0.01f)
        {
            TransEnemy.localScale = new Vector3(1, 1, 1);
            HealthBar.transform.localScale = new Vector3(TransEnemy.localScale.x * 0.1f, 0.1f, 1);
        }
        else if (RigidEnemy.velocity.x < -0.01f)
        {
            TransEnemy.localScale = new Vector3(-1, 1, 1);
            HealthBar.transform.localScale = new Vector3(TransEnemy.localScale.x * 0.1f, 0.1f, 1);
        }
    }

    protected bool CircleCol(Transform tr)
    {
        return Physics2D.OverlapCircle(tr.position, fPhysicRadius, WhatIsGround);
    }

    public void TakeDmg(float dmg)
    {
        if(bUse)
        {
            SoundMng.Ins.Play("Hit");
            GameMng.ins.fEnemyTotalDmg += dmg;
            fHp -= dmg;
            HitAni();
            UiMng.Ins.CreatetextEffect(transform.position, UiMng.Ins.GetDmgColor(dmg), dmg, null);
        }
    }

    public void KnockBack(Vector2 size, float duration)
    {
        state = STATE.KNOCKBACK;
        RigidEnemy.velocity = Vector2.zero;
        RigidEnemy.AddForce(size, ForceMode2D.Force);
        StartCoroutine(KnockCo(duration));
    }

    IEnumerator KnockCo(float dur)
    {
        yield return new WaitForSeconds(dur);
        state = STATE.NORMAL;
    }

    public void BlackOut(float duration)
    {
        state = STATE.BLACKOUT;
        RigidEnemy.velocity = Vector2.zero;
        StartCoroutine(BlackOutCo(duration));
    }

    IEnumerator BlackOutCo(float dur)
    {
        yield return new WaitForSeconds(dur);
        state = STATE.NORMAL;
    }


    void HitAni()
    {
        if(bUse)
        {
            RendereEnemy.color = GameMng.ins.EnemyHitColor;
            StartCoroutine(EnemyHitColor());
        }
    }

    public void KillHeal()
    {
        if (Player.Ins.bEnemyKillHeal)
            Player.Ins.HpHeal(3);
    }

    void ScrollCount()
    {
        HealthBar.value = fHp / fMaxHp;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position - new Vector3(fLength / 2, 0, 0), transform.position + new Vector3(fLength / 2, 0, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - new Vector3(fAttackLength / 2, 0, 0), transform.position + new Vector3(fAttackLength / 2, 0, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(LeftWay.position, fPhysicRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(RightWay.position, fPhysicRadius);
    }

    IEnumerator EnemyHitColor()
    {
        yield return new WaitForSeconds(0.15f);
        RendereEnemy.color = Color.white;
    }


    public virtual void Pattern()
    {

    }

    public virtual void Dead()
    {

    }

    public virtual void DisableFunc()
    {

    }
}
