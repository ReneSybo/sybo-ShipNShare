using System.Collections;
using Classroom.Sound;
using UnityEngine;

namespace Classroom
{
	public class Teacher : MonoBehaviour
	{
		enum TeacherState
		{
			Teaching,
			AboutToLook,
			Alerted,
			Looking,
			Catching
		}
		
		static readonly int Caught = Animator.StringToHash("Caught");
		static readonly int FaceKids = Animator.StringToHash("FaceKids");
		static readonly int FaceBoard = Animator.StringToHash("FaceBoard");
		static readonly int Alert = Animator.StringToHash("Alert");
		
		[SerializeField] Animator _animator;
		[SerializeField] float _teachingTime = 10f;
		[SerializeField] float _lookingTime = 1f;

		bool _isTeaching;
		float _stateTimer;
		TeacherState _State;
		
		Transform _transform;
		bool _caught;

		void OnSoundPlayed(GameSound sound)
		{
			if (sound.Awareness <= 0f)
			{
				return;
			}

			_stateTimer -= sound.Awareness;
		}

		void Awake()
		{
			_State = TeacherState.Teaching;
			_transform = transform;
			
			_isTeaching = true;
			_stateTimer = _teachingTime;
			
			GameEvents.AudioPlayed.AddListener(OnSoundPlayed);
			GameEvents.KidCaught.AddListener(OnKidCaught);
		}

		void OnKidCaught()
		{
			_State = TeacherState.Catching;
			_animator.SetTrigger(Caught);
		}

		void Update()
		{
			_stateTimer -= Time.deltaTime;

			switch (_State)
			{
				case TeacherState.Teaching:
					if (_stateTimer <= 0)
					{
						_State = TeacherState.AboutToLook;
						StartCoroutine(DelayedLook());
					}
					break;
				case TeacherState.Alerted:
					break;
				case TeacherState.Looking:
					if (_stateTimer <= 0)
					{
						_stateTimer = _teachingTime;
						_animator.SetTrigger(FaceBoard);
						_State = TeacherState.Teaching;
						GameEvents.LookOver.Dispatch();
					}
					else
					{
						GameEvents.LookCheck.Dispatch();
					}
					break;
				case TeacherState.Catching:
					//Boo, game over!
					break;
			}
		}

		IEnumerator DelayedLook()
		{
			const float delayTime = 1f;
			GameEvents.LookHappeningIn.Dispatch(delayTime);
			yield return new WaitForSecondsRealtime(delayTime);
			
			_stateTimer = _lookingTime;
			_animator.SetTrigger(FaceKids);
			_State = TeacherState.Looking;
		}
	}
}