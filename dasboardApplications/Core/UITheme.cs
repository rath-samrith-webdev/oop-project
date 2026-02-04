using System.Drawing;

namespace dasboardApplications.Core
{
    public static class UITheme
    {
        // Primary Colors
        public static Color PrimaryBackground = Color.FromArgb(28, 28, 30);
        public static Color SecondaryBackground = Color.FromArgb(44, 44, 46);
        public static Color AccentColor = Color.FromArgb(0, 122, 255); // San Francisco Blue
        public static Color HoverColor = Color.FromArgb(58, 58, 60);

        // Text Colors
        public static Color TextPrimary = Color.White;
        public static Color TextSecondary = Color.FromArgb(174, 174, 178);

        // Form Colors
        public static Color ContentBackground = Color.FromArgb(242, 242, 247);
        public static Color HeaderBackground = Color.FromArgb(32, 32, 32);

        // Fonts
        public static Font HeaderFont = new Font("Segoe UI", 14, FontStyle.Bold);
        public static Font ButtonFont = new Font("Segoe UI", 10, FontStyle.Regular);
        public static Font LabelFont = new Font("Segoe UI", 9, FontStyle.Regular);
    }
}
