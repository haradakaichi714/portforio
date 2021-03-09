using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public struct STAGE_DATA
{
    public E_EVIDENCE[] evidence;
    //public E_SUSPECT[] suspect;

}

//                                //
//                                //
//--------構造体定義終わり--------//
//                                //
//                                //
public class DataManager : Singleton<DataManager>
{
    [Tooltip("Time(float)")]
    [HideInInspector] public float time;

    [Tooltip("Stage(int)")]
    [HideInInspector] public int stage;

    [Tooltip("勝利判定(bool)")]
    [HideInInspector] public bool win_flag;//テストでtrueにしてるよ

    [Tooltip("今隠蔽している証拠の数(int)")]
    [HideInInspector] public int getEvidenceNum;

    [Tooltip("出現する証拠品(Stage1)")]
    [SerializeField] E_EVIDENCE[] STAGE1_EVIDENCES;

    [Tooltip("出現する証拠品(Stage2)")]
    [SerializeField] E_EVIDENCE[] STAGE2_EVIDENCES;

    [Tooltip("出現する証拠品(Stage3)")]
    [SerializeField] E_EVIDENCE[] STAGE3_EVIDENCES;

    /// <summary>
    /// 各評価値をゲームから持ち出すための変数
    /// </summary>
    [NonSerialized]
    public int[] m_evidenceValue;

    /// <summary>
    /// 最終フェーズに持っていきたい容疑者の情報
    /// </summary>
    [NonSerialized]
    public tagStageSuspectData stageSuspect;

    //[Tooltip("出現する容疑者(Stage1)")]
    //[SerializeField] E_SUSPECT[] STAGE1_SUSPECTS;

    //[Tooltip("出現する容疑者(Stage2)")]
    //[SerializeField] E_SUSPECT[] STAGE2_SUSPECTS;

    //[Tooltip("出現する容疑者(Stage3)")]
    //[SerializeField] E_SUSPECT[] STAGE3_SUSPECTS;

    [HideInInspector] public STAGE_DATA[] stageData = new STAGE_DATA[5];

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        for(int i = 0 ; i < 3; i++)
        {
            switch (i + 1)
            {
                case 1:
                    stageData[i].evidence = STAGE1_EVIDENCES;
                    //stageData[stage].suspect = STAGE1_SUSPECTS;
                    break;

                case 2:
                    stageData[i].evidence = STAGE2_EVIDENCES;
                    //stageData[stage].suspect = STAGE2_SUSPECTS;
                    break;

                case 3:
                    stageData[i].evidence = STAGE3_EVIDENCES;
                    //stageData[stage].suspect = STAGE3_SUSPECTS;
                    break;

            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
