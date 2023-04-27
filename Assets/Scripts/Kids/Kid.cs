using Kids;
using UnityEngine;

namespace Classroom.Kids
{
	public class Kid : CatchableItem
	{
		[SerializeField] Collider _collider;

		Vector3 _safeSpot;
		

		void Awake()
		{
			_safeSpot = transform.position;
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