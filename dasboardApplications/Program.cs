namespace dasboardApplications
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try
            {
                // Infrastructure is initialized via ServiceContainer in Dashboard or here
                // ServiceContainer.GetService<IDatabaseService>(); // This triggers initialization

                Application.Run(new AuthenticationForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during startup: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
