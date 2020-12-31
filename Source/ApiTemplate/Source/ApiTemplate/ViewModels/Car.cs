namespace ApiTemplate.ViewModels
{
    using System;

#if Swagger
    /// <summary>
    /// A make and model of car.
    /// </summary>
#endif
    public record Car
    {
#if Swagger
        /// <summary>
        /// Gets or sets the cars unique identifier.
        /// </summary>
        /// <example>1</example>
#endif
        public int CarId { get; init; }

#if Swagger
        /// <summary>
        /// Gets or sets the number of cylinders in the cars engine.
        /// </summary>
        /// <example>6</example>
#endif
        public int Cylinders { get; init; }

#if Swagger
        /// <summary>
        /// Gets or sets the make of the car.
        /// </summary>
        /// <example>Honda</example>
#endif
        public string Make { get; init; }

#if Swagger
        /// <summary>
        /// Gets or sets the model of the car.
        /// </summary>
        /// <example>Civic</example>
#endif
        public string Model { get; init; }

#if Swagger
        /// <summary>
        /// Gets or sets the URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        /// <example>/cars/1</example>
#endif
        public Uri Url { get; init; }
    }
}
