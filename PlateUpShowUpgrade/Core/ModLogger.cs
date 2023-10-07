using Kitchen;

namespace PlateUpShowUpgrade.Core
{
    public static class ModLogger
    {
        public static void Log(string message) {
            Logger.Log(LogSource.Generic, $"[PlateUpShowUpgrade] " + message);
        }
    }
}
