﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;


[CreateAssetMenu(fileName = "NewCostume", menuName = "Customization/Costume", order = 1)]
public class CostumeData : ScriptableObject {

    public enum CostumeType { outfit, hair, face };

    public CostumeType costumeType;

    // List of all meshes in this costume.
    public List<CostumePiece> skinMeshes;


    /// <summary>
    /// GetSkinTargets
    /// Gets all the meshes that should be affected by the skin shader and returns their names.
    /// </summary>
    /// <returns>A list of all mesh names</returns>
    public List<string> GetSkinTargets()
    {
        List<string> skinTargets = new List<string>(skinMeshes.Count);

        foreach(CostumePiece cp in skinMeshes)
        {
            if (cp.GetIsSkin())
                skinTargets.Add(cp.GetSkinTarget());
        }

        return skinTargets;
    }


    public bool IsSelectable(CustomizationManager cm)
    {
        switch (costumeType)
        {
            case CostumeType.outfit:
                return cm.IsCostumeIncluded(this);

            case CostumeType.hair:
                return cm.IsHairStyleIncluded(this);

            case CostumeType.face:
                return cm.IsFaceIncluded(this);
        }

        return false;
    }


    public void MakeSelectable(CustomizationManager cm)
    {
        switch (costumeType)
        {
            case CostumeType.outfit:
                cm.AddCostume(this);
                break;

            case CostumeType.hair:
                cm.AddHairStyle(this);
                break;

            case CostumeType.face:
                cm.AddFace(this);
                break;
        }
    }

    
    public void MakeUnselectable(CustomizationManager cm)
    {
        switch (costumeType)
        {
            case CostumeType.outfit:
                cm.RemoveCostume(this);
                break;

            case CostumeType.hair:
                cm.RemoveHairStyle(this);
                break;

            case CostumeType.face:
                cm.RemoveFace(this);
                break;
        }
    }
}


/// <summary>
/// CostumePiece
/// Holds the data for a part of a costume (sprite mesh, which mesh it overrides, etc.)
/// </summary>
[System.Serializable]
public class CostumePiece
{
    [SerializeField]
    private SpriteMesh mesh;

    [SerializeField]
    private string skinTarget;

    // Indicates if skin shader should be applied.
    [SerializeField]
    private bool isSkin;


    /// <summary>
    /// GetSpriteMesh
    /// Gets the mesh for this costume piece
    /// </summary>
    /// <returns>The mesh for this costume piece</returns>
    public SpriteMesh GetSpriteMesh()
    {
        return mesh;
    }


    /// <summary>
    /// GetSkinTarget
    /// Gets the skin target for this costume piece
    /// </summary>
    /// <returns>The skin target for this costume piece</returns>
    public string GetSkinTarget()
    {
        return skinTarget;
    }


    /// <summary>
    /// GetIsSkin
    /// Gets whether or not this piece should be affected by the skin shader.
    /// </summary>
    /// <returns>Whether or not this piece should be affected</returns>
    public bool GetIsSkin()
    {
        return isSkin;
    }
}