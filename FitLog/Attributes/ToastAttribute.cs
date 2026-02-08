namespace FitLog.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ToastAttribute : Attribute
    {
        public string Message { get; }
        public string? Type { get; }

        public ToastAttribute(string message)
        {
            Message = message;
        }
    }
}
