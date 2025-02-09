using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    Transform Top;
    Transform Bottom;
    Transform ElevatorTrans;
    Rigidbody2D ElevatorRigid;
    [Header("Is Elevator Heigth true == top, false == Bottom")]
    public bool bElevator;
    bool bElevatorUse = false;
    bool bCollision;

    // Start is called before the first frame update
    void Start()
    {
        ElevatorTrans = GetComponent<Transform>().GetChild(0).GetComponent<Transform>();
        ElevatorRigid = ElevatorTrans.GetComponent<Rigidbody2D>();
        Top = GetComponent<Transform>().GetChild(1).GetComponent<Transform>();
        Bottom = GetComponent<Transform>().GetChild(2).GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {


        if (bElevatorUse)
        {
            if (bElevator)
            {
                if (ElevatorTrans.position.y <= Bottom.position.y)
                {
                    SoundMng.Ins.Stop("Chain");
                    ElevatorTrans.position = Bottom.position;
                    ElevatorRigid.velocity = new Vector2(0, 0);
                    bElevator = false;
                    bElevatorUse = false;
                }
                else
                {
                    if (!SoundMng.Ins.IsPlay("Chain"))
                    {
                        SoundMng.Ins.Play("Chain");
                    }
                    ElevatorRigid.velocity = new Vector2(0, -4);
                }
            }
            else
            {

                if (ElevatorTrans.position.y >= Top.position.y)
                {
                    ElevatorRigid.velocity = new Vector2(0, 0);
                    ElevatorTrans.position = Top.position;
                    SoundMng.Ins.Stop("Chain");
                    bElevator = true;
                    bElevatorUse = false;
                }
                else
                {
                    if (!SoundMng.Ins.IsPlay("Chain"))
                    {
                        SoundMng.Ins.Play("Chain");
                    }
                    ElevatorRigid.velocity = new Vector2(0, 4);
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Tab) && bCollision)
        {
            bElevatorUse = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bCollision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bCollision = false;
        }
    }
}
