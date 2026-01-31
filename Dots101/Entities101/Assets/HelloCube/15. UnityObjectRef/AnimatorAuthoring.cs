using Unity.Entities;
using UnityEngine;

public class AnimatorAuthoring : MonoBehaviour {
    public GameObject AnimatorPrefab;

    class AnimatorBaker : Baker<AnimatorAuthoring> {
        public override void Bake(AnimatorAuthoring authoring) {
            var e = GetEntity(TransformUsageFlags.Renderable);
            AddComponent(e, new AnimatorRefConponent() { AnimatorAsGO = authoring.AnimatorPrefab });
        }
    }

    public struct AnimatorRefConponent : IComponentData {
        public UnityObjectRef<GameObject> AnimatorAsGO;
    }
}