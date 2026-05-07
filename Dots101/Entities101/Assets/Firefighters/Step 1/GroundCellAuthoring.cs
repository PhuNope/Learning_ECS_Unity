using Unity.Entities;
using UnityEngine;

namespace Firefighters {
    public class GroundCellAuthoring : MonoBehaviour {
        private class GroundCellAuthoringBaker : Baker<GroundCellAuthoring> {
            public override void Bake(GroundCellAuthoring authoring) {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent<GroundCell>(entity);
            }
        }
    }

    public struct GroundCell : IComponentData {
    }
}