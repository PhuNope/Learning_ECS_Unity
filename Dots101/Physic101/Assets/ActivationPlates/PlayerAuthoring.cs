using Unity.Entities;
using UnityEngine;

namespace ActivationPlates {
    public class PlayerAuthoring : MonoBehaviour {
        private class PLayerAuthoringBaker : Baker<PlayerAuthoring> {
            public override void Bake(PlayerAuthoring authoring) {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

                AddComponent<Player>(entity);
            }
        }
    }

    public struct Player : IComponentData {
    }
}