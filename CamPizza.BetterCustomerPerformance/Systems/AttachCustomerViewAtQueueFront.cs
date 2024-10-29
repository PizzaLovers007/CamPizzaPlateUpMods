using CamPizza.BetterCustomerPerformance.Components;
using Kitchen;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace CamPizza.BetterCustomerPerformance.Systems
{
    [UpdateAfter(typeof(ManageQueue))]
    public class AttachCustomerViewAtQueueFront : DaySystem, IModSystem
    {
        private EntityQuery HiddenGroups;
        private int lastHidden = -1;

        protected override void Initialise() {
            base.Initialise();
            ModLogger.Log("Initialise");
            HiddenGroups =
                GetEntityQuery(
                    new QueryHelper()
                        .All(typeof(CCustomerGroup))
                        .None(typeof(CDidAttachCustomerView)));
        }

        protected override void OnUpdate() {
            using NativeArray<Entity> hiddenGroups = HiddenGroups.ToEntityArray(Allocator.Temp);

            if (hiddenGroups.Length > 0 && lastHidden != hiddenGroups.Length) {
                lastHidden = hiddenGroups.Length;
                ModLogger.Log($"Pending hidden groups... ({hiddenGroups.Length} to go)");
            }

            for (int i = 0; i < hiddenGroups.Length; i++) {
                var group = hiddenGroups[i];

                if (HasComponent<CQueuePosition>(group)
                        && GetComponent<CQueuePosition>(group).QueuePosition > 30) {
                    continue;
                }

                ModLogger.Log("Making group visible!");

                DynamicBuffer<CGroupMember> groupMembers =
                    EntityManager.GetBuffer<CGroupMember>(group);

                foreach (CGroupMember groupMember in groupMembers) {
                    Entity customer = groupMember.Customer;
                    CFutureCustomerRequiresView futureCustomerRequiresView =
                        EntityManager.GetComponentData<CFutureCustomerRequiresView>(customer);
                    ModLogger.Log($"Adding view: customer={customer} future={futureCustomerRequiresView}");
                    EntityManager.AddComponentData(customer, new CRequiresView {
                        Type = futureCustomerRequiresView.IsCat
                                ? ViewType.CustomerCat
                                : ViewType.Customer,
                        PhysicsDriven = true,
                    });
                    EntityManager.RemoveComponent(customer, typeof(CFutureCustomerRequiresView));
                }

                EntityManager.AddComponentData(group, new CDidAttachCustomerView());
            }
        }
    }
}
