using System.Collections.Generic;
using HarmonyLib;
using Kitchen;
using Unity.Entities;
using UnityEngine;

namespace CamPizza.NoConveyorStutter.Patches
{
    [HarmonyPatch]
    public class AllowParallelPushItemsPatch
    {
        private static readonly HashSet<Vector3> updatingPositionsSet = new HashSet<Vector3>();

        private static PushItems? pushItemsInstance;
        private static bool prevHasPerformedAction;

        [HarmonyPatch(typeof(PushItems), "OnUpdate")]
        [HarmonyPrefix]
        public static void OnUpdatePrefix(PushItems __instance) {
            pushItemsInstance = __instance;
            updatingPositionsSet.Clear();
        }

        [HarmonyPatch("Kitchen.PushItems+<>c__DisplayClass_OnUpdate_LambdaJob0, KitchenMode", "OriginalLambdaBody")]
        [HarmonyPrefix]
        public static void OriginalLambdaBodyPrefix(
                ref bool ___has_performed_action,
                Entity e,
                CPosition pos,
                CConveyPushItems push) {
            Vector3 pushPos = CalculatePushingPosition(e, pos, push);
            ___has_performed_action = updatingPositionsSet.Contains(pushPos);

            prevHasPerformedAction = ___has_performed_action;
        }

        [HarmonyPatch("Kitchen.PushItems+<>c__DisplayClass_OnUpdate_LambdaJob0, KitchenMode", "OriginalLambdaBody")]
        [HarmonyPostfix]
        public static void OriginalLambdaBodyPostfix(
                bool ___has_performed_action,
                Entity e,
                CPosition pos,
                CConveyPushItems push) {
            if (___has_performed_action != prevHasPerformedAction) {
                Vector3 pushPos = CalculatePushingPosition(e, pos, push);
                updatingPositionsSet.Add(pushPos);
            }
        }

        [HarmonyPatch(typeof(PushItems), "OnUpdate")]
        [HarmonyPostfix]
        public static void OnUpdatePostfix() {
            if (updatingPositionsSet.Count > 0) {
                ModLogger.Log($"Push happened! {string.Join(", ", updatingPositionsSet)}");
            }
        }

        private static Vector3 CalculatePushingPosition(
                Entity e, CPosition pos, CConveyPushItems push) {
            if (pushItemsInstance == null) {
                return Vector3.zero;
            }

            Orientation orientation = Orientation.Up;
            if (pushItemsInstance.EntityManager.HasComponent<CConveyPushRotatable>(e)) {
                var rotateComponent =
                    pushItemsInstance.EntityManager.GetComponentData<CConveyPushRotatable>(e);
                if (rotateComponent.Target != Orientation.Null) {
                    orientation = rotateComponent.Target;
                }
            }
            Vector3 forward =
                pos.Rotation.RotateOrientation(orientation).ToOffset()
                * ((!push.Reversed) ? 1 : (-1));
            return (pos + forward).Rounded();
        }
    }
}
