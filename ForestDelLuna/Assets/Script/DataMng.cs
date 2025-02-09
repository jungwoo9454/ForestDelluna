using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMng : MonoBehaviour
{
    private static DataMng instance;
    public static DataMng ins
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataMng>();
                if (instance != null)
                    DontDestroyOnLoad(instance.gameObject);

                if (instance == null)
                {
                    GameObject dateobj = new GameObject("DataMng");
                    instance = dateobj.AddComponent<DataMng>();
                    DontDestroyOnLoad(dateobj);
                }
            }
            return instance;
        }
    }

    public float fSFXVolume;
    public float fMusicVolume;
}
