using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class SkinToneAdjust : MonoBehaviour {

    // Min and max ranges of color the material should target.
    [SerializeField]
    private float HSVRangeMin = 0.04f;
    [SerializeField]
    private float HSVRangeMax = 0.08f;

    // List of meshes that will be affected by the shader.
    private List<SpriteMeshInstance> skinBodyParts = new List<SpriteMeshInstance>(1);

    // An instance of the HSV material. Values will be changed at runtime to change skin color.
    private Material skinMaterial;

    // An instance of the default sprite material.
    private Material spriteMaterial;

    // List of all meshes in the player's skeleton
    private List<SpriteMeshInstance> skeleton = new List<SpriteMeshInstance>();

    // The player's head. The head will always be affected by skin color.
    private SpriteMeshInstance head;


    // Use this for initialization
    void Start () {

        CustomizationManager.instance.OnSkinChanged.AddListener(SetSkinSV);
        CustomizationManager.instance.OnCostumeChanged.AddListener(CostumeChanged);

        // Creates instances of the HSV and default sprite materials to be applied at runtime as needed.
        skinMaterial = new Material(Shader.Find("Custom/HSVRangeShader"));
        spriteMaterial = new Material(Shader.Find("Sprites/Default"));

        // Adjust HSV ranges
        skinMaterial.SetFloat("_HSVRangeMin", HSVRangeMin);
        skinMaterial.SetFloat("_HSVRangeMax", HSVRangeMax);

        // Find all sprite meshes attached to the player. This makes recoloring far easier.
        SpriteMeshInstance[] meshes = GetComponentsInChildren<SpriteMeshInstance>();

        foreach (SpriteMeshInstance smi in meshes)
            skeleton.Add(smi);

        // Gets a reference to the player's head
        head = skeleton.Find(h => h.name == "Head");
        skinBodyParts.Add(head);

        // Determine the components that need to be recolored based on the costume.
        CostumeData costume = CustomizationManager.instance.GetCurrentCostume();

        if(costume != null)
        {
            foreach (string s in costume.skinMeshes)
                AddSkinTarget(s);
        }

        // Applies skin color
        ApplySkinColorToTargets();

    }


    /// <summary>
    /// ApplySkinColorToTargets
    /// Applies the current selected skin color to all body parts that are to be affected
    /// </summary>
    private void ApplySkinColorToTargets()
    {
        foreach(SpriteMeshInstance smi in skinBodyParts)
            smi.sharedMaterial = skinMaterial;
    }


    /// <summary>
    /// AddSkinTarget
    /// Adds a new skin target with the given name.
    /// </summary>
    /// <param name="target">The name of the mesh to target.</param>
    private void AddSkinTarget(string target)
    {
        skinBodyParts.Add(skeleton.Find(t => t.name == target));
    }


    /// <summary>
    /// ResetSkinTargets
    /// Defaults all meshes to the standard sprite material.
    /// This should be called whenever a costume change occurs, sense some costume parts will not need to be affected by the shader.
    /// </summary>

    private void ResetSkinTargets()
    {
        foreach(SpriteMeshInstance smi in skinBodyParts)
            smi.sharedMaterial = spriteMaterial;

        skinBodyParts.Clear();

        // Head always needs to be affected
        skinBodyParts.Add(head);
    }


    /// <summary>
    /// SetSkinSV
    /// Sets the skin saturation and value
    /// </summary>
    private void SetSkinSV()
    {
        skinMaterial.SetVector("_HSVAAdjust", 
            new Vector4(0.0f, CustomizationManager.instance.GetSkinSat(), CustomizationManager.instance.GetSkinVal(), 0.0f));
    }


    /// <summary>
    /// Costume Changed
    /// Called whenever a costume is changed and updates the meshes to be impacted by the shader.
    /// </summary>
    private void CostumeChanged()
    {
        ResetSkinTargets();

        // Determine the components that need to be recolored based on the costume.
        CostumeData costume = CustomizationManager.instance.GetCurrentCostume();

        if (costume != null)
        {
            foreach (string s in costume.skinMeshes)
                AddSkinTarget(s);
        }

        ApplySkinColorToTargets();
    }
}
