using System;
using Classroom.Sound;

namespace Classroom
{
	public static class GameEvents
	{
		public static Event<float> AudioAwarenessAdded = new Event<float>();
		public static Event<GameSound> AudioPlayed = new Event<GameSound>();
		public static Event<float> LookHappeningIn = new Event<float>();
		public static Event GameStarted = new Event();
		public static Event GameEnded = new Event();
		public static Event QuitGame = new Event();
		public static Event GameWon = new Event();
		public static Event MovementUnlocked = new Event();
		public static Event KidRescued = new Event();
	}

	public class Event
	{
		event Action _listeners;
		
		public void AddListener(Action listener)
		{
			_listeners += listener;
		}
		
		public void RemoveListener(Action listener)
		{
			_listeners -= listener;
		}

		public void Dispatch()
		{
			_listeners?.Invoke();
		}
	}
	
	public class Event<T>
	{
		event Action<T> _listeners;
		
		public void AddListener(Action<T> listener)
		{
			_listeners += listener;
		}
		
		public void RemoveListener(Action<T> listener)
		{
			_listeners -= listener;
		}

		public void Dispatch(T data)
		{
			_listeners?.Invoke(data);
		}
	}
}