using LeetTools.Debuggable.Events;
using LeetTools.Debuggable.Exceptions;
using LeetTools.Debuggable.Test.TestClasses;

namespace LeetTools.Debuggable.Test
{
    public class ObjectRunnerTests
    {
        [Fact]
        public void DefaultConstructor()
        {
            var runner = new ObjectRunner<Constructors>(["constructors"], [[]]);
            runner.Run();

            Assert.NotNull(runner.BaseObject);
            Assert.Null(runner.BaseObject.Text);
            Assert.Equal(0, runner.BaseObject.Number);
        }

        [Fact]
        public void FindCorrectConstructor()
        {
            var runner = new ObjectRunner<Constructors>(["constructors"], [[1]]);
            runner.Run();

            Assert.NotNull(runner.BaseObject);
            Assert.Equal(1, runner.BaseObject.Number);
            Assert.Null(runner.BaseObject.Text);

            runner.Arguments = [["test"]];
            runner.Run();

            Assert.NotNull(runner.BaseObject);
            Assert.Equal(0, runner.BaseObject.Number);
            Assert.Equal("test", runner.BaseObject.Text);
        }

        [Fact]
        public void ConstructorParameterNumberMismatchError()
        {
            var runner = new ObjectRunner<Constructors>(["constructors"], [[1, 2, 3]]);
            Assert.Throws<ObjectConstructionException>(runner.Run);
        }

        [Fact]
        public void ConstructorInference()
        {
            var runner = new ObjectRunner<ConstructorInference>(["constructorinference"], [[1L]]);
            runner.Run();

            Assert.NotNull(runner.BaseObject);
            Assert.Equal(1, runner.BaseObject.Number);
        }

        [Fact]
        public void CannotInferConstructor()
        {
            var runner = new ObjectRunner<ConstructorInference>(["constructorinference"], [["a"]]);

            Assert.Throws<ObjectConstructionException>(runner.Run);
        }

        public static TheoryData<string[], object[][], int[]> FunctionsInvokedData => new()
        {
            { 
                ["functions", "increment", "increment", "decrement", "set", "increment"],
                [[], [], [], [], [1], []],
                [2, 3, 1, 1]
            },
            {
                ["functions", "decrement"],
                [[], []],
                [-1, 0, 1, 0]
            },
            {
                ["functions", "increment", "increment", "increment", "decrement", "decrement", "set", "decrement", "set"],
                [[], [], [], [], [], [], [10], [], [11]],
                [11, 3, 3, 2]
            }
        };

        [Theory]
        [MemberData(nameof(FunctionsInvokedData))]
        public void FunctionsInvoked(string[] functions, object[][] arguments, int[] expectedValues)
        {
            var runner = new ObjectRunner<Functions>(functions, arguments);

            runner.Run();

            var baseObject = runner.BaseObject;

            Assert.NotNull(baseObject);
            Assert.Equal(expectedValues[0], baseObject.Number);
            Assert.Equal(expectedValues[1], baseObject.IncrementInvocations);
            Assert.Equal(expectedValues[2], baseObject.DecrementInvocations);
            Assert.Equal(expectedValues[3], baseObject.SetInvocations);
        }

        [Fact]
        public void FunctionAndArgumentLengthMismatch()
        {
            var runner = new ObjectRunner<Functions>(["functions", "increment", "decrement"], [[], []]);

            Assert.Throws<InvalidOperationException>(runner.Run);
        }

        [Fact]
        public void FunctionNotFound()
        {
            var runner = new ObjectRunner<Functions>(["functions", "notReal"], [[], []]);

            Assert.Throws<MissingMethodException>(runner.Run);
        }

        [Fact]
        public void FunctionWrongArguments()
        {
            var runner = new ObjectRunner<Functions>(["functions", "increment"], [[], [1]]);

            Assert.Throws<MethodInvocationException>(runner.Run);
        }

        [Fact]
        public void MethodExecutedEventNoReturnValue()
        {
            var runner = new ObjectRunner<Functions>(["functions", "increment"], [[], []]);
            var eventRaised = Assert.Raises<MethodExecutedEventArgs>(
                handler => runner.MethodExecuted += handler,
                handler => runner.MethodExecuted -= handler,
                runner.Run);

            Assert.NotNull(eventRaised);
            Assert.Empty(eventRaised.Arguments.Arguments);
            Assert.Equal(nameof(Functions.Increment), eventRaised.Arguments.MethodName);
            Assert.Null(eventRaised.Arguments.ReturnValue);
        }

        public static TheoryData<string?> ReturnValueData = ["test", null];

        [Theory]
        [MemberData(nameof(ReturnValueData))]
        public void MethodExecutedEventWithReturnValue(string? returnVal)
        {
            var runner = new ObjectRunner<Functions>(["functions", "ReturnAString"], [[], [returnVal]]);
            var eventRaised = Assert.Raises<MethodExecutedEventArgs>(
                handler => runner.MethodExecuted += handler,
                handler => runner.MethodExecuted -= handler,
                runner.Run);

            Assert.NotNull(eventRaised);
            Assert.Equal(nameof(Functions.ReturnAString), eventRaised.Arguments.MethodName);
            Assert.Equal(returnVal, eventRaised.Arguments.Arguments[0]);
            Assert.Equal(returnVal, eventRaised.Arguments.ReturnValue);
        }
    }
}
