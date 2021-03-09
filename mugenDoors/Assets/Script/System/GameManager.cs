// @file   GameManager.cs
// @brief  ゲームマネージャークラスの定義
// @author T,Cho K.Harada
// @date   2020/10/21 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

// @name   GameManager
// @brief  ゲームマネージャークラス
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] GameObject m_zombiPrefabs = default;      //ゾンビのプレハブ
    [SerializeField] Transform m_playerPos = default;           //ゾンビプレハブを作成するときに使うプレイヤーのポジション
    [SerializeField] Camera m_camera = default;
    [SerializeField] Transform m_goalPos = default;
    [SerializeField] GameObject door = default;
    [SerializeField] private TMP_Text m_clearText = default;
    [SerializeField] private TMP_Text m_gameOverText = default;
    [SerializeField] private Button m_titleButton = default;
    [SerializeField] private GameObject m_lastZombie = default;
    private DoorManager m_doorManager;
    private Fade m_fade;	// フェード管理クラス

    public bool goalFlag { get; private set; }                                 //ゲーム自体のゴールフラグ
    // Start is called before the first frame update
    void Start()
    {
        m_fade = Fade.Instance;
        m_fade.StartFadeIn(this.GetCancellationTokenOnDestroy()).Forget();
        StartCoroutine(InstantiateZombie());
        StartCoroutine(WaitGoal());
        StartCoroutine(SEManager.Instance.PlaySELoop("zombie"));
        m_doorManager = door.GetComponent<DoorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // @name   InstantiateZombie
    // @brief  ゾンビが増えるクラス
    IEnumerator InstantiateZombie()
    {
        while (!goalFlag)
        {
            Instantiate(m_zombiPrefabs, new Vector3(m_playerPos.position.x - 8, m_playerPos.position.y, m_playerPos.position.z), m_zombiPrefabs.transform.rotation);
            yield return new WaitForSeconds(6);
        }
    }


    // @name   WaitGoal
    // @brief  ゴール待ち処理
    IEnumerator WaitGoal()
    {
        while (true)
        {
            if (m_goalPos.position.x <= m_playerPos.position.x) goalFlag = true;
            if (goalFlag)
            {
                m_camera.transform.position = new Vector3(m_playerPos.position.x + 4, m_playerPos.position.y + 2, m_playerPos.position.z);
                m_camera.transform.rotation = Quaternion.Euler(0, 90, 0); 
                if(m_doorManager.keyinput&&m_doorManager.lastKeyinput)
                {
                    m_titleButton.enabled = true;
                    m_clearText.enabled = true;
                    break;
                }
                else if(m_doorManager.keyinput && !m_doorManager.lastKeyinput)
                {
                    m_lastZombie.SetActive(true);
                    m_titleButton.enabled = true;
                    m_gameOverText.enabled = true;
                    break;
                }
               // ScenesManager.Instance.ChangeScene();         //シーン飛ばし
            }
            yield return new WaitForFixedUpdate();
        }
    }

    // @name   ChangeGoalFlag
    // @brief  プレイヤーがゴールした時に入れる関数
    public void ChangeGoalFlag(bool _flag)
    {
        goalFlag = _flag;
    }
}
