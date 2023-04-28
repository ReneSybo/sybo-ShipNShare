using System;
using UnityEngine;

namespace Classroom
{
	public class BlobShadow : MonoBehaviour
	{
		Transform _transform;

		[SerializeField] Transform _parentTransform;
		float _originalY;
		Quaternion _originalRotation;

		void Awake()
		{
			_transform = transform;
			_originalY = _transform.position.y;
			_originalRotation = _transform.rotation;
		}

		void Update()
		{
			Vector3 parentPos = _parentTransform.position;
			parentPos.y = _originalY; 
			
			_transform.position = parentPos;
			_transform.rotation = _originalRotation;
		}
	}
}