using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageManager : MonoBehaviour
{

    private enum E_STAGEBUTTON
    {
        PLAY,
        NEXTSTAGE,
        BACKSTAGE,
        MAX
    }


    //--------UI関連--------//


    //ステージ人の写真テクスチャ
    public Image m_guiltySprite;
    public Image[] m_noGuiltySprite;

    [Tooltip("容疑者の顔写真")]
    [SerializeField] public Sprite[] m_suspectsIcon;

    //ステージ詳細テキスト
    public GameObject m_stageInfoText;
    public GameObject m_stageNumberText;

    public Button[] stageButtons;

    private AudioSource bgm_stageSelect, se_button, se_page;
    private float bgmVolume;

    // Start is called before the first frame update
    void Start()
    {
        se_page = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_PAGE);

        se_button = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_BUTTON);

        bgm_stageSelect = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.BGM_SELECT);

        bgmVolume = bgm_stageSelect.volume;
        bgm_stageSelect.volume = 0;

        bgm_stageSelect.Play();
        bgm_stageSelect.DOFade(bgmVolume, 1.0f);


        DataManager.Instance.stage = 0;
        //初期化
        for(int i = 0; i < stageButtons.Length;i++)
        {

            stageButtons[i].GetComponent<ButtonClick>().m_click = false;

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (stageButtons[(int)E_STAGEBUTTON.PLAY].GetComponent<ButtonClick>().m_click)
        {
            se_button.Play();
            stageButtons[(int)E_STAGEBUTTON.PLAY].GetComponent<ButtonClick>().m_click = false;
            bgm_stageSelect.DOFade(0.0f, 1.0f).OnComplete(() => { bgm_stageSelect.Stop(); bgm_stageSelect.volume = bgmVolume; });
            Fade.Instance.FadeOut("Game");
        }

        if (stageButtons[(int)E_STAGEBUTTON.NEXTSTAGE].GetComponent<ButtonClick>().m_click)
        {
            se_page.Play();
            if (DataManager.Instance.stage == 0)
            {

                stageButtons[(int)E_STAGEBUTTON.BACKSTAGE].gameObject.SetActive(true);
            }

            DataManager.Instance.stage++;
            stageButtons[(int)E_STAGEBUTTON.NEXTSTAGE].GetComponent<ButtonClick>().m_click = false;
        }

        if (stageButtons[(int)E_STAGEBUTTON.BACKSTAGE].GetComponent<ButtonClick>().m_click)
        {
            se_page.Play();
            if (DataManager.Instance.stage == 2)
            {

                stageButtons[(int)E_STAGEBUTTON.NEXTSTAGE].gameObject.SetActive(true);
            }

            DataManager.Instance.stage--;
            stageButtons[(int)E_STAGEBUTTON.BACKSTAGE].GetComponent<ButtonClick>().m_click = false;
        }

        if (DataManager.Instance.stage == 2)
        {

            stageButtons[(int)E_STAGEBUTTON.NEXTSTAGE].gameObject.SetActive(false);

        }
        else if(DataManager.Instance.stage==0)
        {

            stageButtons[(int)E_STAGEBUTTON.BACKSTAGE].gameObject.SetActive(false);

        }


        switch (DataManager.Instance.stage)
        {
            case 0:

                m_guiltySprite.sprite = m_suspectsIcon[1];
                m_noGuiltySprite[0].sprite = m_suspectsIcon[0];
                m_noGuiltySprite[1].sprite = m_suspectsIcon[2];

                m_stageInfoText.GetComponent<Text>().text = "犯人:母親\n"+
                                                            "あなたの母親がおとといに殺人を犯した。" +
                                                            "彼は他の二人と容疑\n者と" +
                                                            "して疑われた。"+
                                                            "母親の為、彼女に不利な証拠を隠蔽して、\n" +
                                                            "警察に捜査をあやまらせよう。";
                m_stageNumberText.GetComponent<Text>().text = "Stage" + (DataManager.Instance.stage + 1);
                break;

            case 1:

                m_guiltySprite.sprite = m_suspectsIcon[5];
                m_noGuiltySprite[0].sprite = m_suspectsIcon[3];
                m_noGuiltySprite[1].sprite = m_suspectsIcon[4];


                m_stageInfoText.GetComponent<Text>().text = "犯人:父親\n"+
                                                            "あなたの父親が他人と喧嘩して殺人を犯した。"+
                                                            "警察の同僚は現\n場の" +
                                                            "証拠から彼を疑っている。"+
                                                            "父親の為、署内の資料で彼に不\n利な証" +
                                                            "拠品を隠蔽して、"+
                                                            "捜査をあやまらせよう。";
                m_stageNumberText.GetComponent<Text>().text = "Stage" + (DataManager.Instance.stage + 1);
                break;

            case 2:

                m_guiltySprite.sprite = m_suspectsIcon[6];
                m_noGuiltySprite[0].sprite = m_suspectsIcon[7];
                m_noGuiltySprite[1].sprite = m_suspectsIcon[9];


                m_stageInfoText.GetComponent<Text>().text = "犯人:恋人\n"+
                                                            "あなたと6年以上付き合った恋人は上司への不満で殺人を犯し\n" +
                                                            "た。第三捜査課の同僚は現場の証拠から彼女を疑っている。\n" +
                                                            "恋人が入獄しないように、証拠情報を調べ、彼女に不利\n" +
                                                            "な証拠を隠蔽して捜査をあやまらせよう。";
                m_stageNumberText.GetComponent<Text>().text = "Stage" + (DataManager.Instance.stage + 1);
                break;
        }


    }

    

   

}
