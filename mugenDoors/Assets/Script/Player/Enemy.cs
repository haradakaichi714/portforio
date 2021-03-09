// @file   Enemy.cs
// @brief  エネミークラスの定義
// @author T,Cho K.Harada
// @date   2020/10/19 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @name   ANIM
// @brief  アニメーション列挙型
public enum ANIM
{
    WALK,
    ATTACK,
    IDLE,
    DEAD,
    MAX
}

// @name   Enemy
// @brief  エネミークラス
public class Enemy : MonoBehaviour
{
    CharcterMove m_charcterMove;


    //前方にオブジェクトがあるかどうか
    Ray m_ray;
    RaycastHit m_hit;

    [SerializeField]
    private LayerMask m_layerMask = default;      //前方のオブジェクトを特定のものに絞る
    private Animator m_animator;        //自身のアニメーション変更用
    private bool m_deadFlag;

    public float m_speed;               //速度ベクトルに掛ける変数
    InputManager m_inputManager;

    // Start is called before the first frame update
    void Start()
    {
        m_charcterMove = GetComponent<CharcterMove>();
        m_animator = GetComponent<Animator>();
        ChangeAnimState(ANIM.WALK);

        m_inputManager = InputManager.Instance;
        if (m_inputManager.ZombieSpeed != 0)
        {
            m_speed = m_inputManager.ZombieSpeed;
        }
        else
        {
            m_speed = 2.8f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_hit.collider != null) return;      //例外処理
        if (m_deadFlag)
        {
            m_charcterMove.StopRun();
            return;
        }
        m_ray = new Ray(transform.position, transform.forward);            //前に衝突するオブジェクトがあるかどうかの判断
        Debug.DrawRay(m_ray.origin, m_ray.direction * 1, Color.red, 1, false);
        //前方にオブジェクトがあるかどうかの判定
        if (Physics.Raycast(m_ray, out m_hit, 0.5f, m_layerMask))
        {
            if (m_hit.collider == null) return;      //例外処理
            ChangeAnimState(ANIM.ATTACK);           //アニメーションの変更
            Invoke("BreakDoor", 2.0f);              //2秒後にドアを壊す関数の呼び出し

        }
        else
        {
            m_charcterMove.Run(m_speed);
        }
    }

    // @name   ChangeAnimState
    // @brief  アニメーションのセット
    public void ChangeAnimState(ANIM _state)
    {
        switch (_state)
        {
            case ANIM.ATTACK:
                m_animator.SetBool("is_Walk", false);
                m_animator.SetBool("is_Aidle", false);
                m_animator.SetBool("is_Attack", true);
                break;
            case ANIM.IDLE:
                m_animator.SetBool("is_Walk", false);
                m_animator.SetBool("is_Aidle", true);
                m_animator.SetBool("is_Attack", false);

                break;
            case ANIM.WALK:
                m_animator.SetBool("is_Walk", true);
                m_animator.SetBool("is_Aidle", false);
                m_animator.SetBool("is_Attack", false);
                break;
            case ANIM.DEAD:
                m_animator.SetBool("is_Dead", true);
                m_animator.SetBool("is_Walk", false);
                break;
        }
    }
    // @name   BreakDoor
    // @brief  ドアを壊す
    public void BreakDoor()
    {
        //例外処理
        if (m_hit.collider == null)
        {
            ChangeAnimState(ANIM.WALK);
            return;
        }
        Destroy(m_hit.collider.gameObject);         //前方で当たったオブジェクトの消去
        SEManager.Instance.PlaySE("doorBomb");
        ChangeAnimState(ANIM.WALK);
    }

    // @name   OnCollisionEnter
    // @brief  ドアに当たった時の判定

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.transform.position.x > this.gameObject.transform.position.x) return;
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        if (layerName == "Door")
        {
            ChangeAnimState(ANIM.DEAD);           //アニメーションの変更
            m_deadFlag = true;
            Invoke("Dead", 2.0f);              //2秒後にドアを壊す関数の呼び出し
        }
    }

    // @name   Dead
    // @brief  死亡時消す処理

    private void Dead()
    {
        Destroy(this.gameObject);
    }
}
