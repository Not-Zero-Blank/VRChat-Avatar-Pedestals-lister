using MelonLoader;
using QuickMenuLib;
using System;

public class Main : MelonLoader.MelonMod
{
    public const string Version = "1.0.0.0";
    public const string Name = "Template";
    public const string Author = "Blank";
    public override void OnApplicationStart()
    {
        AppDomain.CurrentDomain.UnhandledException += HandleException;
        Console.WriteLine("Hello");
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
    public override void OnQuickMenuInitialized()
    {
        var category = MyModMenu.AddMenuCategory("TestModMenuCat");
        category.AddButton("Test", "This is a test using QuickMenuLib!", () =>
        {
            MelonLogger.Msg("Test Button!");
        });

        MyModMenu.AddSlider("FunnySlider", "Test", (num) => MelonLogger.Msg(num));
    }
    public override void OnWingMenuLeftInitialized()
    {
        MyLeftWingMenu.AddButton("Test", "Test using QuickMenuLib!", () =>
        {
            MelonLogger.Msg("Test Wing Button!");
        });
    }
    public override void OnWingMenuRightInitialized()
    {
        MyRightWingMenu.AddButton("Test", "Test using QuickMenuLib!", () =>
        {
            MelonLogger.Msg("Test Wing Button!");
        });
    }
    public override void OnTargetMenuInitialized()
    {
        MyTargetMenu.AddButton("Test", "Test using QuickMenuLib!", () =>
        {
            MelonLogger.Msg(SelectedUser.prop_String_0);
        });
    }
}

