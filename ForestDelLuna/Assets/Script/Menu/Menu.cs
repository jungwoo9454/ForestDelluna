using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public GameObject Option;
    public GameObject tutorial;

    public Text Sfxtext;
    public Text Musictext;

    public Slider Music;
    public Slider Sfx;

    public AudioSource Title;
    public AudioSource btn;

    public void GameStart() { Scene.ins.FadeScene("Game"); }
    public void GameQuit() { Application.Quit(); }

    void Update()
    {
        if(!Title.isPlaying)
        {
            Title.Play();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            tutorial.SetActive(false);
        }

        Sfxtext.text = (DataMng.ins.fSFXVolume * 100).ToString("N1") + "%";
        Musictext.text = (DataMng.ins.fMusicVolume * 100).ToString("N1") + "%";

        Title.volume = DataMng.ins.fMusicVolume;
        btn.volume = DataMng.ins.fSFXVolume;
    }

    public void MusicVol()
    {
        DataMng.ins.fMusicVolume = Music.value;

    }

    public void SfxVol()
    {
        DataMng.ins.fSFXVolume = Sfx.value;
    }

    public void OptionWindow()
    {
        Option.SetActive(true);
    }

    public void TutorialStart()
    {
        Scene.ins.FadeScene("Tutorial");
    }
}
