using System;
using System.Collections.Generic;

namespace TurnSystem
{
	public class TurnManager
	{
		private static Queue<ITurnActor> actors = new Queue<ITurnActor>();
		private ITurnActor currentActor;
		public ITurnActor CurrentActor { get => currentActor; }
		public TurnManager()
		{
			
		}

		public void JoinActor(ITurnActor turnActor)
		{
			actors.Enqueue(turnActor);
		}

		public ITurnActor UpdateTurn()
		{
			var actor = actors.Dequeue();
			currentActor = actor;
			actors.Enqueue(actor);

			return actor;
		}

		
	}
}