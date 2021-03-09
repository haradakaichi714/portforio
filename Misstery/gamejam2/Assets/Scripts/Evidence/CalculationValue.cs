using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CalculationValue : MonoBehaviour
{
    /// <summary>
    /// 各容疑者の合計評価値が入っている 0が犯人 1～3が各容疑者
    /// </summary>
    [NonSerialized]
    public int[] m_evidenceValue = new int[4];

    /// <summary>
    /// スクリプト　毎回Findをするのは効率が悪いと思うので
    /// </summary>
    SuspectFactory suspect;

    // Start is called before the first frame update
    void Start()
    {
        SetFirstTotalEvidenceValue();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 最初の各容疑者の初期値を計算するメソッド
    /// 生成された順番によってエラーが発生するので回避するために生成
    /// </summary>
    public void SetFirstTotalEvidenceValue()
    {
        int stage = DataManager.Instance.stage;

        suspect = GameObject.Find("SuspectFactory").GetComponent<SuspectFactory>();

        //犯人の設定された証拠品から犯人の初期評価合計値を計算
        m_evidenceValue[0] = CalculateSuspectValue(suspect.m_arraySuspectData[stage].Guilty.evidence,
                                                     DataManager.Instance.stageData[stage].evidence);

        for (int i = 0; i < suspect.m_arraySuspectData[stage].NoGuilty.Length; ++i)
        {//無実の人をぶんまわす
            m_evidenceValue[i + 1] = CalculateSuspectValue(suspect.m_arraySuspectData[stage].NoGuilty[i].evidence,
                                                 DataManager.Instance.stageData[stage].evidence);
        }
    }

    /// <summary>
    /// 最初の合計値計算に使用する関数
    /// </summary>
    /// <param name="_suspect"></param>容疑者に設定、関連付けされている証拠品配列
    /// <param name="_stagesvidance"></param>ステージに設定、表示される証拠品配列
    /// <returns></returns>この容疑者の評価値の合計
    int CalculateSuspectValue(E_EVIDENCE[] _suspect, E_EVIDENCE[] _stagesvidance)
    {
        EvidenceFactory evidencefac = GameObject.Find("EvidenceFactory").GetComponent<EvidenceFactory>();

        int stage = DataManager.Instance.stage;

        int value = 0;
        //容疑者の設定された証拠品から初期評価合計値を計算する

        for (int j = 0; j < _suspect.Length; ++j)
        {//容疑者の設定された証拠をぶん回す
            for (int i = 0; i < _stagesvidance.Length; ++i)
            {//ステージに出現する証拠品をぶん回す
                if (_stagesvidance[i] == _suspect[j])
                {//ステージに出現するものと容疑者のものが一致した

                    //このあたりでエラーが出たらインスペクター上で犯人に紐づけた証拠品に評価値を設定してない可能性があるので確認してください
                    GameObject obj = evidencefac.GetEvidence(_suspect[j]);
                    //証拠に設定されっている点数を持ってきて足してます
                    int evidencevalue = obj.GetComponent<EvidenceData>().GetEvidenceValue(stage);

                    Debug.Log(_suspect[j] + "の" + evidencevalue + "が足されました");

                    value += evidencevalue;
                    break;
                }
                else if (i == _stagesvidance.Length - 1)
                {//設定されているのにステージに配置しない処理のとき発生
                    Debug.Log("容疑者に設定されている" + _suspect[j] + "はステージ上に存在しません");
                }
            }
        }
        Debug.Log("FirstTotalValue : " + value);
        return value;
    }

    /// <summary>
    /// 当たった証拠品から各評価値を計算するメソッド
    /// TODO 当たったときに呼んでください
    /// </summary>
    /// <param name="_evidence"></param>当たったメソッドのE_EVIDENCE型をいれてください
    public void CheckHitEvidence(E_EVIDENCE _evidence)
    {
        Debug.Log(_evidence + "が計算されました");
        int stage = DataManager.Instance.stage;

        tagStageSuspectData arraysuspect = suspect.m_arraySuspectData[stage];

        //取得した証拠品を取得
        GameObject obj = GameObject.Find("EvidenceFactory").GetComponent<EvidenceFactory>().GetEvidence(_evidence);

        //証拠品の点数を取得
        int evidencevalue = obj.GetComponent<EvidenceData>().GetEvidenceValue(stage);


        //犯人はこの証拠品の対象？か
        m_evidenceValue[0] -= DecreaseEvidenceValue(evidencevalue, arraysuspect.Guilty.evidence, _evidence);

        //無実の人たちはこの
        for (int i = 0; i < suspect.m_arraySuspectData[stage].NoGuilty.Length; ++i)
        {
            m_evidenceValue[i + 1] -= DecreaseEvidenceValue(evidencevalue, arraysuspect.NoGuilty[i].evidence, _evidence);
        }
    }

    /// <summary>
    ///　取得した証拠物をこの容疑者は持っているかを判定
    /// </summary>
    /// <param name="_hitvalue"></param>証拠品の評価値をいれる
    /// <param name="_arrayevidence"></param>容疑者に関連付けした証拠品配列
    /// <param name="_hitevidence"></param>取得した証拠品のE_EVIDENCE型を入れてください
    /// <returns></returns>持っていたら_hitvalueがそのままないなら0が返される
    int DecreaseEvidenceValue(int _hitvalue, E_EVIDENCE[] _arrayevidence, E_EVIDENCE _hitevidence)
    {
        int k = 0;
        for (int i = 0; i < _arrayevidence.Length; ++i)
        {
            if (_arrayevidence[i] == _hitevidence)
            {//この容疑者は取得した証拠品を持っているので
                k = _hitvalue;
            }
        }
        Debug.Log(_hitevidence + "(" + k + ")が対象から引かれました");
        return k;
    }
}