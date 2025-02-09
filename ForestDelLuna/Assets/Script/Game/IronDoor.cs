using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronDoor : MonoBehaviour
{
    public Transform Door;
    public Transform Bottom;
    public Transform Top;


    bool bDoor;

    void Update()
    {
        if (bDoor)
        {
            if (Door.position.y > Top.position.y)
            {
                Door.position = new Vector3(Door.position.x, Top.position.y, 0);
                SoundMng.Ins.Stop("IronDoor");
            }
            else
            {
                if (!SoundMng.Ins.IsPlay("IronDoor"))
                    //SoundMng.Ins.Play("IronDoor");
                Door.Translate(0, 3 * Time.deltaTime, 0);
            }
        }
        else
        {
            if (Door.position.y < Bottom.position.y)
            {
                Door.position = new Vector3(Door.position.x, Bottom.position.y, 0);
                SoundMng.Ins.Stop("IronDoor");
            }
            else
            {
                if (!SoundMng.Ins.IsPlay("IronDoor"))
                    //SoundMng.Ins.Play("IronDoor");
                Door.Translate(0, -3 * Time.deltaTime, 0);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            bDoor = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bDoor = false;
        }
    }
}
