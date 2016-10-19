﻿//-----------------------------------------------------------------------
// <copyright file="ReverseGeocodeResource.cs" company="Mapbox">
//     Copyright (c) 2016 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Mapbox.Geocoding
{
    using System.Collections.Generic;

    /// <summary> A reverse geocode request. </summary>
    public sealed class ReverseGeocodeResource : GeocodeResource<LatLng>
    {
        // Required
        private LatLng query;

        /// <summary> Initializes a new instance of the <see cref="ReverseGeocodeResource" /> class.</summary>
        /// <param name="query"> Location to reverse geocode. </param>
        public ReverseGeocodeResource(LatLng query)
        {
            this.Query = query;
        }

        /// <summary> Gets or sets the location. </summary>
        public override LatLng Query
        {
            get
            {
                return this.query;
            }
         
            set
            {
                this.query = value;
            }
        }

        /// <summary> Builds a complete reverse geocode URL string. </summary>
        /// <returns> A complete, valid reverse geocode URL string. </returns>
        public override string GetUrl()
        {
            var opts = new List<string>();

            if (this.Types != null)
            {
                opts.Add("types=" + ReverseGeocodeResource.GetUrlQueryFromArray(this.Types));
            }

            return Constants.BaseAPI +
                            this.ApiEndpoint +
                            this.Mode +
                            this.Query.ToString() +
                            ".json" +
                            ReverseGeocodeResource.GetOptsString(opts);
        }
    }
}