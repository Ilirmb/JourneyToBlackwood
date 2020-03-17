using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

    public float skinSaturation;
    public float skinValues;
    public int currentCostume;
    public int currentHair;
    public int currentFace;

    public Hashtable socialValues;
    public Hashtable friendshipValues;
    public List<QuestData> questData;

}
