using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UiMng : MonoBehaviour
{
    [Header("Gold Animation")]
    public Animator GoldAddnMinAni;

    [Header("Player Stat Gage")]
    public Slider HpBar;
    public Slider ExpBar;

    [Header("Player Stat Text")]
    public Text Gold;
    public Text Hp;
    public Text Exp;

    [Header("BroadCast Text")]
    public GameObject Broadcast;

    [Header("Stat Info Window Text")]
    public Text[] Stat = new Text[6];
    public Text DeffensiveStat;
    public Text HpStat;

    [Header("Stat Info Window Obj")]
    public GameObject PlayerStatInfoPannel;

    [Header("Text Effect")]
    public Transform WorldCanvas;
    public GameObject OriginalTextEffect;
    List<TextEffect> TextEffectList = new List<TextEffect>();

    [Header("Skill Ui")]
    public GameObject SelectIconActive;
    public GameObject SelectIconPassive;


    [Header("Level Upgrade Window")]
    public GameObject PlayerLevelUpgrade;


    [Header("Level Upgrade Window - Function & Text")]
    public LevelFunc[] level = new LevelFunc[3];
    public Text[] LevelInfotext = new Text[3];

    [Header("Pause Window")]
    public GameObject PauseObj;

    [Header("Option Window")]
    public GameObject Option;
    public Slider Music;
    public Slider Sfx;
    public Text sfxText;
    public Text musictext;

    //Dialog
    [Header("Dialog")]
    public List<Dialog> DialogObjList = new List<Dialog>();
    public Text DialogName;
    public Text DialogComment;
    public GameObject DialogObj;
    Dialog ThisDialog;
    bool bDialogIsPlay;
    int nDialogIndex;
    int nCommentIndex;
    string Thisname;
    string ThisComment;
    string CopyComment;

    [Header("Result Window")]
    public GameObject ResultWindowObj;
    public Image GameResultImg;
    public Sprite LoseImg;
    public Sprite WinImg;

    [Header("Result Text")]
    public Text TotalLifeTime;
    public Text EnemyTotalDmg;
    public Text EnemyKillCount;

    [Header("Color List")]
    public Color[] DmgColor = new Color[5];

    [Header("Sprite List")]
    public Sprite SkillNone;

    [Header("Boss")]
    public GameObject BossStartAni;

    [Header("Enemy Map Info")]
    public GameObject mapInfo;
    public List<GameObject> mapInfoList = new List<GameObject>();
    

    private static UiMng uiMng;
    public static UiMng Ins
    {
        get
        {
            if (uiMng == null)
            {
                uiMng = FindObjectOfType<UiMng>();

                if (uiMng == null)
                {
                    GameObject UiObj = new GameObject();
                    UiObj.name = "UiMng";
                    uiMng = UiObj.AddComponent<UiMng>();
                }
            }
            return uiMng;
        }
    }

    void LateUpdate()
    {
        if(Broadcast.activeSelf)
        {
            Broadcast.transform.GetChild(0).GetComponent<Text>().color = Color.Lerp(Broadcast.transform.GetChild(0).GetComponent<Text>().color, new Color(0, 0, 0, 0), Time.deltaTime * 0.3f);

            if(Broadcast.transform.GetChild(0).GetComponent<Text>().color.a > 5)
            {
                Broadcast.SetActive(false);
            }
        }

        DialogObj.SetActive(bDialogIsPlay);
        ScrollUpdate();
        TextUpdate();
        PlayerStatInfo();
        DialogUpdate();
        PauseWindow();
        MapInfoUpdate();
    }

    //정보 업데이트 함수
    void ScrollUpdate()
    {
        HpBar.value = Player.Ins.GetHp() / Player.Ins.GetMaxHp();
        //수정(Player.Ins.GetLevel() - 1)
        ExpBar.value = Player.Ins.GetExp() / GameMng.ins.PlayerLevelMax[Player.Ins.GetLevel()];
    }

    void TextUpdate()
    {
        Exp.text = "LEVEL : " + Player.Ins.GetLevel().ToString();

        Gold.text = Player.Ins.GetGold().ToString() + "G";

        sfxText.text = (DataMng.ins.fSFXVolume * 100).ToString("N1") + "%";
        musictext.text = (DataMng.ins.fMusicVolume * 100).ToString("N1") + "%";

        StatTextUpdate();

    }

    void MapInfoUpdate()
    {
        for (int i = 0; i < GameMng.ins.enemyList.Count; i++)
        {
            if(!GameMng.ins.enemyList[i].gameObject.activeSelf)
            {
                mapInfoList[i].SetActive(false);
                mapInfoList.RemoveAt(i);
                GameMng.ins.enemyList.RemoveAt(i);
                continue;
            }
            mapInfoList[i].transform.position = GameMng.ins.enemyList[i].transform.position;
        }
    }

    public void StatTextUpdate()
    {
        Hp.text = ((int)Player.Ins.GetHp()).ToString() + "/" + Player.Ins.GetMaxHp().ToString();

        Stat[0].text = Player.Ins.GetAttack().ToString();
        Stat[1].text = Player.Ins.GetSpeed().ToString();
        Stat[2].text = Player.Ins.GetColTime().ToString("N2");
        Stat[3].text = Player.Ins.GetDeffensive().ToString();
        Stat[4].text = Player.Ins.GetMaxHp().ToString();
        Stat[5].text = Player.Ins.GetEvasion().ToString() + "%";

        DeffensiveStat.text = "현재 감소된 피해량 : " + (Player.Ins.GetDeffensive() * 0.4f).ToString("N2");
        HpStat.text = "추가된 체력 : " + (Player.Ins.GetMaxHp() - Player.Ins.fNormalHp).ToString();
    }

    public void CreatetextEffect(Vector3 pos, Color color, float amount, string text)
    {
        bool findtext = false;
        for (int i = 0; i < TextEffectList.Count; i++)
        {
            if (!TextEffectList[i].GetUse())
            {
                TextEffectList[i].CreateTextEffect(pos, amount, color, text);
                findtext = true;
                break;
            }
        }


        if (!findtext)
        {
            TextEffect dmgtext = Instantiate(OriginalTextEffect).GetComponent<TextEffect>();
            TextEffectList.Add(dmgtext);
            dmgtext.transform.SetParent(WorldCanvas, false);
            dmgtext.CreateTextEffect(pos, amount, color, text);
        }
    }

    public Color GetDmgColor(float Dmg)
    {
        if ((int)Dmg <= 100)
        {
            return DmgColor[(int)(Dmg / 20)];
        }
        else
        {
            return DmgColor[DmgColor.Length - 1];
        }
    }

    void PlayerStatInfo()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            PlayerStatInfoPannel.SetActive(!PlayerStatInfoPannel.activeSelf);
        }
    }

    public void StartDialog(string dialog)
    {
        for (int i = 0; i < DialogObjList.Count; i++)
        {
            if(DialogObjList[i].gameObject.name == dialog)
            {
                bDialogIsPlay = true;
                ThisDialog = DialogObjList[i];
                nDialogIndex = 0;
                CopyComment = ThisDialog.comments[nDialogIndex];
                StartCoroutine(Typing());
                break;
            }
        }
    }

    public void StopDialog()
    {
        ThisDialog = null;
        bDialogIsPlay = false;
        nDialogIndex = 0;
        nCommentIndex = 0;
        ThisComment = "";
        CopyComment = "";
    }

    int DialogUpdate()
    {
        if (bDialogIsPlay)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (ThisComment == ThisDialog.comments[nDialogIndex])
                {
                    nDialogIndex++;
                    if (nDialogIndex >= ThisDialog.comments.Length)
                    {
                        StopDialog();
                        return 0;
                    }

                    CopyComment = ThisDialog.comments[nDialogIndex];
                    nCommentIndex = 0;
                    ThisComment = "";
                    StartCoroutine(Typing());
                }
                else
                {
                    ThisComment = ThisDialog.comments[nDialogIndex];
                    StopAllCoroutines();
                }
            }
            Thisname = ThisDialog.name[nDialogIndex];
            DialogComment.text = ThisComment;
            DialogName.text = Thisname;

            if (Thisname == "P")
                DialogName.text = "Player";
        }
        return 1;
    }

    IEnumerator Typing()
    {
        yield return new WaitForSeconds(0.06f);

        ThisComment += CopyComment[nCommentIndex];
        if(CopyComment != ThisComment)
        {
            nCommentIndex++;
            StartCoroutine(Typing());
        }
    }

    public void BroadCast(string comment)
    {
        Broadcast.transform.GetChild(0).GetComponent<Text>().text = comment;
        Broadcast.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        Broadcast.SetActive(true);
    }

    public void Exit_Menu()
    {
        GameMng.ins.SetPause(false);
        Scene.ins.FadeScene("Menu");
        ResultWindowObj.SetActive(false);
    }

    public void Restart()
    {
        ResultWindowObj.SetActive(false);
        SceneManager.LoadScene("Game");
    }

    public void ResultWindow()
    {
        if(!ResultWindowObj.activeSelf)
        {
            EnemyTotalDmg.text = "적에게 가한 총 피해량 - " + GameMng.ins.fEnemyTotalDmg.ToString();
            EnemyKillCount.text = "적 처치 횟수 - " + GameMng.ins.fEnemyTotalKill.ToString();
            TotalLifeTime.text = "생존 시간 - " + GameMng.ins.nMin.ToString("00") + "m " + GameMng.ins.nSecond.ToString("00") + "s";


            GameMng.ins.bPlay = false;
            SoundMng.Ins.Stop("GameBg");
            SoundMng.Ins.Stop("Boss");
            if (GameMng.ins.bClear && !SoundMng.Ins.IsPlay("Clear"))
            {
                SoundMng.Ins.Play("Clear");
            }
            else if(!GameMng.ins.bClear && !SoundMng.Ins.IsPlay("Fail"))
            {
                SoundMng.Ins.Play("Fail");
            }

            ClosePause();
            ResultWindowObj.SetActive(true);
            ResultWindowObj.GetComponent<Animator>().Play("Result");

            if (GameMng.ins.bClear)
                GameResultImg.sprite = WinImg;
            else
                GameResultImg.sprite = LoseImg;
        }
    }

    public void PauseWindow()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameMng.ins.SetPause(!GameMng.ins.bPause);

            if (!GameMng.ins.bPause)
            {
                Option.SetActive(false);
            }
        }

        PauseObj.SetActive(GameMng.ins.bPause);
    }

    public void VolumeSliderValueUpdate()
    {
        Music.value = DataMng.ins.fMusicVolume;
        Sfx.value = DataMng.ins.fSFXVolume;
    }

    public void ClosePause()
    {
        GameMng.ins.SetPause(false);
        PauseObj.SetActive(GameMng.ins.bPause);
    }

    public void BossStartUI()
    {
        BossStartAni.SetActive(true);
        BossStartAni.GetComponent<Animator>().Play("BossStartAni");
    }

    public void Clear()
    {
        if(!GameMng.ins.bClear)
        {
            Player.Ins.bGod = true;
            GameMng.ins.bClear = true;
            Time.timeScale = 0.5f;
            StartCoroutine(ClearCo());
        }
    }

    IEnumerator ClearCo()
    {
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 1;
        StartCoroutine(Result());
    }

    IEnumerator Result()
    {
        yield return new WaitForSeconds(3.0f);
        ResultWindow();
    }

    public void MusicVol() { DataMng.ins.fMusicVolume = Music.value; }
    public void SfxVol() { DataMng.ins.fSFXVolume = Sfx.value; }

    public void ActiveIcon(bool active) { SelectIconActive.SetActive(active); }
    public void PassiveIcon(bool active) { SelectIconPassive.SetActive(active); }

    public void GoldAddAni() { GoldAddnMinAni.SetTrigger("Add"); }
    public void GoldMinAni() { GoldAddnMinAni.SetTrigger("Min"); }

    public void StatUpgrade1() { level[0](); PlayerLevelUpgrade.SetActive(false); Player.Ins.SetMove(false, false);Player.Ins.bGod = false; }
    public void StatUpgrade2() { level[1](); PlayerLevelUpgrade.SetActive(false); Player.Ins.SetMove(false, false);Player.Ins.bGod = false; }
    public void StatUpgrade3() { level[2](); PlayerLevelUpgrade.SetActive(false); Player.Ins.SetMove(false, false); Player.Ins.bGod = false; }
}
