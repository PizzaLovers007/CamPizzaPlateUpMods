using System.Reflection;
using HarmonyLib;
using KitchenMods;

namespace CamPizza.BetterCustomerPerformance
{
    public class Startup : IModInitializer
    {
        private readonly Harmony harmony = new Harmony("campizza.bettercustomerperformance");

        public void PostActivate(Mod mod) {
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void PostInject() { }

        public void PreInject() { }
    }
}
