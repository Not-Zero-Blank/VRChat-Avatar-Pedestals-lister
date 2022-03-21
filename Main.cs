using MelonLoader;
using QuickMenuLib;
using QuickMenuLib.UI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using static ConfigManager;

public class Main : MelonLoader.MelonMod
{
    public const string Version = "1.0.0.0";
    public const string Name = "Avatar-Pedestals-lister";
    public const string Author = "Blank";
    public override void OnApplicationStart()
    {
        AppDomain.CurrentDomain.UnhandledException += HandleException;
        QuickMenuLib.QuickMenuLibMod.RegisterModMenu(_instance);
    }
    static Menu _instance = new Menu();
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        Logs.Debug($"{buildIndex} {sceneName}");
        if (buildIndex == -1)
        {
            MelonCoroutines.Start(_instance.ScanForAP());
        }
    }
    public override void OnApplicationQuit()
    {
        Process.GetCurrentProcess().Kill();
    }

    private void HandleException(object sender, UnhandledExceptionEventArgs e)
    {
        Logs.Error(e.ExceptionObject.ToString());
    }
}
public class Menu : ModMenu
{
    public override string MenuName => Main.Name;
    public Menu()
    {
        Logo = null; // You can put a sprite here and QuickMenuLib will automatically add it.
    }
    internal QuickMenuCategory APFoundButtons;
    List<GameObject> Buttons = new List<GameObject>();
    public override void OnQuickMenuInitialized()
    {
        var utils = MyModMenu.AddMenuCategory("Utils");
        utils.AddButton("Scan", "Scan for Active Avatar Pedestals", delegate ()
        {
            MelonCoroutines.Start(ScanForAP());
        });
        utils.AddButton("Clear", "Clear List", delegate ()
        {
            Clear(true);
        });
        utils.AddButton("Clear\nHistory\nOnWorldChange", "Will Clear the list on world change if enabled", delegate ()
        {
            if (config.ClearListOnWorldChange)
            {
                config.ClearListOnWorldChange = false;
                Logs.Text("History Disabled");
            }
            else
            {
                config.ClearListOnWorldChange = true;
                Logs.Text("History Enabled");
            }
        });
        utils.AddButton("Debug\nMode", "Will print out more Informations in the Console", delegate ()
        {
            if (config.Debug)
            {
                config.Debug = false;
                Logs.Text("Debug Disabled");
            }
            else
            {
                config.Debug = true;
                Logs.Text("Debug Enabled");
            }
        });
        var category = MyModMenu.AddMenuCategory("Avatar Pedestals Found in Current World");
        APFoundButtons = category;
        MelonCoroutines.Start(ScanForAP());
    }
    public void Clear(bool force = false)
    {
        if (force || config.ClearListOnWorldChange)
        {
            foreach (GameObject a in Buttons)
            {
                if (a != null)
                {
                    GameObject.Destroy(a);
                }
            }
            Buttons.Clear();
            return;
        }
    }
    public IEnumerator ScanForAP()
    {
        if (APFoundButtons == null) yield return null;
        Clear();
        var AP = GameObject.FindObjectsOfType<AvatarPedestal>();
        int created = 0;
        foreach (var a in AP)
        {
            if (!a.isActiveAndEnabled) continue;
            if (a.field_Private_ApiAvatar_0 == null) continue;
            if (a.field_Private_ApiAvatar_0.id.StartsWith("avtr"))
            {
                MelonCoroutines.Start(CreateButton(a));
                created++;
            }
        }
        Logs.Text($"Found {AP.Count} Avatar Pedestals Active: {created} Inactive: {AP.Count -created} (AP will not be shown in QM)");
        yield return null;
    }
    public IEnumerator CreateButton(AvatarPedestal avatar)
    {
        Logs.Debug("Creating Button for " + avatar.field_Private_ApiAvatar_0.name);
        var button = APFoundButtons.AddButton("", "", () =>
        {
            avatar.SetAvatarUse(VRCPlayer.field_Internal_Static_VRCPlayer_0._player);
        });
        var iconImage = button.GameObject.GetComponentInChildren<Image>(false);
        Logs.Debug("Loading Picture from " + avatar.field_Private_ApiAvatar_0.thumbnailImageUrl);
        var sprite = GetSprite(avatar.field_Private_ApiAvatar_0.thumbnailImageUrl);
        iconImage.sprite = sprite;
        iconImage.overrideSprite = sprite;
        Buttons.Add(button.GameObject);
        Logs.Debug("Successfully Created Button for " + avatar.field_Private_ApiAvatar_0.name);
        yield return null;
    }
    Sprite GetSprite(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }
        Logs.Debug("Sending Request to " + url);
        var wc = new WebClient();
        wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.109 Safari/537.36 OPR/84.0.4316.43");
        byte[] array = wc.DownloadData(url);
        Logs.Debug($"Request Succesfully!");
        if (array == null || array.Length == 0)
        {
            Logs.Debug($"Array Null!");
            return null;
        }
        Texture2D texture2D = new Texture2D(512, 512);
        if (!Il2CppImageConversionManager.LoadImage(texture2D, array))
        {
            Logs.Debug($"LoadImage failed!");
            return null;
        }
        Sprite sprite = Sprite.CreateSprite(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f, 0U, 0, default(Vector4), false);
        sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        if (sprite == null)
        {
            Logs.Debug($"Sprite Null!");
            return null;
        }
        return sprite;
    }
}

