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
		
		[SerializeField] Animator _animator;
		[SerializeField] float _awarenessCap = 10f;
		[SerializeField] float _awrenessDropPerSecond = 1f;
		[SerializeField] float _currentAwareness = 0f;

		bool _hasCaughtYou;
		
		public float CurrentAwareness => _currentAwareness;
		public float AwarenessCap => _awarenessCap;
		
		void Awake()
		{
			GameEvents.AudioPlayed.AddListener(OnSoundPlayed);
			GameEvents.AudioAwarenessAdded.AddListener(OnAwarenessAdded);
			GameEvents.QuitGame.AddListener(OnGameEnded);
		}

		void OnGameEnded()
		{
			_animator.SetTrigger(FaceBoard);
			_currentAwareness = 0f;
			_hasCaughtYou = false;
		}

		void OnAwarenessAdded(float awareness)
		{
			if (awareness > 0f)
			{
				_currentAwareness += awareness;
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

			_currentAwareness -= _awrenessDropPerSecond * Time.deltaTime;
			if (_currentAwareness < 0)
			{
				_currentAwareness = 0;
			}
		}
	}
}