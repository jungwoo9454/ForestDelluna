using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    bool bUse = false;
    bool bMove;
    int nGold = 0;
    Transform GoldTrans;
    SpriteRenderer GoldRendere;
    Rigidbody2D GoldRigid;
    Collider2D GoldCollider;

    float fTime;

    public bool GetUse()
    {
        return bUse;
    }

    void FixedUpdate()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.4f, GameMng.ins.GetPlayerMask().value))
        {
            bMove = true;
            GoldCollider.isTrigger = true;
        }

        if (bMove)
        {
            //Vector2 dir = EntityMng.ins.GoldPoint.transform.position - transform.position;
            //GoldRigid.velocity = dir.normalized * 12;

            fTime += Time.deltaTime;
            GoldRigid.position = Vector3.Lerp(GoldRigid.position, EntityMng.ins.GoldPoint.transform.position, fTime);
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
        bUse = false;
    }

    public void CreateGold(Vector3 pos, int Gold)
    {
        if (GoldRendere == null)
        {
            GoldRendere = GetComponent<SpriteRenderer>();
            GoldTrans = GetComponent<Transform>();
            GoldRigid = GetComponent<Rigidbody2D>();
            GoldCollider = GetComponent<Collider2D>();
        }

        fTime = 0;

        GoldCollider.isTrigger = false;

        GoldTrans.position = pos;
        nGold = Gold;

        bUse = true;
        bMove = false;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("GoldPoint"))
        {
            SoundMng.Ins.Play("Coin");
            Player.Ins.AddGold(nGold);
            Dead();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Equals("GoldPoint"))
        {
            SoundMng.Ins.Play("Coin");
            Player.Ins.AddGold(nGold);
            Dead();
        }
    }
}
