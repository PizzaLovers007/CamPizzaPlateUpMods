﻿using System.Diagnostics;

namespace CamPizza.NoConveyorStutter
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
            Debug.Log($"[NoConveyorStutter] {message}");
        }
    }
}
