using AutoMapper;
using HR_Managemnet.PL.ErrorsHandle;
using HR_Managemnet.PL.Helpers;
using HR_Managment.BLL.Interfaces;
using HR_Managment.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Services.DbIntializer;

namespace Services.Extensions
{
    public static class AplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            services.AddScoped(typeof(IUserSeed), typeof(UserSeed));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddAutoMapper(typeof(MappingProfiles));

            #region Configure Validation Exceptions
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    // ModelState => Dic [keyValuePair]
                    // Key => Name Of Param
                    // Value => Errors
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                        .SelectMany(p => p.Value.Errors)
                                                        .Select(e => e.ErrorMessage);
                    var validationErrorRespons = new ApiValidationErrorResponce()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorRespons);
                };
            });
            #endregion
            return services;
        }
    }
}
