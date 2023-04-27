using System.Collections;
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
		
		Vector3 _oldPos;
		Vector3 _overviewPos;

		bool _isInOverview;
		float _revertTimer;
		Coroutine _routine;
		Vector3 _desiredPosition;

		void Awake()
		{
			_transform = transform;
			_backwards = -_transform.forward;
			_overviewPos = _overviewTransform.position;

			_isInOverview = false;
			_revertTimer = 0f;
			
			GameEvents.LookHappeningIn.AddListener(OnAboutToLook);
			GameEvents.LookOver.AddListener(OnLookOver);
		}

		void OnLookOver()
		{
			if (_routine != null)
			{
				StopCoroutine(_routine);
				_routine = null;
			}

			_desiredPosition = _oldPos;
			_routine = StartCoroutine(MoveCameraTo(true));
		}

		void OnAboutToLook(float delay)
		{
			if (_routine != null)
			{
				StopCoroutine(_routine);
				_routine = null;
			}
			
			_oldPos = transform.position;
			_isInOverview = true;
			
			_desiredPosition = _overviewPos;
			_routine = StartCoroutine(MoveCameraTo(false));
		}

		IEnumerator MoveCameraTo(bool returningToNormal)
		{
			Vector3 current;
			Vector3 start = _transform.position;

			float moveRatio = 0f;
			float timer = 0f;
			
			while (moveRatio < 1)
			{
				if (returningToNormal)
				{
					_desiredPosition =  _target.position + (_backwards * _distance);
				}
				
				current = Vector3.Slerp(start, _desiredPosition, moveRatio);
				_transform.position = current;

				timer += Time.deltaTime;
				moveRatio = timer / _overviewTransitionTime;
				
				yield return new WaitForEndOfFrame();
			}

			if (returningToNormal)
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