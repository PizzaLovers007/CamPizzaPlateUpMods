using System.Reflection;
using HarmonyLib;
using KitchenMods;

namespace PlateUpShowUpgrade
{
    public class Startup : IModInitializer
    {
        private readonly Harmony harmony = new Harmony("plateupshowupgrade");

        public void PostActivate(Mod mod) {
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void PostInject() { }

        public void PreInject() { }
    }
}
