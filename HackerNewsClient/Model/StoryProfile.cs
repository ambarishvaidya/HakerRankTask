using AutoMapper;

namespace HackerNewsClient.Model
{
    public class StoryProfile : Profile
    {
        private static IMapper mapper;
        private static string DateFormat = @"yyyy-MM-dd'T'HH:mm:sszzz";
        static StoryProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Story, StoryReadDto>()
                .ForMember(dto => dto.uri, opts => opts.MapFrom(src => src.url))
                .ForMember(dto => dto.postedBy, opts => opts.MapFrom(src => src.by))
                .ForMember(dto => dto.commentCount, opts => opts.MapFrom(src => src.descendants))
                .ForMember(dto => dto.time, opts => opts.MapFrom(src => DateTimeOffset.FromUnixTimeSeconds(src.time).ToString(DateFormat)));
            });
            mapper = config.CreateMapper();
        }        

        public static IMapper Mapper => mapper;
    }
}
