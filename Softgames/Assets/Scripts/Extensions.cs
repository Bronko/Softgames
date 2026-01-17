public static class Extensions
{
    /// <summary>
    /// In C# modulo breaks when looking at negative numbers.
    /// So we need something like this. 
    /// </summary>
    /// <returns>Mathematical correct modulo result</returns>
    public static int TrueModulo(this int a, int b)
    {
        return ((a % b) + b) % b;
    }
}
