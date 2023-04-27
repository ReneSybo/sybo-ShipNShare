using Classroom.Sound;
using UnityEngine;

namespace Classroom
{
	public class SoundPlayer : MonoBehaviour
	{
		[SerializeField] AudioSource _source;

		void Awake()
		{
			GameEvents.AudioPlayed.AddListener(OnAudioPlayed);
		}

		void OnAudioPlayed(GameSound sound)
		{
			_source.PlayOneShot(sound.Clip);
		}
	}
}