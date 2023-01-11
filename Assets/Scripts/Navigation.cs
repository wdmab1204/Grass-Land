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

	public IEnumerator GoDestination(TileNode start, TileNode end, Transform target)
	{
        foreach (var next in TilemapReader.Graph.ShortestPath(start, end))
        {
			var nextWorldPos = TilemapReader.ChangeLocalToWorldPosition(next.position);
            while (Vector3.Distance(target.position, nextWorldPos) > 0.0f)
            {
                target.position = Vector3.MoveTowards(target.position, nextWorldPos, Time.deltaTime);
                yield return null;
            }
        }

		destination = null;
    }

	public IEnumerator GoDestination(TileNode end, Transform target)
	{
		var targetPosition = TilemapReader.ChangeWorldToLocalPosition(target.position);
		yield return GoDestination(start: (TileNode)targetPosition, end, target);
	}

	public IEnumerator GoDestination(Vector3 end, Transform target)
	{
		var endPosition = TilemapReader.ChangeWorldToLocalPosition(end);
		yield return GoDestination(end: (TileNode)endPosition, target);
	}

	public void CreateDestination(HighlightTile hTile)
	{
		var worldPosition = hTile.transform.position;
		var destination = TilemapReader.ChangeWorldToLocalPosition(worldPosition);

		this.destination = (TileNode)destination;
	}

	public IEnumerator WaitForClickDestination()
	{
		yield return new WaitUntil(() => destination != null);
	}
}

