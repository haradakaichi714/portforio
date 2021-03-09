using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FinalManager : MonoBehaviour
{
    /// <summary>
    /// ステージ上で使用sるボタンを格納した配列
    /// </summary>
    [SerializeField]
    Button[] arrayButton = new Button[4];

    [SerializeField]
    Button m_backButton;
    [SerializeField]
    Button m_decideButton;

    /// <summary>
    /// 詳細表示時のキャラ用のスプライト
    /// </summary>
    [SerializeField]
    GameObject m_sprite;


    /// <summary>
    /// この前ステージで使用した被疑者データを格納した配列
    /// </summary>
    tagStageSuspectData m_arraySuspectData;

    //情報表示用変数
    [SerializeField]
    GameObject infomationObject;

    private AudioSource bgm_final,se_page,se_judge;
    private float bgmVolume;

    /// <summary>
    /// 選択している容疑者が何番目か保存する変数
    /// </summary>
    int m_select = 0;

    // Start is called before the first frame update
    void Start()
    {

        se_page = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_PAGE);
        se_judge = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_JUDGE);
        bgm_final = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.BGM_FINAL);

        bgmVolume = bgm_final.volume;
        bgm_final.volume = 0;

        bgm_final.Play();
        bgm_final.DOFade(bgmVolume, 1.0f);

        DataManager datamanager = DataManager.Instance;

        datamanager.getEvidenceNum = 0;

        //DataManagerからもってくる
        m_arraySuspectData = datamanager.stageSuspect;

        SetAllButtonFalse();


        for (int i = 0; i < m_arraySuspectData.NoGuilty.Length; ++i)
        {//ボタンの位置を修正する
            RectTransform buttonrect = arrayButton[i].GetComponent<RectTransform>();

            float posx;
            if (m_arraySuspectData.NoGuilty.Length % 2 == 1)
            {//ボタンの数が1or3のとき
                posx = -buttonrect.rect.width * 2;
                posx += buttonrect.rect.width * 2 * i;
            }
            else
            {//ボタンの数が2のとき
                posx = -buttonrect.rect.width;
                posx += buttonrect.rect.width * 2 * i;
            }
            Vector3 pos = new Vector3(posx, buttonrect.transform.position.y, 0.0f);
            arrayButton[i].GetComponent<RectTransform>().anchoredPosition = pos;
            arrayButton[i].gameObject.SetActive(true);
            arrayButton[i].GetComponent<Image>().sprite = m_arraySuspectData.NoGuilty[i].sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FinalPhasePushButton();
    }

    void SetAllButtonFalse()
    {

        for (int i = 0; i < arrayButton.Length; ++i)
        {
            arrayButton[i].gameObject.SetActive(false);
        }
        m_backButton.gameObject.SetActive(false);
        m_decideButton.gameObject.SetActive(false);
    }
    private void FinalPhasePushButton()
    {
        for (int i = 0; i < m_arraySuspectData.NoGuilty.Length; ++i)
        {
            if (arrayButton[i].GetComponent<ButtonClick>().m_click)
            {
                se_page.Play();
                //無実の人の情報を表示
                PushSuspectButton(i);
                arrayButton[i].GetComponent<ButtonClick>().m_click = false;
            }
        }
        //なすりつけるボタンが押されたとき
        if (m_decideButton.GetComponent<ButtonClick>().m_click)
        {
            se_judge.Play();

            for (int i = 0; i < m_arraySuspectData.NoGuilty.Length; ++i)
            {
                arrayButton[i].gameObject.SetActive(true);
            }

            infomationObject.SetActive(false);

            //詳細ページではないのでfalseに
            m_sprite.SetActive(false);
            m_decideButton.GetComponent<ButtonClick>().m_click = false;
            m_decideButton.gameObject.SetActive(false);
            m_backButton.gameObject.SetActive(false);

            m_select++;

            Debug.Log(m_select + "番目が選択されました");
            if (this.GetComponent<CaluculateSelect>().CaluculateCriminal(DataManager.Instance.m_evidenceValue) == m_select)
            {//なすりつけ成功
                Debug.Log("なすりつけ選びに成功");
                DataManager.Instance.win_flag = true;
            }
            else
            {//なすりつけ失敗
                Debug.Log("なすりつけ選びに失敗");
                DataManager.Instance.win_flag = false;
            }

            //シーン移動
            bgm_final.DOFade(0.0f, 1.0f).OnComplete(() => { bgm_final.Stop(); bgm_final.volume = bgmVolume; });
            Fade.Instance.FadeOut("Result");
        }

        //戻るボタンが押されたとき
        if (m_backButton.GetComponent<ButtonClick>().m_click)
        {
            se_page.Play();

            for (int i = 0; i < m_arraySuspectData.NoGuilty.Length; ++i)
            {
                arrayButton[i].gameObject.SetActive(true);
            }

            infomationObject.SetActive(false);

            //詳細ページではないのでfalseに
            m_sprite.SetActive(false);
            m_decideButton.gameObject.SetActive(false);
            m_backButton.GetComponent<ButtonClick>().m_click = false;
            m_backButton.gameObject.SetActive(false);
        }
    }

    void PushSuspectButton(int _button)
    {
        m_select = _button;

        Debug.Log((_button + 1) + "番目を選択しました");
        SetAllButtonFalse();
        infomationObject.SetActive(true);
        m_backButton.gameObject.SetActive(true);
        m_decideButton.gameObject.SetActive(true);

        string text = m_arraySuspectData.NoGuilty[_button].details;
        infomationObject.GetComponent<InfomationText>().SetText(text);

        //押されたボタンから画像を取得
        m_sprite.GetComponent<Image>().sprite = arrayButton[_button].GetComponent<Image>().sprite;

        //大きさを一緒にする
        m_sprite.GetComponent<RectTransform>().localScale = arrayButton[_button].GetComponent<RectTransform>().localScale;

        //
        m_sprite.GetComponent<RectTransform>().sizeDelta = arrayButton[_button].GetComponent<RectTransform>().sizeDelta;

        m_sprite.SetActive(true);

    }


}
