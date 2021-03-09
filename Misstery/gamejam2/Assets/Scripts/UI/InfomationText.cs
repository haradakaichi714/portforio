using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfomationText : MonoBehaviour
{
    Text m_text;

    // Start is called before the first frame update
    void Start()
    {
        m_text = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetText(string _derails)
    {
        m_text.text = _derails;
    }

}
