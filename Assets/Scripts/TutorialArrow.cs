using System;
using System.Collections;
using UnityEngine;

namespace Classroom
{
	public class TutorialArrow : MonoBehaviour
	{
		MeshRenderer _renderer;

		void Awake()
		{
			GameEvents.MovementUnlocked.AddListener(OnCanMove);
			GameEvents.QuitGame.AddListener(OnQuitGame);

			_renderer = GetComponent<MeshRenderer>();
		}

		void OnQuitGame()
		{
			_renderer.enabled = true;
		}

		void OnCanMove()
		{
			StartCoroutine(DelayedHide());
		}

		IEnumerator DelayedHide()
		{
			yield return new WaitForSecondsRealtime(1f);

			_renderer.enabled = false;
		}
	}
}