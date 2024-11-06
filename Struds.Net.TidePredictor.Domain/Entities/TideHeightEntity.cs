namespace Struds.Net.TidePredictor.Domain.Entities
{
    using System;

    public class TideHeightEntity
    { 
        public short StationId { get; set; }

        /// <summary>
        /// Gets or sets the date time for this <see cref="TideHeight"/>.
        /// </summary>
        /// <value>The date time.</value>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the height of Tide above Chart Datum .
        /// </summary>
        /// <value>The height.</value>
        public double Height { get; set; }
    }
}