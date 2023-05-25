using System;
using System.Collections.Generic;

namespace TurnSystem
{
	public class TurnManager
	{
		private Queue<TurnActor> actors = new Queue<TurnActor>();
		private TurnActor currentActor;
		public TurnActor CurrentActor { get => currentActor; }
		public TurnManager()
		{
			
		}

		public void JoinActor(TurnActor turnActor)
		{
			actors.Enqueue(turnActor);
		}

		public void AllRemoveActor()
		{
			actors.Clear();
		}

		public TurnActor GetNextTurn()
		{
			var actor = actors.Dequeue();
			currentActor = actor;
			actors.Enqueue(actor);

			return actor;
		}

		public void LogTurnQueue()
		{
			foreach(var e in actors)
			{
				UnityEngine.Debug.Log(e.name);
			}
		}
		
	}
}