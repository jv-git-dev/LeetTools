namespace LeetTools.Debuggable.Test.TestClasses
{
    public class Functions
    {
        public int Number { get; private set; }

        public int IncrementInvocations { get; private set; }
        public int DecrementInvocations { get; private set; }
        public int SetInvocations { get; private set; }

        public void Increment()
        {
            Number++;
            IncrementInvocations++;
        }

        public void Decrement()
        {
            Number--;
            DecrementInvocations++;
        }

        public void Set(int value)
        {
            Number = value;
            SetInvocations++;
        }
    }
}
