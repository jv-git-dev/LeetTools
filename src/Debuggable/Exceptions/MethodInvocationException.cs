namespace LeetTools.Debuggable.Exceptions
{
    public class MethodInvocationException : Exception
    {
        public string MethodName { get; }
        public int InvocationNumber { get; }
        public object?[] Arguments { get; }

        public MethodInvocationException(string methodName, int invocationNumber, object?[] arguments, Exception? innerException = null) 
            : base($"Error invoking ${methodName}.", innerException)
        {
            MethodName = methodName;
            InvocationNumber = invocationNumber;
            Arguments = arguments;
        }
    }
}
