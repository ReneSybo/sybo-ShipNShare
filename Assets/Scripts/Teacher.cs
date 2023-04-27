using System;
using Classroom.Sound;
using UnityEngine;

namespace Classroom
{
	public class Teacher : MonoBehaviour
	{
		static readonly int Caught = Animator.StringToHash("Caught");
		static readonly int FaceKids = Animator.StringToHash("FaceKids");
		static readonly int FaceBoard = Animator.StringToHash("FaceBoard");
		static readonly int Alert = Animator.StringToHash("Alert");
		
		[SerializeField] AudioSource _source;
		[SerializeField] AudioClip _gameOverSound;
		[SerializeField] Animator _animator;
		[SerializeField] float _awarenessCap = 10f;
		[SerializeField] float _awrenessDropPerSecond = 1f;
		[SerializeField] float _currentAwareness = 0f;
		
		[Header("Idle sounds")]
		[SerializeField] AwarenessSoundConfig[] _soundConfigs = null;
		AwarenessState _currentState;
		
		
		enum AwarenessState
		{
			Unknown,
			Low,
			Medium,
			High
		}

		[Serializable]
		class AwarenessSoundConfig
		{
			public AwarenessState State;
			public AudioClip Clip;
			public int Level;
		}

		bool _hasCaughtYou;
		
		public float CurrentAwareness => _currentAwareness;
		public float AwarenessCap => _awarenessCap;
		
		void Awake()
		{
			GameEvents.AudioPlayed.AddListener(OnSoundPlayed);
			GameEvents.AudioAwarenessAdded.AddListener(OnAwarenessAdded);
			GameEvents.QuitGame.AddListener(OnQuitGame);
			GameEvents.GameEnded.AddListener(OnGameEnded);
			GameEvents.GameStarted.AddListener(OnGameStarted);
		}

		void OnGameEnded()
		{
			_source.Stop();
			_source.PlayOneShot(_gameOverSound);
		}

		void OnGameStarted()
		{
			_currentAwareness = 0;
			_currentState = AwarenessState.Unknown;
			UpdateAudioType();
		}

		void OnQuitGame()
		{
			_animator.SetTrigger(FaceBoard);
			_currentAwareness = 0f;
			_hasCaughtYou = false;
			_source.Stop();
		}

		void OnAwarenessAdded(float awareness)
		{
			if (awareness > 0f)
			{
				int beforeAwareness = (int) _currentAwareness;
				_currentAwareness += awareness;
				int afterAwareness = (int) _currentAwareness;

				if (beforeAwareness != afterAwareness)
				{
					UpdateAudioType();
				}
			}
		}

		void OnSoundPlayed(GameSound sound) => OnAwarenessAdded(sound.Awareness);

		void Update()
		{
			if (_hasCaughtYou)
			{
				return;
			}
			
			if (_currentAwareness >= _awarenessCap)
			{
				_hasCaughtYou = true;
				_animator.SetTrigger(Caught);
				GameEvents.GameEnded.Dispatch();
			}

			int beforeAwareness = (int) _currentAwareness;
			_currentAwareness -= _awrenessDropPerSecond * Time.deltaTime;
			if (_currentAwareness < 0)
			{
				_currentAwareness = 0;
			}
			int afterAwareness = (int) _currentAwareness;
			if (beforeAwareness != afterAwareness)
			{
				UpdateAudioType();
			}
		}

		void UpdateAudioType()
		{
			int awarenessInt = (int) _currentAwareness;
			for (int i = _soundConfigs.Length - 1; i >= 0; i--)
			{
				AwarenessSoundConfig config = _soundConfigs[i];
				if (awarenessInt >= config.Level)
				{
					if (_currentState != config.State)
					{
						_currentState = config.State;
						_source.Stop();
						_source.clip = config.Clip;
						_source.Play();
					}
					
					break;
				}
			}
		}
	}
}