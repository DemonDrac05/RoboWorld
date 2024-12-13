using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    public SkinnedMeshRenderer skinned_SurfaceMesh;
    public SkinnedMeshRenderer skinned_JointMesh;

    public Material[] skinned_SurfaceMaterials;
    public Material[] skinned_JointMaterials;

    public VisualEffect VFXGraph;
    public GameObject splashEffect;
    public GameObject circleEffect;

    private const float dissolveRate = 0.0125f;
    private const float refreshRate = 0.025f;

    private void Start()
    {
        skinned_SurfaceMaterials = skinned_SurfaceMesh?.materials;
        skinned_JointMaterials = skinned_JointMesh?.materials;

        StartCoroutine(HandleDissolve(false));
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(HandleDissolve(true));
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    StartCoroutine(HandleDissolve(false));
        //}
    }

    public IEnumerator HandleDissolve(bool dissolve)
    {
        if (!dissolve)
        {
            GameObject circle = Instantiate(circleEffect);

            yield return DissolveProcess(dissolve);

            GameObject splash = Instantiate(splashEffect);

            yield return new WaitUntil(() => splash.GetComponent<ParticleSystem>().isStopped);

            Destroy(splash); Destroy(circle);
        }
        else
        {
            yield return DissolveProcess(dissolve);
        }
    }

    IEnumerator DissolveProcess(bool dissolve)
    {
        if (VFXGraph != null)
        {
            VFXGraph.SetBool("Reverse", !dissolve);
            VFXGraph.Play();
        }

        Player.player.rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        var targetMaterials = new[] { skinned_SurfaceMaterials, skinned_JointMaterials };
        float counter = dissolve ? 0 : 1;
        float targetValue = dissolve ? 1 : 0;
        float step = dissolve ? dissolveRate : -dissolveRate;

        while ((dissolve && counter < targetValue) || (!dissolve && counter > targetValue))
        {
            counter += step;
            counter = Mathf.Clamp(counter, 0, 1);

            foreach (var materials in targetMaterials)
            {
                if (materials == null) continue;
                foreach (var mat in materials)
                {
                    if (mat.HasProperty("_DissolveAmount"))
                        mat.SetFloat("_DissolveAmount", counter);
                }
            }

            yield return new WaitForSeconds(refreshRate);
        }

        Player.player.rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
