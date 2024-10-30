namespace MagikarpMayhem.Models
{
    public class NavButtonModel
    {
        public string Controller { get; set; } = "Home";
        public string Action { get; set; } = "Index";
        public string Method { get; set; } = "get";
        public string ButtonText { get; set; } = "[Button Text]";

        public string? Emoji { get; set; } = null;

        public string? BackgroundColor { get; set; } = null;
    }
}