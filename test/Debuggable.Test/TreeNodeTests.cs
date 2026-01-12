namespace LeetTools.Debuggable.Test
{
    public class TreeNodeTests
    {
        [Fact]
        public void TreeNodeGeneratedFromList()
        {
            var list = new int?[] { 0, 1, 2, null, 3, 4, null };

            var root = TreeNode<int>.FromArray(list);
            var (l1, r1) = (root!.left, root.right);
            var (l1l, l1r, r1l, r1r) = (l1!.left, l1.right, r1!.left, r1.right);

            Assert.Equal(0, root.val);
            Assert.Equal(1, l1.val);
            Assert.Equal(2, r1.val);
            Assert.Null(l1l);
            Assert.Equal(3, l1r!.val);
            Assert.Equal(4, r1l!.val);
            Assert.Null(r1r);
        }

        [Fact]
        public void TreeNodeGeneratedFromImpossibleList()
        {
            // 3 and 4 are attached to a null parent and should therefore just be ignored
            var list = new int?[] { 0, 1, null, null, null, 3, 4 };

            var root = TreeNode<int>.FromArray(list);
            var (l1, r1) = (root!.left, root.right);

            Assert.Equal(0, root.val);
            Assert.Equal(1, l1!.val);
            Assert.Null(r1);
        }

        [Fact]
        public void EmptyArrayReturnsNullTreeNode()
        {
            var root = TreeNode<int>.FromArray([]);

            Assert.Null(root);
        }

        [Fact]
        public void TreeNodeToArray()
        {
            var root = new TreeNode<int>(0);
            var l1 = new TreeNode<int>(1);
            var r1 = new TreeNode<int>(2);

            var l1l = new TreeNode<int>(3);
            var l1r = new TreeNode<int>(4);
            var r1r = new TreeNode<int>(5);

            root.left = l1;
            root.right = r1;

            l1.left = l1l;
            l1.right = l1r;
            r1.right = r1r;

            var expected = new int?[] { 0, 1, 2, 3, 4, null, 5 };
            var actual = root.ToList();

            for(var i = 0; i < expected.Length; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }

        [Fact]
        public void TreeNodeToArrayTrimNullValues()
        {
            var root = new TreeNode<int>(0);
            var l1 = new TreeNode<int>(1);

            root.left = l1;

            var expected = new int?[] { 0, 1 };
            var actual = root.ToList();

            Assert.Equal(expected.Length, actual.Count);

            for (var i = 0; i < actual.Count; i++)
            {
                Assert.Equal(expected[i], actual[i]);
            }
        }
    }
}
