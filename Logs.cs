using System.IO;
using static ConfigManager;
internal class Logs
{
    static MelonLoader.MelonLogger.Instance log = new MelonLoader.MelonLogger.Instance(Main.Name);
    static StreamWriter Log = new StreamWriter(Path.Combine(ConfigManager.FilePath, "Log.txt"), true);
    static StreamWriter ErrorLog = new StreamWriter(Path.Combine(ConfigManager.FilePath, "ErrorLog.txt"), true);
    static StreamWriter WarningLog = new StreamWriter(Path.Combine(ConfigManager.FilePath, "WarningLog.txt"), true);
    static StreamWriter DebugLog = new StreamWriter(Path.Combine(ConfigManager.FilePath, "DebugLog.txt"), true);
    internal static void Text(object text)
    {
        log.Msg(text);
        Log.WriteLine(text.ToString());
    }
    internal static void Warning(object text)
    {
        log.Warning(text);
        Log.WriteLine(text.ToString());
        WarningLog.WriteLine(text.ToString());
    }
    internal static void Error(object text)
    {
        log.Error(text);
        Log.WriteLine(text.ToString());
        ErrorLog.WriteLine(text.ToString());
    }
    internal static void Debug(object text)
    {
        DebugLog.WriteLine(text.ToString());
        if (config.Debug)
        {
            log.Msg(text);
            Log.WriteLine(text.ToString());
        }
    }
}