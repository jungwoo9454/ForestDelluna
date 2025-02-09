using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMng : MonoBehaviour
{
    List<Bullet> BulletList = new List<Bullet>();

    public List<Bullet> BulletObjList = new List<Bullet>();
    public GameObject OriginalBullet;
    Transform mngTrans;

    private static BulletMng bulletMng;
    public static BulletMng ins
    {
        get
        {
            if (bulletMng == null)
            {
                bulletMng = FindObjectOfType<BulletMng>();

                if (bulletMng == null)
                {
                    GameObject bulletObj = new GameObject();
                    bulletObj.name = "BulletMng";
                    bulletMng = bulletObj.AddComponent<BulletMng>();
                }
            }
            return bulletMng;
        }
    }

    void Start()
    {
        mngTrans = transform;
    }


    public void CreateBullet(string name_, DeadFunc func, Vector2 spawnpos, float angle, float spd, float lifetime, float Attack, bool player, bool Homing, bool pass)
    {
        bool findBullet = false;
        for (int i = 0; i < BulletList.Count; i++)
        {
            if (!BulletList[i].GetDead() && BulletList[i].gameObject.name == name_)
            {
                BulletList[i].CreateBullet(name_,null, null, null, func, spawnpos, angle, spd, lifetime, Attack, player, Homing, pass);
                findBullet = true;
                break;
            }
        }


        if (!findBullet)
        {
            Bullet playerBullet = new Bullet();
            for (int i = 0; i < BulletObjList.Count; i++)
            {
                if(BulletObjList[i].gameObject.name == name_)
                {
                    playerBullet = Instantiate(BulletObjList[i]).GetComponent<Bullet>();
                    BulletList.Add(playerBullet);
                    playerBullet.transform.parent = mngTrans;
                    playerBullet.CreateBullet(name_, BulletObjList[i].GetComponent<BoxCollider2D>(), BulletObjList[i].GetComponent<SpriteRenderer>().sprite, BulletObjList[i].GetComponent<Animator>()
                        , func, spawnpos, angle, spd, lifetime, Attack, player, Homing, pass);
                    break;
                }
            }
        }
    }

    public Bullet GetBullet(string name)
    {
        for (int i = 0; i < BulletObjList.Count; i++)
        {
            if(BulletObjList[i].gameObject.name == name)
            {
                return BulletObjList[i];
            }
        }
        return null;
    }
}
