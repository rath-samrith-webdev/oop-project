using System.Drawing;

namespace dasboardApplications.Core
{
    public static class UITheme
    {
        // Primary Colors
        public static Color PrimaryBackground = Color.FromArgb(15, 15, 18);    // Deep Midnight
        public static Color SecondaryBackground = Color.FromArgb(26, 26, 29); // Dark Slate
        public static Color AccentColor = Color.FromArgb(0, 122, 255);       // Electric Blue
        public static Color HoverColor = Color.FromArgb(0, 180, 255);        // Hyper Blue
        public static Color DangerColor = Color.FromArgb(255, 59, 48);       // Modern Red
        public static Color SuccessColor = Color.FromArgb(52, 199, 89);      // Modern Green

        // Text Colors
        public static Color TextPrimary = Color.FromArgb(255, 255, 255);   // Pure White
        public static Color TextSecondary = Color.FromArgb(142, 142, 147); // Muted Grey
        public static Color TextMuted = Color.FromArgb(72, 72, 74);        // Dark Muted

        // Form Colors
        public static Color ContentBackground = Color.FromArgb(10, 10, 12);  // Near Black
        public static Color CardBackground = Color.White;                    // Feature Cards
        public static Color HeaderBackground = Color.FromArgb(15, 15, 18);   // Matching Sidebar

        // Fonts
        public static Font HeaderFont = new Font("Segoe UI Semibold", 18, FontStyle.Bold);
        public static Font TitleFont = new Font("Segoe UI", 12, FontStyle.Bold);
        public static Font ButtonFont = new Font("Segoe UI", 10, FontStyle.Regular);
        public static Font BodyFont = new Font("Segoe UI", 9, FontStyle.Regular);
    }
}
