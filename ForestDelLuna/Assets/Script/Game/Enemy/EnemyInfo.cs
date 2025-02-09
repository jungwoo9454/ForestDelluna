using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Game/Enemy")]
public class EnemyInfo : ScriptableObject
{
    public new string name;
    public bool bUse;
    public float fMoveSpeed;
    public float fMaxHp;
    public float fAttack;
    public float fAttackSpeed;
}
