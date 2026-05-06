using Tutorials.Kickball.Step_2;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Tutorials.KickBall.Step_3 {
    public class BallAuthoring : MonoBehaviour {
        private class BallAuthoringBaker : Baker<BallAuthoring> {
            public override void Bake(BallAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                // A single authoring component can add multiple components to the entity
                AddComponent<Ball>(entity);
                AddComponent<Velocity>(entity);

                // Used in step 5
                AddComponent<Carry>(entity);
                SetComponentEnabled<Carry>(entity, false);
            }
        }
    }

    public struct Ball : IComponentData {
    }

    public struct Velocity : IComponentData {
        public float2 Value;
    }
}