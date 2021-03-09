using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    public GameObject[] m_cableCars;
    public float m_cableVelocity;


    [SerializeField] public Button button;     //ゲームを始めるボタン

    private AudioSource bgm_title,se_button;
    private float bgmVolume;

    // Start is called before the first frame update
    void Start()
    {
        se_button = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_BUTTON);

        bgm_title = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.BGM_TITLE);

        bgmVolume = bgm_title.volume;
        bgm_title.volume = 0;

        bgm_title.Play();
        bgm_title.DOFade(bgmVolume, 1.0f);

        //ケーブルカーの移動処理
        for (int i = 0; i < m_cableCars.Length; i++)
        {

            m_cableCars[i].transform.DOMoveX(-10, 10.0f+(1*i), true).From(10+(10*i), false).SetEase(Ease.Linear).OnComplete(() =>
            { m_cableCars[i].transform.DOMoveX(-10, 10.0f + (1 * i), true).From(10 + (10 * i), false).SetEase(Ease.Linear); }).SetLoops(-1);

        }



    }

    // Update is called once per frame
    void Update()
    {

        //始めるボタンを押したらtrue
        if (button.GetComponent<ButtonClick>().m_click)     
        {
            se_button.Play();
            button.GetComponent<ButtonClick>().m_click = false;
            //ゲームシーンへ移動
            bgm_title.DOFade(0.0f, 1.0f).OnComplete(() => { bgm_title.Stop(); bgm_title.volume = bgmVolume; }) ;
            Fade.Instance.FadeOut("StageSelect");
        }

    }
}
