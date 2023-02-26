using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KMolenda.Aisd.Graph;
using UnityEngine.Tilemaps;

public class Navigation
{
	private TilemapManager tilemapManager;
	private TileNode destination;
	public TileNode Destination
	{
		get => destination;
		set => destination = value;
	}

	public Navigation(TilemapManager tilemapManager)
	{
		this.tilemapManager = tilemapManager;
    }

	public IEnumerator GoDestination(TileNode start, TileNode end, Transform actor, int pathLength = 999)
	{
		foreach (var next in tilemapManager.Graph.ShortestPath(start, end))
		{
			if (start.Equals(next)) continue;
			if (pathLength <= 0) break;
			var nextWorldPos = tilemapManager.ChangeLocalToWorldPosition(next.position);
			while (Vector3.Distance(actor.position, nextWorldPos) > 0.0f)
			{
				actor.position = Vector3.MoveTowards(actor.position, nextWorldPos, Time.deltaTime);
				yield return null;
			}
			pathLength--;
		}

		destination = null;
	}

	public IEnumerator GoDestination(TileNode end, Transform actor)
	{
		var targetPosition = tilemapManager.ChangeWorldToLocalPosition(actor.position);
		yield return GoDestination(start: (TileNode)targetPosition, end, actor);
	}

	public void SetDestination(Vector3 tileWorldPosition)
	{
		var destination = tilemapManager.ChangeWorldToLocalPosition(tileWorldPosition);

		this.destination = (TileNode)destination;
	}

	public IEnumerator WaitForClickDestination()
	{
		yield return new WaitUntil(() => destination != null);
	}

	public TileNode[] GetShortestPathCoordArray(TileNode start, TileNode end)
	{
		List<TileNode> coordList = new List<TileNode>();
		foreach(var coord in tilemapManager.Graph.ShortestPath(start, end))
		{
			coordList.Add(coord);
		}

		return coordList.ToArray();
	}
}

