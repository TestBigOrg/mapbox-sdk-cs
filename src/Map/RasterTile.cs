//-----------------------------------------------------------------------
// <copyright file="RasterTile.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Map
{
	/// <summary>
	/// A raster tile from the Mapbox Style API, an encoded image representing a geographic
	/// bounding box. Usually JPEG or PNG encoded.
	/// <example>
	/// How to use 'RasterTile':
	/// <code>
	/// var parameters = new Tile.Parameters();
	/// parameters.Fs = MapboxAccess.Instance;
	/// parameters.Id = new CanonicalTileId(_zoom, _tileCoorindateX, _tileCoordinateY);
	/// parameters.MapId = "mapbox://styles/mapbox/satellite-v9";
	/// var rasterTile = new RasterTile();
	/// 
	/// // Make the request.
	/// rasterTile.Initialize(parameters, (Action)(() =>
	/// {
	/// 	if (string.IsNullOrEmpty(rasterTile.Error))
	/// 	{
	///			// Handle the error.
	///		}
	/// 
	/// 	// Consume the <see cref="Data"/>.
	///	}));
	/// </code>
	/// </example>
	/// </summary>
	public class RasterTile : Tile
	{
		private byte[] data;

		/// <summary> Gets the raster tile raw data. </summary>
		/// <value> The raw data, usually an encoded JPEG or PNG. </value>
		/// <example>What does this tag do?</example>
		/// <code>
		/// // Example usage in Unity
		/// var texture = new Texture2D(0, 0);
		/// texture.LoadImage(rasterTile.Data);
		/// _sampleMaterial.mainTexture = texture;
		/// </code>
		public byte[] Data
		{
			get
			{
				return this.data;
			}
		}

		internal override TileResource MakeTileResource(string styleUrl)
		{
			return TileResource.MakeRaster(Id, styleUrl);
		}

		internal override bool ParseTileData(byte[] data)
		{
			// We do not parse raster tiles as they are
			this.data = data;

			return true;
		}
	}
}
