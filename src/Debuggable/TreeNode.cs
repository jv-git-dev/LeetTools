using System.Text.Json;

namespace LeetTools.Debuggable
{
    /// <summary>
    /// A single-direction node in a binary tree.
    /// </summary>
    /// <typeparam name="T">The type to be used for <paramref name="val"/>.</typeparam>
    /// <param name="val">The value of this particular node.</param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public class TreeNode<T>(T val, TreeNode<T>? left = null, TreeNode<T>? right = null) where T : struct
    {
#pragma warning disable IDE1006 // Naming Styles.  This is what LeetCode use
        public T val { get; set; } = val;
        public TreeNode<T>? left { get; set; } = left;
        public TreeNode<T>? right { get; set; } = right;
#pragma warning restore IDE1006 // Naming Styles

        /// <summary>
        /// Generate a binary tree of type <typeparamref name="T"/> from the provided array.
        /// </summary>
        /// <param name="vals">The level-ordered array to turn into binary tree.</param>
        /// <returns>The root of the tree.</returns>
        public static TreeNode<T>? FromArray(T?[] vals)
        {
            if (vals.Length == 0)
            {
                return null;
            }

            return ParseArray(vals, 1, 0);
        }

        /// <summary>
        /// Generate a list of type <typeparamref name="T"/> where the current node is the root.
        /// </summary>
        /// <returns>A level-ordered list of values.</returns>
        public List<T?> ToList()
        {
            // In order to fill this with null values, we need T to either be a struct or a class so the compiler knows how to handle nullability
            // After a little looking, it seems like there isn't a simple one-line way to do this
            // Might need to make a second version of TreeNode where T can be a class?
            // Low priority because from what I've seen it's usually an int anyway
            // TODO: Future work.  
            var valList = new List<T?>();

            var currentIndex = 0;
            var lastValueIndex = 0;

            var treeQueue = new Queue<TreeNode<T>?>();
            treeQueue.Enqueue(this);

            while(treeQueue.TryDequeue(out var currentNode))
            {
                if(currentNode != null)
                {
                    treeQueue.Enqueue(currentNode.left);
                    treeQueue.Enqueue(currentNode.right);

                    lastValueIndex = currentIndex;
                }

                valList.Add(currentNode?.val);

                currentIndex++;
            }

            return valList[..(lastValueIndex + 1)];
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(ToList());
        }

        private static TreeNode<T>? ParseArray(T?[] vals, int depth, int currentIndex)
        {
            if(currentIndex >= vals.Length)
            {
                return null;
            }

            if (vals[currentIndex] is not T val)
            {
                return null;
            }

            var currentNode = new TreeNode<T>(val);
            var baseIndex = depth * currentIndex;

            currentNode.left = ParseArray(vals, depth + 1, baseIndex + 1);
            currentNode.right = ParseArray(vals, depth + 1, baseIndex + 2);

            return currentNode;
        }
    }
}
