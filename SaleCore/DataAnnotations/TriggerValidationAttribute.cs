using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SaleCore.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Method)]
	public class TriggerValidationAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var parameters = filterContext.ActionDescriptor.GetParameters();
			foreach (var pd in parameters)
			{
				var atts = pd.GetCustomAttributes(typeof (ValidationAttribute), false);
				var parameterValue = GetParameterValue(pd, filterContext);

				foreach (ValidationAttribute va in atts)
				{
					if (!va.IsValid(parameterValue))
					{
						var modelState = filterContext.Controller.ViewData.ModelState;
						modelState.AddModelError(pd.ParameterName, va.FormatErrorMessage(pd.ParameterName));
					}
				}
			}
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{

		}

		private Predicate<string> GetPropertyFilter(ParameterDescriptor parameterDescriptor)
		{
			ParameterBindingInfo bindingInfo = parameterDescriptor.BindingInfo;
			var ba = new BindAttribute()
			{
				Exclude = string.Join(",", bindingInfo.Exclude),
				Include = string.Join(",", bindingInfo.Include)
			};
			return ba.IsPropertyAllowed;
		}

		private object GetParameterValue(ParameterDescriptor pd, ActionExecutingContext filterContext)
		{
			Type parameterType = pd.ParameterType;
			IModelBinder binder = pd.BindingInfo.Binder ?? ModelBinders.Binders.GetBinder(pd.ParameterType);
			IValueProvider valueProvider = filterContext.Controller.ValueProvider;
			string parameterName = pd.BindingInfo.Prefix ?? pd.ParameterName;
			Predicate<string> propertyFilter = GetPropertyFilter(pd);

			ModelBindingContext bindingContext = new ModelBindingContext()
			{
				FallbackToEmptyPrefix = (pd.BindingInfo.Prefix == null),
				ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, parameterType),
				ModelName = parameterName,
				ModelState = filterContext.Controller.ViewData.ModelState,
				PropertyFilter = propertyFilter,
				ValueProvider = valueProvider
			};

			object result = binder.BindModel(filterContext, bindingContext);
			return result ?? pd.DefaultValue;
		}
	}
}
