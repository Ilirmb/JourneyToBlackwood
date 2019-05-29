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
        CustomizationManager.instance.OnHairStyleChanged.AddListener(HairStyleChanged);
        CustomizationManager.instance.OnFaceChanged.AddListener(FaceChanged);

        // Find all sprite meshes attached to the player. This will avoid accessive GetComponent calls
        skeleton = PlayerMeshSkeleton.GetSkeleton();

        // Update the player's costume
        CostumeChanged();
        HairStyleChanged();
        FaceChanged();
    }


    /// <summary>
    /// CostumeChanged
    /// Called whenever a costume is changed and updates the meshes based on the costume.
    /// </summary>
    private void CostumeChanged()
    {
        // Determine the components that need to be changed based on the costume.
        CostumeData costume = CustomizationManager.instance.GetCurrentCostume();

        // If costume is valid
        if (costume != null)
            UpdateSpriteMeshes(costume.skinMeshes);
    }


    /// <summary>
    /// HairStyleChanged
    /// Called whenever the hairstyle is changed and updates the meshes based on the hairstyle.
    /// </summary>
    private void HairStyleChanged()
    {
        // Determine the components that need to be changed based on the hair.
        CostumeData hair = CustomizationManager.instance.GetCurrentHairStyle();

        // If the hair is valid
        if (hair != null)
            UpdateSpriteMeshes(hair.skinMeshes);
    }


    /// <summary>
    /// FaceChanged
    /// Called whenever the face is changed and updates the meshes based on the face.
    /// </summary>
    private void FaceChanged()
    {
        // Determine the components that need to be changed based on the hair.
        CostumeData face = CustomizationManager.instance.GetCurrentFace();

        // If the hair is valid
        if (face != null)
            UpdateSpriteMeshes(face.skinMeshes);
    }


    /// <summary>
    /// UpdateSpriteMeshes
    /// Updates the sprite mesh of all skin targets in the given list.
    /// </summary>
    /// <param name="cpList">List of meshes to update</param>
    private void UpdateSpriteMeshes(List<CostumePiece> cpList)
    {
        // Loop through all costume parts in the costume's list
        foreach (CostumePiece cp in cpList)
        {
            Debug.Log(cp.GetSkinTarget());
            SpriteMeshInstance mesh = skeleton.Find(t => t.name == cp.GetSkinTarget());

            // If a mesh target is found, replace the sprite
            if (mesh)
                mesh.spriteMesh = cp.GetSpriteMesh();
        }
    }
}
