using Newtonsoft.Json;
using System;
using System.IO;

internal static class ConfigManager
{
    #region Load
    static bool ConfigLoaded = LoadConfig();
    internal static string FilePath { get; set; }
    public static Config config { get; set; }
    static bool LoadConfig()
    {

        FilePath = Directory.GetCurrentDirectory() + "/BlanksMods/" + Main.Name + "/";
        if (!Directory.Exists(FilePath))
        {
            Directory.CreateDirectory(FilePath);
        }
        if (File.Exists(FilePath + "/config.cfg"))
        {
            try
            {
                config = GetConfig();
            }
            catch (Exception e)
            {
                config = new Config();
                Save();
            }
        }
        else
        {
            config = new Config();
            Save();
        }
        System.Timers.Timer Loop = new System.Timers.Timer(500) { Enabled = true, AutoReset = true, };
        Loop.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string currentconfig = JsonConvert.SerializeObject(config);
                string FileConfig = GetConfigRaw();
                if (currentconfig != FileConfig)
                {
                    Logs.Debug($"Current:\n{currentconfig}\nFile:\n{FileConfig}");
                    Save();
                }
            }
            catch
            { }
        };
        Loop.Start();
        return true;
    }
    static void Save()
    {
        File.WriteAllText(FilePath + "/config.cfg", JsonConvert.SerializeObject(config));
    }
    static Config GetConfig()
    {
        try
        {
            return JsonConvert.DeserializeObject<Config>(GetConfigRaw());
        }
        catch (Exception e)
        {
            throw new Exception(e.ToString());
        }
    }
    static string GetConfigRaw() => File.ReadAllText(FilePath + "/config.cfg");
    #endregion
}
/// <summary>
/// To Access the config do: using static ConfigManger;
/// after that you can Access as Example the Debug bool like following
/// config.Debug
/// for a better Example look in Logs.cs
/// the Config get saved auto on change
/// </summary>
public class Config
{
    public bool Debug;
}