using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    // --- PLAYER MESHES MATERIALS ----------
    public Material[] _skinnedSurfaceMaterials;
    public Material[] _skinnedJointMaterials;

    // --- DISSOLVE EFFECT ----------
    public VisualEffect VFXGraph;

    // --- SHADER GRAPH PROPERTIES -----------
    private const float _dissolveRate = 0.0125f;
    private const float _refreshRate = 0.025f;

    public IEnumerator DissolveProcess(bool dissolve)
    {
        if (VFXGraph != null)
        {
            VFXGraph.SetBool("Reverse", !dissolve);
            VFXGraph.Play();
        }

        var targetMaterials = new[] { _skinnedSurfaceMaterials, _skinnedJointMaterials };
        float counter = dissolve ? 0 : 1;
        float targetValue = dissolve ? 1 : 0;
        float step = dissolve ? _dissolveRate : -_dissolveRate;

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

            yield return new WaitForSeconds(_refreshRate);
        }
    }
}
