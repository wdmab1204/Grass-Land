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

		public TurnActor UpdateTurn()
		{
			var actor = actors.Dequeue();
			currentActor = actor;
			actors.Enqueue(actor);

			UnityEngine.Debug.Log(actor.name);
			return actor;
		}

		
	}
}