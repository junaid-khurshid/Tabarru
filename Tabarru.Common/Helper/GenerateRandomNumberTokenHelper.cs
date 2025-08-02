namespace Tabarru.Common.Helper
{
    public static class GenerateRandomNumberTokenHelper
    {
        public static string GenerateRandomNumberToken(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than zero.");

            var random = new Random();
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = (char)('0' + random.Next(0, 10)); // Digits 0–9
            }

            return new string(result);
        }
    }
}
