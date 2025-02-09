using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{
    bool bUse = false;
    bool bReset = false;
    float fAlpha;
    Rigidbody2D TextRigid;
    RectTransform Texttrans;
    Text textEffect;

    public bool GetUse()
    {
        return bUse;
    }

    void Update()
    {
        if(bUse)
        {
            if(TextRigid.velocity.y <= 0)
            {
                fAlpha -= Time.deltaTime;

                Color aColor = textEffect.color;
                aColor.a = fAlpha;
                textEffect.color = aColor;

                if(fAlpha <= 0f)
                {
                    bUse = false;
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(!bReset)
        {
            bReset = true;
            TextRigid.velocity = new Vector2(1, 3);
        }
    }


    public void CreateTextEffect(Vector3 pos, float amount, Color color, string text)
    {
        if(Texttrans == null || TextRigid == null || textEffect== null)
        {
            Texttrans = GetComponent<RectTransform>();
            TextRigid = GetComponent<Rigidbody2D>();
            textEffect = GetComponent<Text>();
        }

        Vector3 AddPos;

        AddPos.x = Random.Range(-1.0f, 1.0f);
        AddPos.y = Random.Range(0.5f, 1.5f);

        fAlpha = 1.2f;


        Texttrans.position = pos + new Vector3(AddPos.x, AddPos.y, 0);
        TextRigid.velocity = new Vector2(1, 3);

        if(text == null)
            textEffect.text = ((int)amount).ToString();
        else
            textEffect.text = text;

        textEffect.color = color;

        bUse = true;
        bReset = false;
        gameObject.SetActive(true);

    }
}
