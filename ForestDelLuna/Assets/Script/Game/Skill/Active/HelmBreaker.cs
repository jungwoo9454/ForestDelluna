using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class HelmBreaker : Skill
{
    private bool _isSkillUse;
    
    new Collider2D collider2D;
    //스킬은 처음 시전하였을때(키보드에서 누를시) 딱 1번 들어오는곳
    public override void SkillOn()
    {
        ColRot();
        if(!Player.Ins.bIsOnGround)
        {
            if (collider2D == null)
                collider2D = GetComponent<Collider2D>();

            collider2D.enabled = true;
            Player.Ins.PlayerRigid.velocity = new Vector2(Player.Ins.PlayerRigid.velocity.x, -40);
            Player.Ins.bBurst = true;

            Player.Ins.PlayerAni.Play("HelmBreaker");

            _isSkillUse = true;
            
            StartCoroutine(Colco());
        }
        else
        {
            fCoolTime = fCoolDown;
        }
    }

    IEnumerator Colco()
    {
        yield return new WaitForSeconds(0.4f);
        collider2D.enabled = false;
    }

    //스킬의 쿨이 돌때 계속 돌아옴
    public override void SkillUse()
    {
        if (Player.Ins.bIsOnGround)
        {
            _isSkillUse = false;
            Player.Ins.PlayerAni.enabled = true;
        }
        else if(_isSkillUse)
        {
            if (Player.Ins.PlayerAni.GetCurrentAnimatorClipInfoCount(0) >= 0)
            {
                Player.Ins.PlayerAni.enabled = false;
            }
        }
        //collider2D.enabled = false;
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
            if (!Player.Ins.bIsOnGround)
            {
                collider.GetComponent<Enemy>().TakeDmg(Player.Ins.GetAttack() * 1.5f);
                collider.GetComponent<Rigidbody2D>().velocity = new Vector2(collider.GetComponent<Rigidbody2D>().velocity.x, -15);
            }
        }
    }
}
