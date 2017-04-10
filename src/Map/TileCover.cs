//-----------------------------------------------------------------------
// <copyright file="TileCover.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Map
{
	using System;
	using System.Collections.Generic;
	using Mapbox.Utils;

	/// <summary>
	///     Helper funtions to get a tile cover, i.e. a set of tiles needed for
	///     covering a bounding box.
	/// </summary>
	/// <example>
	/// Build a map of Colorado using TileCover:
	/// <code>
	/// var sw = new Vector2d(36.997749, -109.0524961);
	/// var ne = new Vector2d(41.0002612, -102.0609668);
	/// var coloradoBounds = new Vector2dBounds(sw, ne);
	/// var tileCover = TileCover.Get(coloradoBounds, 8);
	/// Debug.Log("Tiles Needed: " + tileCover.Count);
	/// foreach (var id in tileCover)
	/// {
	/// 	var tile = new RasterTile();
	/// 	var parameters = new Tile.Parameters();
	/// 	parameters.Id = id;
	///		parameters.Fs = MapboxAccess.Instance;
	///		parameters.MapId = "mapbox://styles/mapbox/outdoors-v10";
	///		tile.Initialize(parameters, (Action)(() =>
	///		{
	///			var tileQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
	/// 		tileQuad.transform.SetParent(transform);
	///			tileQuad.name = tile.Id.ToString();
	///			tileQuad.transform.position = new Vector3(tile.Id.X, -tile.Id.Y, 0);
	/// 		var texture = new Texture2D(0, 0);
	/// 		texture.LoadImage(tile.Data);
	///			var material = new Material(Shader.Find("Unlit/Texture"));
	/// 		material.mainTexture = texture;
	///			tileQuad.GetComponent&lt;MeshRenderer&gt().sharedMaterial = material;
	///		}));
	///	}
	/// </code>
	/// </example>
	public static class TileCover
	{
		/// <summary> Get a tile cover for the specified bounds and zoom. </summary>
		/// <param name="bounds"> Geographic bounding box.</param>
		/// <param name="zoom"> Zoom level. </param>
		/// <returns> The tile cover set. </returns>
		public static HashSet<CanonicalTileId> Get(Vector2dBounds bounds, int zoom)
		{
			var tiles = new HashSet<CanonicalTileId>();

			if (bounds.IsEmpty() ||
				bounds.South > Constants.LatitudeMax ||
				bounds.North < -Constants.LatitudeMax)
			{
				return tiles;
			}

			var hull = Vector2dBounds.FromCoordinates(
				new Vector2d(Math.Max(bounds.South, -Constants.LatitudeMax), bounds.West),
				new Vector2d(Math.Min(bounds.North, Constants.LatitudeMax), bounds.East));

			var sw = CoordinateToTileId(hull.SouthWest, zoom);
			var ne = CoordinateToTileId(hull.NorthEast, zoom);

			// Scanlines.
			for (var x = sw.X; x <= ne.X; ++x)
			{
				for (var y = ne.Y; y <= sw.Y; ++y)
				{
					tiles.Add(new UnwrappedTileId(zoom, x, y).Canonical);
				}
			}

			return tiles;
		}

		/// <summary> Converts a coordinate to a tile identifier. </summary>
		/// <param name="coord"> Geographic coordinate. </param>
		/// <param name="zoom"> Zoom level. </param>
		/// <returns>The to tile identifier.</returns>
		/// <example>
		/// Convert a geocoordinate to a TileId:
		/// <code>
		/// var unwrappedTileId = TileCover.CoordinateToTileId(new Vector2d(40.015, -105.2705), 18);
		/// Debug.Log("UnwrappedTileId: " + unwrappedTileId.ToString());
		/// </code>
		/// </example>
		public static UnwrappedTileId CoordinateToTileId(Vector2d coord, int zoom)
		{
			var lat = coord.x;
			var lng = coord.y;

			// See: http://wiki.openstreetmap.org/wiki/Slippy_map_tilenames
			var x = (int)Math.Floor((lng + 180.0) / 360.0 * Math.Pow(2.0, zoom));
			var y = (int)Math.Floor((1.0 - Math.Log(Math.Tan(lat * Math.PI / 180.0)
					+ 1.0 / Math.Cos(lat * Math.PI / 180.0)) / Math.PI) / 2.0 * Math.Pow(2.0, zoom));

			return new UnwrappedTileId(zoom, x, y);
		}
	}
}
