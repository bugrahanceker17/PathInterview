using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace PathInterview.Core.Extensions
{
    public static class AutoMapperConfiguration
    {
        public static void AutoMapperConfig(this IServiceCollection serviceCollection, MapperConfiguration mapperConfiguration)
        {
            IMapper mapper = mapperConfiguration.CreateMapper();
            serviceCollection.AddSingleton(mapper);
        }
    }
}

