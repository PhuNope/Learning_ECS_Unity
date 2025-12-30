using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace HelloCube.GameObjectSync {
#if !UNITY_DISABLE_MANAGED_COMPONENTS
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    partial struct RotatorInitSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<DirectoryManaged>();
            state.RequireForUpdate<ExecuteGameObjectSync>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var directory = SystemAPI.ManagedAPI.GetSingleton<DirectoryManaged>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (rotationSpeed, entity) in SystemAPI.Query<RefRO<RotationSpeed>>().WithNone<RotatorGO>().WithEntityAccess()) {
                var go = GameObject.Instantiate(directory.RotatorPrefab);

                ecb.AddComponent(entity, new RotatorGO(go));
            }

            ecb.Playback(state.EntityManager);
        }
    }

    public class RotatorGO : IComponentData {
        public GameObject Value;

        public RotatorGO(GameObject value) {
            Value = value;
        }

        // Every IComponentData class must have a no-arg constructor.
        public RotatorGO() {
        }
    }
#endif
}