using System.Configuration;
using System.Collections.Specialized;
using System.Globalization;

namespace UniversityProgram;

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

        SetLocalizationFromConfig();

        ApplicationConfiguration.Initialize();
        Application.Run(new FormMain());
    }

    private static void SetLocalizationFromConfig()
    {
        var culture = new CultureInfo(
                ConfigurationManager.AppSettings["localization"] ?? "en-EN");
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}