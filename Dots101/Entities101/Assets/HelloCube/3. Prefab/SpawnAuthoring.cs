using Unity.Entities;
using UnityEngine;

namespace HelloCube.Prefabs {
    public class SpawnAuthoring : MonoBehaviour {
        public GameObject Prefab;

        class Baker : Baker<SpawnAuthoring> {
            public override void Bake(SpawnAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Spawner() {
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.None)
                });
            }
        }
    }

    struct Spawner : IComponentData {
        public Entity Prefab;
    }
}