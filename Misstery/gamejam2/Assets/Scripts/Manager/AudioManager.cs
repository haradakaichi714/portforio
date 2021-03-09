using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum E_AUDIOTYPE
    {
        BGM_TITLE,BGM_SELECT,BGM_GAME,BGM_FINAL,BGM_RESULT,BGM_WIN,BGM_LOSE,
        SE_GETEVIDENCE,SE_PAGE,SE_WALK,SE_BUTTON,SE_JUDGE
    }

    [System.Serializable]
    public struct AudioData
    {
        public AudioClip clip;
        public E_AUDIOTYPE type;
        [Range(0, 1)]
        public float volume;

        internal AudioSource source;

        public AudioData(AudioClip clip, E_AUDIOTYPE type,float volume, AudioSource source) : this()
        {
            this.clip = clip;
            this.type = type;
            this.volume = volume;
            this.source = source;
        }
    }

    public List<AudioData> bgm, se;

    public AudioSource GetAudioSourceByType(E_AUDIOTYPE type)
    {
        var source = bgm.Find(data => data.type == type).source;
        if (source)
            return source;

        source = se.Find(data => data.type == type).source;
        if (source)
            return source;

        return null;
    }

    public void UpdateVolume()
    {
        foreach (var audio in bgm)
            audio.source.volume = 1 * audio.volume;

        foreach (var audio in se)
            audio.source.volume = 1 * audio.volume;
    }

    private void Awake()
    {
        //BGM
        for(int i = 0;i<bgm.Count;++i)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = bgm[i].clip;
            source.loop = true;

            bgm[i] = new AudioData(bgm[i].clip, bgm[i].type, bgm[i].volume, source);
        }

        //SE
        for (int i = 0; i < se.Count; ++i)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = se[i].clip;
            source.loop = false;

            se[i] = new AudioData(se[i].clip, se[i].type, se[i].volume, source);
        }

        UpdateVolume();
    }
}
