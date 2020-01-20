using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RemoveComponent : MonoBehaviour
{
    
    void OnEnable() {

        var gos = GameObject.FindObjectsOfType<CookObject>();
        foreach (var go in gos) {
            Debug.Log(go.name);
            DestroyImmediate(go.GetComponent<CookObject>());
        }

    }

}
