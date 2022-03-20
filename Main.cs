using MelonLoader;
using QuickMenuLib;
using QuickMenuLib.UI.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class Main : MelonLoader.MelonMod
{
    public const string Version = "1.0.0.0";
    public const string Name = "Avatar-Pedestals-lister";
    public const string Author = "Blank";
    public override void OnApplicationStart()
    {
        AppDomain.CurrentDomain.UnhandledException += HandleException;
        Console.WriteLine("Hello");
        QuickMenuLib.QuickMenuLibMod.RegisterModMenu(_instance);
        Console.WriteLine("Registered");
    }
    static Menu _instance = new Menu();
    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        Logs.Text($"{buildIndex} {sceneName}");
        if (buildIndex == -1)
        {
            _instance.ScanForAP();
        }
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
        var category = MyModMenu.AddMenuCategory("Avatar Pedestals Found in Current World");
        APFoundButtons = category;
        ScanForAP();
    }
    public void ScanForAP()
    {
        //foreach (GameObject a in Buttons) GameObject.Destroy(a);
        var AP = GameObject.FindObjectsOfType<AvatarPedestal>();
        foreach (var a in AP)
        {
            if (a.field_Private_ApiAvatar_0.id.StartsWith("avtr"))
            {
                MelonCoroutines.Start(CreateButton(a));
            }
        }
        Logs.Text($"Found {AP.Count} Avatar Pedestals");
    }
    public IEnumerator CreateButton(AvatarPedestal avatar)
    {
        if (APFoundButtons == null) return null;
        var button = APFoundButtons.AddButton("", "", () =>
        {
            avatar.SetAvatarUse(VRCPlayer.field_Internal_Static_VRCPlayer_0._player);
        });
        var iconImage = button.RectTransform.Find("Icon").GetComponent<Image>();
        var sprite = GetSprite(avatar.field_Private_ApiAvatar_0.thumbnailImageUrl);
        iconImage.sprite = sprite;
        iconImage.overrideSprite = sprite;
        //Buttons.Add(button.GameObject);
        return null;
    }
    Sprite GetSprite(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }
        byte[] array = new WebClient().DownloadData(url);
        if (array == null || array.Length == 0)
        {
            return null;
        }
        Texture2D texture2D = new Texture2D(512, 512);
        if (!Il2CppImageConversionManager.LoadImage(texture2D, array))
        {
            return null;
        }
        Sprite sprite = Sprite.CreateSprite(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f, 0U, 0, default(Vector4), false);
        sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        if (sprite == null)
        {
            return null;
        }
        return sprite;
    }
}

