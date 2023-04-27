﻿using System.Collections;
using UnityEngine;

namespace Classroom
{
	public class CameraController : MonoBehaviour
	{
		[Header("Special overview behavior")]
		[SerializeField] Transform _overviewTransform;
		[SerializeField] float _overviewTransitionTime = 1f;
		
		[Header("Normal follow behavior")]
		[SerializeField] Transform _target;
		[SerializeField] float _distance;
		
		Transform _transform;
		Vector3 _backwards;
		
		Vector3 _overviewPos;

		bool _isInOverview;

		void Awake()
		{
			_transform = transform;
			_backwards = -_transform.forward;
			_overviewPos = _overviewTransform.position;

			_transform.position = _overviewPos;
			_isInOverview = true;
			
			GameEvents.GameEnded.AddListener(OnKidCaught);
			GameEvents.GameStarted.AddListener(OnGameStarted);
		}

		void OnGameStarted()
		{
			Vector3 targetPosition = _target.position + (_backwards * _distance);
			StartCoroutine(MoveCameraTo(targetPosition, 2f, true));
		}

		void OnKidCaught()
		{
			_isInOverview = true;
			StartCoroutine(MoveCameraTo(_overviewPos, 0f, false));
		}

		IEnumerator MoveCameraTo(Vector3 pos, float delay, bool removeOverview)
		{
			Vector3 current;
			Vector3 start = _transform.position;

			float moveRatio = 0f;
			float timer = 0f;

			yield return new WaitForSecondsRealtime(delay);
			
			while (moveRatio < 1)
			{
				current = Vector3.Slerp(start, pos, moveRatio);
				_transform.position = current;

				timer += Time.deltaTime;
				moveRatio = timer / _overviewTransitionTime;
				
				yield return new WaitForEndOfFrame();
			}

			if (removeOverview)
			{
				_isInOverview = false;
			}
		}

		void Update()
		{
			if (!_isInOverview)
			{
				_transform.position = _target.position + (_backwards * _distance);
			}
			
		}

		[ContextMenu("Update")]
		void ForceUpdate()
		{
			Awake();
			Update();
		}
	}
}