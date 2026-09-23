using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Ham format khung tu dung. Game co Converter rieng va co the goi lai cac ham nay.
/// </summary>
public static class Formatter
{
    private static readonly string[] Units = { "", "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc", "Ud", "Dd", "Td", "Qad", "Qid", "Sxd", "Spd", "Ocd", "Nod", "Vg" };

    public static string FormatNumber(double value)
    {
        if (value < 1000)
            return value.ToString("0", CultureInfo.InvariantCulture);

        int unitIndex = 0;
        double displayValue = value;

        while (displayValue >= 1000 && unitIndex < Units.Length - 1)
        {
            displayValue /= 1000d;
            unitIndex++;
        }

        return displayValue.ToString("0.0", CultureInfo.InvariantCulture) + Units[unitIndex];
    }

    public static string FormatTime(int seconds)
    {
        int minutes = seconds / 60;
        int second = seconds % 60;

        return $"{minutes:00}:{second:00}";
    }

    public static string FormatName(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var result = Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
        result = Regex.Replace(result, @"[-_\s]+", " ").Trim();
        return Regex.Replace(result, @"\b[a-z]", m => m.Value.ToUpperInvariant());
    }

    public static string FormatColor(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(color);
    }
}
