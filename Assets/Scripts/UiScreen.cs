using System;
using Classroom.Kids;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Classroom
{
	public class UiScreen : MonoBehaviour
	{
		[SerializeField] GameObject _mainMenuRoot;
		[SerializeField] GameObject _endScreen;
		[SerializeField] GameObject _gameScreen;
		
		[SerializeField] GameObject _winText;
		[SerializeField] GameObject _loseText;
		
		[SerializeField] Button _playButton;
		[SerializeField] Button _retryButton;
		
		[SerializeField] TMP_Text _timerText;
		[SerializeField] TMP_Text _kidsText;
		
		[SerializeField] TMP_Text _endTimerText;
		[SerializeField] TMP_Text _endKidText;

		float _timeSpend;
		bool _playing;
		int _kidsRescued;
		int _kidCount;
		
		void Awake()
		{
#if UNITY_WEBGL
			Application.targetFrameRate = 50;
#endif
			
			GameEvents.KidRescued.AddListener(OnKidRescued);
			GameEvents.GameEnded.AddListener(OnGameEnded);
			GameEvents.GameWon.AddListener(OnGameWon);
			_playButton.onClick.AddListener(OnPlayClick);
			_retryButton.onClick.AddListener(OnRetry);
			
			_mainMenuRoot.SetActive(true);
			_gameScreen.SetActive(false);
			_endScreen.SetActive(false);
			
			_winText.SetActive(false);
			_loseText.SetActive(false);
			_playing = false;

			Kid[] allKids = FindObjectsOfType<Kid>();
			_kidCount = allKids.Length;
		}

		void OnKidRescued()
		{
			_kidsRescued++;
			if (_kidsRescued >= _kidCount)
			{
				GameEvents.GameWon.Dispatch();
			}

			_kidsText.text = $"Kids: {_kidsRescued}/{_kidCount}";
		}

		void OnGameWon()
		{
			SharedEndScreenSetup();
			_winText.SetActive(true);
		}
		
		void OnGameEnded()
		{
			SharedEndScreenSetup();
			_loseText.SetActive(true);
		}

		void SharedEndScreenSetup()
		{
			_gameScreen.SetActive(false);
			_endScreen.SetActive(true);
			_playing = false;
			
			TimeSpan t = TimeSpan.FromSeconds(_timeSpend);
			string timeText = string.Format("Time spend: {0:D1}:{1:D2}", t.Minutes, t.Seconds);

			_endTimerText.text = timeText;
			_endKidText.text = "Kids rescued: " + _kidsRescued;
		}

		void OnRetry()
		{
			GameEvents.QuitGame.Dispatch();
			_mainMenuRoot.SetActive(true);
			_endScreen.SetActive(false);
			
			_winText.SetActive(false);
			_loseText.SetActive(false);
			_kidsRescued = 0;
		}

		void OnPlayClick()
		{
			_timeSpend = 0;
			_playing = true;
			_gameScreen.SetActive(true);
			_mainMenuRoot.SetActive(false);
			_kidsText.text = $"Kids: {_kidsRescued}/{_kidCount}";
			GameEvents.GameStarted.Dispatch();
		}

		void Update()
		{
			if (_playing)
			{
				TimeSpan t = TimeSpan.FromSeconds(_timeSpend);
				string timeText = string.Format("Time: {0:D1}:{1:D2}", t.Minutes, t.Seconds);
				
				_timerText.text = timeText;

				_timeSpend += Time.deltaTime;
			}
			else
			{
				bool pressedToReset = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space);
				if (pressedToReset)
				{
					if (_retryButton.gameObject.activeInHierarchy)
					{
						OnRetry();
					}
					else if (_playButton.gameObject.activeInHierarchy)
					{
						OnPlayClick();
					}
				}
			}
		}
	}
}