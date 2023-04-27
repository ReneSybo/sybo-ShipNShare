using Classroom.Sound;
using UnityEngine;

namespace Classroom
{
	public class BrekableObject : MonoBehaviour
	{
		[Header("Sound")]
		[SerializeField] GameSound _breakSound;
		[SerializeField] float _impactForBreak;
		
		[Header("Objects")]
		[SerializeField] GameObject _unbrokenRoot;
		[SerializeField] GameObject _brokenRoot;
		[SerializeField] ImpactDispatcher _dispatcher;

		bool _broken;
		Rigidbody _unbrokenBody;
		Rigidbody[] _brokenBodies;
		
		Vector3 _startPos;
		Quaternion _startRot;
		Transform _transform;

		void Awake()
		{
			_transform = transform;
			_dispatcher.OnHit += OnBreakableHit;
			
			_unbrokenRoot.SetActive(true);
			_brokenRoot.SetActive(false);

			_unbrokenBody = _unbrokenRoot.GetComponentInParent<Rigidbody>();
			_brokenBodies = _brokenRoot.GetComponentsInChildren<Rigidbody>();

			_startPos = _transform.localPosition;
			_startRot = _transform.localRotation;
			
			GameEvents.QuitGame.AddListener(OnReset);

			BrokenObject[] brokenChildren = GetComponentsInChildren<BrokenObject>(true);
			foreach (BrokenObject child in brokenChildren)
			{
				child.Prepare();
			}
		}

		void OnReset()
		{
			_broken = false;
			_transform.localPosition = _startPos;
			_transform.localRotation = _startRot;
			
			_unbrokenRoot.SetActive(true);
			_brokenRoot.SetActive(false);
		}

		void OnBreakableHit(float impactimpulse)
		{
			if (_broken)
			{
				return;
			}
			
			if (impactimpulse >= _impactForBreak * _impactForBreak)
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

			if (_breakSound != null)
			{
				GameEvents.AudioPlayed.Dispatch(_breakSound);
			}
		}
	}
}