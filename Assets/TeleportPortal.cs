using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class TeleportPortal : MonoBehaviour
{
    [Header("=== Components' Meshes ==========")]
    [SerializeField] private MeshRenderer upperSkinnedMesh;
    [SerializeField] private MeshRenderer lowerSkinnedMesh;

    [Header("=== Player Skin Meshes ==========")]
    [SerializeField] private SkinnedMeshRenderer skinned_SurfaceMesh;
    [SerializeField] private SkinnedMeshRenderer skinned_JointMesh;

    // --- COMPONENTS' MATERIALS ----------
    private Material[] upperSkinnedMaterials;
    private Material[] lowerSkinnedMaterials;

    [Header("=== Materials' Properties ===========")]
    [SerializeField] private Vector2 activateEmission = new Vector2(5f, 5f);
    [SerializeField] private Vector2 deactivateEmission = new Vector2(0f, 0.1f);

    [Header("=== VFX Effect ==========")]
    [SerializeField] private GameObject splashEffect;
    [SerializeField] private GameObject circleEffect;
    [SerializeField] private VisualEffect VFXGraph_Dissolve;

    // --- CLASSES' DELEGATION ----------
    private DissolveController dissolveController;
    private Player player;

    private void OnEnable()
    {
        InitializeClasses();
        InitializeMaterials();
    }

    private void InitializeMaterials()
    {
        upperSkinnedMaterials = upperSkinnedMesh.materials;
        lowerSkinnedMaterials = lowerSkinnedMesh.materials;

        dissolveController.skinned_SurfaceMaterials = skinned_SurfaceMesh.materials;
        dissolveController.skinned_JointMaterials = skinned_JointMesh.materials;
    }

    private void InitializeClasses()
    {
        dissolveController = new DissolveController();
        player = Player.player;
    }

    private void Start()
    {
        dissolveController.VFXGraph = VFXGraph_Dissolve;

        StartRespawnProcess();
    }

    private void StartRespawnProcess()
    {
        Vector3 teleportPos = transform.position;
        Vector3 playerPos = player.transform.position;
        teleportPos.y = playerPos.y = 0f;

        if (playerPos == teleportPos)
        {
            StartCoroutine(RespawnProcess());
        }
    }

    private IEnumerator RespawnProcess()
    {
        GameObject circle = Instantiate(circleEffect, player.transform.position, circleEffect.transform.rotation);

        yield return DissolveProcess(false);

        GameObject splash = Instantiate(splashEffect, this.transform.position, splashEffect.transform.rotation);

        yield return new WaitUntil(() => splash.GetComponent<ParticleSystem>().isStopped);

        Destroy(splash); Destroy(circle);
    }

    private IEnumerator DissolveProcess(bool dissolve)
    {
        player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        yield return dissolveController.DissolveProcess(dissolve);

        player.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider == Player.player.Collider)
        {
            SetMaterialEmission(activateEmission);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider == Player.player.Collider)
        {
            SetMaterialEmission(deactivateEmission);
        }
    }

    private void SetMaterialEmission(Vector2 intensity)
    {
        var targetMaterials = new[] { upperSkinnedMaterials, lowerSkinnedMaterials };
        foreach (var materials in targetMaterials)
        {
            if (materials == null) continue;
            foreach (var mat in materials)
            {
                if (mat.HasProperty("_ActivateColor"))
                    mat.SetVector("_ActivateColor", intensity);
            }
        }
    }
}
