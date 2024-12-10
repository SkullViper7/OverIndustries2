using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomLightInitializer : MonoBehaviour
{
    private Room _room;

    void Start()
    {
        _room = GetComponentInParent<Room>();
        InitProbes();
    }

    /// <summary>
    /// Starts the light probe runtime by resetting all probes, adding the ambient light and the contribution of all lights to all probes.
    /// </summary>
    /// <returns>The coroutine.</returns>
    void InitProbes()
    {
        List<Light> lights = GetComponentsInChildren<Light>().ToList();

        LightProbe[] lightProbes = LightProbeManager.Instance.GetProbesForThisRoom(_room).ToArray();

        // Add all lights' contribution to all probes
        for (int i = 0; i < lights.Count; i++)
        {
            if (lights[i].type == LightType.Directional)
            {
                // Add directional light contribution to all probes
                for (int j = 0; j < lightProbes.Length; j++)
                {
                    if (lightProbes[j].ProbePosition.z <= transform.position.z + GetComponent<MeshCollider>().bounds.size.z)
                    {
                        lightProbes[j].Probe.AddDirectionalLight(-lights[i].transform.forward, lights[i].color, lights[i].intensity);
                    }
                }
            }
            else if (lights[i].type == LightType.Point)
            {
                // Add point light contribution to all probes
                for (int j = 0; j < lightProbes.Length; j++)
                {
                    if (lightProbes[j].ProbePosition.z <= transform.position.z + GetComponent<MeshCollider>().bounds.size.z)
                    {
                        SHAddPointLight(lightProbes[j].ProbePosition, lights[i].transform.position, lights[i].range, lights[i].color, lights[i].intensity, ref lightProbes[j].Probe);
                    }
                }
            }
            else if (lights[i].type == LightType.Area)
            {
                // Add area light contribution to all probes
                for (int j = 0; j < lightProbes.Length; j++)
                {
                    if (lightProbes[j].ProbePosition.z <= transform.position.z + GetComponent<MeshCollider>().bounds.size.z)
                    {
                        SHAddAreaLight(lightProbes[j].ProbePosition, lights[i].transform.position, lights[i].GetComponent<BoxCollider>().size, lights[i].color, lights[i].intensity, ref lightProbes[j].Probe);
                    }
                }
            }
        }

        var globalProbes = LightmapSettings.lightProbes.bakedProbes;

        for (int i = 0; i < lightProbes.Length; i++)
        {
            int globalIndex = lightProbes[i].GlobalIndex; // Indice global
            globalProbes[globalIndex] = lightProbes[i].Probe; // Mise à jour de la probe globale
        }

        // Réinjecte le tableau global dans Unity
        LightmapSettings.lightProbes.bakedProbes = globalProbes;
    }

    /// <summary>
    /// Adds the light contribution of a point light to all probes.
    /// </summary>
    /// <param name="probePosition">The position of the probe.</param>
    /// <param name="position">The position of the light.</param>
    /// <param name="range">The range of the light.</param>
    /// <param name="color">The color of the light.</param>
    /// <param name="intensity">The intensity of the light.</param>
    /// <param name="sh">The SphericalHarmonicsL2 to modify.</param>
    void SHAddPointLight(Vector3 probePosition, Vector3 position, float range, Color color, float intensity, ref SphericalHarmonicsL2 sh)
    {
        // From the point of view of an SH probe, point light looks no different than a directional light,
        // just attenuated and coming from the right direction.
        Vector3 probeToLight = position - probePosition;
        float attenuation = 1.0F / (1.0F + 25.0F * probeToLight.sqrMagnitude / (range * range));
        sh.AddDirectionalLight(probeToLight.normalized, color, intensity * attenuation);
    }

    /// <summary>
    /// Adds the light contribution of an area light to all probes.
    /// </summary>
    /// <param name="probePosition">The position of the probe.</param>
    /// <param name="lightPosition">The position of the light.</param>
    /// <param name="size">The size of the light.</param>
    /// <param name="color">The color of the light.</param>
    /// <param name="intensity">The intensity of the light.</param>
    /// <param name="sh">The SphericalHarmonicsL2 to modify.</param>
    void SHAddAreaLight(Vector3 probePosition, Vector3 lightPosition, Vector3 size, Color color, float intensity, ref SphericalHarmonicsL2 sh)
    {
        // Sample the area light at a few points to get an approximation of the light that reaches the probe.
        Vector3[] sampleOffsets = {
            new Vector3(-0.5f, 0f, -0.5f),
            new Vector3(0.5f, 0f, -0.5f),
            new Vector3(-0.5f, 0f, 0.5f),
            new Vector3(0.5f, 0f, 0.5f)
        };

        for (int i = 0; i < sampleOffsets.Length; i++)
        {
            var offset = sampleOffsets[i];
            Vector3 realBoxSize = new Vector3(size.x, size.z, size.y);
            Vector3 samplePosition = lightPosition + Vector3.Scale(realBoxSize, offset);
            Vector3 lightToProbe = probePosition - samplePosition;

            Debug.Log("test");

            // If the probe can see the sample point, add the light contribution to the SH coefficients.
            if (!Physics.Raycast(samplePosition, lightToProbe.normalized, lightToProbe.magnitude))
            {
                Debug.DrawRay(samplePosition, lightToProbe.normalized * lightToProbe.magnitude, Color.red, 1000);

                // Attenuation
                float attenuation = 1.0F / Mathf.Max(1f, lightToProbe.sqrMagnitude * LightProbeManager.Instance.Attenuation);

                float contribution = intensity * attenuation / sampleOffsets.Length;

                // Add to SH coefficients
                sh.AddDirectionalLight(-lightToProbe.normalized, color, contribution);
            }
        }
    }
}
