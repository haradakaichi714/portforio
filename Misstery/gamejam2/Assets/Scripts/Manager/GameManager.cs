using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private enum E_GAMEBUTTON
    {
        NOGUILTY1, NOGUILTY2, NOGUILTY3, GUILTY, POLICE, BACK
    }

    [Tooltip("ボタンオブジェクトを格納(GameObject[])")]
    [SerializeField] Button[] Buttons;

     SuspectFactory suspectfactorySc;

    [SerializeField] GameObject m_PickedupEvidenceObject;


    public GameObject m_suspectDetail;

    //情報テキスト表示スクリプト取得用オブジェクト
    [SerializeField] GameObject m_infomationText;

    private AudioSource bgm_game, se_page, se_button;
    private float bgmVolume;


    // Start is called before the first frame update
    void Start()
    {

        se_button = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_BUTTON);

        se_page = GameObject.FindWithTag("AudioManager")
           .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_PAGE);

        bgm_game = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.BGM_GAME);

        bgmVolume = bgm_game.volume;
        bgm_game.volume = 0;

        bgm_game.Play();
        bgm_game.DOFade(bgmVolume, 1.0f);

        Buttons[(int)E_GAMEBUTTON.BACK].gameObject.SetActive(false);


        m_suspectDetail.SetActive(false);

        suspectfactorySc = GameObject.Find("SuspectFactory").GetComponent<SuspectFactory>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Buttons[(int)E_GAMEBUTTON.NOGUILTY1].GetComponent<ButtonClick>().m_click)
        {
            //無実の人の情報を表示
            Debug.Log("無実");
            PushSuspectButton(E_GAMEBUTTON.NOGUILTY1);
        }
        if (Buttons[(int)E_GAMEBUTTON.NOGUILTY2].GetComponent<ButtonClick>().m_click)
        {
            //無実の人の情報を表示
            Debug.Log("無実");
            PushSuspectButton(E_GAMEBUTTON.NOGUILTY2);
        }
        if (Buttons[(int)E_GAMEBUTTON.NOGUILTY3].GetComponent<ButtonClick>().m_click)
        {
            //無実の人の情報を表示
            Debug.Log("無実");
            PushSuspectButton(E_GAMEBUTTON.NOGUILTY3);
        }


        if (Buttons[(int)E_GAMEBUTTON.GUILTY].GetComponent<ButtonClick>().m_click)
        {
            //犯人の情報を表示
            Debug.Log("犯人");
            PushSuspectButton(E_GAMEBUTTON.GUILTY);
        }

        if (Buttons[(int)E_GAMEBUTTON.POLICE].GetComponent<ButtonClick>().m_click)
        {
            se_button.Play();

            bgm_game.DOFade(0.0f, 1.0f).OnComplete(()=> { bgm_game.Stop(); bgm_game.volume = bgmVolume; });
            //このステージで使用した容疑者の情報(画像等)を渡す
            DataManager.Instance.stageSuspect = suspectfactorySc.m_arraySuspectData[DataManager.Instance.stage];

            //容疑者たちの最終評価値の値を指名フェーズに持ち出すための処理
            DataManager.Instance.m_evidenceValue = this.GetComponent<CalculationValue>().m_evidenceValue;

            for (int i = 0; i < DataManager.Instance.stageSuspect.NoGuilty.Length + 1; i++)
            {
                if (DataManager.Instance.m_evidenceValue[i] >= 0)
                {
                    Debug.Log(i + "番目の最終評価点 : " + DataManager.Instance.m_evidenceValue[i]);
                }
                else
                {
                    Debug.LogError(i + "番目のどこかで点数のエラーが発生しています");
                }
            }

            Fade.Instance.FadeOut("FinalPhase");
            Buttons[(int)E_GAMEBUTTON.POLICE].GetComponent<ButtonClick>().m_click = false;
        }

        if (Buttons[(int)E_GAMEBUTTON.BACK].GetComponent<ButtonClick>().m_click)
        {
            //戻るボタンが押されたときの処理
            se_page.Play();

            suspectfactorySc.SetStageSuspect();

            m_PickedupEvidenceObject.SetActive(true);

            Buttons[(int)E_GAMEBUTTON.POLICE].gameObject.SetActive(true);
            Buttons[(int)E_GAMEBUTTON.BACK].gameObject.SetActive(false);
            m_infomationText.SetActive(false);

            //詳細ページではないのでfalseに
            m_suspectDetail.SetActive(false);

            Buttons[(int)E_GAMEBUTTON.BACK].GetComponent<ButtonClick>().m_click = false;
        }
    }


    /// <summary>
    /// 被疑者のボタンが押されたときに呼ぶメソッド
    /// </summary>
    /// <param name="_button"></param>押されたメソッド(Enum)を指定
    void PushSuspectButton(E_GAMEBUTTON _button)
    {
        se_page.Play();

        m_PickedupEvidenceObject.SetActive(false);

        for (int i = 0; i < Buttons.Length; ++i)
        {//すべてのボタンを非アクティブに
            Buttons[i].gameObject.SetActive(false);
        }
        m_infomationText.SetActive(true);

        if (_button == E_GAMEBUTTON.GUILTY)
        {
            //情報表示テキスト関数の呼び出し
            m_infomationText.GetComponent<InfomationText>().SetText(suspectfactorySc.GetText(0));
        }
        else if (_button == E_GAMEBUTTON.NOGUILTY1)
        {//1人目の無実の人
            m_infomationText.GetComponent<InfomationText>().SetText(suspectfactorySc.GetText(1));
        }
        else if (_button == E_GAMEBUTTON.NOGUILTY2)
        {//2人目の無実の人
            m_infomationText.GetComponent<InfomationText>().SetText(suspectfactorySc.GetText(2));
        }
        else if (_button == E_GAMEBUTTON.NOGUILTY3)
        {//3人目の無実の人
            m_infomationText.GetComponent<InfomationText>().SetText(suspectfactorySc.GetText(3));
        }


        //戻るボタンは押せるように
        Buttons[(int)E_GAMEBUTTON.BACK].gameObject.SetActive(true);

        //押されたボタンから画像を取得

        m_suspectDetail.GetComponent<Image>().sprite = Buttons[(int)_button].GetComponent<Image>().sprite;

        m_suspectDetail.SetActive(true);

        Buttons[(int)_button].GetComponent<ButtonClick>().m_click = false;
    }
}