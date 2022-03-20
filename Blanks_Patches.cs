using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

public class Blanks_Patches
{
    internal static List<Blanks_Patches> Created_Patches = new List<Blanks_Patches>();
    internal string id { get; set; }
    internal MethodBase basis { get; set; }
    internal MethodInfo PatchingMethod { get; set; }
    internal PatchType type { get; set; }
    internal enum PatchType
    {
        prefix = 0,
        postfix = 1,
        transpiler = 2,
        ilmanipulator = 3,
        finalizer = 4
    }
    internal Blanks_Patches(MethodBase basis, Action Patch, PatchType type, string id = null)
    {
        this.basis = basis;
        this.PatchingMethod = Patch.Method;
        this.type = type;
        this.id = Patch.Method.Name;
        if (id != null)
        {
            this.id = id;
        }
        Created_Patches.Add(this);
    }
    internal Blanks_Patches(MethodBase basis, string PatchingMethodName, PatchType type, Type PatchingClass, string id = null)
    {
        this.basis = basis;
        this.PatchingMethod = PatchingClass.GetMethod(PatchingMethodName);
        this.type = type;
        this.id = PatchingMethodName;
        if (id != null)
        {
            this.id = id;
        }
        Created_Patches.Add(this);
    }
    internal static async Task<Exception> UnPatchSingle(string ID)
    {
        foreach (Blanks_Patches a in Created_Patches)
        {
            if (a.id == ID)
            {
                return await UnPatchSingle(a);
            }
        }
        return new Exception($"Coudnt find a Patch which Matches to the following ID: {ID}");
    }
    internal static async Task<Exception> UnPatchSingle(Blanks_Patches Patch)
    {
        if (Patch.id == null) return new Exception("Patching ID is null!");
        if (Patch.basis == null) return new Exception("Patching Basis is null!");
        if (Patch.PatchingMethod == null) return new Exception("PatchingMethod is null!");
        try
        {
            new HarmonyLib.Harmony(Patch.id).Unpatch(Patch.basis, Patch.PatchingMethod);
        }
        catch (Exception e)
        {
            return e;
        }
        return new Exception("Patch type doesnt match to Any PatchType");
    }
    internal static async Task<Dictionary<Blanks_Patches, Exception>> UnPatchAll(Action<Blanks_Patches, Exception> Action_on_Patch_Fail = null, Action<Blanks_Patches, Exception> Action_on_Patch_Success = null)
    {
        var result = new Dictionary<Blanks_Patches, Exception>();
        foreach (Blanks_Patches Patch in Created_Patches)
        {
            var ex = await UnPatchSingle(Patch);
            result.Add(Patch, ex);
            if (ex == null)
            {
                if (Action_on_Patch_Success != null)
                {
                    try
                    {
                        Action_on_Patch_Success.Invoke(Patch, ex);
                    }
                    catch { }
                }
            }
            if (ex != null)
            {
                if (Action_on_Patch_Fail != null)
                {
                    try
                    {
                        Action_on_Patch_Fail.Invoke(Patch, ex);
                    }
                    catch { }
                }
            }
        }
        return result;
    }
    internal static async Task<Exception> PatchSingle(Blanks_Patches Patch)
    {
        if (Patch.id == null) return new Exception("Patching ID is null!");
        if (Patch.basis == null) return new Exception("Patching Basis is null!");
        if (Patch.PatchingMethod == null) return new Exception("PatchingMethod is null!");
        switch (Patch.type)
        {
            case PatchType.prefix:
                {
                    try
                    {
                        var result = new HarmonyLib.Harmony(Patch.id).Patch(Patch.basis, prefix: new HarmonyMethod(Patch.PatchingMethod));
                        if (result == null) return new Exception("Patch is Null!");
                        return null;
                    }
                    catch (Exception e)
                    {
                        return e;
                    }
                }
            case PatchType.postfix:
                {
                    try
                    {
                        var result = new HarmonyLib.Harmony(Patch.id).Patch(Patch.basis, postfix: new HarmonyMethod(Patch.PatchingMethod));
                        if (result == null) return new Exception("Patch is Null!");
                        return null;
                    }
                    catch (Exception e)
                    {
                        return e;
                    }
                }
            case PatchType.transpiler:
                {
                    try
                    {
                        var result = new HarmonyLib.Harmony(Patch.id).Patch(Patch.basis, transpiler: new HarmonyMethod(Patch.PatchingMethod));
                        if (result == null) return new Exception("Patch is Null!");
                        return null;
                    }
                    catch (Exception e)
                    {
                        return e;
                    }
                }
            case PatchType.finalizer:
                {
                    try
                    {
                        var result = new HarmonyLib.Harmony(Patch.id).Patch(Patch.basis, finalizer: new HarmonyMethod(Patch.PatchingMethod));
                        if (result == null) return new Exception("Patch is Null!");
                        return null;
                    }
                    catch (Exception e)
                    {
                        return e;
                    }
                }
            case PatchType.ilmanipulator:
                {
                    try
                    {
                        var result = new HarmonyLib.Harmony(Patch.id).Patch(Patch.basis, ilmanipulator: new HarmonyMethod(Patch.PatchingMethod));
                        if (result == null) return new Exception("Patch is Null!");
                        return null;
                    }
                    catch (Exception e)
                    {
                        return e;
                    }
                }
        }
        return new Exception("Patch type doesnt match to Any PatchType");
    }
    internal static async Task<Dictionary<Blanks_Patches, Exception>> PatchAll(Action<Blanks_Patches, Exception> Action_on_Patch_Fail = null, Action<Blanks_Patches, Exception> Action_on_Patch_Success = null)
    {
        var result = new Dictionary<Blanks_Patches, Exception>();
        foreach (Blanks_Patches Patch in Created_Patches)
        {
            var ex = await PatchSingle(Patch);
            result.Add(Patch, ex);
            if (ex == null)
            {
                if (Action_on_Patch_Success != null)
                {
                    try
                    {
                        Action_on_Patch_Success.Invoke(Patch, ex);
                    }
                    catch { }
                }
            }
            if (ex != null)
            {
                if (Action_on_Patch_Fail != null)
                {
                    try
                    {
                        Action_on_Patch_Fail.Invoke(Patch, ex);
                    }
                    catch { }
                }
            }
        }
        return result;
    }
}
