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

            // Infrastructure is initialized via ServiceContainer in Dashboard or here
            // ServiceContainer.Get<IDatabaseService>(); // This triggers initialization

            Application.Run(new AuthenticationForm());
        }
    }
}
