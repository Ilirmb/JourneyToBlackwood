using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;


/// <summary>
/// CostumeChange
/// This script handles all sprite mesh changes on the player
/// (Ex: costume, face, hair, etc.)
/// </summary>
public class CostumeChange : MonoBehaviour {

    // List of all meshes in the player's skeleton
    private List<SpriteMeshInstance> skeleton = new List<SpriteMeshInstance>();


    // Use this for initialization
    void Start () {

        CustomizationManager.instance.OnCostumeChanged.AddListener(CostumeChanged);

        // Find all sprite meshes attached to the player. This will avoid accessive GetComponent calls
        skeleton = PlayerMeshSkeleton.GetSkeleton();

        // Update the player's costume
        CostumeChanged();

    }


    /// <summary>
    /// Costume Changed
    /// Called whenever a costume is changed and updates the meshes based on the costume.
    /// </summary>
    private void CostumeChanged()
    {
        // Determine the components that need to be changed based on the costume.
        CostumeData costume = CustomizationManager.instance.GetCurrentCostume();

        // If costume is valid
        if (costume != null)
        {
            // Loop through all costume parts in the costume's list
            foreach(CostumePiece cp in costume.skinMeshes)
            {
                SpriteMeshInstance mesh = skeleton.Find(t => t.name == cp.GetSkinTarget());

                // If a mesh target is found, replace the sprite
                if (mesh)
                    mesh.spriteMesh = cp.GetSpriteMesh();
            }
        }
    }
}
