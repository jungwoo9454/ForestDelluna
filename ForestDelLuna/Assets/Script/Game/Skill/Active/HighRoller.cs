using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighRoller : Skill
{
    public ParticleSystem Readyeff;
    public ParticleSystem Slasheff;
    public Vector2 BoxSize;
    //스킬은 처음 시전하였을때(키보드에서 누를시) 딱 1번 들어오는곳
    public override void SkillOn()
    {
        Player.Ins.PlayerAni.Play("HighRoller");
        Readyeff.Play();

        StartCoroutine(SkillAniUse());
    }

    IEnumerator SkillAniUse()
    {
        yield return new WaitForSeconds(0.5f);
        Player.Ins.PlayerRigid.velocity = new Vector2(Player.Ins.PlayerRigid.velocity.x, 18);
        Collider2D[] col = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0, GameMng.ins.EnemyMask.value);
        for (int i = 0; i < col.Length; i++)
        {
            col[i].GetComponent<Enemy>().TakeDmg(Player.Ins.GetAttack() * 1.2f);
            col[i].GetComponent<Rigidbody2D>().velocity = new Vector2(col[i].GetComponent<Rigidbody2D>().velocity.x, 20);
        }
        Slasheff.Play();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position - new Vector3(0, 1, 0), BoxSize);
    //}
}
