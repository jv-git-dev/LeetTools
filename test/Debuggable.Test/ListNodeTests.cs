using System.Diagnostics.CodeAnalysis;

namespace LeetTools.Debuggable.Test
{
    [ExcludeFromCodeCoverage]
    public class ListNodeTests
    {
        public static IEnumerable<object[]> FromArrayTestData =>
            [
                [new object[] { 0, 1, 2, 3, 4, 5, 6, 7 }],
                [new object[] { 'a', 'b', 'c', 'd' }],
                [new object[] { 1.0, 1.1, 1.2, 1.3 }],
                [new object?[] { null }]
            ];

        [Theory]
        [MemberData(nameof(FromArrayTestData))]
        public void ListNodeGeneratedFromArray(object[] testArray)
        {
            var head = ListNode<object>.FromArray(testArray);

            foreach(var nodeVal in testArray)
            {
                Assert.NotNull(head);
                Assert.Equal(nodeVal, head.val);
                head = head.next;
            }

            Assert.Null(head); // At this point there should be no more ListNodes
        }

        [Fact]
        public void EmptyArrayReturnsNullListNode()
        {
            var head = ListNode<int>.FromArray([]);

            Assert.Null(head);
        }

        [Fact]
        public void ListNodeToArray()
        {
            var head = new ListNode<int>(0, 
                new ListNode<int>(1, 
                new ListNode<int>(2, 
                new ListNode<int>(3))));


            Assert.Equal([0, 1, 2, 3], head.ToList());
            head = head!.next;

            Assert.Equal([1, 2, 3], head!.ToList());
            head = head!.next;

            Assert.Equal([2, 3], head!.ToList());
            head = head!.next;

            Assert.Equal([3], head!.ToList());
            head = head!.next;

            Assert.Null(head);
        }
    }
}
