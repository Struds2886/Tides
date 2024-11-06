namespace Struds.Net.TidePredictor.Domain.Mappings
{
    using AutoMapper;
    using Entities;

    public class TidePredictorProfile : Profile
    {
        public TidePredictorProfile()
        {
            this.CreateMap<TideStation, FavoriteTideStation>()
                .ForMember(
                    dest => dest.StationIdentifier, 
                    opt => opt.MapFrom(src => src.StationIdentifier))
                .ForMember(
                    dest => dest.StationName, 
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(
                    dest => dest.Latitude, 
                    opt => opt.MapFrom(src => src.Latitude))
                .ForMember(
                    dest => dest.Longitude, 
                    opt => opt.MapFrom(src => src.Longitude))
                .ForMember(
                    dest => dest.IsInternational,
                    opt => opt.MapFrom(src => src.IsInternational))

                .ReverseMap()
                .ForMember(
                    dest => dest.StationIdentifier, 
                    opt => opt.MapFrom(src => src.StationIdentifier))
                .ForMember(
                    dest => dest.Name, 
                    opt => opt.MapFrom(src => src.StationName))
                .ForMember(
                    dest => dest.Latitude, 
                    opt => opt.MapFrom(src => src.Latitude))
                .ForMember(
                    dest => dest.Longitude, 
                    opt => opt.MapFrom(src => src.Longitude))
                .ForMember(
                    dest => dest.IsInternational,
                    opt => opt.MapFrom(src => src.IsInternational));
        }
    }
}