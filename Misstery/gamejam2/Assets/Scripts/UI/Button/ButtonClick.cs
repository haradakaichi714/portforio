using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public bool m_click;

    // Start is called before the first frame update
    void Start()
    {
        m_click = false;
    }

    public void OnClick()
    {
        m_click = true;
    }

}
