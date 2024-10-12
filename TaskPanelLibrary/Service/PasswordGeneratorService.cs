using System.Text;

public class PasswordGeneratorService
{
    private const string UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const string SpecialCharacters = "@#$%^&*()-_=+[]{}|;:,.<>?";

    private static Random random = new Random();

    public string GeneratePassword(int length)
    {
        if (length < 4)
        {
            throw new ArgumentException("Password length less than 4 should throw an exception.");
        }
        
        StringBuilder password = new StringBuilder();
        password.Append(UpperCaseLetters[random.Next(UpperCaseLetters.Length)]);
        password.Append(LowerCaseLetters[random.Next(LowerCaseLetters.Length)]);
        password.Append(Numbers[random.Next(Numbers.Length)]);
        password.Append(SpecialCharacters[random.Next(SpecialCharacters.Length)]);
        string allCharacters = UpperCaseLetters + LowerCaseLetters + Numbers + SpecialCharacters;
        
        while (password.Length < length)
        {
            password.Append(allCharacters[random.Next(allCharacters.Length)]);
        }
        return new string(password.ToString().OrderBy(c => random.Next()).ToArray());
    }
}