using System;
using UnityEngine;

namespace Classroom
{
	public class SoundMeter : MonoBehaviour
	{
		[SerializeField] Transform _transform;
		
		Teacher _teacher;

		void Awake()
		{
			_teacher = FindObjectOfType<Teacher>();
		}

		void Update()
		{
			float current = _teacher.CurrentAwareness;
			float cap = _teacher.AwarenessCap;

			float ratio = current / cap;
			float inverse = Mathf.Clamp(1 - ratio, 0, 1);
			
			Vector3 scale = new Vector3(1, inverse, 1);
			_transform.localScale = scale;
		}
	}
}