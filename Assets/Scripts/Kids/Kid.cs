using UnityEngine;

namespace Classroom.Kids
{
	public class Kid : MonoBehaviour
	{
		[SerializeField] Collider _collider;
		[SerializeField] Rigidbody _body;

		bool _freed;
		Vector3 _safeSpot;
		
		public bool IsFreed => _freed;

		void Awake()
		{
			_safeSpot = transform.position;
			_freed = false;
		}

		void OnTriggerEnter(Collider collision)
		{
			if (collision.gameObject.name == "Exit")
			{
				_body.constraints = RigidbodyConstraints.FreezeAll;
				_freed = true;
				_collider.enabled = false;
			}
		}
	}
}