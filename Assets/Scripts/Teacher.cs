using System;
using System.Collections;
using UnityEngine;

namespace Classroom
{
	public class Teacher : MonoBehaviour
	{
		[SerializeField] float _idleTime = 1f;
		[SerializeField] float _activeTime = 1f;
		[SerializeField] float _rotateSpeed = 1f;

		bool _isActive;
		bool _isRotating;
		float _rotateTimer;
		
		Quaternion _activeRotation;
		Quaternion _idleRotation;

		Transform _transform;
		
		void Awake()
		{
			_transform = transform;
			
			_activeRotation = _transform.localRotation;
			_transform.Rotate(Vector3.up, 180);
			_idleRotation = _transform.localRotation;

			_isActive = false;
			_isRotating = false;
			_rotateTimer = _idleTime;
		}

		void Update()
		{
			if (!_isRotating)
			{
				_rotateTimer -= Time.deltaTime;
				if (_rotateTimer <= 0f)
				{
					_isActive = !_isActive;
					StartCoroutine(RotateAround());
				}
			}

			if (_isActive)
			{
				
			}
		}

		IEnumerator RotateAround()
		{
			_isRotating = true;

			Quaternion target = _isActive ? _activeRotation : _idleRotation;
			Quaternion current;
			
			do
			{
				current = Quaternion.RotateTowards(_transform.localRotation, target, Time.deltaTime * _rotateSpeed);
				_transform.localRotation = current;
				yield return new WaitForEndOfFrame();
			} while (target != current);

			_isRotating = false;
			_rotateTimer = _isActive ? _activeTime : _idleTime;
		}
	}
}