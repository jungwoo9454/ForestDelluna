using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : Skill
{
    public Vector2 BoxSize;
    public ParticleSystem Crasheff;
    //스킬은 처음 시전하였을때(키보드에서 누를시) 딱 1번 들어오는곳
    public override void SkillOn()
    {
        Player.Ins.PlayerAni.Play("Crash");
        StartCoroutine(SkillAni());
    }

    IEnumerator SkillAni()
    {
        yield return new WaitForSeconds(0.5f);
        Collider2D[] col = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0, GameMng.ins.EnemyMask.value);
        for (int i = 0; i < col.Length; i++)
        {
            if(Player.Ins.transform.localScale.x > 0 && (Player.Ins.transform.position - col[i].transform.position).x < 0)
            {
                col[i].GetComponent<Enemy>().TakeDmg(Player.Ins.GetAttack() * 1.3f);
                col[i].GetComponent<Enemy>().BlackOut(3f);
            }
            else if(Player.Ins.transform.localScale.x < 0 && (Player.Ins.transform.position - col[i].transform.position).x > 0)
            {
                col[i].GetComponent<Enemy>().TakeDmg(Player.Ins.GetAttack() * 1.3f);
                col[i].GetComponent<Enemy>().BlackOut(3f);
            }
            EffectMng.Ins.CreateParticle("Flash", col[i].transform.position);
        }
        Crasheff.Play();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawCube(transform.position - new Vector3(0, 1, 0), BoxSize);
    //}
}
