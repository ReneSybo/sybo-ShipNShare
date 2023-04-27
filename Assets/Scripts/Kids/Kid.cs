using System;
using Kids;
using UnityEngine;

namespace Classroom.Kids
{
	public class Kid : CatchableItem
	{
		static readonly int Moving = Animator.StringToHash("Moving");
		
		[SerializeField] Collider _collider;
		[SerializeField] float _velocityForMoving;

		float _animCheckTimer;
		Vector3 _safeSpot;
		
		void Awake()
		{
			_safeSpot = transform.position;
		}

		void Update()
		{
			_animCheckTimer -= Time.deltaTime;
			if (_animCheckTimer <= 0)
			{
				_animCheckTimer = 0.5f;
				_animator.SetBool(Moving, _body.velocity.sqrMagnitude > _velocityForMoving * _velocityForMoving);
			}
		}

		void OnTriggerEnter(Collider collision)
		{
			if (collision.gameObject.name == "Exit")
			{
				Free();
				_collider.enabled = false;
			}
		}
	}
}