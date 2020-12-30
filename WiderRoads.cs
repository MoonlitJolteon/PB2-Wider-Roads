using BepInEx;
using BepInEx.Configuration;
using Common.Extension;
using HarmonyLib;
using PolyTechFramework;
using System;
using System.Reflection;

namespace WiderRoads
{
    [BepInPlugin(pluginGuid, pluginName, pluginVerson)]
    [BepInProcess("Poly Bridge 2")]
    [BepInDependency(PolyTechMain.PluginGuid, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(ConfigurationManager.ConfigurationManager.GUID, BepInDependency.DependencyFlags.HardDependency)]
    public class WiderRoadsMain : PolyTechMod
    {
        public const string pluginGuid = "polytech.widerRoads";
        public const string pluginName = "Wider Roads";
        public const string pluginVerson = "1.0";


        public static ConfigDefinition modEnabledDef = new ConfigDefinition("PolyTech Framework", "Enabled");
        public static ConfigEntry<bool> modEnabled;

        public void Awake()
        {
            Config.Bind(modEnabledDef, true, new ConfigDescription("Enable Mod"));
            modEnabled = (ConfigEntry<bool>)Config[modEnabledDef];
            modEnabled.SettingChanged += onEnableDisable;

            var harmony = new Harmony(pluginGuid);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            this.isEnabled = true;
            this.isCheat = false;
            PolyTechMain.registerMod(this);
        }

        private void onEnableDisable(object sender, EventArgs e)
        {
            if (modEnabled.Value) enableMod();
            else disableMod();
            this.isEnabled = modEnabled.Value;
        }

        public override void enableMod()
        {
        }

        public override void disableMod()
        {
        }

        [HarmonyPatch(typeof(BridgeEdge))]
        [HarmonyPatch("_UpdateTransform")]
        static class patchRoads
        {
            static void Postfix(BridgeEdge __instance)
            {
                if(!modEnabled.Value) return;
                if (__instance.m_Material.m_MaterialType == BridgeMaterialType.ROAD || __instance.m_Material.m_MaterialType == BridgeMaterialType.REINFORCED_ROAD)
                {
                    __instance.m_MeshRenderer.transform.SetLocalScaleZ(1.15f);
                }
            }
        }
    }
}
