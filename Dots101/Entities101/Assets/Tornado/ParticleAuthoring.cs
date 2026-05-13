using Unity.Entities;
using UnityEngine;

namespace Tornado {
    public class ParticleAuthoring : MonoBehaviour {
        private class ParticleAuthoringBaker : Baker<ParticleAuthoring> {
            public override void Bake(ParticleAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<Particle>(entity);
            }
        }
    }

    public struct Particle : IComponentData {
        public float radiusMult;
    }
}