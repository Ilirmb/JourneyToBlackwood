using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum vectorVal
{
    x, y, z
}

enum colorVal
{
    r,b,g,a
}

[System.Serializable]
public class SaveData {

    public string name;

    public CustomizerState PlayerCustomization;

    public int sceneID;
    public float[] checkpoint;

    public int lastScene;
    public string[] sceneNames;
    public Hashtable[] statValues;
    public Hashtable[] socialValues;
    public Hashtable[] friendshipValues;

    public List<QuestData> questData;
    public string[][] questFlags;

}
