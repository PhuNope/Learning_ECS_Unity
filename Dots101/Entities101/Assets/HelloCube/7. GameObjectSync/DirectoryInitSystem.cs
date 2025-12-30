using System;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace HelloCube.GameObjectSync {
#if !UNITY_DISABLE_MANAGED_COMPONENTS
    public partial struct DirectoryInitSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<ExecuteGameObjectSync>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            state.Enabled = false;

            var directoryObject = GameObject.Find("Directory");
            if (directoryObject == null) throw new Exception("GameObject 'Directory' not found.");

            var directory = directoryObject.GetComponent<Directory>();

            var directoryManaged = new DirectoryManaged();
            directoryManaged.RotatorPrefab = directory.RotatorPrefab;
            directoryManaged.RotationToggle = directory.RotationToggle;

            var entity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(entity, directoryManaged);
        }
    }

    public class DirectoryManaged : IComponentData {
        public GameObject RotatorPrefab;
        public Toggle RotationToggle;

        // Tất cả các IComponentData class phải có constructor không tham số
        public DirectoryManaged() {
        }
    }

#endif
}