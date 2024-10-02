using BookReservationSystem.Application.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookReservationSystem.WebApi.Filters
{
    public class DefaultValueSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // Debug output to see the type being processed
            Console.WriteLine($"Processing type: {context.Type.Name}");

            if (context.Type == typeof(BookDto))
            {
                // Check if the properties contain IsReserved
                if (schema.Properties.ContainsKey("isReserved"))
                {
                    schema.Properties["isReserved"].Default = new OpenApiBoolean(false);
                }
                else
                {
                    Console.WriteLine("Property 'IsReserved' not found in schema properties.");
                }

                // Similar check for ReservationComment
                if (schema.Properties.ContainsKey("reservationComment"))
                {
                    schema.Properties["reservationComment"].Default = new OpenApiString(string.Empty);
                }
                else
                {
                    Console.WriteLine("Property 'ReservationComment' not found in schema properties.");
                }
            }
        }
    }

}
