using System.Collections.Generic;
using HarmonyLib;
using Kitchen;
using UnityEngine;

namespace CamPizza.NoConveyorStutter.Patches
{
    [HarmonyPatch]
    public class AllowParallelGrabItemsPatch
    {
        private static readonly HashSet<Vector3> updatingPositionsSet = new HashSet<Vector3>();

        private static bool prevHasPerformedAction;

        [HarmonyPatch("Kitchen.GrabItems+<>c__DisplayClass_OnUpdate_LambdaJob0, KitchenMode", "OriginalLambdaBody")]
        [HarmonyPrefix]
        public static void OriginalLambdaBodyPrefix(
                ref bool ___has_performed_action, CPosition pos) {
            Vector3 grabPos = CalculateGrabPosition(pos);
            ___has_performed_action = updatingPositionsSet.Contains(grabPos);

            prevHasPerformedAction = ___has_performed_action;
        }

        [HarmonyPatch("Kitchen.GrabItems+<>c__DisplayClass_OnUpdate_LambdaJob0, KitchenMode", "OriginalLambdaBody")]
        [HarmonyPostfix]
        public static void OriginalLambdaBodyPostfix(
                bool ___has_performed_action, CPosition pos) {
            if (___has_performed_action != prevHasPerformedAction) {
                Vector3 grabPos = CalculateGrabPosition(pos);
                updatingPositionsSet.Add(grabPos);
            }
        }

        private static Vector3 CalculateGrabPosition(CPosition pos) {
            return (pos.Forward(-1f) + pos).Rounded();
        }
    }
}
