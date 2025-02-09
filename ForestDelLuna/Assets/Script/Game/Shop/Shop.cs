using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    ItemUse use;

    Transform ShopTrans;
    Transform infoPannel;
    SpriteRenderer ShopRenderer;

    Text Pricetext;
    Text Thingname;
    Text ThingInfo;

    Image ImageIcon;

    float fTime;
    bool bDown = false;

    public ItemInfo Iteminfo;
    public int nPrice;
    new string name;
    string info;
    string FunctionName;
    
    void Start()
    {
        name = Iteminfo.name;
        info = Iteminfo.info;
        FunctionName = Iteminfo.Codename;


        ShopTrans = GetComponent<Transform>();
        ShopRenderer = GetComponent<SpriteRenderer>();
        Pricetext = transform.GetChild(0).GetComponentInChildren<Text>();
        Thingname = transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Text>();
        ThingInfo = transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<Text>();
        ImageIcon = transform.GetChild(0).GetChild(1).GetChild(4).GetComponent<Image>();

        ShopRenderer.sprite = Iteminfo.sprite;

        infoPannel = transform.GetChild(0).GetChild(1);
        Pricetext.text = nPrice.ToString();
        Thingname.text = name;
        ThingInfo.text = info;
        ImageIcon.sprite = GetComponent<SpriteRenderer>().sprite;

        use = EntityMng.ins.GetItemUseFunc(FunctionName);
    }

    void Update()
    {
        fTime += Time.deltaTime;
        if (fTime >= 1)
        {
            fTime = 0;
            bDown = !bDown;
        }

        if (bDown)
            ShopTrans.Translate(0, Time.deltaTime * 0.5f, 0);
        else
            ShopTrans.Translate(0, -(Time.deltaTime * 0.5f), 0);
    }
    public virtual void Buy()
    {
        use();
        Player.Ins.MinGold(nPrice);
        gameObject.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            infoPannel.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Tab) && Player.Ins.GetGoldUse(nPrice))
            {
                SoundMng.Ins.Play("Buy");
                Buy();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            infoPannel.gameObject.SetActive(false);
        }
    }
}
