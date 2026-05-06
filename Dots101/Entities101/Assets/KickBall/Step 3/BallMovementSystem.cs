using Tutorials.Kickball.Execute;
using Tutorials.Kickball.Step_1;
using Tutorials.Kickball.Step_2;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Tutorials.KickBall.Step_3 {
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct BallMovementSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BallMovement>();
            state.RequireForUpdate<Config>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var config = SystemAPI.GetSingleton<Config>();

            // The world or its SystemGroup may sometime override the current time or delta time, for various reasons, so you use SystemAPI.Time instead of UnityEngine.Time.
            var dt = SystemAPI.Time.DeltaTime;

            var decayFactor = config.BallVelocityDecay * dt;
            var minDist = config.ObstacleRadius + 0.5f; // the ball radius is 0.5f
            var minDistSQ = minDist * minDist;

            // For every ball entity, need to read and modify its LocalTransform and Velocity.
            foreach (var (ballTransform, velocity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Velocity>>()
                         .WithAll<Ball>()
                         .WithDisabled<Carry>()) // Relevant in Step 5
            {
                // Skip the ball if its isn't moving!
                if (velocity.ValueRO.Value.Equals(float2.zero)) continue;

                var magnitude = math.length(velocity.ValueRO.Value);
                var newPosition = ballTransform.ValueRW.Position + new float3(velocity.ValueRO.Value.x, 0, velocity.ValueRO.Value.y) * dt;

                // check if the ball intersects an object. If so, reflect the ball's velocity vector.
                foreach (var obstacleTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Obstacle>()) {
                    if (math.distancesq(newPosition, obstacleTransform.ValueRO.Position) <= minDistSQ) {
                        newPosition = DeflectBall(ballTransform.ValueRO.Position, obstacleTransform.ValueRO.Position, velocity, magnitude, dt);

                        // As long as the obstacles are spaced apart, it's impossible for one ball to hit tow obstacles in a single frame, so we can break after detecting collision with one obstacle.
                        break;
                    }
                }

                ballTransform.ValueRW.Position = newPosition;

                // Velocity decay
                var newMagnitude = math.max(magnitude - decayFactor, 0);
                velocity.ValueRW.Value = math.normalizesafe(velocity.ValueRO.Value) * newMagnitude;
            }
        }

        private float3 DeflectBall(float3 ballPos, float3 obstaclePos, RefRW<Velocity> velocity, float magnitude, float dt) {
            var obstacleBallVector = math.normalize((ballPos - obstaclePos).xz);
            velocity.ValueRW.Value = math.reflect(math.normalize(velocity.ValueRO.Value), obstacleBallVector) * magnitude;
            return ballPos + new float3(velocity.ValueRO.Value.x, 0, velocity.ValueRO.Value.y) * dt;
        }
    }
}