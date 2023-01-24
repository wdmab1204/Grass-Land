using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using KMolenda.Aisd.Graph;
using UnityEngine.Tilemaps;

public class Navigation
{
	private TilemapReader TilemapReader;
	private TileNode destination;
	public TileNode Destination
	{
		get => destination;
		set => destination = value;
	}

	public Navigation(TilemapReader tilemapReader)
	{
		this.TilemapReader = tilemapReader;
    }

	public IEnumerator GoDestination(TileNode start, TileNode end, Transform actor)
	{
        foreach (var next in TilemapReader.Graph.ShortestPath(start, end))
        {
			var nextWorldPos = TilemapReader.ChangeLocalToWorldPosition(next.position);
            while (Vector3.Distance(actor.position, nextWorldPos) > 0.0f)
            {
                actor.position = Vector3.MoveTowards(actor.position, nextWorldPos, Time.deltaTime);
                yield return null;
            }
        }

		destination = null;
    }

	public IEnumerator GoDestination(TileNode end, Transform actor)
	{
		var targetPosition = TilemapReader.ChangeWorldToLocalPosition(actor.position);
		yield return GoDestination(start: (TileNode)targetPosition, end, actor);
	}

	public IEnumerator GoDestination(Vector3 end, Transform actor)
	{
		var endPosition = TilemapReader.ChangeWorldToLocalPosition(end);
		yield return GoDestination(end: (TileNode)endPosition, actor);
	}

	public void SetDestination(Vector3 tileWorldPosition)
	{
		var destination = TilemapReader.ChangeWorldToLocalPosition(tileWorldPosition);

		this.destination = (TileNode)destination;
	}

	public IEnumerator WaitForClickDestination()
	{
		yield return new WaitUntil(() => destination != null);
	}

	public TileNode[] GetShortestPathCoordArray(TileNode start, TileNode end)
	{
		List<TileNode> coordList = new List<TileNode>();
		foreach(var coord in TilemapReader.Graph.ShortestPath(start, end))
		{
			coordList.Add(coord);
		}

		return coordList.ToArray();
	}
}

