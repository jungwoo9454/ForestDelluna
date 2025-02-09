using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionAttack : Skill
{
    public Vector2 BoxSize;
    public ParticleSystem slice;
    //스킬은 처음 시전하였을때(키보드에서 누를시) 딱 1번 들어오는곳
    public override void SkillOn()
    {
        SoundMng.Ins.Play("SwordSlicing");
        slice.Play();
        Player.Ins.SetMove(true, false);
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.05f);
        Collider2D[] col = Physics2D.OverlapBoxAll(GameMng.ins.CamHold.position, BoxSize, 0, GameMng.ins.EnemyMask.value);
        for (int i = 0; i < col.Length; i++)
        {
            Debug.Log(col[i].name);
            Enemy enemy = col[i].GetComponent<Enemy>();
            enemy.TakeDmg(enemy.fHp + 1);
        }
        StartCoroutine(ColliderFalse());
    }

    IEnumerator ColliderFalse()
    {
        yield return new WaitForSeconds(0.2f);
        Player.Ins.SetMove(false, false);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(GameMng.ins.CamHold.position, BoxSize);
    //}
}
