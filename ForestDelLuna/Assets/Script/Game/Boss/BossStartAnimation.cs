using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartAnimation : MonoBehaviour
{
    public bool bAni = false;
    public bool bEnd = false;

    public GameObject Boos;
    public GameObject MiniMap;
    public Transform BossTrans;
    public Vector3 PrevPlayerPos;
    public float fAniTime;

    void Update()
    {
        if(bAni)
        {
            MiniMap.SetActive(false);
            fAniTime += Time.deltaTime;

            if (fAniTime <= 7f)
                GameMng.ins.CamHold.position = Vector3.Lerp(GameMng.ins.CamHold.position, BossTrans.position, Time.deltaTime);
            else
                GameMng.ins.CamHold.position = Vector3.Lerp(GameMng.ins.CamHold.position, PrevPlayerPos, 0.1f);

            if(fAniTime >= 8f)
            {
                Player.Ins.SetMove(false, false);
                GameMng.ins.bDonMoveCamera = false;
                bAni = false;
                UiMng.Ins.BossStartAni.SetActive(false);
                Boos.SetActive(true);
            }
            //else
            //{
            //    Player.Ins.PlayerAni.ResetTrigger("Run");
            //    Player.Ins.PlayerAni.SetTrigger("Idle");
            //}
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            if(!bAni && !GameMng.ins.bBoss)
            {
                GameMng.ins.bBoss = true;
                bAni = true;
                PrevPlayerPos = GameMng.ins.CamHold.position;
                GameMng.ins.bDonMoveCamera = true;
                Player.Ins.SetMove(true, true);
                UiMng.Ins.BossStartUI();
                fAniTime = 0;
            }
        }
    }
}
