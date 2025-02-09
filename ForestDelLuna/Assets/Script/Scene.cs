using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{

    public Slider LoadingSlider;
    public GameObject LoadingObj;
    public Text ProgressText;

    Animator SceneAni;
    string Scenename;

    private static Scene bulletMng;
    public static Scene ins
    {
        get
        {
            if (bulletMng == null)
            {
                bulletMng = FindObjectOfType<Scene>();

                if (bulletMng == null)
                {
                    GameObject bulletObj = new GameObject();
                    bulletObj.name = "BulletMng";
                    bulletMng = bulletObj.AddComponent<Scene>();
                }
            }
            return bulletMng;
        }
    }

    void Start()
    {
        SceneAni = GetComponent<Animator>();
    }

    public void FadeScene(string name)
    {
        SceneAni.SetTrigger("Fade_Out");
        Scenename = name;
    }

    public void LoadingScene(string name)
    {
        StartCoroutine(LoadAsynchronously(name));
    }

    IEnumerator LoadAsynchronously(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingSlider.value = progress;
            ProgressText.text = progress * 100f + "%";

            LoadingObj.SetActive(true);

            yield return null;
        }
    }

    public void OnFadeSceneLoad()
    {
        SceneManager.LoadScene(Scenename);
    }
}
