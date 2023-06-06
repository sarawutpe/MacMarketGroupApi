using System;

public static class ColorsUtil
{
    private static Random random = new Random();

    public static string GenerateRandomHexColor()
    {
        byte[] buffer = new byte[3];
        random.NextBytes(buffer);

        string hexColor = "#" + BitConverter.ToString(buffer).Replace("-", string.Empty);
        return hexColor;
    }
}

