using System;

public static class LogExtension
{

    /// <summary>
    /// Colorize a message with a specific color
    /// </summary>
    /// <param name="message"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Color(this string message, FixedColor color = FixedColor.Yellow)
    {
        return new[] { message }.Color(color);
    }

    /// <summary>
    /// Colorize a message with multiple colors
    /// </summary>
    /// <param name="tuple"></param>
    /// <param name="colors"></param>
    /// <returns></returns>
    public static string Color(this (string, string) tuple, params FixedColor[] colors)
    {
        return new[] { tuple.Item1, tuple.Item2 }.Color(colors);
    }

    /// <summary>
    /// Colorize a message with multiple colors
    /// </summary>
    /// <param name="tuple"></param>
    /// <param name="colors"></param>
    /// <returns></returns>
    public static string Color(this (string, string, string) tuple, params FixedColor[] colors)
    {
        return new[] { tuple.Item1, tuple.Item2, tuple.Item3 }.Color(colors);
    }

    /// <summary>
    /// Colorize a message with multiple colors
    /// </summary>
    /// <param name="messages"></param>
    /// <param name="colors"></param>
    /// <returns></returns>
    public static string Color(this string[] messages, params FixedColor[] colors)
    {
        string formattedMessage = "";
        int min = Math.Min(messages.Length, colors.Length);
        for (int i = 0; i < min; i++)
        {
            string hexColor = colors[i].ToHex();
            formattedMessage += $"<color={hexColor}>| {messages[i]} | </color> ";
        }
        while (min < messages.Length)
        {
            string hexColor = FixedColor.Yellow.ToHex();
            formattedMessage += $"<color={hexColor}>| {messages[min]} | </color> ";
            min++;
        }

        return formattedMessage.Trim();
    }
}