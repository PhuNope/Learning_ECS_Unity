using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace HelloCube.RandomSpawn {
    public partial struct SpawnSystem : ISystem {
        private uint m_SeedOffset;
        private float m_SpawnTimer;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<ExecuteRandomSpawn>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            const int count = 200;
            const float spawnWait = 0.05f;

            m_SpawnTimer -= SystemAPI.Time.DeltaTime;

            if (m_SpawnTimer > 0) return;

            m_SpawnTimer = spawnWait;

            var newSpawnQuery = SystemAPI.QueryBuilder().WithAll<NewSpawn>().Build();
            state.EntityManager.RemoveComponent<NewSpawn>(newSpawnQuery);

            var prefab = SystemAPI.GetSingleton<Config>().Prefab;
            state.EntityManager.Instantiate(prefab, count, Allocator.Temp);

            m_SeedOffset += count;

            new RandomPositionJob() { SeedOffset = m_SeedOffset }.ScheduleParallel();
        }
    }

    [WithAll(typeof(NewSpawn))]
    [BurstCompile]
    partial struct RandomPositionJob : IJobEntity {
        public uint SeedOffset;

        void Execute([EntityIndexInQuery] int index, ref LocalTransform transform) {
            var random = Random.CreateFromIndex(SeedOffset + (uint)index);
            var xz = random.NextFloat2Direction() * 50;
            transform.Position = new float3(xz[0], 50, xz[1]);
        }
    }
}