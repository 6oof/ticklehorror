using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AreaLightController : MonoBehaviour
{
    [SerializeField] private List<Light> lights;
    private void OnTriggerEnter(Collider other)
    {
        foreach (var light in lights)
        {
            light.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var light in lights)
        {
            light.enabled = false;
        }
    }
}
