using UnityEngine;

namespace Classroom
{
	public class SoundPlayer : MonoBehaviour
	{
		[SerializeField] AudioSource _source;

		static SoundPlayer Instance;
		
		void Awake()
		{
			Instance = this;
		}

		public static void PlaySound(AudioClip clip)
		{
			if (Instance)
			{
				Instance._source.PlayOneShot(clip);
			}
		}
	}
}