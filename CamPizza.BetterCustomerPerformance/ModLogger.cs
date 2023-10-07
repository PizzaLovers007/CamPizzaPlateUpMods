using Kitchen;

namespace CamPizza.BetterCustomerPerformance
{
    public static class ModLogger
    {
        public static void Log(string message) {
            Logger.Log(LogSource.Generic, $"[BetterCustomerPerformance] " + message);
        }
    }
}
