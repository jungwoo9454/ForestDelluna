using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPad : MonoBehaviour
{
    public bool IsActive;
    float fStandTime;
    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.CompareTag("Player") && IsActive)
        {
            fStandTime += Time.deltaTime;
            if(fStandTime >= 1f)
            {
                fStandTime = 0;
                Player.Ins.TakeDmg(18);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            fStandTime = 0;
        }
    }
}
