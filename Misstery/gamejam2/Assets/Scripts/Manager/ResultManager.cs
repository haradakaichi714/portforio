using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ResultManager : MonoBehaviour
{
    [SerializeField] public Button button;
    [SerializeField] GameObject[] resultBg;
    [SerializeField] GameObject[] resultImage;

    private AudioSource bgm_win, bgm_lose,se_button;
    private float bgmVolume;

    // Start is called before the first frame update
    void Start()
    {
        se_button = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_BUTTON);

        bgm_win = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.BGM_WIN);

        bgm_lose = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.BGM_LOSE);

            //勝ちの時の処理
        if(DataManager.Instance.win_flag)
        {
            bgmVolume = bgm_win.volume;
            bgm_win.volume = 0;

            bgm_win.Play();
            bgm_win.DOFade(bgmVolume, 1.0f);

            resultBg[1].SetActive(true);
            resultImage[1].SetActive(true);

            resultBg[0].SetActive(false);
            resultImage[0].SetActive(false);
        }
        else//負けの時の処理
        {
            bgmVolume = bgm_lose.volume;
            bgm_lose.volume = 0;

            bgm_lose.Play();
            bgm_lose.DOFade(bgmVolume, 1.0f);

            resultBg[0].SetActive(true);
            resultImage[0].SetActive(true);

            resultBg[1].SetActive(false);
            resultImage[1].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ステージ選択に戻るボタンを押したらtrue
        if(button.GetComponent<ButtonClick>().m_click)
        {
            se_button.Play();

            button.GetComponent<ButtonClick>().m_click = false;
            //ステージ選択シーンに移動
            if(DataManager.Instance.win_flag)
            {
                bgm_win.DOFade(0.0f, 1.0f).OnComplete(() => { bgm_win.Stop(); bgm_win.volume = bgmVolume; });
            }
            else
            {
                bgm_lose.DOFade(0.0f, 1.0f).OnComplete(() => { bgm_lose.Stop(); bgm_lose.volume = bgmVolume; });
            }
           
            Fade.Instance.FadeOut("StageSelect");
        }
    }
}
