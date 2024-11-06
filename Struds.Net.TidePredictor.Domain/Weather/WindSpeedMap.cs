namespace Struds.Net.TidePredictor.Domain.Weather
{
    public static class WindSpeedMap
    {
        /*
            Force       Description     km/h    mph     knots   Specifications
            0	        Calm	        <1	    <1	    <1      Smoke rises vertically
            1	        Light Air       1-5     1-3     1-3     Direction shown by smoke drift but not by wind vanes
            2	        Light Breeze    6-11	4-7	    4-6	    Wind felt on face; leaves rustle; wind vane moved by wind
            3	        Gentle Breeze	12-19	8-12	7-10	Leaves and small twigs in constant motion; light flags extended
            4	        Moderate Breeze	20-28	13-18	11-16	Raises dust and loose paper; small branches moved.
            5	        Fresh Breeze	29-38	19-24	17-21	Small trees in leaf begin to sway; crested wavelets form on inland waters.
            6	        Strong Breeze	38-49	25-31	22-27	Large branches in motion; whistling heard in telegraph wires; umbrellas used with difficulty.
            7	        Near Gale	    50-61	32-38	28-33	Whole trees in motion; inconvenience felt when walking against the wind.
            8	        Gale	        62-74	39-46	34-40	Twigs break off trees; generally impedes progress.
            9	        Strong Gale	    75-88	47-54	41-47	Slight structural damage (chimney pots and slates removed).
            10	        Storm	        89-102	55-63	48-55	Seldom experienced inland; trees uprooted; considerable structural damage
            11	        Violent Storm	103-117	64-72	56-63	Very rarely experienced; accompanied by widespread damage.
            12	        Hurricane	    118+    73+     64+     Devastation
         */
        public static BeaufortScale ToBeaufortScale(this int mph)
        {
            if (mph < 1)
            {
                return BeaufortScale.Force0;
            }

            if (mph <= 3)
            {
                return BeaufortScale.Force1;
            }

            if (mph <= 7)
            {
                return BeaufortScale.Force2;
            }

            if (mph <= 12)
            {
                return BeaufortScale.Force3;
            }

            if (mph <= 18)
            {
                return BeaufortScale.Force4;
            }

            if (mph <= 24)
            {
                return BeaufortScale.Force5;
            }

            if (mph <= 31)
            {
                return BeaufortScale.Force6;
            }

            if (mph <= 38)
            {
                return BeaufortScale.Force7;
            }

            if (mph <= 46)
            {
                return BeaufortScale.Force8;
            }

            if (mph <= 54)
            {
                return BeaufortScale.Force9;
            }

            if (mph <= 63)
            {
                return BeaufortScale.Force10;
            }

            if (mph <= 72)
            {
                return BeaufortScale.Force11;
            }

            return BeaufortScale.Force12;
        }


    }
}
