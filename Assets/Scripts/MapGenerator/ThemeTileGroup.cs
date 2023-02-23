using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class ThemeTileGroup : MonoBehaviour
{
	[System.Serializable]
	public class ThemeTile
	{
		public TileBase[] grounds;
		public TileBase[] walls;
		public TileBase[] doors;
	}

	public ThemeTile grassland;
	public ThemeTile deadmine;
	public ThemeTile ancientremains;
}

