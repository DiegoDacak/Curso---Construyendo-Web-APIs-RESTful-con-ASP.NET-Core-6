using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MoviesApi.Common.Messages;
using Newtonsoft.Json;

namespace MoviesApi.Services.ModelBinder
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.ModelName;
            var valueProvider = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProvider == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                if (valueProvider.FirstValue != null)
                {
                    var valueDeserialized = JsonConvert.DeserializeObject<T>(valueProvider.FirstValue);
                    bindingContext.Result = ModelBindingResult.Success(valueDeserialized);
                }
                else
                {
                    return Task.CompletedTask;
                }
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(modelName, ErrorMessages.ModelBinder);
            }
            return Task.CompletedTask;
        }
    }
}