namespace ApiTemplate.Mappers
{
    using System;
    using ApiTemplate.Constants;
    using ApiTemplate.ViewModels;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class CarToCarMapper : IImmutableMapper<Models.Car, Car>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public CarToCarMapper(
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        public Car Map(Models.Car source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new Car()
            {
                CarId = source.CarId,
                Cylinders = source.Cylinders,
                Make = source.Make,
                Model = source.Model,
                Url = new Uri(this.linkGenerator.GetUriByRouteValues(
                    this.httpContextAccessor.HttpContext,
                    CarsControllerRoute.GetCar,
                    new { source.CarId })),
            };
        }
    }
}
