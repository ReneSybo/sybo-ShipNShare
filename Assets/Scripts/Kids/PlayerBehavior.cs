﻿using Kids;
using UnityEngine;

namespace Classroom
{
	public class PlayerBehavior : CatchableItem
	{
		static readonly int Moving = Animator.StringToHash("Moving");
		static readonly int Contact = Animator.StringToHash("Contact");
		
		[SerializeField] Transform _meshContainer;
		[SerializeField] float _speed;
		[SerializeField] float _maxVelocity = 99999;
		[SerializeField] float _currentVelocity;
		[SerializeField] float _collisionCheckTime = 1f;

		bool _canMove;
		Vector3 _currentMeshForward;
		float _contactTime;
		RigidbodyConstraints _defaultConstraints;

		protected override void Awake()
		{
			base.Awake();
			
			_canMove = false;
			
			GameEvents.GameStarted.AddListener(OnStartGame);
			GameEvents.GameEnded.AddListener(OnEndGame);

			_currentMeshForward = _meshContainer.forward;
			_defaultConstraints = _body.constraints;
		}

		void OnEndGame()
		{
			_canMove = false;
			_meshContainer.forward = _currentMeshForward;
			_contactTime = 0;
			_animator.SetBool(Moving, false);
			_animator.SetBool(Contact, false);
			_body.constraints = RigidbodyConstraints.FreezeAll;
		}

		void OnStartGame()
		{
			_body.constraints = _defaultConstraints;
			_canMove = true;
		}

		void Update()
		{
			Vector3 movement = GetMovement(out bool moved);
			_body.AddForce(movement);
			_animator.SetBool(Moving, moved);
			if (moved)
			{
				_currentMeshForward = Vector3.Slerp(_currentMeshForward, movement.normalized, Time.deltaTime * 10f);
			}
			_meshContainer.forward = _currentMeshForward;


			Vector3 velocity = _body.velocity;
			float velocityLength = velocity.sqrMagnitude;
			if (velocityLength > _maxVelocity)
			{
				_body.velocity = velocity * (_maxVelocity / velocityLength);
			}

			_currentVelocity = velocityLength;

			if (_contactTime > 0)
			{
				_contactTime -= Time.deltaTime;
				if (_contactTime <= 0)
				{
					_animator.SetBool(Contact, false);
				}
			}
		}

		Vector3 GetMovement(out bool moved)
		{
			Vector3 movement = Vector3.zero;
			moved = false;
			
			if (!_canMove)
			{
				return movement;
			}
			
			if (Input.GetKey(KeyCode.W))
			{
				moved = true;
				movement.z += _speed * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				moved = true;
				movement.z -= _speed * Time.deltaTime;
			}
			
			if (Input.GetKey(KeyCode.D))
			{
				moved = true;
				movement.x += _speed * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				moved = true;
				movement.x -= _speed * Time.deltaTime;
			}

			return movement;
		}

		void OnCollisionEnter(Collision collision)
		{
			Rigidbody body = collision.rigidbody;
			if (body != null)
			{
				_contactTime = _collisionCheckTime;
				_animator.SetBool(Contact, true);
			}
		}

		void OnCollisionStay(Collision collision)
		{
			Rigidbody body = collision.rigidbody;
			if (body != null)
			{
				if (_contactTime > 0)
				{
					_contactTime = _collisionCheckTime;
				}
			}
		}
	}
}