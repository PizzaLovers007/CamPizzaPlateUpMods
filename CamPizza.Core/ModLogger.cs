using Kitchen;

namespace CamPizza.Core
{
    public static class ModLogger
    {
        public static void Log(string message) {
            Logger.Log(LogSource.Generic, $"[CamPizza] " + message);
        }
    }
}
