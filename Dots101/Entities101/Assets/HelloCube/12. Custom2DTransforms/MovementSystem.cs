using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace HelloCube.CustomTransforms {
    public partial struct MovementSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<LocalTransform2D>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            float rotation = SystemAPI.Time.DeltaTime * 180f; // Half a rotation every second (in degrees)
            float elapseTime = (float)SystemAPI.Time.ElapsedTime;
            float xPostion = math.sin(elapseTime) * 2f - 1f;
            float scale = math.sin(elapseTime * 2f) + 1f;

            foreach (var localTransform2D in SystemAPI.Query<RefRW<LocalTransform2D>>().WithNone<Parent>()) {
                localTransform2D.ValueRW.Position.x = xPostion;
                localTransform2D.ValueRW.Rotation = localTransform2D.ValueRO.Rotation + rotation;
                localTransform2D.ValueRW.Scale = scale;
            }
        }
    }
}