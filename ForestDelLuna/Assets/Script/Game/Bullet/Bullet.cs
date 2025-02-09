using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeadFunc(Collider2D col);
public class Bullet : MonoBehaviour
{
    DeadFunc deadFunc;
    Transform TransBullet;
    Rigidbody2D RigidBullet;
    SpriteRenderer RendereBullet;
    BoxCollider2D Boxcollider;
    Animator BulletAni;
    Vector2 BulletDir;
    Vector2 Target;

    bool bHoming;
    bool bUse; 
    bool bPlayer;

    bool bPass;

    float fSpeed;
    float fLifeTime;
    float fAttack;
    float fAngle;

    string Bulletname;

    void FixedUpdate()
    {
        if(bUse)
        {
            if (bHoming)
            {
                fAngle = (Mathf.Atan2(transform.position.x - Player.Ins.transform.position.x, Player.Ins.transform.position.y - transform.position.y) * 180 / Mathf.PI) + 90;
            }

            float xDir = Mathf.Cos((fAngle) * Mathf.Deg2Rad);
            float yDir = Mathf.Sin((fAngle) * Mathf.Deg2Rad);

            BulletDir = new Vector2(xDir, yDir) * fSpeed;

            RigidBullet.velocity = BulletDir;


            if (Bulletname == "Stone")
            {
                TransBullet.Rotate(0, 0, 60 * Time.deltaTime);
            }
            else
            {
                TransBullet.rotation = Quaternion.Euler(0, 0, fAngle);
            }
        }
    }


    IEnumerator DeadBullet()
    {
        yield return new WaitForSeconds(fLifeTime);

        Dead();
    }



    public void CreateBullet(string n,BoxCollider2D col, Sprite sprite, Animator ani, DeadFunc func,Vector2 spawnpos, float angle, float spd, float lifetime, float Attack, bool player, bool Homing, bool pass)
    {

        if (RigidBullet == null || TransBullet == null || RendereBullet == null)
        {
            TransBullet = GetComponent<Transform>();
            RigidBullet = GetComponent<Rigidbody2D>();
            RendereBullet = GetComponent<SpriteRenderer>();
            Boxcollider = GetComponent<BoxCollider2D>();
            BulletAni = GetComponent<Animator>();
        }

        Bulletname = n;
        deadFunc = func;

        if(ani != null)
            BulletAni = ani;

        if (col != null)
        {
            Boxcollider.offset = col.offset;
            Boxcollider.size = col.size;
        }

        if(sprite != null)
            RendereBullet.sprite = sprite;

        TransBullet.position = spawnpos;
        TransBullet = transform;

        float xDir = Mathf.Cos(angle * Mathf.Deg2Rad);
        float yDir = Mathf.Sin(angle * Mathf.Deg2Rad);

        fAngle = angle;

        fSpeed = spd;        
        fLifeTime = lifetime;
        fAttack = Attack;    
        bPlayer = player;
        bHoming = Homing;
        bUse = true;
        bPass = pass;

        BulletDir = new Vector2(xDir, yDir) * fSpeed;

        gameObject.SetActive(true);

        if (fLifeTime > 0.0f)
        {
            StartCoroutine(DeadBullet());
        }
    }

    public bool GetDead()
    {
        return bUse;
    }

    void Dead()
    {
        if (Bulletname == "MagicBall")
        {
            EffectMng.Ins.CreateParticle("MagicBallDead", transform.position);
            SoundMng.Ins.Play("BallBoom");
        }

        bUse = false;
        gameObject.SetActive(false);
        TransBullet.position = new Vector2(-10000, -10000);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(bPlayer)
        {
            if(collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Enemy>().TakeDmg(fAttack);

                if (!bPass)
                    Dead();

                if(deadFunc != null)
                {
                    deadFunc(collider);
                }
            }
        }
        else
        {
            if (collider.CompareTag("Player"))
            {
                Player.Ins.TakeDmg(fAttack);

                if (!bPass)
                    Dead();

                if (deadFunc != null)
                {
                    deadFunc(collider);
                }
            }
        }
    }

}
