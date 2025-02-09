using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SKILLTYPE { ACTIVE, PASSIVE }
public class Skill : MonoBehaviour
{
    public SkillInfo skillinfo;

    public new string name;
    public string info;

    //스킬의 대기시간
    public float fPrevCoolDown;
    public float fCoolDown;

    //현재 스킬의 재사용 대기시간
    public float fCoolTime;

    //현재 스킬의 쿨타임이 있는지 없는지
    public bool bTime;
    public SKILLTYPE Type;

    void Start()
    {
        name = skillinfo.name;
        info = skillinfo.info;
        Type = skillinfo.Type;
        fCoolDown = skillinfo.fCollDown;
        fPrevCoolDown = fCoolDown;
        fCoolTime = fCoolDown;
    }

    public void ItemUseUpdate()
    {
        fCoolDown = fPrevCoolDown * Player.Ins.GetColTime();

        transform.position = Player.Ins.transform.position;
        if (fCoolTime >= fCoolDown)
        {
            bTime = true;
            Reset();
        }
        else
        {
            bTime = false;
        }
    }

    public void ColRot()
    {
        if (Player.Ins.transform.localScale.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public virtual void SkillOn()
    {

    }
    public virtual void SkillUse()
    {

    }
    public virtual void Reset()
    {

    }
}

