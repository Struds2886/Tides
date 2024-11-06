namespace Struds.Net.TidePredictor.Domain.Mappings
{
    using AutoMapper;

    public class MapperRegistration
    {
        public static MapperConfiguration Config()
        {
            var config = new MapperConfiguration(
                c =>
                {
                    c.AddProfile(new TidePredictorProfile());
                });

            return config;
        }
    }
}
