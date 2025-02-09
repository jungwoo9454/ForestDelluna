using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            GameMng.ins.Shake(0.2f, 0.5f);
            bool bEnemy = false;
            if (Player.Ins.HitEnemyList.Contains(collider.name))
                bEnemy = true;

            if (!bEnemy)
            {
                GameMng.ins.nAttackSuccess++;
                EffectMng.Ins.CreateParticle("HitSpark", collider.transform.position);
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.TakeDmg(Player.Ins.GetAttack());

                if (Player.Ins.bAttackBurn)
                {
                    enemy.GetComponent<Enemy>().bBurn = true;
                    enemy.GetComponent<Enemy>().fTotalBurnTime = 0;
                }
                Player.Ins.HitEnemyList.Add(collider.name);
            }
        }
    }
}
