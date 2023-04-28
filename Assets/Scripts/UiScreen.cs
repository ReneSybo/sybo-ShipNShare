using System;
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
		[SerializeField] TMP_Text _awarenessText;
		[SerializeField] TMP_Text _timerText;

		float timeSpend;
		bool _playing;
		
		void Awake()
		{
			Application.targetFrameRate = 50;
			
			GameEvents.GameEnded.AddListener(OnGameEnded);
			_playButton.onClick.AddListener(OnPlayClick);
			_retryButton.onClick.AddListener(OnRetry);
			
			_mainMenuRoot.SetActive(true);
			_gameOverScreen.SetActive(false);
			_playing = false;
		}

		void OnRetry()
		{
			GameEvents.QuitGame.Dispatch();
			_mainMenuRoot.SetActive(true);
			_gameOverScreen.SetActive(false);
			_awarenessText.gameObject.SetActive(true);
		}

		void OnGameEnded()
		{
			_awarenessText.gameObject.SetActive(false);
			_gameOverScreen.SetActive(true);
			_playing = false;
		}

		void OnPlayClick()
		{
			timeSpend = 0;
			_playing = true;
			_mainMenuRoot.SetActive(false);
			GameEvents.GameStarted.Dispatch();
		}

		void Update()
		{
			if (_playing)
			{
				string current = _teacher.CurrentAwareness.ToString("0.00");
				string cap = _teacher.AwarenessCap.ToString("0");

				TimeSpan t = TimeSpan.FromSeconds(timeSpend);
				string timeText = string.Format("Time: {0:D1}:{1:D2}", t.Minutes, t.Seconds);
				
				_awarenessText.text = $"{current}/{cap}";
				_timerText.text = timeText;

				timeSpend += Time.deltaTime;
			}
		}
	}
}