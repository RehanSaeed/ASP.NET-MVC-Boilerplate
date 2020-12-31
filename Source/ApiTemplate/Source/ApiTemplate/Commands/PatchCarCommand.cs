namespace ApiTemplate.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using ApiTemplate.Repositories;
    using ApiTemplate.ViewModels;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class PatchCarCommand : IPatchCarCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IObjectModelValidator objectModelValidator;
        private readonly ICarRepository carRepository;
        private readonly IImmutableMapper<Models.Car, Car> carToCarMapper;
        private readonly IMapper<Models.Car, SaveCar> carToSaveCarMapper;
        private readonly IMapper<SaveCar, Models.Car> saveCarToCarMapper;

        public PatchCarCommand(
            IActionContextAccessor actionContextAccessor,
            IObjectModelValidator objectModelValidator,
            ICarRepository carRepository,
            IImmutableMapper<Models.Car, Car> carToCarMapper,
            IMapper<Models.Car, SaveCar> carToSaveCarMapper,
            IMapper<SaveCar, Models.Car> saveCarToCarMapper)
        {
            this.actionContextAccessor = actionContextAccessor;
            this.objectModelValidator = objectModelValidator;
            this.carRepository = carRepository;
            this.carToCarMapper = carToCarMapper;
            this.carToSaveCarMapper = carToSaveCarMapper;
            this.saveCarToCarMapper = saveCarToCarMapper;
        }

        public async Task<IActionResult> ExecuteAsync(
            int carId,
            JsonPatchDocument<SaveCar> patch,
            CancellationToken cancellationToken)
        {
            var car = await this.carRepository.GetAsync(carId, cancellationToken).ConfigureAwait(false);
            if (car is null)
            {
                return new NotFoundResult();
            }

            var saveCar = this.carToSaveCarMapper.Map(car);
            var modelState = this.actionContextAccessor.ActionContext.ModelState;
            patch.ApplyTo(saveCar, modelState);
            this.objectModelValidator.Validate(
                this.actionContextAccessor.ActionContext,
                validationState: null,
                prefix: null,
                model: saveCar);
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(modelState);
            }

            this.saveCarToCarMapper.Map(saveCar, car);
            await this.carRepository.UpdateAsync(car, cancellationToken).ConfigureAwait(false);
            var carViewModel = this.carToCarMapper.Map(car);

            return new OkObjectResult(carViewModel);
        }
    }
}
