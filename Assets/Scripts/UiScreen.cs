using TMPro;
using UnityEngine;

namespace Classroom
{
	public class UiScreen : MonoBehaviour
	{
		[SerializeField] Teacher _teacher;
		[SerializeField] TMP_Text _text;

		void Update()
		{
			string current = _teacher.CurrentAwareness.ToString("0.00");
			string cap = _teacher.AwarenessCap.ToString("0");

			_text.text = $"{current}/{cap}";
		}
	}
}