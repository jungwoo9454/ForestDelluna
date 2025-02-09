using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAttack : Skill
{
    //스킬은 처음 시전하였을때(키보드에서 누를시) 딱 1번 들어오는곳
    public override void SkillOn()
    {
        Player.Ins.PlayerAni.Play("Wave Attack");
    }


    //이거 그냥 추가한거임 별도의 기본구조에서 더 필요하다 싶으면 그냥 이 코드와 적용할 오브젝트에서 더 추가시킴 됨
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.GetComponent<Enemy>().TakeDmg(Player.Ins.GetAttack() * 1.3f);


            Vector2 heading = collider.transform.position - Player.Ins.transform.position;
            float dis = heading.magnitude;
            Vector2 dir = heading / dis;

            collider.GetComponent<Enemy>().KnockBack(new Vector2(dir.x * 5, 10), 3);
        }
    }
}
