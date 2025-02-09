using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void LevelFunc();
public class GameMng : MonoBehaviour
{
    [Header("Gold Value")]
    public Color GoldColor;
    public GameObject OriginGold;
    public Transform GoldParents;
    List<Gold> GoldList = new List<Gold>();

    //SKILL
    [Header("Player Skills")]
    public Skill[] Skills = new Skill[4];
    public List<Skill> SkillList = new List<Skill>();

    //Exp
    [Header("Exp Value")]
    public int[] PlayerLevelMax = new int[10];
    public List<LevelFunc> LevelLoop = new List<LevelFunc>();
    public LevelFunc[] LevelStat = new LevelFunc[15];
    string[] LevelInfo = new string[15];


    //시스템
    [Header("System Value")]
    public bool bPause;
    public bool bClear;
    public bool bPlay = true;
    public bool bDonMoveCamera = false;
    public bool bBoss;

    //매터리얼
    [Header("Material List")]
    public Material OutLineMat;
    public Material DeffaultSpriteMat;
    public Material WhiteMat;

    //카메라
    [Header("Camera")]
    public Camera MainCamera;
    public Transform CamHold;

    //플레이
    [Header("Game Play Data")]
    public float fEnemyTotalDmg;
    public int fEnemyTotalKill;
    public int nMin;
    public int nSecond;
    float fTimeCount;

    //기타
    [Header("ETC")]
    public Color EnemyHitColor;
    public LayerMask PlayerMask;
    public LayerMask ObjectLayer;
    public LayerMask GroundLayer;
    public LayerMask EnemyMask;
    public int nAttackSuccess = 0;

    public GameObject SkillInfoObj;
    public GameObject WarpPoint;
    public Text skilname;
    public Text skillInfo;
    public Text skillcoolTIme;
    public Image skillicon;
    float fHealTime;

    public List<Enemy> enemyList = new List<Enemy>();

    private static GameMng gameMng;
    public static GameMng ins
    {
        get
        {
            if (gameMng == null)
            {
                gameMng = FindObjectOfType<GameMng>();

                if (gameMng == null)
                {
                    GameObject gameObj = new GameObject();
                    gameObj.name = "GameMng";
                    gameMng = gameObj.AddComponent<GameMng>();
                }
            }
            return gameMng;
        }
    }

    private void Awake()
    {
        GameObject[] gl = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < gl.Length; i++)
        {
            if (!gl[i].name.Equals("Enemy_Ghost Wizzard"))
            {
                enemyList.Add(gl[i].GetComponent<Enemy>());
                GameObject obj = Instantiate(UiMng.Ins.mapInfo);
                obj.transform.SetParent(UiMng.Ins.gameObject.transform);
                UiMng.Ins.mapInfoList.Add(obj);
            }
        }
    }

    void Start()
    {
        LevelStat[0] = Level1;
        LevelStat[1] = Level2;
        LevelStat[2] = Level3;
        LevelStat[3] = Level4;
        LevelStat[4] = Level5;
        LevelStat[5] = Level6;
        LevelStat[6] = Level7;
        LevelStat[7] = Level8;
        LevelStat[8] = Level9;
        LevelStat[9] = Level10;
        LevelStat[10] = Level11;
        LevelStat[11] = Level12;
        LevelStat[12] = Level13;
        LevelStat[13] = Level14;
        LevelStat[14] = Level15;


        LevelInfo[0] = "체력 60+";
        LevelInfo[1] = "공격력 12+";
        LevelInfo[2] = "회피 8+";
        LevelInfo[3] = "이동속도 0.5+";
        LevelInfo[4] = "재사용 대기시간 감소 0.15+";
        LevelInfo[5] = "10초마다 체력 3회복";
        LevelInfo[6] = "공격력 10%+";
        LevelInfo[7] = "랜덤 스킬 1개 생성";
        LevelInfo[8] = "회피 10%+";
        LevelInfo[9] = "방어력 15+";
        LevelInfo[10] = "이동속도 10%+";
        LevelInfo[11] = "이동속도 20%- 대신, 공격력 10+";
        LevelInfo[12] = "랜덤 스킬 2개 생성";
        LevelInfo[13] = "방어력 5+";
        LevelInfo[14] = "체력 20- 대신, 방어력 10+,이동속도및 재사용 대기시간 감소량 15%+";

    }



    void Update()
    {

        if (!SoundMng.Ins.IsPlay("GameBg") && !UiMng.Ins.ResultWindowObj.activeSelf && !bBoss)
            SoundMng.Ins.Play("GameBg");
        else if(bBoss)
        {
            if (SoundMng.Ins.IsPlay("GameBg"))
                SoundMng.Ins.Stop("GameBg");
            if(!SoundMng.Ins.IsPlay("Boss") && !UiMng.Ins.ResultWindowObj.activeSelf)
                SoundMng.Ins.Play("Boss");
        }

        for (int i = 0; i < LevelLoop.Count; i++)
            LevelLoop[i]();

        SkillInfoWIndow();
        TimeMng();
    }

    void SkillInfoWIndow()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            int n = -1;
            if (Input.GetKey(KeyCode.Alpha1))
                n = 0;
            if (Input.GetKey(KeyCode.Alpha2))
                n = 1;
            if (Input.GetKey(KeyCode.Alpha3))
                n = 2;
            if (Input.GetKey(KeyCode.Alpha4))
                n = 3;

            if(n != -1)
            {
                skilname.text = Skills[n].name;
                skillInfo.text = Skills[n].info;
                skillcoolTIme.text = "재사용 대기시간 : " + Skills[n].fCoolDown;
                skillicon.sprite = Skills[n].skillinfo.SkillIcon;
                SkillInfoObj.SetActive(true);
            }
            else
            {
                SkillInfoObj.SetActive(false);
            }
        }
    }

    public IEnumerator ShakeCoru(float duration, float magnitude)
    {
        Vector3 OriginalPos = MainCamera.transform.localPosition;

        float elSpeed = 0.0f;

        while (elSpeed <= duration)
        {
            float x = Random.Range(-1.0f, 1.0f) * magnitude;
            float y = Random.Range(-1.0f, 1.0f) * magnitude;

            MainCamera.transform.localPosition = new Vector3(x, y, OriginalPos.z);

            elSpeed += Time.deltaTime;

            yield return null;
        }

        MainCamera.transform.localPosition = OriginalPos;
    }

    public void Shake(float duration, float magnitude)
    {
        if(!bPause)
        {
            StartCoroutine(ShakeCoru(duration, magnitude));
        }
    }

    public Skill GetSkill(string name)
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            if(SkillList[i].gameObject.name == name)
            {
                return SkillList[i];
            }
        }
        return null;
    }

    public bool ChangeSkill(int key, string name)
    {
        if (GetSkill(name) != null)
        {
            Skills[key] = GetSkill(name);
            SoundMng.Ins.Play("SkillSet");
            return true;
        }

        return false;
    }

    public void CreateGold(Vector3 pos, int nGold)
    {
        bool findGold = false;
        for (int i = 0; i < GoldList.Count; i++)
        {
            if (!GoldList[i].GetUse())
            {
                GoldList[i].CreateGold(pos, nGold);
                findGold = true;
                break;
            }
        }
        if (!findGold)
        {
            Gold gold = Instantiate(OriginGold).GetComponent<Gold>();
            GoldList.Add(gold);
            gold.transform.parent = GoldParents;
            gold.CreateGold(pos, nGold);
        }
    }

    /// <summary>
    /// name이라는 이름의 애니메이션이 현재 실행중인지 알아오는 함수
    /// </summary>
    /// <param name="ani">인자로 넘길 애니메이터(검색할 애니메이터)</param>
    /// <param name="name">현재 애니메이션 상태를 검사할 이름</param>
    /// <returns></returns>
    public bool GetAniState(Animator ani, string name)
    {
        return ani.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void TimeMng()
    {
        if(bPlay)
        {
            fTimeCount += Time.deltaTime;
            if(fTimeCount >= 1)
            {
                fTimeCount = 0;
                nSecond++;

                if(nSecond >= 60)
                {
                    nSecond = 0;
                    nMin++;
                }
            }
        }
    }

    public void Pause()
    {
        if(bPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void StatWindowClose()
    {
        Player.Ins.SetMove(false, false);
        Player.Ins.bGod = false;
    }

    public void LevelUP()
    {
        Player.Ins.bGod = true;
        UiMng.Ins.PlayerLevelUpgrade.SetActive(true);
        LevelFunc[] ThreeLevel = new LevelFunc[3];
        string[] ThreeInfo = new string[3];
        Player.Ins.SetMove(true, true);

        for (int i = 0; i < 3; i++)
        {
            int n = Random.Range(0, 15);
            ThreeLevel[i] = LevelStat[n];
            ThreeInfo[i] = LevelInfo[n];
        }

        UiMng.Ins.level[0] = ThreeLevel[0];
        UiMng.Ins.level[1] = ThreeLevel[1];
        UiMng.Ins.level[2] = ThreeLevel[2];
        UiMng.Ins.LevelInfotext[0].text = ThreeInfo[0];
        UiMng.Ins.LevelInfotext[1].text = ThreeInfo[1];
        UiMng.Ins.LevelInfotext[2].text = ThreeInfo[2];
    }

    void AddList(LevelFunc i)
    {
        if (!LevelLoop.Contains(i))
            LevelLoop.Add(i);
    }

    void Level1()
    {
        Player.Ins.AddHp(60);
        Player.Ins.HpHeal(60);
        AddList(Level1_);
    }

    void Level1_()
    {
        Player.Ins.AddHp(60);
    }

    void Level2()
    {
        Player.Ins.AddAttack(12);
        AddList(Level2);
    }

    void Level3()
    {
        Player.Ins.AddEvasion(8);
        AddList(Level3);
    }

    void Level4()
    {
        Player.Ins.AddSkillCoolDown(0.2f);
        AddList(Level4);
    }

    void Level5()
    {
        Player.Ins.AddSpeed(0.5f);
        AddList(Level5);
    }

    void Level6()
    {
        fHealTime += Time.deltaTime;
        if(fHealTime >= 10)
        {
            fHealTime = 0;
            Player.Ins.HpHeal(3);
        }
        AddList(Level6);
    }

    void Level7()
    {
        Player.Ins.AddPctAttack(0.1f);
        AddList(Level7);
    }

    void Level8()
    {
        EntityMng.ins.CreateEntity_Skill(Player.Ins.transform.position - new Vector3(0.8f, 0, 0), SkillList[Random.Range(0, SkillList.Count)]);
    }

    void Level9()
    {
        Player.Ins.AddPctEvasion(0.1f);
        AddList(Level9);
    }

    void Level10()
    {
        Player.Ins.AddDeffensive(15);
        AddList(Level10);
    }

    void Level11()
    {
        Player.Ins.AddPctSpeed(0.1f);
        AddList(Level11);
    }

    void Level12()
    {
        Player.Ins.MinPctSpeed(0.2f);
        Player.Ins.AddAttack(10);
        AddList(Level12);
    }

    void Level13()
    {
        EntityMng.ins.CreateEntity_Skill(Player.Ins.transform.position - new Vector3(0.8f, 0, 0), SkillList[Random.Range(0, SkillList.Count)]);
        EntityMng.ins.CreateEntity_Skill(Player.Ins.transform.position + new Vector3(0.8f, 0, 0), SkillList[Random.Range(0, SkillList.Count)]);
    }

    void Level14()
    {
        Player.Ins.AddDeffensive(5);
        AddList(Level14);
    }

    void Level15()
    {
        Player.Ins.MinHp(20);
        Player.Ins.AddDeffensive(10);
        Player.Ins.AddSkillCoolDown(0.15f);
        Player.Ins.AddPctSpeed(0.15f);
        AddList(Level15);
    }

    public void SetPause(bool p) { bPause = p; Pause(); }

    public Camera GetCamera() { return MainCamera; }

    public LayerMask GetPlayerMask() { return PlayerMask; }
}
