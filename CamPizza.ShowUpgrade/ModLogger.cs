using System.Diagnostics;
using Kitchen;

namespace CamPizza.ShowUpgrade
{
    public static class ModLogger
    {
        private static bool debugging;

        static ModLogger() {
            CheckForDebugging();
        }

        [Conditional("DEBUG")]
        public static void CheckForDebugging() {
            debugging = true;
        }

        public static void Log(string message) {
            if (!debugging) {
                return;
            }
            Logger.Log(LogSource.Generic, $"[ShowUpgrade] " + message);
        }
    }
}
