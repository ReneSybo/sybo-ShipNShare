using UnityEngine;

namespace Kids
{
	public abstract class CatchableItem : MonoBehaviour
	{
		[SerializeField] protected Rigidbody _body;
		[SerializeField] protected Animator _animator;
		
		bool _isFree;

		public bool IsFree => _isFree;
		public float Speed => _body.velocity.sqrMagnitude;

		public void Free()
		{
			_body.constraints = RigidbodyConstraints.FreezeAll;
			_isFree = true;
		}
	}
}