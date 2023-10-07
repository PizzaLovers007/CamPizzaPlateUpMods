using Kitchen;

namespace CamPizza.ShowUpgrade
{
    public static class ModLogger
    {
        public static void Log(string message) {
            Logger.Log(LogSource.Generic, $"[ShowUpgrade] " + message);
        }
    }
}
