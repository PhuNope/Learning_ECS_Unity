using Unity.Burst;
using Unity.Entities;

namespace HelloCube.GameObjectSync {
#if !UNITY_DISABLE_MANAGED_COMPONENTS
    public partial struct RotationSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<ExecuteGameObjectSync>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
        }
    }

#endif
}