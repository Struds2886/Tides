// ReSharper disable InconsistentNaming

namespace Struds.Net.TidePredictor.Domain.Weather
{
    using System;

    public static class WindDirectionMap
    {
        public static double ToBearing(this CardinalPoints direction)
        {
            switch (direction)
            {
                case CardinalPoints.N:
                    return 0.0;
                case CardinalPoints.NNE:
                    return 22.50;
                case CardinalPoints.NE:
                    return 45.0;
                case CardinalPoints.ENE:
                    return 67.50;
                case CardinalPoints.E:
                    return 90.0;
                case CardinalPoints.ESE:
                    return 112.50;
                case CardinalPoints.SE:
                    return 135.0;
                case CardinalPoints.SSE:
                    return 157.50;
                case CardinalPoints.S:
                    return 180.0;
                case CardinalPoints.SSW:
                    return 202.50;
                case CardinalPoints.SW:
                    return 225.0;
                case CardinalPoints.WSW:
                    return 247.50;
                case CardinalPoints.W:
                    return 270.00;
                case CardinalPoints.WNW:
                    return 292.50;
                case CardinalPoints.NW:
                    return 315.00;
                case CardinalPoints.NNW:
                    return 337.50;
                default:
                    return 0.0;
            }
        }

        public static CardinalPoints ToCardinalPoints(this string direction)
        {
            if (Enum.TryParse<CardinalPoints>(direction, true, out var point))
            {
                return point;
            }

            return CardinalPoints.Unknown;
        }

        public static CardinalPoints ToCardinalPoints(this int i)
        {
            if (i >= 349 && i <= 11)
            {
                return CardinalPoints.N;
            }
            else if (i >= 12 && i <= 33)
            {
                return CardinalPoints.NNE;
            }
            else if (i >= 34 && i <= 56)
            {
                return CardinalPoints.NE;
            }
            else if (i >= 57 && i <= 78)
            {
                return CardinalPoints.ENE;
            }
            else if (i >= 79 && i <= 101)
            {
                return CardinalPoints.E;
            }
            else if (i >= 102 && i <= 123)
            {
                return CardinalPoints.ESE;
            }
            else if (i >= 124 && i <= 146)
            {
                return CardinalPoints.SE;
            }
            else if (i >= 147 && i <= 168)
            {
                return CardinalPoints.SSE;
            }
            else if (i >= 169 && i <= 191)
            {
                return CardinalPoints.S;
            }
            else if (i >= 192 && i <= 213)
            {
                return CardinalPoints.SSW;
            }
            else if (i >= 214 && i <= 236)
            {
                return CardinalPoints.SW;
            }
            else if (i >= 237 && i <= 258)
            {
                return CardinalPoints.WSW;
            }
            else if (i >= 259 && i <= 281)
            {
                return CardinalPoints.W;
            }
            else if (i >= 282 && i <= 303)
            {
                return CardinalPoints.WNW;
            }
            else if (i >= 304 && i <= 326)
            {
                return CardinalPoints.NW;
            }
            else if (i >= 327 && i <= 348)
            {
                return CardinalPoints.NNW;
            }

            return CardinalPoints.Unknown;
        }
    }
}
