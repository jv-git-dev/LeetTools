namespace LeetTools.Debuggable.Events
{
    public class MethodExecutedEventArgs(string methodName, long elapsedMs, object? returnValue = null) : EventArgs()
    {
        public string MethodName { get; set; } = methodName;

        public long ElapsedMs { get; set; } = elapsedMs;

        public object? ReturnValue { get; set; } = returnValue;

        public override string ToString() => $"{MethodName}: {ElapsedMs} ms, returns {ReturnValue ?? "null"}";
    }
}
