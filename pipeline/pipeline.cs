using MacMarketGroupApi.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


public class CustomActionFilter : IActionFilter
{
    private readonly ILogger<CustomActionFilter> _logger;

    public CustomActionFilter(ILogger<CustomActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {

        var formData = context.HttpContext.Request.Form;
        // _logger.LogInformation("Form data: {@FormData}", formData);

        // Replace "data" with your desired form field key
        if (formData.TryGetValue("data", out var data))
        {
            // Convert the custom data value to an object (assuming it's a JSON string)
            // var json = data.ToString();
            // var obj = JsonSerializer.Deserialize<Product>(json);

            // // Perform validation on the object
            // var validationResults = new List<ValidationResult>();
            // var validationContext = new ValidationContext(obj);
            // if (!Validator.TryValidateObject(obj, validationContext, validationResults, true))
            // {
            //     // Validation failed, handle the errors
            //     // var validationErrors = validationResults.Select(result => result.ErrorMessage);
            //     // Do something with the validation errors, such as logging or returning an error response
            // }

            // Replace the original data value with the deserialized object
            // Create a new FormCollection with the updated values
            // var updatedFormData = new FormCollection(formData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            // updatedFormData["data"] = obj.ToString(); // Assuming you want to store the serialized object as a string

            // // Replace the original FormCollection with the updated one
            // context.HttpContext.Request.Form = updatedFormData;
        }



        // _logger.LogInformation($"Form data: {formData.GetType("data")}");


        // Logic to be executed before the action method is called
        _logger.LogInformation("Before executing the action method");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Logic to be executed after the action method is called
        _logger.LogInformation("After executing the action method");
    }
}
