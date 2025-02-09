using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeAttack : Skill
{
    new Collider2D collider2D;
    //스킬은 처음 시전하였을때(키보드에서 누를시) 딱 1번 들어오는곳
    public override void SkillOn()
    {
        if (collider2D == null)
            collider2D = GetComponent<Collider2D>();

        collider2D.enabled = true;
    }

    //스킬의 쿨이 돌때 계속 돌아옴
    public override void SkillUse()
    {
        collider2D.enabled = false;
    }

    public override void Reset()
    {
        if (collider2D == null)
            collider2D = GetComponent<Collider2D>();

        collider2D.enabled = false;
    }

    //이거 그냥 추가한거임 별도의 기본구조에서 더 필요하다 싶으면 그냥 이 코드와 적용할 오브젝트에서 더 추가시킴 됨
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            for(int i = 0; i< 2; i++)
            {
                collider.GetComponent<Enemy>().TakeDmg(Player.Ins.GetAttack() * 1.2f);
                collider2D.enabled = false;
            }

            collider.GetComponent<Enemy>().TakeDmg(Player.Ins.GetAttack() * 1.5f);
            collider2D.enabled = false;
        }
    }
}
