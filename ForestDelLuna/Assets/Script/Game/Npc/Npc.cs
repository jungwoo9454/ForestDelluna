using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    SpriteRenderer MyRenderer;
    GameObject ShopUI;
    void Start()
    {
        MyRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(SpriteFlip());
    }

    IEnumerator SpriteFlip()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 10.0f));
            MyRenderer.flipX = !MyRenderer.flipX;
        }
    }
}
