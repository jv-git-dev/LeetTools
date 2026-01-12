namespace LeetTools.Debuggable.Test.TestClasses
{
    public class Constructors
    {
        public int Number { get; }
        public string? Text { get; }

        public Constructors() { }

        public Constructors(int number)
        {
            Number = number;
        }

        public Constructors(string text)
        {
            Text = text;
        }

        public Constructors(int number, string text)
        {
            Number = number;
            Text = text;
        }
    }
}
