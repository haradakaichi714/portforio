using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_startPosition;

    [SerializeField]
    private float m_velocity;     //移動速度


    public Animator m_animator;    //アニメーション関連

    private AudioSource se_walk;
    private float m_soundSpan;

    // Start is called before the first frame update
    void Awake()
    {
        se_walk = GameObject.FindWithTag("AudioManager")
            .GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.SE_WALK);
        m_soundSpan = 0.5f;

        this.transform.position = m_startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーの移動処理
        Transform myTransform = this.transform;
        Vector3 position = myTransform.position;

        if (Input.GetKey(KeyCode.W))
        {
            position.y += m_velocity * Time.deltaTime;
            m_animator.SetBool("isWalk", true);

        }
        else if (Input.GetKey(KeyCode.S))
        {
            position.y -= m_velocity * Time.deltaTime;
            m_animator.SetBool("isWalk", true);

        }
        else if (Input.GetKey(KeyCode.A))
        {
            position.x -= m_velocity * Time.deltaTime;
            m_animator.SetBool("isWalk", true);
            m_animator.SetBool("isLeft", true);
            m_animator.SetBool("isRight", false);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            position.x += m_velocity * Time.deltaTime;
            m_animator.SetBool("isWalk", true);
            m_animator.SetBool("isLeft", false);
            m_animator.SetBool("isRight", true);

        }
        else
        {
            m_animator.SetBool("isWalk", false);
        }

        if(position != myTransform.position)
        {
            m_soundSpan -= Time.deltaTime;
            if(m_soundSpan <= 0)
            {
                se_walk.PlayOneShot(se_walk.clip);
                m_soundSpan = 0.5f/(m_velocity*0.3f);
            }
        }


        myTransform.position = position;

    }
}
