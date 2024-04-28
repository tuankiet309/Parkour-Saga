using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    private int bgmIndex;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PlayBGM(Random.Range(0,bgm.Length));
    }
    private void Update()
    {
        if (!bgm[bgmIndex].isPlaying)
        {
            PlayRandomBGM();
        }
    }
    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }
    public void PlaySFX(int index)
    {
        if(index<sfx.Length)
        {
            sfx[index].pitch = Random.Range(0.65f, 1.45f);
            sfx[index].Play();
        }
    }   
    public void StopSFX(int index)
    {
        sfx[index].Stop();
    }
    public void PlayBGM(int index)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
        bgm[index].Play();
    }
    public void StopBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
}

