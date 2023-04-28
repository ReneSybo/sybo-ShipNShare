﻿using System;
using Classroom.Kids;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

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
		[SerializeField] Teacher _teacher;
		
		[SerializeField] TMP_Text _awarenessText;
		[SerializeField] TMP_Text _timerText;
		
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
			_gameScreen.SetActive(true);
			_endScreen.SetActive(false);
			
			_winText.SetActive(false);
			_loseText.SetActive(false);
			_playing = false;

			Kid[] allKids = Object.FindObjectsOfType<Kid>();
			_kidCount = allKids.Length;
			_kidCount = 1;
		}

		void OnKidRescued()
		{
			_kidsRescued++;
			if (_kidsRescued >= _kidCount)
			{
				GameEvents.GameWon.Dispatch();
			}
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
			GameEvents.GameStarted.Dispatch();
		}

		void Update()
		{
			if (_playing)
			{
				string current = _teacher.CurrentAwareness.ToString("0.00");
				string cap = _teacher.AwarenessCap.ToString("0");

				TimeSpan t = TimeSpan.FromSeconds(_timeSpend);
				string timeText = string.Format("Time: {0:D1}:{1:D2}", t.Minutes, t.Seconds);
				
				_awarenessText.text = $"{current}/{cap}";
				_timerText.text = timeText;

				_timeSpend += Time.deltaTime;
			}
		}
	}
}