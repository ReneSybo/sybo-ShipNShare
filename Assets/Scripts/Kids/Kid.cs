using Classroom.Sound;
using Kids;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Classroom.Kids
{
	public class Kid : CatchableItem
	{
		static readonly int Moving = Animator.StringToHash("Moving");
		static readonly int Reset = Animator.StringToHash("Reset");
		
		[SerializeField] GameSound _freedomSounds;
		[SerializeField] Collider _collider;
		[SerializeField] float _velocityForMoving;
		[SerializeField] GameSound[] _rollingSounds;
		[SerializeField] AudioSource _audioSource;
		[SerializeField] float _audioAwarenessCooldown;

		GameSound _currentSound;
		bool _isRolling;
		float _awarenessTimer;
		float _animCheckTimer;

		protected override void Awake()
		{
			base.Awake();
			
			_currentSound = _rollingSounds[(int)(Random.value * _rollingSounds.Length)];
			_audioSource.clip = _currentSound.Clip;
		}

		void Update()
		{
			_animCheckTimer -= Time.deltaTime;
			if (_animCheckTimer <= 0)
			{
				_animCheckTimer = 0.5f;
				bool shouldRoll = _body.velocity.sqrMagnitude > _velocityForMoving * _velocityForMoving;

				if (_isRolling != shouldRoll)
				{
					_isRolling = shouldRoll;
					_animator.SetBool(Moving, _isRolling);
					if (_isRolling)
					{
						_audioSource.Play();
					}
					else
					{
						_audioSource.Stop();
					}
				}
			}

			if (_isRolling)
			{
				_awarenessTimer -= Time.deltaTime;
				if (_awarenessTimer <= 0)
				{
					_awarenessTimer = _audioAwarenessCooldown;
					GameEvents.AudioAwarenessAdded.Dispatch(_currentSound.Awareness);
				}
			}
		}

		void OnTriggerEnter(Collider collider)
		{
			string colliderName = collider.name;
			
			if (colliderName == "Exit")
			{
				GameEvents.KidRescued.Dispatch();
				
				gameObject.SetActive(false);
				Free();
				GameEvents.AudioPlayed.Dispatch(_freedomSounds);
			}
			else if (colliderName == "LoseZone")
			{
				GameEvents.AudioAwarenessAdded.Dispatch(100f);
			}
		}

		protected override void ResetState()
		{
			base.ResetState();

			gameObject.SetActive(true);
			
			_animator.SetTrigger(Reset);
			_collider.enabled = true;

			_body.velocity = Vector3.zero;
		}
	}
}