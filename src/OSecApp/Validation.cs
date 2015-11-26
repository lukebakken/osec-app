namespace OSecApp
{
    public static class Validation
    {
        public static bool NonEmptyString(string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }
    }
}
