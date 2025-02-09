using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform CamHold;
    public float fSmoothScale;
    Vector3 CamOffSet;
    //bool bMove = true;

    void Start()
    {
        CamOffSet = CamHold.position - Player.Ins.transform.position;
    }

    void Update()
    {
        if(!GameMng.ins.bDonMoveCamera)
        {
            //CamHold.position = Vector3.Lerp(CamHold.position, Player.Ins.transform.position + CamOffSet, Time.deltaTime * 5);
            CamHold.position = Player.Ins.transform.position + CamOffSet;
        }
    }

    //void FixedUpdate()
    //{
    //    if(bMove)
    //    {
    //        Vector3 MovePos = Vector3.zero;

    //        MovePos = Vector3.Lerp(CamHold.position, Player.Ins.transform.position + CamOffSet, Time.deltaTime * fSmoothScale);

    //        CamHold.position = MovePos;
    //    }
    //}

}
