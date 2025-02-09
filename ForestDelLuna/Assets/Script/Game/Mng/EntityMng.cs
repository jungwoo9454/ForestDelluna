using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemUse();
public class EntityMng : MonoBehaviour
{
    public GameObject GoldPoint;
    public GameObject OriginalEntity;
    public List<ItemInfo> ItemInfoList = new List<ItemInfo>();
    public Dictionary<string,ItemUse> ItemFuncList = new Dictionary<string, ItemUse>();
    List<Entity> EntityList = new List<Entity>();
    List<ItemUse> ItemLoop = new List<ItemUse>();

    private static EntityMng itemMng;
    public static EntityMng ins
    {
        get
        {
            if (itemMng == null)
            {
                itemMng = FindObjectOfType<EntityMng>();

                if (itemMng == null)
                {
                    GameObject itemObj = new GameObject();
                    itemObj.name = "ItemMng";
                    itemMng = itemObj.AddComponent<EntityMng>();
                }
            }
            return itemMng;

        }
    }

    void Awake()
    {
        ItemFuncList.Add(ItemInfoList[0].Codename, AttackPctAdd);
        ItemFuncList.Add(ItemInfoList[1].Codename, AddEvasion);
        ItemFuncList.Add(ItemInfoList[2].Codename, AddMaxHpPct);
        ItemFuncList.Add(ItemInfoList[3].Codename, AddSkillCoolDown);
        ItemFuncList.Add(ItemInfoList[4].Codename, AddSpeedPct);
        ItemFuncList.Add(ItemInfoList[5].Codename, AllUpgrade);
        ItemFuncList.Add(ItemInfoList[6].Codename, DeffensiveAdd);
        ItemFuncList.Add(ItemInfoList[7].Codename, FireAttack);
        ItemFuncList.Add(ItemInfoList[8].Codename, FullHeal);
        ItemFuncList.Add(ItemInfoList[9].Codename, KillHeal);
        ItemFuncList.Add(ItemInfoList[10].Codename, MinHpAddAttack);
        ItemFuncList.Add(ItemInfoList[11].Codename, RaondomSkillDrop);
        ItemFuncList.Add(ItemInfoList[12].Codename, SkillDrop);
    }

    void Update()
    {
        for (int i = 0; i < ItemLoop.Count; i++)
        {
            ItemLoop[i]();
        }
    }

    public void CreateEntity_Skill(Vector3 pos, Skill skill)
    {
        bool findEntity = false;
        for (int i = 0; i < EntityList.Count; i++)
        {
            if (!EntityList[i].GetUse())
            {
                EntityList[i].CreateEntity_Skill(pos, skill);
                findEntity = true;
                break;
            }
        }


        if (!findEntity)
        {
            Entity entity = Instantiate(OriginalEntity).GetComponent<Entity>();
            EntityList.Add(entity);
            entity.transform.parent = transform;
            entity.CreateEntity_Skill(pos, skill);
        }
    }

    public void CreateEntity_Item(Vector3 pos, ItemInfo info, ItemUse use)
    {
        bool findEntity = false;
        for (int i = 0; i < EntityList.Count; i++)
        {
            if (!EntityList[i].GetUse())
            {
                EntityList[i].CreateEntity_Item(pos, info, use);
                findEntity = true;
                break;
            }
        }


        if (!findEntity)
        {
            Entity entity = Instantiate(OriginalEntity).GetComponent<Entity>();
            EntityList.Add(entity);
            entity.transform.parent = transform;
            entity.CreateEntity_Item(pos, info, use);
        }
    }

    public ItemInfo GetItemInfo(string name)
    {
        for (int i = 0; i < ItemInfoList.Count; i++)
        {
            if(ItemInfoList[i].Codename == name)
            {
                return ItemInfoList[i];
            }
        }
        return null;
    }

    public ItemUse GetItemUseFunc(string name)
    {
        return ItemFuncList[name];
    }

    void AddList(ItemUse i)
    {
        if (!ItemLoop.Contains(i))
            ItemLoop.Add(i);
    }

    ///////////////////////////////////////////////////////////////////

    void MaxHpAddPostion()
    {
        Player.Ins.AddHp(150);
        Player.Ins.HpHeal(150);
        AddList(MaxHpAddPostion2);
    }

    void MaxHpAddPostion2()
    {
        Player.Ins.AddHp(150);
    }


    void AttackPctAdd()
    {
        Player.Ins.AddPctAttack(0.5f);
        AddList(AttackPctAdd);
    }

    void DeffensiveAdd()
    {
        Player.Ins.AddDeffensive(40);
        AddList(DeffensiveAdd);
    }

    void RaondomSkillDrop()
    {
        CreateEntity_Skill(Player.Ins.transform.position, GameMng.ins.SkillList[Random.Range(3, GameMng.ins.SkillList.Count)]);
    }

    void FireAttack()
    {
        Player.Ins.bAttackBurn = true;
    }

    void AddSkillCoolDown()
    {
        Player.Ins.AddSkillCoolDown(0.3f);
        AddList(AddSkillCoolDown);
    }

    void AddEvasion()
    {
        Player.Ins.AddEvasion(10);
        AddList(AddEvasion);
    }

    void AddMaxHpPct()
    {
        float prevHp = Player.Ins.GetMaxHp();
        Player.Ins.AddPctHp(0.4f);
        Player.Ins.HpHeal(Player.Ins.GetMaxHp() - prevHp);
        AddList(AddMaxHpPct2);
    }

    void AddMaxHpPct2()
    {
        Player.Ins.AddPctHp(0.4f);
    }

    void AddSpeedPct()
    {
        Player.Ins.AddPctSpeed(0.15f);
        AddList(AddSpeedPct);
    }

    void AllUpgrade()
    {
        Player.Ins.AddPctAttack(0.2f);
        Player.Ins.AddSkillCoolDown(0.2f);
        Player.Ins.AddPctSpeed(0.2f);
        Player.Ins.AddPctDeffensive(0.2f);
        Player.Ins.AddPctEvasion(0.2f);

        float prevHp = Player.Ins.GetMaxHp();
        Player.Ins.AddPctHp(0.2f);
        Player.Ins.HpHeal(Player.Ins.GetMaxHp() - prevHp);
        AddList(AllUpgrade2);
    }

    void AllUpgrade2()
    {
        Player.Ins.AddPctAttack(0.2f);
        Player.Ins.AddSkillCoolDown(0.2f);
        Player.Ins.AddPctSpeed(0.2f);
        Player.Ins.AddPctDeffensive(0.2f);
        Player.Ins.AddPctEvasion(0.2f);
        Player.Ins.AddPctHp(0.2f);
    }

    void Exp500Add()
    {
        Player.Ins.AddExp(500);
    }

    void FullHeal()
    {
        Player.Ins.HpHeal(Player.Ins.GetMaxHp());
    }

    void KillHeal()
    {
        Player.Ins.bEnemyKillHeal = true;
    }

    void MinHpAddAttack()
    {
        Player.Ins.MinHp(60);
        Player.Ins.AddAttack(30);
        AddList(MinHpAddAttack);
    }

    void SkillDrop()
    {
        CreateEntity_Skill(Player.Ins.transform.position, GameMng.ins.GetSkill("DimensionAttack"));
    }
}
