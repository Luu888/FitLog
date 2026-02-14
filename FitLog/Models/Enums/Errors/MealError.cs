using FitLog.Attributes;

namespace FitLog.Models.Enums.Errors
{
    public enum MealError
    {
        [Toast("Not found")]
        NotFound = -1,

        [Toast("Empty meal values!")]
        EmptyValues = -2,
    }
}
