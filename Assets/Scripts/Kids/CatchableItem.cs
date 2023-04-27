using Classroom;
using UnityEngine;

namespace Kids
{
	public abstract class CatchableItem : MonoBehaviour
	{
		[SerializeField] protected Rigidbody _body;
		[SerializeField] protected Animator _animator;
		
		bool _isFree;
		Vector3 _startPos;
		Quaternion _startRot;
		Transform _transform;
		RigidbodyConstraints _startConstraints;

		protected virtual void Awake()
		{
			_transform = transform;
			GameEvents.QuitGame.AddListener(ResetState);

			_startPos = _transform.localPosition;
			_startRot = _transform.localRotation;
			_startConstraints = _body.constraints;
		}

		protected virtual void ResetState()
		{
			_transform.localPosition = _startPos;
			_transform.localRotation = _startRot;
			_body.constraints = _startConstraints;
			_body.velocity = Vector3.zero;
			_body.angularVelocity = Vector3.zero;
		}

		protected void Free()
		{
			_body.constraints = RigidbodyConstraints.FreezeAll;
			_isFree = true;
		}
	}
}