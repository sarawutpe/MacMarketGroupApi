using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using System.Reflection;
namespace MacMarketGroupApi.Models;

public class AuthorEntityBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        return new AuthorEntityBinder();
    }
}

public class AuthorEntityBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        var projectName = Assembly.GetEntryAssembly()?.GetName().Name;
        var modelTypeName = context.ModelMetadata.ModelType.Name;
        var modelName = context.ModelName;
        var valueProviderResult = context.ValueProvider.GetValue(modelName);


        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value))
            return Task.CompletedTask;

        try
        {
            // Get Class of Models
            var classType = Type.GetType($"{projectName}.Models.{modelTypeName}");

            if (classType is null)
            {
                context.ModelState.TryAddModelError(modelName, "Invalid Class type of model.");
                return Task.CompletedTask;
            }

            // Deserialize the JSON string to a object
            var jsonObject = JsonSerializer.Deserialize(value, classType);
            // Next Task
            context.Result = ModelBindingResult.Success(jsonObject);
        }
        catch (JsonException)
        {
            context.ModelState.TryAddModelError(modelName, "Invalid JSON format.");
        }
        return Task.CompletedTask;
    }
}
