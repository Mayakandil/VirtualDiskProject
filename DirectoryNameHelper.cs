using System;
using System.Text;

public static class DirectoryNameHelper
{
    // bytes → "NAME.EXT"
    public static string Parse8Dot3Name(byte[] raw)
    {
        if (raw == null || raw.Length < 11)
            return string.Empty;

        string basePart = Encoding.ASCII.GetString(raw, 0, 8).TrimEnd(' ');
        string extPart  = Encoding.ASCII.GetString(raw, 8, 3).TrimEnd(' ');

        if (string.IsNullOrEmpty(basePart))
            return string.Empty;
        if (string.IsNullOrEmpty(extPart))
            return basePart;

        return basePart + "." + extPart;
    }

    // "hello.txt" → "HELLO   TXT" (11 chars)
    public static string FormatNameTo8Dot3(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new string(' ', 11);

        name = name.ToUpperInvariant();

        string basePart;
        string extPart = "";

        int dotIndex = name.IndexOf('.');
        if (dotIndex >= 0)
        {
            basePart = name.Substring(0, dotIndex);
            if (dotIndex + 1 < name.Length)
                extPart = name.Substring(dotIndex + 1);
        }
        else
        {
            basePart = name;
        }

        if (basePart.Length > 8)
            basePart = basePart.Substring(0, 8);
        if (extPart.Length > 3)
            extPart = extPart.Substring(0, 3);

        basePart = basePart.PadRight(8, ' ');
        extPart  = extPart.PadRight(3, ' ');

        return basePart + extPart;
    }
}
