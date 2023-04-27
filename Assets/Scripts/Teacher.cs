using System.Collections;
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

		public float CurrentAwareness => _currentAwareness;
		public float AwarenessCap => _awarenessCap;
		
		void Awake()
		{
			GameEvents.AudioPlayed.AddListener(OnSoundPlayed);
			GameEvents.KidCaught.AddListener(OnKidCaught);
		}
		
		void OnSoundPlayed(GameSound sound)
		{
			if (sound.Awareness <= 0f)
			{
				return;
			}

			_currentAwareness += sound.Awareness;
		}

		void OnKidCaught()
		{
			_animator.SetTrigger(Caught);
		}

		void Update()
		{
			if (_currentAwareness >= _awarenessCap)
			{
				_animator.SetTrigger(Caught);
			}

			_currentAwareness -= _awrenessDropPerSecond * Time.deltaTime;
			if (_currentAwareness < 0)
			{
				_currentAwareness = 0;
			}
		}
	}
}