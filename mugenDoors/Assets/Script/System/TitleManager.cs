// @file   TitleManager.cs
// @brief  タイトルマネージャークラスの定義
// @author T,Cho K.Harada
// @date   2020/10/22 作成

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

// @file   TitleManager
// @brief  タイトルの管理クラス
public class TitleManager : SingletonMonoBehaviour<TitleManager>
{
	private Fade m_fade;    // フェード管理クラス
	[SerializeField] TMP_InputField m_inputField;
	private void Start()
	{
		m_fade = Fade.Instance;
		m_fade.StartFadeIn(this.GetCancellationTokenOnDestroy()).Forget();
	}

	public void SetSpeed()
    {
		InputManager.Instance.OnChangeSpeed(m_inputField.text);
    }

}
