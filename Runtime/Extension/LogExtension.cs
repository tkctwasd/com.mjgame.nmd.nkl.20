public static class LogExtension
{
    private static readonly string[] DefaultColors = { "yellow", "cyan", "red", "orange", "white", "green", "purple", "pink" };
    /// <summary>
    /// Colorize a message with a specific color
    /// </summary>
    /// <param name="message"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string Color(this string message, string color = "yellow")
    {
        return new[] { message }.Color(color);
    }

    /// <summary>
    /// Colorize a message with multiple colors
    /// </summary>
    /// <param name="string"></param>
    /// <param name="tuple"></param>
    /// <param name="colors"></param>
    /// <returns></returns>
    public static string Color(this (string, string) tuple, params string[] colors)
    {
        return new[] { tuple.Item1, tuple.Item2 }.Color(colors);
    }

    /// <summary>
    /// Colorize a message with multiple colors
    /// </summary>
    /// <param name="string"></param>
    /// <param name="tuple"></param>
    /// <param name="colors"></param>
    /// <returns></returns>
    public static string Color(this (string, string, string) tuple, params string[] colors)
    {
        return new[] { tuple.Item1, tuple.Item2, tuple.Item3 }.Color(colors);
    }

    /// <summary>
    /// Colorize a message with multiple colors
    /// </summary>
    /// <param name="messages"></param>
    /// <param name="colors"></param>
    /// <returns></returns>
    public static string Color(this string[] messages, params string[] colors)
    {
        if (colors.Length == 0)
        {
            colors = DefaultColors;
        }

        string formattedMessage = "";
        for (int i = 0; i < messages.Length; i++)
        {
            string color = colors[i % colors.Length];
            formattedMessage += $"<color={color}>| {messages[i]} | </color> ";
        }

        return formattedMessage.Trim();
    }
}
