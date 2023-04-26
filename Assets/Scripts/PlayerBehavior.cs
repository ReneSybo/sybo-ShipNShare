using System;
using UnityEngine;

namespace Classroom
{
	public class PlayerBehavior : MonoBehaviour
	{
		[SerializeField] Rigidbody _body;
		[SerializeField] float _speed;
		[SerializeField] float _maxVelocity = 99999;
		[SerializeField] float _currentVelocity;
		
		void Update()
		{
			Vector3 movement = GetMovement();
			_body.AddForce(movement);
		
			Vector3 velocity = _body.velocity;
			float velocityLength = velocity.sqrMagnitude;
			if (velocityLength > _maxVelocity)
			{
				_body.velocity = velocity * (_maxVelocity / velocityLength);
			}

			_currentVelocity = velocityLength;
		}

		Vector3 GetMovement()
		{
			Vector3 movement = Vector3.zero;
			
			if (Input.GetKey(KeyCode.W))
			{
				movement.z += _speed * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				movement.z -= _speed * Time.deltaTime;
			}
			
			if (Input.GetKey(KeyCode.D))
			{
				movement.x += _speed * Time.deltaTime;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				movement.x -= _speed * Time.deltaTime;
			}

			return movement;
		}

		void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Rigidbody body = hit.rigidbody;
			if (body != null)
			{
				Vector3 movement = GetMovement();
				Vector3 position = body.position;
				position.y *= 1.5f;
				
				Debug.DrawLine(transform.position, position);

				body.AddForceAtPosition(movement * -100, position);
			}
		}
	}
}