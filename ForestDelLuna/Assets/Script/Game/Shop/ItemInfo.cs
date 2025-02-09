using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
public class ItemInfo : ScriptableObject
{
    public Sprite sprite;
    public new string name;
    public string Codename;
    public string info;
}
