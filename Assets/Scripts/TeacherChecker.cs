using Kids;
using UnityEngine;

namespace Classroom
{
	public class TeacherChecker : MonoBehaviour
	{
		[SerializeField] CatchableItem[] _kids;
		[SerializeField] float _catchSpeed;
		
		void Awake()
		{
			GameEvents.TeachCheck.AddListener(OnTeachCheck);
		}

		void OnTeachCheck()
		{
			foreach (CatchableItem item in _kids)
			{
				if (item.IsFree)
				{
					continue;
				}

				if (item.Speed >= _catchSpeed)
				{
					Debug.Log("Caught something: " + item);
				}
			}
		}
	}
}