namespace Infrastructure.Helper;

public static class PasswordGenerate
{
    public static string GeneratePassword(int length = 6)
    {
        const string upperChars = "ABCDE";
        const string numbers = "0123456789";
        const string specialChars = "!@#$%^&*_+-=";
        var random = new Random();
        var chars = new List<char>();
        chars.Add(upperChars[random.Next(upperChars.Length)]);
        chars.Add(numbers[random.Next(numbers.Length)]);
        chars.Add(specialChars[random.Next(specialChars.Length)]);
        for (int i = chars.Count; i < length; i++)
        {
            var allChars = upperChars + numbers + specialChars;
            chars.Add(allChars[random.Next(allChars.Length)]);
        }

        for (var i = chars.Count - 1; i >= 0; i--)
        {
            var swapIndex = random.Next(i + 1);
            (chars[i], chars[swapIndex]) = (chars[swapIndex], chars[i]);
        }
        return new string(chars.ToArray());
    }
}