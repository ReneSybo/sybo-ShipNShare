using UnityEngine;

namespace Classroom.Sound
{
	[CreateAssetMenu(menuName = "RollingGame/GameSound" ,fileName = "GameSound")]
	public class GameSound : ScriptableObject
	{
		public AudioClip Clip;
		public float Awareness;
	}
}