// @file   CharcterMove.cs
// @brief  キャラクタ移動クラスの定義
// @author T,Cho K.Harada
// @date   2020/10/16 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// @name   CharcterMove
// @brief  キャラクター移動クラス
public class CharcterMove : MonoBehaviour
{
    private Vector2 m_charcterVector;   //速度ベクトル

    private Rigidbody m_rb;             //自身の物理演算用
    // Start is called before the first frame update
    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    { 

    }

    public void Run(float _speed)
    {

        //ものに当たらない限り歩き続ける
        m_charcterVector.x = 1 * _speed;
        m_rb.velocity = m_charcterVector;
    }

    public void StopRun()
    {
        //その場で止まる
        m_charcterVector = m_rb.velocity;
        m_charcterVector.x = 0;
        m_rb.velocity = m_charcterVector;
    }

}
