namespace TaskOperator.Logic.Helpers
{
    public static class StringHelper
    {
        public static string Quote(this string text)
        {
            return '"' + text + '"';
        }
    }
}
