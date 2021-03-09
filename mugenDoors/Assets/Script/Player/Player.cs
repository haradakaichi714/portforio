// @file   Player.cs
// @brief  プレイヤークラスの定義
// @author T,Cho K.Harada
// @date   2020/10/19 作成

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// @name   Player
// @brief  プレイヤークラス
public class Player : SingletonMonoBehaviour<Player>
{
    CharcterMove m_charcterMove;

    bool m_gameOver;

   
    Ray m_backRay;                           //後方にオブジェクトがあるかどうか
    Ray m_fowardRay;                         //前方にオブジェクトがあるかどうか

    RaycastHit m_hit;

    [SerializeField]
    private LayerMask m_layerMaskZombie = default;      //前方のオブジェクトを特定のものに絞る

    [SerializeField]
    private LayerMask m_layerMaskDoor = default;      //前方のオブジェクトを特定のものに絞る

    [SerializeField] private TMP_Text m_gameOverText = default;
    [SerializeField] private Button m_titleButton = default;

    private Animator m_animator;        //自身のアニメーション変更用

    // Start is called before the first frame update
    void Start()
    {
        m_charcterMove = GetComponent<CharcterMove>();
        m_animator = GetComponent<Animator>();

        ChangeAnimState(ANIM.WALK);
    }

    // Update is called once per frame
    void Update()
    {
        m_backRay = new Ray(transform.position, new Vector3(-1,0,0));            //後ろに衝突するオブジェクトがあるかどうかの判断
        m_fowardRay = new Ray(transform.position, new Vector3(1,0,0));            //前に衝突するオブジェクトがあるかどうかの判断
        Debug.DrawRay(m_fowardRay.origin, m_fowardRay.direction * 1, Color.red, 1, false);
        if (Physics.Raycast(m_fowardRay, out m_hit, 2.0f, m_layerMaskDoor))
        {
            if (m_hit.collider == null) return;      //例外処理
            ChangeAnimState(ANIM.IDLE);             //扉に当たった時
        }
        else if(Physics.Raycast(m_backRay, out m_hit, 1.0f, m_layerMaskZombie))
        {
            if (m_hit.collider == null) return;      //例外処理
            Gameover();             //ゲームオーバー呼び出し
        }
        else
        {
            ChangeAnimState(ANIM.WALK);
            m_charcterMove.Run(2.0f);

        }

    }


    void ChangeAnimState(ANIM _state)
    {
        switch (_state)
        {
            case ANIM.IDLE:
                m_animator.SetBool("is_Running", false);
                m_animator.SetBool("is_Wait", true);

                break;
            case ANIM.WALK:
                m_animator.SetBool("is_Running", true);
                m_animator.SetBool("is_Wait", false);
                break;
        }
    }

    //@name  Gameover
    //@brief ゲームオーバー処理
    private void Gameover()
    {
        if (m_gameOver) return;
        m_gameOver = true;
        m_titleButton.enabled = true;
        m_gameOverText.enabled = true;
        Debug.Log("ゲームオーバー");
    }
}
;