using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace HelloCube.JobEntity {
    public partial struct RotationSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<ExecuteIJobEntity>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var job = new RotateAndScaleJob() {
                DeltaTime = SystemAPI.Time.DeltaTime,
                ElapsedTime = (float)SystemAPI.Time.ElapsedTime
            };
            job.Schedule();
        }
    }

    [BurstCompile]
    partial struct RotateAndScaleJob : IJobEntity {
        public float DeltaTime;
        public float ElapsedTime;

        void Execute(ref LocalTransform transform, ref PostTransformMatrix postTransformMatrix, in RotationSpeed speed) {
            transform = transform.RotateY(speed.RadianPerSecond * DeltaTime);
            postTransformMatrix.Value = float4x4.Scale(1, math.sin(ElapsedTime), 1);
        }
    }
}