using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum E_SUSPECT
{
    STAGE1_GUILTY,
    STAGE1_NOGUILTY,
    STAGE2_GUILTY,
    STAGE2_NOGUILTY,
    STAGE3_GUILTY,
    STAGE3_NOGUILTY,
    MAX,
}


[System.Serializable]
public struct SuspectInfo
{
    [Header("情報を指定したい人の画像")]
    public Sprite sprite;

    [TextArea(0, 3)]
    [Header("この指定した人の情報")]
    public string details;

    [Header("この指定した人の関係する証拠品")]
    public E_EVIDENCE[] evidence;
}


[System.Serializable]
public struct tagStageSuspectData
{

    [Header("有罪の人")]
    public
    SuspectInfo Guilty;

    [Header("無実の人の配列")]
    public
    SuspectInfo[] NoGuilty;
}

public class SuspectFactory : MonoBehaviour
{
    /// <summary>
    /// シーン上の犯人のボタンを格納
    /// </summary>
    [Header("有罪の人のボタン")]
    [SerializeField] Button m_buttonGuilty = null;

    /// <summary>
    ///シーン上の無実の人のボタンを格納
    /// </summary>
    [Header("無罪の人のボタン（複数）")]
    [SerializeField] Button[] m_buttonNoGuilty = new Button[3];

    /// <summary>
    /// 犯人と無実の人の画像や情報が入っている
    /// </summary>
    [Header("ステージごとの無罪、有罪の人の情報が入っている配列")]
    public tagStageSuspectData[] m_arraySuspectData = new tagStageSuspectData[3];

    // Start is called before the first frame update
    void Start()
    {

        SetAllButtonFalse();
        SetStageSuspect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// ステージ指定時にボタンの画像を指定していたものに差し替えるメソッド
    /// </summary>
    public void SetStageSuspect()
    {
        int stagenum = DataManager.Instance.stage;

        

        m_buttonGuilty.image.sprite = m_arraySuspectData[stagenum].Guilty.sprite;
        m_buttonGuilty.gameObject.SetActive(true);

        for (int i = 0; i < m_arraySuspectData[stagenum].NoGuilty.Length; i++)
        {//指定していた数だけ表示する
            //画像の差し替え
            m_buttonNoGuilty[i].image.sprite = m_arraySuspectData[stagenum].NoGuilty[i].sprite;

            //表示する無実の人のボタンのみアクティブにする
            m_buttonNoGuilty[i].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 指定した容疑者の情報テキストを取得する
    /// </summary>
    /// <param name="details"></param> 0が犯人 1-3が無実の人
    /// <returns></returns> 取得したい容疑者の情報テキスト
    public string GetText(int details)
    {
        int stagenum = DataManager.Instance.stage;

        switch (details)
        {
            case 0:
                return m_arraySuspectData[stagenum].Guilty.details;
            case 1:
                return m_arraySuspectData[stagenum].NoGuilty[0].details;
            case 2:
                return m_arraySuspectData[stagenum].NoGuilty[1].details;
            case 3:
                return m_arraySuspectData[stagenum].NoGuilty[2].details;
            default:
                return m_arraySuspectData[stagenum].NoGuilty[0].details;
        }
    }

    /// <summary>
    /// すべてのボタンを非アクティブにする
    /// </summary>
    void SetAllButtonFalse()
    {
        for (int i = 0; i < m_buttonNoGuilty.Length; i++)
        {
            m_buttonNoGuilty[i].gameObject.SetActive(false);
        }
        m_buttonGuilty.gameObject.SetActive(false);
    }
}
