// ***********************************************************************
// Assembly         : Struds.Net.TidePredictor.Api
// Author           : Mark Strudwick
// Created          : 05-21-2020
//
// Last Modified By : Mark Strudwick
// Last Modified On : 05-21-2020
// ***********************************************************************
// <copyright file="TideHeight.cs" company="Struds.Net.TidePredictor.Api">
//     Copyright (c) Struds.Net Ltd. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Struds.Net.TidePredictor.Domain
{
    using System;

    /// <summary>
    /// Class TideHeight.
    /// </summary>
    public class TideHeight
    {
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
