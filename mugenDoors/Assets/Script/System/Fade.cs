// @file   Fade.cs
// @brief  フェードの処理
// @author T,Cho K.Harada
// @date   2020/10/22 作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

// @name   Fade
// @brief  フェードクラス
public class Fade : SingletonMonoBehaviour<Fade>
{
	[SerializeField] Image m_fadeImage;		// フェードする画像
	private float m_alpha = 1.0f;			// 画像を変化させるアルファ値
	private const float FADE_SPEED = 0.02f;	// フェードスピード

	public void Start()
	{
		m_fadeImage.enabled = true;
	}

	// @name   StartFadeIn
	// @brief  フェードインを開始する処理
	public async UniTask StartFadeIn(CancellationToken cancellation_token)
	{
		// フェードする画像のアルファ値を変更
		while (m_alpha > 0.0f)
		{
			Color color = m_fadeImage.color;
			color.a = m_alpha;
			m_fadeImage.color = color;
			m_alpha -= FADE_SPEED;

			await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellation_token);
		}
		m_fadeImage.enabled = false;

	}

	// @name   StartFadeOut
	// @brief  フェードアウトを開始する処理
	public async UniTask StartFadeOut(CancellationToken cancellation_token)
	{
		m_fadeImage.enabled = true;
		// フェードする画像のアルファ値を変更
		while (m_alpha < 1.0f)
		{
			Color color = m_fadeImage.color;
			color.a = m_alpha;
			m_fadeImage.color = color;
			m_alpha += FADE_SPEED;

			await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellation_token);
		}
	}

	// @name   ChangeScene
	// @brief  フェードアウト後にシーンを移動する処理
	public async UniTask ChangeScene(CancellationToken cancellation_token, string _scene)
	{
		await StartFadeOut(cancellation_token);
		SceneManager.LoadScene(_scene);
	}

	// @name   CallChangeScene
	// @brief  フェードアウト後にシーンを移動する処理を始める処理
	public void CallChangeScene(string scene)
	{
		ChangeScene(this.GetCancellationTokenOnDestroy(), scene).Forget();
	}
}
