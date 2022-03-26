using System.IO;
internal class Logs
{
    static MelonLoader.MelonLogger.Instance log = new MelonLoader.MelonLogger.Instance(Main.Name);
    internal static void Text(object text)
    {
        log.Msg(text);
    }
    internal static void Warning(object text)
    {
        log.Warning(text);
    }
    internal static void Error(object text)
    {
        log.Error(text);
    }
    internal static void Debug(object text)
    {

    }
}