using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ENTITYTYPE { SKILL,ITEM }
public class Entity : MonoBehaviour
{
    //아이템
    ItemUse Itemuse;
    ItemInfo ItemInfo;

    //스킬
    Skill skill;

    ENTITYTYPE Type;
    bool bUse = true;
    bool IsCol = false;
    float fRadius = 1f;
    Sprite sprite;

    Transform EntityTrans;
    Rigidbody2D EntityRigid;
    SpriteRenderer EntityRendere;

    public GameObject PanelInfo;
    public Image Icon;
    public Text nameText;
    public Text InfoText;
    public Text CoolDownText;

    void Update()
    {
        if (bUse)
        {
            ImgActive();
            Eat();
        }
    }

    public void CreateEntity_Item(Vector3 pos, ItemInfo iteminfo, ItemUse itemuse)
    {
        if (EntityTrans == null || EntityRendere != null || EntityRigid != null)
        {
            EntityTrans = GetComponent<Transform>();
            EntityRendere = GetComponent<SpriteRenderer>();
            EntityRigid = GetComponent<Rigidbody2D>();
        }

        EntityTrans.position = pos;
        EntityRendere.sprite = iteminfo.sprite;
        Icon.sprite = iteminfo.sprite;

        nameText.text = iteminfo.name;
        InfoText.text = iteminfo.info;

        CoolDownText.text = "";

        bUse = true;

        Type = ENTITYTYPE.ITEM;

        ItemInfo = iteminfo;
        Itemuse = itemuse;

        gameObject.SetActive(true);
    }

    public void CreateEntity_Skill(Vector3 pos, Skill skill_)
    {
        if (EntityTrans == null || EntityRendere != null || EntityRigid != null)
        {
            EntityTrans = GetComponent<Transform>();
            EntityRendere = GetComponent<SpriteRenderer>();
            EntityRigid = GetComponent<Rigidbody2D>();
        }

        EntityTrans.position = pos;
        EntityRendere.sprite = skill_.skillinfo.SkillIcon;
        Icon.sprite = skill_.skillinfo.SkillIcon;

        nameText.text = skill_.name;
        InfoText.text = skill_.info;

        if (skill_.skillinfo.Type == SKILLTYPE.ACTIVE)
            CoolDownText.text = "재사용 대기시간 : " + ((int)skill_.fCoolDown).ToString();
        else
            CoolDownText.text = "";

        bUse = true;

        Type = ENTITYTYPE.SKILL;

        skill = skill_;
        gameObject.SetActive(true);
    }

    public bool GetUse()
    {
        return bUse;
    }

    void Eat()
    {
        if(IsCol)
        {
            bool bSearch = false;
            for (int i = 0; i < 4; i++)
            {
                if(GameMng.ins.Skills[i] == skill)
                {
                    bSearch = true;
                }
            }

            if(!bSearch)
            {
                if (Type == ENTITYTYPE.SKILL)
                {
                    if (skill.Type == SKILLTYPE.PASSIVE)
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha4))
                        {
                            GameMng.ins.ChangeSkill(3, skill.gameObject.name);
                            UiMng.Ins.PassiveIcon(false);
                            SetDisable();
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            GameMng.ins.ChangeSkill(0, skill.gameObject.name);
                            UiMng.Ins.ActiveIcon(false);
                            SetDisable();
                        }

                        if (Input.GetKeyDown(KeyCode.Alpha2))
                        {
                            GameMng.ins.ChangeSkill(1, skill.gameObject.name);
                            UiMng.Ins.ActiveIcon(false);
                            SetDisable();
                        }

                        if (Input.GetKeyDown(KeyCode.Alpha3))
                        {
                            GameMng.ins.ChangeSkill(2, skill.gameObject.name);
                            UiMng.Ins.ActiveIcon(false);
                            SetDisable();
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Tab))
                    {
                        Itemuse();
                        SetDisable();
                    }
                }
            }
        }
    }

    void SetDisable()
    {
        gameObject.SetActive(false);
        bUse = false;
    }

    void ImgActive()
    {
        IsCol = Physics2D.OverlapCircle(transform.position, fRadius, GameMng.ins.GetPlayerMask().value);
        if (Type == ENTITYTYPE.SKILL)
        {
            if (IsCol)
            {
                PanelInfo.SetActive(true);

                if (skill.Type == SKILLTYPE.ACTIVE)
                    UiMng.Ins.ActiveIcon(true);

                if (skill.Type == SKILLTYPE.PASSIVE)
                    UiMng.Ins.PassiveIcon(true);
            }
            else
            {
                PanelInfo.SetActive(false);

                if (skill.Type == SKILLTYPE.ACTIVE)
                    UiMng.Ins.ActiveIcon(false);

                if (skill.Type == SKILLTYPE.PASSIVE)
                    UiMng.Ins.PassiveIcon(false);
            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.position, fRadius);
    //}
}
