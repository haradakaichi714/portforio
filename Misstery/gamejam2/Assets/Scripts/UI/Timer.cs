using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text m_timeText;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.time = 10.0f;
        this.m_timeText.text = ((int)DataManager.Instance.time).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        DataManager.Instance.time -= Time.deltaTime;
        this.m_timeText.text = "残り時間："+((int)DataManager.Instance.time).ToString();

        if (DataManager.Instance.time <= 0.0f)
        {
            this.m_timeText.text = "残り時間：0";

        }

    }
}
