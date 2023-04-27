using UnityEngine;

namespace Classroom
{
	public class ImpactDispatcher : MonoBehaviour
	{
		public delegate void BreakableHit(float impactImpulse);
		
		public event BreakableHit OnHit;
		
		void OnCollisionEnter(Collision other)
		{
			OnHit?.Invoke(other.impulse.sqrMagnitude);
		}
	}
}