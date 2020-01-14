using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCam : MonoBehaviour {
    public Material materialDefault;

    void Start() {
        // combine mesh
        GameObject combinedGo = CreateCombinedMesh();
    }

    // combine all mesh in scene
    GameObject CreateCombinedMesh() {
        // fill combine Instances
        MeshFilter[] meshFilters = FindObjectsOfType<MeshFilter>();
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];

        int iCombine = 0;
        for (int i = 0; i <meshFilters.Length; i++) 
        {
            if (meshFilters[i].sharedMesh == null) continue;
            combineInstances[iCombine].mesh = meshFilters[i].sharedMesh;
            combineInstances[iCombine++].transform = meshFilters[i].transform.localToWorldMatrix;
        }
        
        var go = new GameObject("CombinedMesh");
        var filter = go.AddComponent<MeshFilter>();
        var renderer = go.AddComponent<MeshRenderer>();
        go.layer = 12; // ModelCam Culling layer
        filter.sharedMesh = new Mesh();
        filter.sharedMesh.CombineMeshes(combineInstances);
        renderer.material = materialDefault;
        return go;
    }
}
