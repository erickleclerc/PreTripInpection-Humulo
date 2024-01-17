using UnityEditor;
using UnityEngine;

public static class MeshReadabilityChecker
{
    [MenuItem("Custom/Check Mesh Readability")]
    private static void CheckMeshesReadability()
    {
        Mesh[] meshes = Resources.FindObjectsOfTypeAll<Mesh>();

        foreach (var mesh in meshes)
        {
            if (!mesh.isReadable)
            {
                Debug.LogWarningFormat(mesh, "Mesh '{0}' is not readable. Please enable Read/Write in the mesh import settings.", mesh.name);
            }
        }
    }
}