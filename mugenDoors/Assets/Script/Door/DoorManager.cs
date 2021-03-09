// @file   DoorManager.cs
// @brief  ドアマネージャークラスの定義
// @author T,Cho K.Harada
// @date   2020/10/16 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.UI;
public enum TYPE
{
    OPEN,
    CLOSE
}



// @name   DoorManager
// @brief  ドアマネージャークラス
public class DoorManager : MonoBehaviour
{
    KeyCode[] m_keyCodes = { KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.X, KeyCode.Y, KeyCode.Z };          //キーコード配列
    KeyCode[] m_keyCodes2 = { KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W };          //キーコード配列２
    public bool keyinput { get; set;}           //要求されたキーが押されたかどうか。
    public bool lastKeyinput { get; set;}
    private bool m_near;                          //プレイヤーとドアの距離が近い時trueにするための変数
    private float m_time;                         //長押し時間計測

    [SerializeField] TMP_Text m_keyText = default;    //要求されたキーをドアの上に表示するためのInputField
    [SerializeField] Image m_circle = default;
    private Transform m_playerPos;         //プレイヤーとの距離を取るためのプレイヤーのTransform箱
    private Transform m_transform;                //プレイヤーとの距離を取るための自分のTransform箱
    private float distance;                   //プレイヤーとゾンビとの距離表示。
    public int keyNum;                      //キーコードの数
    public TYPE type;
    public const float inputInterval = 1.5f;   //長押し時間定数（2秒）
    // Start is called before the first frame update
    void Start()
    {

        //Keyがなく、押されていないことの初期化
        m_keyText.text = "";
        keyinput = false;

        //自身のTransformを取得
        m_transform = GetComponent<Transform>();

        //プレイヤーのポジションを取得
        m_playerPos = GameObject.Find("Player").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        distance = m_transform.position.x - m_playerPos.position.x;
        if (m_near) return;
        switch (type)
        {
            case TYPE.CLOSE:
                CheckCloseDoor(distance);
                break;
            case TYPE.OPEN:
                CheckOpenDoor(distance);
                break;
        }
        if (!m_near) return;


        var keyRandom = Random(m_keyCodes);
        var keyRandom2 = Random(m_keyCodes2);

        switch (keyNum)
        {
            case 1:
                StartCoroutine(DownKeyCheck(keyRandom, null));
                m_keyText.text = keyRandom.ToString();
                break;
            case 2:
                StartCoroutine(DownKeyCheck(keyRandom, keyRandom2));
                m_keyText.text = keyRandom.ToString() + " + " + keyRandom2.ToString();
                break;
        }

    }

    // @name   CloseDoor
    // @brief  ドアを閉める
    public void CloseDoor()
    {
        lastKeyinput = true;
        Debug.Log("ドアを閉めます");
        SEManager.Instance.PlaySE("close");
        m_keyText.text = "";
        //m_keyinput == true  → ドアを閉める
        // 1秒かけて90度まで回転
        m_transform.DORotate(
            new Vector3(0f, -90f, 0f),   // 終了時点のRotation
            1.0f                    // アニメーション時間
        );
    }

    // @name   OpenDoor
    // @brief  ドアを開ける
    public void OpenDoor()
    {
        Debug.Log("ドアを開けます");
        SEManager.Instance.PlaySE("open");
        m_keyText.text = "";
        // 1秒かけて90度まで回転
        m_transform.DORotate(
            new Vector3(0f, 0f, 0f),   // 終了時点のRotation
            1.0f                    // アニメーション時間
        );
    }



    // @name   DownKeyCheck
    // @brief  キー入力処理
    IEnumerator DownKeyCheck(object _keyCode, object _keyCode2)
    {
        while (keyinput == false)
        {
            if(distance<=-2)
            {
                keyinput = true;
                m_keyText.text = "";
            }
            switch (type)
            {
                case TYPE.OPEN:
                    if (_keyCode2 == null && Input.GetKey((KeyCode)_keyCode))
                    {
                        m_time += Time.deltaTime;
                        m_circle.fillAmount = m_time / inputInterval;
                        //処理を書く
                        if (m_time >= inputInterval)
                        {
                            OpenDoor();
                            m_circle.fillAmount = 0;
                            keyinput = true;
                        }
                    }
                    else if (Input.GetKey((KeyCode)_keyCode) && Input.GetKey((KeyCode)_keyCode2))
                    {
                        m_time += Time.deltaTime;
                        m_circle.fillAmount = m_time / inputInterval;
                        //処理を書く
                        if (m_time >= inputInterval)
                        {
                            OpenDoor();
                            m_circle.fillAmount = 0;
                            keyinput = true;
                        }
                    }
                    else
                    {
                        m_time = 0;
                    }
                    break;
                case TYPE.CLOSE:
                    if (_keyCode2 == null && Input.GetKey((KeyCode)_keyCode))
                    {
                        //処理を書く
                        CloseDoor();
                        keyinput = true;
                    }
                    else if (Input.GetKey((KeyCode)_keyCode) && Input.GetKey((KeyCode)_keyCode2))
                    {
                        //処理を書く
                        CloseDoor();
                        keyinput = true;
                    }
                    break;
            }
            yield return new WaitForFixedUpdate();
        }
    }



    // @name   Random
    // @brief  指定された配列の中からランダムに要素を返します
    public static T Random<T>(params T[] values)
    {
        return values[UnityEngine.Random.Range(0, values.Length)];
    }

    // @name   CheckCloseDoor
    // @brief  ドアを閉める判定
    private void CheckCloseDoor(float _distance)
    {
        if (_distance <= 0) m_near = true;
    }
    // @name   CheckOpenDoor
    // @brief  ドアを開ける判定
    private void CheckOpenDoor(float _distance)
    {
        Mathf.Abs(_distance);
        if (_distance <= 2) m_near = true;
    }
}
