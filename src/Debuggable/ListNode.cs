using System.Text.Json;

namespace LeetTools.Debuggable
{
    /// <summary>
    /// A node in a linked list.
    /// </summary>
    /// <typeparam name="T">The type to be used for <paramref name="val"/>.</typeparam>
    /// <param name="val">The value of this particular node.</param>
    /// <param name="next">The next node in the list - if any.</param>
    public class ListNode<T>(T? val = default, ListNode<T>? next = null)
    {
#pragma warning disable IDE1006 // Naming Styles.  This is what LeetCode uses
        public T? val { get; set; } = val;
        public ListNode<T>? next { get; set; } = next;

#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Create a linked list of <see cref="ListNode{T}"/> while returning the head.
        /// </summary>
        /// <param name="vals">Series of values in the order the <see cref="ListNode{T}"/> should be linked.  The first value is the head.</param>
        /// <returns>The head of the linked list.</returns>
        public static ListNode<T>? FromArray(T[] vals)
        {
            if (vals.Length == 0)
            {
                return null;
            }

            var placeholderHead = new ListNode<T>();
            var tail = placeholderHead;

            foreach (var val in vals)
            {
                tail.next = new ListNode<T>(val);
                tail = tail.next;
            }

            return placeholderHead.next;
        }

        /// <summary>
        /// Returns a <see cref="List{T}"/> with the first element being the head.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> with the first element being the head.</returns>
        public List<T?> ToList()
        {
            var valList = new List<T?>();

            var currentNode = this;

            while (currentNode != null)
            {
                valList.Add(currentNode.val);
                currentNode = currentNode.next;
            }

            return valList;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(ToList());
        }
    }
}
