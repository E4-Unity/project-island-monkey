namespace Assets._0_IslandMonkey.Scripts.Extension
{
    public static class StringUtilExtension
    {
        private static string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        public static string FormatLargeNumber(this int number)
        {
            if (number < 1000)
            {
                return number.ToString();
            }
            else
            {
                int index = 0;
                while (number >= 1000)
                {
                    number /= 1000;
                    index++;
                }

                if (index <= 26)
                {
                    return number.ToString() + alphabet[index - 1];
                }
                else
                {
                    int firstIndex = (index - 1) / 26;
                    int secondIndex = (index - 1) % 26;
                    return number.ToString() + alphabet[firstIndex - 1] + alphabet[secondIndex - 1];
                }
            }
        }
    }
}
