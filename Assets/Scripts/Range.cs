using System;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Range
{
	public Vector2Int[] localCoords;
	public Vector3[] worldCoords;
	private Transform center;
	private Tilemap tilemap;

	public Range(Transform center, string rangeString, Tilemap tilemap)
	{
        localCoords = CardNameSpace.Base.CoordConverter.ConvertToCoords(rangeString);

		worldCoords = new Vector3[localCoords.Length];
		for(int i=0; i<localCoords.Length; i++)
		{
			var worldCoord = center.position + tilemap.ChangeLocalToWorldPosition((Vector3Int)localCoords[i]);
			worldCoords[i] = worldCoord;
		}
    }
}

