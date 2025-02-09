using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<string> HitEnemyList = new List<string>();
    [HideInInspector] public Transform PlayerTrans;

    [Header("Player Weapon")]
    public Transform Muzzle;
    public Collider2D CrashCol;
    public Collider2D HighCol;

    [Header("Player Component")]
    public Animator PlayerAni;
    public Rigidbody2D PlayerRigid;
    public SpriteRenderer PlayerRendere;
    public PlayerSkill playerSkill;

    [Header("Player HitColor")]
    public Color HitBeforeColor;

    [Header("Player Physics")]
    public Transform feetpos;
    public LayerMask WhatIsGround;
    public float fCheckRadius;
    public float fJumpPower;
    public bool bIsOnGround = false;

    [Header("Player Movement")]
    public bool bVelocityZero;
    bool bDontMove;

    [Header("Player Normal Hp")]
    public float fNormalHp = 200.0f;
    float fNormalSpeed = 4.5f;
    float fNormalAttack = 15.0f;
    float fNormalCoolDown = 1.0f;
    float fNormalDeffensive = 10.0f;

    [Header("Player Special Skill")]
    public bool bAttackBurn;
    public bool bEnemyKillHeal;
    public bool bGod;
    public bool bCheatGod;
    public int nSwordAttack;

    [Header("Effect")]
    public bool bBurst;
    public ParticleSystem Burst;
    public ParticleSystem LevelUpEff;

    /*
     * 스피드      플레이어의 이동속도
     * 공격력      플레이어의 기본무기(평타)공격력
     * 최대 체력   플레이어의 최대 체력
     * 방어력      받은 피해에서 방어력의 40&만큼의 피해를 방어한다.
     * 공격속도    공격 시전시간이 빨라진다.(최대 2배)
     * 회피        공격 받을시 일정 확률로 데미지를 안 받는다.(0~100%, 최대 80%)
     */


    float fMaxHp;

    //플레이어 현재 능력치
    float fSpeed;                   //스피드
    float fAttack;                  //공격력
    float fHp = 200.0f;             //현재 체력
    float fDeffensive;              //방어력
    float fCollDown;                //재사용 대시시간 감소량
    float fEvasion;                 //회피

    //플레이어 추가 능력치
    float fAddSpeed;                //스피드
    float fAddAttack;               //공격력
    float fAddMaxHp;                //최대체력
    float fAddDeffensive;           //방어력
    float fAddSkillCoolDown;        //재사용 대시시간 감소량
    float fAddEvasion;              //회피

    //플레이어 추가 능력치 %
    float fPctSpeed = 1.0f;         //스피드
    float fPctAttack = 1.0f;        //공격력
    float fPctMaxHp = 1.0f;         //최대체력
    float fPctDeffensive = 1.0f;    //방어력
    float fPctEvasion = 1.0f;       //회피

    bool bDownVel;
    int nGold;                      //플레이어 소유중인 골드
    int nLevel;
    float fExp;                     //플레이어 경험치

    private static Player PlayerIns;
    public static Player Ins
    {
        get
        {
            if (PlayerIns == null)
            {
                PlayerIns = FindObjectOfType<Player>();
                if (PlayerIns == null)
                {
                    GameObject PlayerObj = new GameObject();
                    PlayerObj.name = "Player";
                    PlayerIns = PlayerObj.AddComponent<Player>();
                }
            }
            return PlayerIns;
        }
    }

    void Start()
    {
        PlayerTrans = GetComponent<Transform>();
        PlayerRigid = GetComponent<Rigidbody2D>();
        PlayerRendere = GetComponent<SpriteRenderer>();
        PlayerAni = GetComponent<Animator>();
        playerSkill = GetComponent<PlayerSkill>();
    }

    void Update()
    {
        if (bCheatGod)
            bGod = true;

        StatAllReset();
        StatUpdate();
        Attack();
        SwordAttack();

        LevelUp();

        Result();

        PlayerMove();
        Jump();
        Roll();

        fHp = Mathf.Clamp(fHp, 0, fMaxHp);

        if (bIsOnGround)
        {
            PlayerAni.ResetTrigger("Down");
        }
    }

    void FixedUpdate()
    {
        bIsOnGround = Physics2D.OverlapCircle(feetpos.position, fCheckRadius, WhatIsGround);

        if(PlayerRigid.velocity.y <= -8)
        {
            bDownVel = true;
        }

        if (bDontMove && !GameMng.ins.GetAniState(PlayerAni, "Roll") && !bVelocityZero)
            PlayerRigid.velocity = new Vector2(0, PlayerRigid.velocity.y);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(feetpos.position, fCheckRadius);
    //}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("EnemyAttack"))
        {
            TakeDmg(collider.GetComponentInParent<Enemy>().fAttack);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("BackGround"))
        {
            if (bBurst)
            {
                SoundMng.Ins.Play("Down");
                bBurst = false;
                Burst.Play();
                GameMng.ins.ShakeCoru(1f, 0.3f);
            }

            if(bDownVel)
            {
                bDownVel = false;
                SoundMng.Ins.Play("Down");
            }
        }
    }

    void PlayerMove()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        PlayerAni.SetBool("isMove", Mathf.Abs(hor) >= 0.1f && bIsOnGround);

        if(!bDontMove && !GetAS("Roll"))
        {
            PlayerRigid.velocity = new Vector2(hor * fSpeed, PlayerRigid.velocity.y);

            if ((hor > 0 || hor < 0) && bIsOnGround && GameMng.ins.GetAniState(PlayerAni, "Run")) 
            {
                if (!SoundMng.Ins.IsPlay("Walk"))
                    SoundMng.Ins.Play("Walk");
            }
        }

        if(!GameMng.ins.GetAniState(PlayerAni,"Attack"))
        {
            if (hor > 0)
            {
                PlayerTrans.localScale = Vector3.one;
                Muzzle.localScale = Vector3.one;
            }
            else if (hor < 0)
            {
                PlayerTrans.localScale = new Vector3(-1, 1, 1);
                Muzzle.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    void Jump()
    {
        PlayerAni.SetBool("isGround", bIsOnGround);
        if (Input.GetKeyDown(KeyCode.Space) && !bDontMove && bIsOnGround && GetAS("Roll") == false)
        {
            PlayerRigid.AddForce(Vector3.up * fJumpPower, ForceMode2D.Impulse);
            PlayerAni.SetTrigger("Jump");
        }

        if (PlayerRigid.velocity.y <= 0 && bIsOnGround == false)
        {
            PlayerAni.ResetTrigger("Jump");
            PlayerAni.SetTrigger("Down");
        }
    }

    void Roll()
    {
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(1)) && !bDontMove)
        {
            if (GameMng.ins.GetAniState(PlayerAni,"Idle") || GameMng.ins.GetAniState(PlayerAni, "Run"))
            {
                PlayerAni.SetTrigger("Roll");

                SoundMng.Ins.Play("Roll");
                if (PlayerTrans.localScale.x > 0)
                    PlayerRigid.velocity = new Vector2(10, PlayerRigid.velocity.y);
                else
                    PlayerRigid.velocity = new Vector2(-10, PlayerRigid.velocity.y);
            }
        }
    }

    void StatUpdate()
    {
        fMaxHp = (fAddMaxHp + fNormalHp) * fPctMaxHp;
        fSpeed = (fAddSpeed + fNormalSpeed) * fPctSpeed;
        fAttack = (fAddAttack + fNormalAttack) * fPctAttack;
        fCollDown = (fAddSkillCoolDown + fNormalCoolDown);
        fDeffensive = (fAddDeffensive + fNormalDeffensive) * fPctDeffensive;
        fEvasion = fAddEvasion * fPctEvasion;

        if(fEvasion > 80)
            fEvasion = 80;
    }

    void StatAllReset()
    {
        fAddSpeed = 0;
        fAddAttack = 0;
        fAddDeffensive = 0;
        fAddSkillCoolDown = 0;
        fAddEvasion = 0;
        fAddMaxHp = 0;

        fPctSpeed = 1.0f;
        fPctAttack = 1.0f;     
        fPctMaxHp = 1.0f;         
        fPctDeffensive = 1.0f; 
        fPctEvasion = 1.0f;    
        fPctMaxHp = 1.0f;
    }

    void LevelUp()
    {
        //수정(Player.Ins.GetLevel() - 1)
        if (GameMng.ins.PlayerLevelMax[nLevel] <= fExp && nLevel < 9)
        {
            AddLevel();
        }
    }

    void HitAni()
    {
        SoundMng.Ins.Play("PlayerHit1");
        PlayerRendere.color = HitBeforeColor;
        GameMng.ins.Shake(0.1f, 0.07f);
        StartCoroutine(HitColor());
    }

    IEnumerator HitColor()
    {
        yield return new WaitForSeconds(0.15f);
        PlayerRendere.color = Color.white;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameMng.ins.GetAniState(PlayerAni, "Run") || GameMng.ins.GetAniState(PlayerAni, "Idle") ||
                GameMng.ins.GetAniState(PlayerAni, "Jump") ||
                GameMng.ins.GetAniState(PlayerAni, "Down"))
            {
                PlayerAni.ResetTrigger("Jump");
                PlayerAni.ResetTrigger("Down");
                PlayerAni.SetTrigger("Attack");
            }
        }
    }

    public void Result()
    {
        if (fHp <= 0 && !GameMng.ins.GetAniState(PlayerAni, "Dead"))
        {
            PlayerAni.SetBool("Dead", true);
            SetMove(true, true);
        }
    }

    //애니메이션 이벤트 함수
    public void ResultWindowOpen()
    {
        UiMng.Ins.ResultWindow();
    }

    public void SetMove(bool m,bool reset)
    {
        bDontMove = m;
        if (reset)
            ZeroVel();
    }

    public void ZeroVel()
    {
        PlayerRigid.velocity = Vector2.zero;
    }
    //애니메이션 이벤트 함수
    public void SwordAudioPlay()
    {
        SoundMng.Ins.Play("Sword");
    }
    //애니메이션 이벤트 함수
    public void EnemyHitReset()
    {
        HitEnemyList.Clear();
    }
    //애니메이션 이벤트 함수
    public void moveStop1()
    {
        bDontMove = true;
    }
    //애니메이션 이벤트 함수
    public void moveStart()
    {
        bDontMove = false;
    }

    public void Swords()
    {
        if (transform.localScale.x < 0)
            BulletMng.ins.CreateBullet("Swords", null, Muzzle.position, 180, 15, 5, GetAttack() * 1.2f, true, false, false);
        else
            BulletMng.ins.CreateBullet("Swords", null, Muzzle.position, 0, 15, 5, GetAttack() * 1.2f, true, false, false);

        SoundMng.Ins.Play("Swords");
    }

    public void KnockBackSword()
    {
        if (transform.localScale.x < 0)
            BulletMng.ins.CreateBullet("Swords", KnockbackS, Muzzle.position, 180, 15, 5, GetAttack() * 1.2f, true, false, true);
        else
            BulletMng.ins.CreateBullet("Swords", KnockbackS, Muzzle.position, 0, 15, 5, GetAttack() * 1.2f, true, false, true);
        SoundMng.Ins.Play("Swords");
    }

    void KnockbackS(Collider2D col)
    {
        Vector2 heading = col.transform.position - transform.position;
        float dis = heading.magnitude;
        Vector2 dir = heading / dis;
        col.GetComponent<Enemy>().KnockBack(new Vector2(dir.x * 5, 10), 2);
    }

    public void SwordAttack()
    {
        if (nSwordAttack > 0 && Input.GetMouseButtonDown(0))
        {
            nSwordAttack--;
            if (transform.localScale.x < 0)
                BulletMng.ins.CreateBullet("Swords", null, Muzzle.position, 180, 15, 5, GetAttack() * 1.2f, true, false, false);
            else
                BulletMng.ins.CreateBullet("Swords", null, Muzzle.position, 0, 15, 5, GetAttack() * 1.2f, true, false, false);

            SoundMng.Ins.Play("Swords");
        }
    }

    public void AddGold(int n)
    {
        nGold += n;
        UiMng.Ins.GoldAddAni();
        UiMng.Ins.CreatetextEffect(transform.position, GameMng.ins.GoldColor, n, n.ToString() + "G+");
    }


    IEnumerator LevelUpWindow()
    {
        yield return new WaitForSeconds(2.0f);
        GameMng.ins.LevelUP();

        PlayerAni.ResetTrigger("Attack");
        PlayerAni.ResetTrigger("Jump");
        PlayerAni.ResetTrigger("Down");
        PlayerAni.SetTrigger("Idle");
    }
    public void AddLevel()
    {
        nLevel++;
        fExp = 0;
        StartCoroutine(LevelUpWindow());
        SoundMng.Ins.Play("LevelUp");
        LevelUpEff.Play();
        UiMng.Ins.CreatetextEffect(transform.position, Color.yellow, 0, "LEVEL UP!");

    }
    public void AddExp(int n) { fExp += n; }
    public void AddSpeed(float n) { fAddSpeed += n; StatUpdate(); }
    public void AddAttack(float n) { fAddAttack += n; StatUpdate(); }
    public void AddDeffensive(float n) { fAddDeffensive += n; StatUpdate(); }
    public void AddSkillCoolDown(float n) { fAddSkillCoolDown += n; StatUpdate(); }
    public void AddEvasion(float n) { fAddEvasion += n; StatUpdate(); }
    public void AddHp(float n) { fAddMaxHp += n; StatUpdate(); }
    public void AddPctSpeed(float n) { fPctSpeed += n; StatUpdate(); }
    public void AddPctAttack(float n) { fPctAttack += n; StatUpdate(); }
    public void AddPctDeffensive(float n) { fPctDeffensive += n; StatUpdate(); }
    public void AddPctEvasion(float n) { fPctEvasion += n; StatUpdate(); }
    public void AddPctHp(float n)
    {
        float hp = fMaxHp;
        fPctMaxHp += n;
        StatUpdate();
    }


    /////////////////////////////////////////////////////////////////////////////

    public void MinSpeed(float n) { fAddSpeed -= n; StatUpdate(); }
    public void MinAttack(float n) { fAddAttack -= n; StatUpdate(); }
    public void MinDeffensive(float n) { fAddDeffensive -= n; StatUpdate(); }
    public void MinAttackSpeed(float n) { fAddSkillCoolDown -= n; StatUpdate(); }
    public void MinEvasion(float n) { fAddEvasion -= n; StatUpdate(); }
    public void MinHp(float n)
    {
        fAddMaxHp -= n;
        StatUpdate();

        if (fHp > fMaxHp)
            fHp = fMaxHp;
    }
                
    public void MinPctSpeed(float n) { fPctSpeed -= n; StatUpdate(); }
    public void MinPctAttack(float n) { fPctAttack -= n; StatUpdate(); }
    public void MinPctDeffensive(float n) { fPctDeffensive -= n; StatUpdate(); }
    public void MinPctEvasion(float n) { fPctEvasion -= n; StatUpdate(); }
    public void MinPctHp(float n) { fPctMaxHp -= n; StatUpdate(); }


    public void TakeDmg(float n)
    {
        if ((fEvasion <= Random.Range(0, 100)) && !bGod)
        {
            n -= GetDeffensive() * 0.4f;
            GameMng.ins.Shake(0.1f, 0.75f);
            n = Mathf.Clamp(n, 0, 100000);
            UiMng.Ins.CreatetextEffect(transform.position, Color.red, n, ((int)n).ToString());

            fHp -= n;
            HitAni();
        }
        else
        {
            UiMng.Ins.CreatetextEffect(transform.position, Color.cyan, 0, "회피");
        }
    }
    public void HpHeal(float n) { fHp += n; UiMng.Ins.CreatetextEffect(transform.position, Color.green, n, ((int)n).ToString() + "HP+"); }
    public void MinGold(int n) { nGold -= n; UiMng.Ins.GoldMinAni(); }

    //변수 얻어오는 함수들
    public int GetGold() { return nGold; }
    public int GetLevel() { return nLevel; }
    public float GetExp() { return fExp; }

    public float GetSpeed() { return fSpeed; }
    public float GetAttack() { return fAttack; }
    public float GetDeffensive() { return fDeffensive; }
    public float GetColTime() { return fCollDown; }
    public float GetEvasion() { return fEvasion; }
    public float GetHp() { return fHp; }
    public float GetMaxHp() { return fMaxHp; }
    public bool GetMove() { return bDontMove; }
    public bool GetGoldUse(int money) { return nGold >= money ? true : false; }

    public bool GetAS(string stateName)
    {
        return PlayerAni.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
