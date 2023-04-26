using System;
using UnityEngine;

namespace Classroom
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] Transform _target;
		[SerializeField] float _distance;
		
		Transform _transform;
		Vector3 _backwards;

		void Awake()
		{
			_transform = transform;
			_backwards = -_transform.forward;
		}

		void Update()
		{
			_transform.position = _target.position + (_backwards * _distance);
		}

		[ContextMenu("Update")]
		void ForceUpdate()
		{
			Awake();
			Update();
		}
	}
}