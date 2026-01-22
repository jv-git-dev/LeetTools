using LeetTools.Debuggable.Events;
using LeetTools.Debuggable.Exceptions;
using System.Diagnostics;
using System.Reflection;

namespace LeetTools.Debuggable
{
    /// <summary>
    /// A class used for performing a series of method calls on an instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The class to be tested.</typeparam>
    /// <param name="functionNames">An ordered list of case-insensitive function names.</param>
    /// <param name="arguments">The arguments corresponding to the functions defined in <paramref name="functionNames"/>.</param>
    public class ObjectRunner<T>(string[] functionNames, object?[][] arguments) where T : class
    {
        /// <summary>
        /// The inner object under test. Returns <see langword="null"/> if <see cref="Run"/> hasn't been executed yet.
        /// </summary>
        public T? BaseObject { get; private set; }

        /// <summary>
        /// An ordered list of case-insensitive function names.
        /// </summary>
        public string[] FunctionNames { get; set; } = functionNames;

        /// <summary>
        /// The arguments corresponding to the functions defined in <see cref="FunctionNames"/>.
        /// </summary>
        public object?[][] Arguments { get; set; } = arguments;

        private readonly Dictionary<string, MethodInfo> _cachedMethods = [];

        /// <summary>
        /// An event raised on each method execution for diagnostic and performance profiling purposes.
        /// </summary>
        public event EventHandler<MethodExecutedEventArgs>? MethodExecuted;

        /// <summary>
        /// Creates an object of type <typeparamref name="T"/> and executes each function in <see cref="FunctionNames"/> in order with its corresponding set of arguments in <see cref="Arguments"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The lengths of <see cref="FunctionNames"/> and <see cref="Arguments"/> must be equal.</exception>
        /// <exception cref="ObjectConstructionException">There was an error in either finding an appropriate constructor for <typeparamref name="T"/> or if an error occurred in its execution.</exception>
        /// <exception cref="MissingMethodException">A method name was not found with a name found in <see cref="FunctionNames"/>.</exception>
        /// <exception cref="MethodInvocationException">An exception was thrown when attempting to execute a method call against <see cref="BaseObject"/>.</exception>
        public void Run()
        {
            if (FunctionNames.Length != Arguments.Length)
            {
                throw new InvalidOperationException($"Each entry of {nameof(FunctionNames)} must have a corresponding entry in {nameof(Arguments)}.");
            }

            var constructorArgTypes = new List<Type>();
            var constructorArgs = new List<object?>();

            foreach(var constructorArg in Arguments[0])
            {
                constructorArgTypes.Add(constructorArg!.GetType());
                constructorArgs.Add(constructorArg);
            }

            try
            {
                var constructor = (typeof(T).GetConstructor([.. constructorArgTypes]) ?? 
                    InferConstructor(constructorArgTypes, constructorArgs)) ?? 
                    throw new ObjectConstructionException($"Could not find constructor with matching parameter types.");

                BaseObject = (T)constructor.Invoke([.. constructorArgs]);
            }
            catch (Exception e) when (e is not ObjectConstructionException)
            {
                throw new ObjectConstructionException(e);
            }

            for (var i = 1; i < Arguments.Length; i++)
            {
                if (!_cachedMethods.TryGetValue(FunctionNames[i], out var methodInfo))
                {
                    // As far as I know, static methods and methods with generic types are never used
                    // Nor are overloaded methods.
                    // Can add support for it later if an instance of it is spotted.
                    methodInfo = typeof(T).GetMethod(FunctionNames[i], BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                    if(methodInfo == null)
                    {
                        throw new MissingMethodException(nameof(T), FunctionNames[i]);
                    }

                    _cachedMethods.Add(FunctionNames[i], methodInfo);
                }

                var stopWatch = new Stopwatch();
                try
                {
                    stopWatch.Restart();
                    var returnVal = methodInfo.Invoke(BaseObject, Arguments[i]);
                    stopWatch.Stop();

                    MethodExecuted?.Invoke(this, new MethodExecutedEventArgs(methodInfo.Name, Arguments[i], stopWatch.ElapsedMilliseconds, returnVal));
                }
                catch (Exception e) //Are there ever exceptions we don't want to catch?
                {
                    throw new MethodInvocationException(FunctionNames[i], i, Arguments[i], e);
                }
            }
        }

        private static ConstructorInfo? InferConstructor(List<Type> constructorArgTypes, List<object?> constructorArgs)
        {
            var constructors = typeof(T).GetConstructors();

            foreach (var c in constructors)
            {
                var parameters = c.GetParameters();
                if (parameters.Length == constructorArgTypes.Count)
                {
                    var newConstructorArgs = new List<object?>();
                    for(var i = 0; i < parameters.Length; i++)
                    {
                        if (constructorArgs[i] == null)
                        {
                            newConstructorArgs.Add(constructorArgs[i]);
                        }
                        else
                        {
                            try
                            {
                                // TOOD: This could be more elegant.  But Type.IsAssignableFrom only works for inheritance
                                // This will also handle instancecs where explicit casts exist.
                                newConstructorArgs.Add(Convert.ChangeType(constructorArgs[i], parameters[i].ParameterType));
                            }
                            catch { }
                        }
                    }

                    if(newConstructorArgs.Count == constructorArgs.Count)
                    {
                        constructorArgs.Clear();
                        constructorArgs.AddRange(newConstructorArgs);
                        return c;
                    }
                }
            }

            return null;
        }
    }
}
