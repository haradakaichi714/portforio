// @file   SEManager.cs
// @brief  SEマネージャークラスの定義
// @author T,Cho K.Harada
// @date   2020/10/20 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// @name   SEManager
// @brief  SEマネージャークラス
public class SEManager : SingletonMonoBehaviour<SEManager>
{
    AudioSource m_audioSource;
    Dictionary<string, AudioClip> m_seList = new Dictionary<string, AudioClip>();
    // Start is called before the first frame update
    void Start()
    {
        // SE用オーディオソースをコンポーネントに追加
        m_audioSource = this.gameObject.AddComponent<AudioSource>();
        AudioClip audioClip = Resources.Load<AudioClip>("Sounds/SE/StrongClose");
        m_seList.Add("close", audioClip);
        audioClip = Resources.Load<AudioClip>("Sounds/SE/StrongOpen");
        m_seList.Add("open", audioClip);
        audioClip = Resources.Load<AudioClip>("Sounds/SE/ZombieVoice");
        m_seList.Add("zombie", audioClip);
        audioClip = Resources.Load<AudioClip>("Sounds/SE/bomb");
        m_seList.Add("doorBomb", audioClip);

        DontDestroyOnLoad(this);
    }

    // @name   PlaySE
    // @brief  SEを鳴らす
    public void PlaySE(string _str)
    {
        m_audioSource.PlayOneShot(m_seList[_str]);
    }



    // @name   PlaySE
    // @brief  SEを鳴らす
    public IEnumerator PlaySELoop(string _str)
    {
        while (!GameManager.Instance.goalFlag)
        {
            m_audioSource.PlayOneShot(m_seList[_str]);
            yield return new WaitForSeconds(8);
        }
    }
}
