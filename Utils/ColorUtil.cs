namespace MagikarpMayhem.Utils;

public static class ColorUtil
{
    public static List<string> GetRainbowColors(int count)
    {
        var colors = new List<string>();
        var hueStep = 360.0 / count;
        for (int i = 0; i < count; i++)
        {
            var hue = i * hueStep;
            var color = $"hsl({hue}, 80%, 70%)";
            colors.Add(color);
        }

        return colors;
    }
}