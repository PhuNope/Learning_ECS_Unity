using Unity.Entities;
using UnityEngine;

namespace Firefighters {
    public class PondAuthoring : MonoBehaviour {
        private class PondAuthoringBaker : Baker<PondAuthoring> {
            public override void Bake(PondAuthoring authoring) {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent<Pond>(entity);
            }
        }
    }

    public struct Pond : IComponentData {
    }
}