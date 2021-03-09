using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class AudioObject : MonoBehaviour
{
    //BGM
    private AudioSource bgm_title,bgm_stageSelect, bgm_game,bgm_finalPhase;
    static private AudioObject instance = null;

    private bool InGameFlag = false;

    public bool DontDestroyEnabled = true;
    void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(this);
            return;
        }

        if (DontDestroyEnabled)
        {
            // Sceneを遷移してもオブジェクトが消えないようにする
            DontDestroyOnLoad(this);
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        InGameFlag = false;

        //BGM_TITLEをセット
        //bgm_title = GameObject.FindWithTag("AudioManager").
        //    GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.BGM_TITLE);

        //BGM_SELECTをセット
        bgm_stageSelect = GameObject.FindWithTag("AudioManager").
            GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.BGM_SELECT);

        //BGM_GAMEをセット
        bgm_game = GameObject.FindWithTag("AudioManager").
            GetComponent<AudioManager>().GetAudioSourceByType(AudioManager.E_AUDIOTYPE.BGM_GAME);

        //BGM再生
        //bgm_title.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //if (SceneManager.GetActiveScene().name == "StageSelect" && !InGameFlag)
        //{
        //    bgm_title.Stop();
        //    bgm_stageSelect.Play();
        //    InGameFlag = false;
        //}
        //
        //if (SceneManager.GetActiveScene().name == "Game" && !InGameFlag)
        //{
        //    bgm_stageSelect.Stop();
        //    bgm_game.Play();
        //    InGameFlag = true;
        //}
        //
        //
        //if (SceneManager.GetActiveScene().name == "FinalPhase" && InGameFlag)
        //{
        //    bgm_game.Stop();
        //    bgm_finalPhase.Play();
        //
        //    InGameFlag = false;
        //}
    }

}
