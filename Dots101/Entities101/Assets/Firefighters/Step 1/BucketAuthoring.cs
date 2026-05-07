using Unity.Entities;
using UnityEngine;

namespace Firefighters {
    public class BucketAuthoring : MonoBehaviour {
        private class BucketAuthoringBaker : Baker<BucketAuthoring> {
            public override void Bake(BucketAuthoring authoring) {
            }
        }
    }

    public struct Bucket : IComponentData {
        public float Water; // 0 = empty, 1 = full
        public Entity CarryingBot;
        public bool IsCarried;
    }
}