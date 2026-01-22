using System.Text.Json;

namespace LeetTools.Debuggable.Events
{
    public class MethodExecutedEventArgs(string methodName, object?[] arguments, long elapsedMs, object? returnValue = null) : EventArgs()
    {
        /// <summary>
        /// Name of method executed.
        /// </summary>
        public string MethodName { get; set; } = methodName;
        /// <summary>
        /// What arguments were passed for this invocation.
        /// </summary>
        public object?[] Arguments { get; } = arguments;
        /// <summary>
        /// How many milliseconds the execution takes.
        /// </summary>
        public long ElapsedMs { get; set; } = elapsedMs;
        /// <summary>
        /// The value returned from the method.  <see langword="null"/> if the method has a return type of <see langword="void"/>.
        /// </summary>
        public object? ReturnValue { get; set; } = returnValue;

        public override string ToString() => $"{MethodName}, {JsonSerializer.Serialize(Arguments)}: {ElapsedMs} ms returns {ReturnValue ?? "null"}";
    }
}
