using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource _BGMSource;
    public AudioSource _SFXSource;

    public List<SFX_Data> sFX_Datas;

    public Dictionary<string, AudioClip> SFXDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Initialise dictionary
        SFXDictionary = new Dictionary<string, AudioClip>();

        UpdateSFX();

        //Adding SFX data into Dictionary<>
        for (int i = 0; i < sFX_Datas.Count; i++)
        {
            SFXDictionary.Add(sFX_Datas[i].ClipName, sFX_Datas[i].SFX);
        }
    }

    private void Update()
    {
        if (_BGMSource.isPlaying)
        {
            return;
        }
        else
        {
            PlayBGM();
        }
    }

    public void UpdateSFX()
    {
        var clips_SFX = Resources.LoadAll("SFX", typeof(AudioClip)).Cast<AudioClip>().ToArray();

        sFX_Datas = new List<SFX_Data>();

        for (int i = 0; i < clips_SFX.Length; i++)
        {
            SFX_Data sFX_Data = new SFX_Data();

            sFX_Data.SFX = clips_SFX[i];
            sFX_Data.ClipName = clips_SFX[i].name;

            sFX_Datas.Add(sFX_Data);
        }
    }

    public void PlayBGM()
    {
        _BGMSource.Play();
    }

    public void PlaySFX(string clipName)
    {
        AudioClip audioClip;

        if (SFXDictionary.TryGetValue(clipName, out audioClip))
        {
            _SFXSource.Stop();
            _SFXSource.clip = audioClip;
            _SFXSource.Play();
        }
    }
}

public class SFX_Data
{
    public string ClipName;
    public AudioClip SFX;
}