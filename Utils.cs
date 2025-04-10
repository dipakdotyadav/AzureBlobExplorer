public static class Utils
{
    public static string ToReadableSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }

    public static string ToReadableDate(DateTimeOffset? date)
    {
        return date.HasValue ? date.Value.ToLocalTime().ToString("f") : "-";
    }
}
