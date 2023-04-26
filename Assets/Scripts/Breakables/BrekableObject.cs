using UnityEngine;

namespace Classroom
{
	public class BrekableObject : MonoBehaviour
	{
		[SerializeField] BreakableDispatcher _dispatcher;
		[SerializeField] GameObject _unbrokenRoot;
		[SerializeField] GameObject _brokenRoot;
		[SerializeField] float _strength;

		bool _broken;
		Rigidbody _unbrokenBody;
		Rigidbody[] _brokenBodies;

		void Awake()
		{
			_dispatcher.OnHit += OnBreakableHit;
			
			_unbrokenRoot.SetActive(true);
			_brokenRoot.SetActive(false);

			_unbrokenBody = _unbrokenRoot.GetComponentInParent<Rigidbody>();
			_brokenBodies = _brokenRoot.GetComponentsInChildren<Rigidbody>();
		}

		void OnBreakableHit(float impactimpulse)
		{
			if (_broken)
			{
				return;
			}
			
			if (impactimpulse >= _strength)
			{
				Break();
			}
		}

		void Break()
		{
			_broken = true;
			
			_unbrokenRoot.SetActive(false);
			_brokenRoot.SetActive(true);

			_brokenRoot.transform.localPosition = _unbrokenRoot.transform.localPosition;
			_brokenRoot.transform.localRotation = _unbrokenRoot.transform.localRotation;

			foreach (Rigidbody targetBody in _brokenBodies)
			{
				targetBody.velocity = _unbrokenBody.velocity;
				targetBody.angularVelocity = _unbrokenBody.angularVelocity;
			}
		}
	}
}