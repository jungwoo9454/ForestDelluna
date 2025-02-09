using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Game/Skill")]
public class SkillInfo : ScriptableObject
{
    public SKILLTYPE Type;
    public Sprite SkillIcon;
    public new string name;
    public string info;
    public float fCollDown;
}