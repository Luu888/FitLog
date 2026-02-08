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

            string message = toastAttr?.Message ?? enumValue.ToString();

            string type = result > 0 ? "success" :
                          result == 0 ? "warning" :
                          "danger";
            if (result < 0 && toastAttr != null)
            {
                type = toastAttr.Type ?? "danger";
                message = toastAttr.Message ?? message;
            }

            bool success = result > 0;

            return new JsonResult(new { success, message, type });
        }
    }


}
