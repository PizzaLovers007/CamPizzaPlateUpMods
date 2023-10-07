using System.Reflection;
using HarmonyLib;
using KitchenMods;

namespace CamPizza.ShowUpgrade
{
    public class Startup : IModInitializer
    {
        private readonly Harmony harmony = new Harmony("campizza.showupgrade");

        public void PostActivate(Mod mod) {
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void PostInject() { }

        public void PreInject() { }
    }
}
