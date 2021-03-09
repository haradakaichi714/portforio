using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public enum E_EVIDENCE
{
    NONE = -1,

    GLASS,
    COAT,
    CONTACT,
    DYINGMESSAGE,
    FAKEHAIR,
    KNIFE,
    MEDICINE,
    PEN,
    PISTOL,
    RING,
    SHOVEL,

    MAX,
}
public class EvidenceFactory : MonoBehaviour
{
    GameObject[] m_evidence = new GameObject[(int)E_EVIDENCE.MAX];
    [SerializeField] GameObject[] m_evidences;


    //resourcesフォルダ内にある証拠品のプレハブ名を格納
    string[] prefabName =
    {
        "Glass",
        "Coat",
        "Contact",
        "DyingMessage",
        "FakeHair",
        "Knife",
        "Medicine",
        "Pen",
        "Pistol",
        "Ring",
        "Shovel",

    };
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DataManager.Instance.stage);
        for(int i = 0; i < (int)(DataManager.Instance.stageData[DataManager.Instance.stage].evidence.Length);i++)
        {
            m_evidences[(int)(DataManager.Instance.stageData[DataManager.Instance.stage].evidence[i])].SetActive(true);
            //Debug.Log("a");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetEvidence(E_EVIDENCE num)
	{
        return m_evidences[(int)num];
	}

    public E_EVIDENCE GetEvidence(GameObject obj)
	{
        for(int i = 0;i<m_evidences.Length;i++)
		{
            if (m_evidences[i] == obj)
            {
                return (E_EVIDENCE)Enum.ToObject(typeof(E_EVIDENCE), i);
            }
		}
        return E_EVIDENCE.NONE;
	}
}
