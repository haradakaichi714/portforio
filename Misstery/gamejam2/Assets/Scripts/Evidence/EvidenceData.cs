using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class EvidenceData : MonoBehaviour
{
    [Tooltip("評価値(ステージごと)(int)")]
    [SerializeField] int[] m_evidenceValue;

    [Tooltip("この証拠品に対する説明文(string)")]
    [SerializeField] string m_evidenceName;

    [TextArea(1, 3)]
    [Tooltip("この証拠品に対する説明文(string)")]
    [SerializeField] string m_evidenceTip;

    [SerializeField] GameObject m_playerObject;
    [SerializeField] GameObject m_evidenceTipObject;
    [SerializeField] Text m_evidenceNameText;
    [SerializeField] Text m_evidenceTipText;

    [SerializeField] Sprite thisSprite;
    [SerializeField] Image[] m_evidenceImage;

    private bool canGetFlag;

    private AudioSource se_getEvidence;


    // Start is called before the first frame update
    void Start()
    {
        se_getEvidence = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_GETEVIDENCE);

        m_evidenceTipObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (canGetFlag)
        {
            if (Input.GetKey(KeyCode.E))
            {

                //証拠品の取得
                if (DataManager.Instance.getEvidenceNum >= 3) return;

                se_getEvidence.Play();
                m_evidenceImage[DataManager.Instance.getEvidenceNum].sprite = thisSprite;
                DataManager.Instance.getEvidenceNum++;

                this.gameObject.SetActive(false);
                E_EVIDENCE evidence = GameObject.Find("EvidenceFactory").GetComponent<EvidenceFactory>().GetEvidence(this.gameObject);
                GameObject.Find("GameManager").GetComponent<CalculationValue>().CheckHitEvidence(evidence);

                canGetFlag = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.tag == "Player")
        {
            m_evidenceTipObject.SetActive(true);
            //notebookにあるspriteを自身のspriteに変更
            m_evidenceTipObject.transform.position = m_playerObject.transform.position;
            m_evidenceNameText.text = m_evidenceName;
            m_evidenceTipText.text = m_evidenceTip;
            Debug.Log(m_evidenceName);
            canGetFlag = true;
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.tag == "Player")
        {
            m_evidenceTipObject.SetActive(false);
            canGetFlag = false;
        }
    }

    public int GetEvidenceValue(int stagenum)
    {
        if (m_evidenceValue.Length > stagenum)
        {
            return m_evidenceValue[stagenum];
        }
        else
        {//エラー確認用　表示の通り評価値が足りないのでインスペクター上で追加してください
            Debug.LogError("エラーが発生しました。\nステージに設定されている" + this.name + "の" + stagenum + 1 + "番目の評価値がありません");
            Debug.Log("設定するか表示しないようにしてください");
            return 0;
        }
    }
}