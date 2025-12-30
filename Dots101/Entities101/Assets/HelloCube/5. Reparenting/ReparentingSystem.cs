using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace HelloCube.Reparenting {
    public partial struct ReparentingSystem : ISystem {
        private bool m_Attached;
        private float m_Timer;
        private const float k_Interval = 0.7f;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            m_Timer = k_Interval;
            m_Attached = true;
            state.RequireForUpdate<ExecuteReparenting>();
            state.RequireForUpdate<RotationSpeed>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            m_Timer -= SystemAPI.Time.DeltaTime;
            if (m_Timer > 0) return;

            m_Timer = k_Interval;

            var rotatorEntity = SystemAPI.GetSingletonEntity<RotationSpeed>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            if (m_Attached) {
                DynamicBuffer<Child> children = SystemAPI.GetBuffer<Child>(rotatorEntity);
                for (int i = 0; i < children.Length; i++) {
                    ecb.RemoveComponent<Parent>(children[i].Value);
                }
            }
            else {
                foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithNone<RotationSpeed>().WithEntityAccess()) {
                    ecb.AddComponent(entity, new Parent() { Value = rotatorEntity });
                }
            }

            ecb.Playback(state.EntityManager);

            m_Attached = !m_Attached;
        }
    }
}