# LeetTools
A helpful library for debugging your .NET LeetCode projects locally.
## ObjectRunner
The `ObjectRunner<T>` is useful for executing test cases that involve multiple method invocations on an object.

When creating an `ObjectRunner`, two sets of arrays need to be provided:
* `FunctionNames` - An ordered `string[]` of case-insensitive function names.  Note that `FunctonName[0]` should be the name of the class under test to represent object construction.
* `Arguments` - An ordered `object[][]` of arguments to be passed to the corresponding function name at the same index.

Then `Run()` can be used to construct the object and execute the specified functions.  After `Run()` is invoked once, you can use `ObjectRunner.BaseObject` to inspect the created object.

For example, take the following class:
```C#
public class TestClass
{
	public int Value { get; private set; }

	public TestClass(int initialValue)
	{
		Value = initialValue;
	}

	public void Increment()
	{
		Value++;
	}

	public void SetMax(int val1, int val2)
	{
		Value = Math.Max(val1, val2);
	}
}
```

If we wanted to run a series of calls on this object for debugging purposes we would have to do something like the following:
```C#
var test = new TestClass(1); // 1
test.Increment(); // 2
test.Increment(); // 3
test.SetMax(5, 10); // 10
test.Increment(); // 11
```
The more calls that need to be made, the more cumbersome this process can be.

Instead, we can use the `ObjectRunner` to directly copy and paste our test cases from LeetCode.  The above example would become the following:
```C#
var runner = new ObjectRunner<TestClass>(["testclass", "increment", "increment", "setmax", "increment"], [[1], [], [], [5, 10] []]);
runner.Run();
```
After calling `Run()`, the underlying object can be accessed with `runner.BaseObject`.

For diagnostic purposes, `ObjectRunner` also includes a `MethodExecuted` event which details how long each call took.
## Aliasing
This library also includes both `ListNode` and `TreeNode`, however they use the generic `T` to work with any type.  In contrast, LeetCode expects things to be type `ListNode` not `ListNode<T>`.  To avoid having to change the code between your IDE and your submission, you can use aliasing.

For example putting the following at the top of your code:
```C#
using ListNode = LeetTools.Debuggable.ListNode<int>
```
Allows you to use `ListNode` throughout your code instead of `ListNode<int>` and therefore satisfying the typing that LeetCode expects.