using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCostume", menuName = "Customization/Costume", order = 1)]
public class CostumeData : ScriptableObject {

    // List of all meshes that should be affected by the skin shader.
    public List<string> skinMeshes;

}
