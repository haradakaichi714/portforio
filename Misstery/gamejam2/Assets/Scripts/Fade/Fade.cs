using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Fade : Singleton<Fade>
{
    [SerializeField] Image m_fadeImage;
    const float FADE_DURATION = 1.0f;
    Color m_fadeColor;

    // Start is called before the first frame update
    void Start()
    {
        m_fadeImage.enabled = true;
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        m_fadeImage.DOFade(0.0f, FADE_DURATION).OnComplete(() => { m_fadeImage.enabled = false; });

    }
    public void FadeOut(string _sceneName)
    {
        m_fadeImage.enabled = true;
        m_fadeImage.DOFade(1.0f, FADE_DURATION).OnComplete(() => { SceneManager.LoadScene(_sceneName); });
    }
}
