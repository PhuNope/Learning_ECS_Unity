using Unity.Entities;
using UnityEngine;

public partial struct SpawnAnimatedCubeSystem : ISystem {
    private EntityQuery m_AnimatorRefComponentQuery;

    public void OnCreate(ref SystemState state) {
        m_AnimatorRefComponentQuery = SystemAPI.QueryBuilder()
            .WithAll<AnimatorAuthoring.AnimatorRefConponent>()
            .WithNone<Animator>()
            .Build();

        state.RequireForUpdate(m_AnimatorRefComponentQuery);
    }

    public void OnUpdate(ref SystemState state) {
        var entities = SystemAPI.QueryBuilder()
            .WithAll<AnimatorAuthoring.AnimatorRefConponent>()
            .WithNone<Animator>()
            .Build()
            .ToEntityArray(state.WorldUpdateAllocator);

        foreach (var entity in entities) {
            // Get the animator reference
            var animRef = SystemAPI.GetComponent<AnimatorAuthoring.AnimatorRefConponent>(entity);

            // Instantiate the GO
            var rotatingCube = (GameObject)Object.Instantiate(animRef.AnimatorAsGO);

            // Add the animator to the entity
            state.EntityManager.AddComponentObject(entity, rotatingCube.GetComponent<Animator>());
        }
    }
}