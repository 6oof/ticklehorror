using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AreaLightController : MonoBehaviour
{
    [SerializeField] private List<Light> lights;
    [SerializeField] private List<Renderer> lightModels;

    private void Awake()
    {
        FindLights(transform);
        FindLightModels(lights);

        foreach (var lightModel in lightModels)
        {
            Material material = lightModel.material;
            material.SetColor("_EmissionColor", Color.black);
        }
    }

    private void FindLights(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Light lightComponent = child.GetComponent<Light>();
            if (lightComponent != null)
            {
                lights.Add(lightComponent);
            }

            FindLights(child);



        }
    }

    private void FindLightModels(List<Light> lights)
    {
        foreach (Light light in lights)
        {
            Transform parent = light.transform.parent;
            Renderer renderer = parent.GetComponent<Renderer>();

            lightModels.Add(renderer);

        }
    }

    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ticklable"))
        {
            if (other.gameObject.CompareTag("Ticklable"))
            {
                Debug.Log("ticklable entered");
                foreach (var light in lights)
                {
                    light.enabled = true;
                }

                foreach (var lightModel in lightModels)
                {
                    Material material = lightModel.material;
                    material.SetColor("_EmissionColor", Color.white);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ticklable"))
        {
            Debug.Log("ticklable exited");
            foreach (var light in lights)
            {
                light.enabled = false;
            }
            foreach (var lightModel in lightModels)
            {
                Material material = lightModel.material;
                material.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}
