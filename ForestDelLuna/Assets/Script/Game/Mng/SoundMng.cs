using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Sound
{
    public enum AUDIOTYPE { SFX, BG }
    public AUDIOTYPE type;

    public AudioClip clip;
    public string name;
    public float fVolume;
    public bool bLoop;
    AudioSource source;


    public void SetSound(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = bLoop;
        source.volume = fVolume;
        source.playOnAwake = false;
    }

    public void SoundInfoUpdate()
    {
        source.loop = bLoop;

        if (type == AUDIOTYPE.BG)
            source.volume = fVolume * DataMng.ins.fMusicVolume;
        else
            source.volume = fVolume * DataMng.ins.fSFXVolume;


        if (GameMng.ins.bPause)
            source.Pause();
        else
            source.UnPause();

    }

    public void Play() { source.Play(); }
    public void Stop() { source.Stop(); }
    public void Pause() { source.Pause(); }
    public void UnPause() { source.UnPause(); }
    public bool IsPlay() { return source.isPlaying; }
    public void SetVolume(float vol) { fVolume = vol; }
    public void SetLoop(bool loop) { bLoop = loop; }
}

public class SoundMng : MonoBehaviour
{
    [SerializeField]
    public Sound[] sound;
    public AudioSource btn;
    private static SoundMng soungMng;
    public static SoundMng Ins
    {
        get
        {
            if (soungMng == null)
            {
                soungMng = FindObjectOfType<SoundMng>();

                if (soungMng == null)
                {
                    GameObject soundObj = new GameObject();
                    soundObj.name = "SoundMng";
                    soungMng = soundObj.AddComponent<SoundMng>();
                }
            }
            return soungMng;
        }
    }

    void Start()
    {
        for (int i = 0; i < sound.Length; i++) 
        {
            GameObject snd = new GameObject("Sound : " + sound[i].name);
            sound[i].SetSound(snd.AddComponent<AudioSource>());
            snd.transform.SetParent(gameObject.transform);
        }
    }

    void Update()
    {
        for (int i = 0; i < sound.Length; i++)
            sound[i].SoundInfoUpdate();

        btn.volume = DataMng.ins.fSFXVolume;
    }

    public void Play(string _name)
    {
        for (int i = 0; i < sound.Length; i++) 
        {
            if(sound[i].name == _name)
            {
                sound[i].Play();
                break;
            }
        }
    }


    public void Stop(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].name == _name)
            {
                sound[i].Stop();
                break;
            }
        }
    }

    public void SetVolume(string _name, float vol)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].name == _name)
            {
                sound[i].SetVolume(vol);
                break;
            }
        }
    }

    public void SetLoop(string _name, bool Loop)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].name == _name)
            {
                sound[i].SetLoop(Loop);
                break;
            }
        }
    }

    public void Pause(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].name == _name)
            {
                sound[i].Pause();
                break;
            }
        }
    }

    public void UnPause(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].name == _name)
            {
                sound[i].UnPause();
                break;
            }
        }
    }

    public bool IsPlay(string _name)
    {
        for (int i = 0; i < sound.Length; i++)
        {
            if (sound[i].name == _name)
            {
                return sound[i].IsPlay();
            }
        }
        return false;
    }
}
