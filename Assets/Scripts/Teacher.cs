using System.Collections;
using Classroom.Sound;
using UnityEngine;

namespace Classroom
{
	public class Teacher : MonoBehaviour
	{
		[SerializeField] float _teachingTime = 10f;
		[SerializeField] float _lookingTime = 1f;
		[SerializeField] float _rotateTime = 1f;

		bool _isTeaching;
		bool _isRotating;
		
		float _rotateTimer;
		
		Quaternion _activeRotation;
		Quaternion _idleRotation;

		Transform _transform;

		void OnSoundPlayed(GameSound sound)
		{
			if (sound.Awareness <= 0f)
			{
				return;
			}

			_rotateTimer -= sound.Awareness;
		}

		void Awake()
		{
			_transform = transform;
			
			_activeRotation = _transform.localRotation;
			_transform.Rotate(Vector3.up, 180);
			_idleRotation = _transform.localRotation;

			_isTeaching = true;
			_isRotating = false;
			_rotateTimer = _teachingTime;
			
			GameEvents.AudioPlayed.AddListener(OnSoundPlayed);
		}

		void Update()
		{
			if (!_isRotating)
			{
				_rotateTimer -= Time.deltaTime;
				if (_rotateTimer <= 0f)
				{
					StartCoroutine(RotateAround());
				}
			}

			if (!_isTeaching)
			{
				GameEvents.TeachCheck.Dispatch();
			}
		}

		IEnumerator RotateAround()
		{
			_isRotating = true;

			Quaternion target = _isTeaching ? _activeRotation : _idleRotation;
			Quaternion current;

			if (_isTeaching)
			{
				const float delayTime = 1f;
				GameEvents.LookHappeningIn.Dispatch(delayTime);
				yield return new WaitForSecondsRealtime(delayTime);
			}

			float rotateRatio = 0f;
			float timer = 0f;

			Quaternion startRotation = _transform.localRotation;

			while (rotateRatio < 1)
			{
				current = Quaternion.Lerp(startRotation, target, rotateRatio);
				_transform.localRotation = current;
				
				timer += Time.deltaTime;
				rotateRatio = timer / _rotateTime;
				
				yield return new WaitForEndOfFrame();
			}

			_isRotating = false;
			_rotateTimer = _isTeaching ? _lookingTime : _teachingTime;
			
			_isTeaching = !_isTeaching;
			
			if (_isTeaching)
			{
				Debug.Log("Safe again");
				GameEvents.LookOver.Dispatch();
			}
		}
	}
}