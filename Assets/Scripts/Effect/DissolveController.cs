using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    // --- PLAYER MESHES MATERIALS ----------
    public Material[] skinned_SurfaceMaterials;
    public Material[] skinned_JointMaterials;

    // --- DISSOLVE EFFECT ----------
    public VisualEffect VFXGraph;

    // --- SHADER GRAPH PROPERTIES -----------
    private const float dissolveRate = 0.0125f;
    private const float refreshRate = 0.025f;

    public IEnumerator DissolveProcess(bool dissolve)
    {
        if (VFXGraph != null)
        {
            VFXGraph.SetBool("Reverse", !dissolve);
            VFXGraph.Play();
        }

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
    }
}
