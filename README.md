# LeetTools

## Aliasing

In this library, both `ListNode` and `TreeNode` use the generic `T` to work with any type.  In contrast, LeetCode expects things to be type `ListNode` not `ListNode<T>`.  To avoid having to change the code between your IDE and your submission, you can use aliasing.

For example putting the following at the top of your code:
```C#
using ListNode = LeetTools.Debuggable.ListNode<int>
```
Allows you to use `ListNode` throughout your code instead of `ListNode<int>` and therefore satisfying the typing that LeetCode expects.