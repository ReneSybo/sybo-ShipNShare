using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Classroom
{
	public class UiScreen : MonoBehaviour
	{
		[SerializeField] GameObject _mainMenuRoot;
		[SerializeField] GameObject _gameOverScreen;
		
		[SerializeField] Button _playButton;
		[SerializeField] Button _retryButton;
		[SerializeField] Teacher _teacher;
		[SerializeField] TMP_Text _text;

		void Awake()
		{
			GameEvents.GameEnded.AddListener(OnGameEnded);
			_playButton.onClick.AddListener(OnPlayClick);
			_retryButton.onClick.AddListener(OnRetry);
			
			_mainMenuRoot.SetActive(true);
			_gameOverScreen.SetActive(false);
		}

		void OnRetry()
		{
			GameEvents.QuitGame.Dispatch();
			_mainMenuRoot.SetActive(true);
			_gameOverScreen.SetActive(false);
		}

		void OnGameEnded()
		{
			_gameOverScreen.SetActive(true);
		}

		void OnPlayClick()
		{
			_mainMenuRoot.SetActive(false);
			GameEvents.GameStarted.Dispatch();
		}

		void Update()
		{
			string current = _teacher.CurrentAwareness.ToString("0.00");
			string cap = _teacher.AwarenessCap.ToString("0");

			_text.text = $"{current}/{cap}";
		}
	}
}