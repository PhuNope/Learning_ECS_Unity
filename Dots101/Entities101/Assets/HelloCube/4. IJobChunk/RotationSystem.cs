using Unity.Assertions;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace HelloCube.JobChunk {
    public partial struct RotationSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<ExecuteIJobChunk>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var spinningCubesQuery = SystemAPI.QueryBuilder().WithAll<RotationSpeed, LocalTransform>().Build();

            var job = new RotationJob() {
                TransformTypeHandle = SystemAPI.GetComponentTypeHandle<LocalTransform>(),
                RotationSpeedTypeHandle = SystemAPI.GetComponentTypeHandle<RotationSpeed>(),
                DeltaTime = SystemAPI.Time.DeltaTime
            };

            state.Dependency = job.Schedule(spinningCubesQuery, state.Dependency);
        }
    }

    [BurstCompile]
    struct RotationJob : IJobChunk {
        public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
        [ReadOnly] public ComponentTypeHandle<RotationSpeed> RotationSpeedTypeHandle;
        public float DeltaTime;

        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask) {
            // ussEnableMask laf true khi 1 hoặc nhiều entities trong chunk có component bị vô hiệu hoá.
            // Nếu ko có component nào triển khai IEnableableComponent, useEnableMask luôn là false.
            // Thêm bước này đề phòng trường hợp ai đó thay đổi kiểu truy vấn hoặc component types sau này.
            Assert.IsFalse(useEnabledMask);

            var transform = chunk.GetNativeArray(ref TransformTypeHandle);
            var rotationSpeed = chunk.GetNativeArray(ref RotationSpeedTypeHandle);
            for (int i = 0, chunkEntityCount = chunk.Count; i < chunkEntityCount; i++) {
                transform[i] = transform[i].RotateY(rotationSpeed[i].RadianPerSecond * DeltaTime);
            }
        }
    }
}