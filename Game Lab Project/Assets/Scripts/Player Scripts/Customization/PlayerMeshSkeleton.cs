using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

/// <summary>
/// PlayerMeshSkeleton
/// Creates and holds a reference to the player's "skeleton," or every sprite mesh instance that makes up their body.
/// </summary>
public class PlayerMeshSkeleton : MonoBehaviour {

    private static List<SpriteMeshInstance> skeleton = new List<SpriteMeshInstance>();

	// Use this for initialization
	void Awake () {

        SpriteMeshInstance[] meshes = GetComponentsInChildren<SpriteMeshInstance>();

        foreach (SpriteMeshInstance smi in meshes)
            skeleton.Add(smi);

    }


    /// <summary>
    /// GetSkeleton
    /// Returns the player's skeleton
    /// </summary>
    /// <returns>The player's skeleton</returns>
    public static List<SpriteMeshInstance> GetSkeleton()
    {
        return skeleton;
    }
}
