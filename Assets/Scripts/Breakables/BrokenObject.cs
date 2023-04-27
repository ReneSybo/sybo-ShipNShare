using Classroom.Sound;
using UnityEngine;

namespace Classroom
{
	public class BrokenObject : MonoBehaviour
	{
		[SerializeField] Rigidbody _body = null;
		[SerializeField] GameSound _sound = null;
		[SerializeField] float _impactForceRequired = 3;
		[SerializeField] float timeBetweenImpact = 1f;

		float _lastImpactTime;
		Vector3 _startPos;
		Quaternion _startRot;
		Transform _transform;

		public void Prepare()
		{
			_transform = transform;
			_startPos = _transform.localPosition;
			_startRot = _transform.localRotation;
			
			GameEvents.QuitGame.AddListener(OnReset);
		}

		void OnReset()
		{
			_transform.localPosition = _startPos;
			_transform.localRotation = _startRot;
			
			_body.velocity = Vector3.zero;
			_body.angularVelocity = Vector3.zero;
		}

		void OnCollisionEnter(Collision other)
		{
			if (other.impulse.sqrMagnitude >= _impactForceRequired * _impactForceRequired)
			{
				Rigidbody body = other.rigidbody;
				if (body == null)
				{
					return;
				}
				
				float timeNow = Time.realtimeSinceStartup;
				if (timeNow - _lastImpactTime >= timeBetweenImpact)
				{
					_lastImpactTime = timeNow;
					GameEvents.AudioPlayed.Dispatch(_sound);
				}
			}
		}
	}
}