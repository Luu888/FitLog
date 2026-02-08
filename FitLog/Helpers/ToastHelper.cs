using FitLog.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Reflection;

namespace FitLog.Helpers
{
    public static class ToastHelper
    {
        public static IActionResult ToJsonResult<T>(int result) where T : Enum
        {
            T? enumValue = Enum.IsDefined(typeof(T), result)
                ? (T)Enum.ToObject(typeof(T), result)
                : default;

            var memberInfo = typeof(T).GetMember(enumValue.ToString()).FirstOrDefault();
            var toastAttr = memberInfo?.GetCustomAttribute<ToastAttribute>();

            string message;
            string type;

            if (result > 0)
            {
                type = "success";
                message = "Success";
            }
            else if (result == 0)
            {
                type = "warning";
                message = toastAttr?.Message ?? "No entries were imported.";
            }
            else
            {
                type = "danger";
                message = toastAttr?.Message ?? "Error occurred.";
            }

            bool success = result > 0;

            return new JsonResult(new { success, message, type });
        }
    }



}
