using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SKILLKEY { Q, E, R, Passive }
public class PlayerSkill : MonoBehaviour
{
    public Text[] SkillText = new Text[4];
    public Image[] SkillCoolImg = new Image[4];
    public Image[] SkillIconImg = new Image[4];
    public Color CoolDownColor;

    void Update()
    {
        for (int i = 0; i < GameMng.ins.Skills.Length; i++)
        {
            if(GameMng.ins.Skills[i] != null)
            {
                SkillIconImg[i].enabled = true;
                GameMng.ins.Skills[i].fCoolTime += Time.deltaTime;
                GameMng.ins.Skills[i].ItemUseUpdate();
                if (GameMng.ins.Skills[i].skillinfo.SkillIcon != SkillIconImg[i].sprite)
                {
                    SkillIconImg[i].sprite = GameMng.ins.Skills[i].skillinfo.SkillIcon;
                }

                if (!GameMng.ins.Skills[i].bTime)
                    SkillIconImg[i].color = CoolDownColor;
                else
                    SkillIconImg[i].color = Color.white;

            }
            else
            {
                SkillIconImg[i].sprite = UiMng.Ins.SkillNone;
            }
        }

        for (int i = 0; i < SkillCoolImg.Length; i++)
        {
            if(GameMng.ins.Skills[i] != null)
            {
                SkillCoolImg[i].fillAmount = (GameMng.ins.Skills[i].fCoolDown - GameMng.ins.Skills[i].fCoolTime) / GameMng.ins.Skills[i].fCoolDown;
            }
        }

        Skill_Q();
        Skill_E();
        Skill_R();
        Skill_Passive();
    }

    void Skill_Q()
    {
        if (GameMng.ins.Skills[0] != null)
        {
            SkillText[0].gameObject.SetActive(!GameMng.ins.Skills[0].bTime);
            if (!GameMng.ins.Skills[0].bTime)
            {
                SkillText[0].text = (Mathf.Abs((int)GameMng.ins.Skills[0].fCoolTime - GameMng.ins.Skills[0].fCoolDown)).ToString();
            }

            if (Input.GetKey(KeyCode.Q) && GameMng.ins.Skills[0].fCoolTime >= GameMng.ins.Skills[0].fCoolDown)
            {
                GameMng.ins.Skills[0].fCoolTime = 0;
                GameMng.ins.Skills[0].SkillOn();
            }

            if (!GameMng.ins.Skills[0].bTime)
            {
                GameMng.ins.Skills[0].SkillUse();
            }
        }
    }

    void Skill_E()
    {
        if (GameMng.ins.Skills[1] != null)
        {
            SkillText[1].gameObject.SetActive(!GameMng.ins.Skills[1].bTime);
            if (!GameMng.ins.Skills[1].bTime)
            {
                SkillText[1].text = (Mathf.Abs((int)GameMng.ins.Skills[1].fCoolTime - GameMng.ins.Skills[1].fCoolDown)).ToString();
            }

            if (Input.GetKey(KeyCode.E) && GameMng.ins.Skills[1].fCoolTime >= GameMng.ins.Skills[1].fCoolDown)
            {
                GameMng.ins.Skills[1].fCoolTime = 0;
                GameMng.ins.Skills[1].SkillOn();
            }

            if (!GameMng.ins.Skills[1].bTime)
            {
                GameMng.ins.Skills[1].SkillUse();
            }
        }
    }

    void Skill_R()
    {
        if (GameMng.ins.Skills[2] != null)
        {
            SkillText[2].gameObject.SetActive(!GameMng.ins.Skills[2].bTime);
            if (!GameMng.ins.Skills[2].bTime)
            {
                SkillText[2].text = (Mathf.Abs((int)GameMng.ins.Skills[2].fCoolTime - GameMng.ins.Skills[2].fCoolDown)).ToString();
            }

            if (Input.GetKey(KeyCode.R) && GameMng.ins.Skills[2].fCoolTime >= GameMng.ins.Skills[2].fCoolDown)
            {
                GameMng.ins.Skills[2].fCoolTime = 0;
                GameMng.ins.Skills[2].SkillOn();
            }

            if (!GameMng.ins.Skills[2].bTime)
            {
                GameMng.ins.Skills[2].SkillUse();
            }
        }
    }

    void Skill_Passive()
    {
        if (GameMng.ins.Skills[3] != null)
        {
            GameMng.ins.Skills[3].SkillOn();
        }
    }
}
