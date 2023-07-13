using AutoMapper;

namespace HackerRankClient.Model
{
    public class StoryProfile : Profile
    {
        private static IMapper mapper;
        static StoryProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Story, StoryReadDto>()
                .ForMember(dto => dto.uri, opts => opts.MapFrom(src => src.url))
                .ForMember(dto => dto.postedBy, opts => opts.MapFrom(src => src.by))
                .ForMember(dto => dto.commentCount, opts => opts.MapFrom(src => src.descendants));
            });
            mapper = config.CreateMapper();
        }        

        public static IMapper Mapper => mapper;
    }
}
