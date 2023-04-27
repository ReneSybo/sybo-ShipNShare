using System.Collections.Generic;
using Classroom.Sound;
using UnityEngine;

namespace Classroom
{
	public class BrokenObject : MonoBehaviour
	{
		[SerializeField] GameSound _sound = null;
		[SerializeField] float _impactForceRequired = 3;
		[SerializeField] float timeBetweenImpact = 1f;

		float _lastImpactTime;

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