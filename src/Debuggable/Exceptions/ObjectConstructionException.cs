namespace LeetTools.Debuggable.Exceptions
{
    public class ObjectConstructionException : Exception
    {
        private const string DEFAULT_MESSAGE = "Error constructing object.";

        public ObjectConstructionException() : base(DEFAULT_MESSAGE)
        {
        }

        public ObjectConstructionException(string? message) : base(message)
        {
        }

        public ObjectConstructionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public ObjectConstructionException(Exception? innerException) : base(DEFAULT_MESSAGE, innerException) 
        { 
        }
    }
}
