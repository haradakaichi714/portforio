// @file   SliderManager.cs
// @brief  スライダーマネージャークラスの定義
// @author T,Cho K.Harada
// @date   2020/10/19 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// @name   SliderManager
// @brief  スライダーマネージャークラス
public class SliderManager : SingletonMonoBehaviour<SliderManager>
{
  [SerializeField]
  Slider m_goalSlider;      //ゴールまでの距離メーターＵＩ
    
    [SerializeField]
    Transform m_goalPos = default;
    [SerializeField]
    Transform m_startPos = default;
    [SerializeField]
    Transform m_nowPos = default;

    // Start is called before the first frame update
    void Start()
    {
        m_goalSlider = GetComponent<Slider>();
        m_goalSlider.maxValue = m_goalPos.position.x;
        m_goalSlider.minValue = m_startPos.position.x;
        m_goalSlider.value = m_nowPos.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        m_goalSlider.value = m_nowPos.position.x;

    }
}
