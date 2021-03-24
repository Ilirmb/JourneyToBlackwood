using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.Experimental.VFX;

/// <summary>
/// Loads up the profile prefabs for each .sav file found in the save directory
/// <para>This script requires the prefab SaveFileContent to be ordered such that the first Image component found is the Profile Image and the first text the profile name
/// <para>Unity checks from the top down if I recall so the profile image in the prefab MUST be the first in the heirarcy view. This is somewhat bad design but without cluttering up Tags or doing expensive GameObject.Find() calls it's efficient so long as the prefab structure is maintained</para></para>
/// </summary>

public class LoadFileListManager : MonoBehaviour
{
    public PlayerStatistics playermodel;
    public GameObject SavePrefab;
    public PhotoMaker profilecamera;

    List<GameObject> FileObjects = new List<GameObject>();

    /// <summary>
    /// Because of silly silly loading restrictions the function waits until the end of the current frame before taking the profile picture
    /// <para>this means it takes a frame for each individual profile picture to load, which means at 30 fps it loads a mere 30 profile pictures a second. This is disappointing but acceptable.</para>
    /// </summary>
    private void Start()
    {
        FileStream[] filesInDirectory = GameManager.instance.LoadSaveFiles();
        Dictionary<CustomizerState, Image> profilestateimage = new Dictionary<CustomizerState, Image>();

        foreach (FileStream stream in filesInDirectory)
        {
            Debug.Log("Creating Loadmenu Object for file " + stream.Name);

            BinaryFormatter bf = new BinaryFormatter();
            SaveData saveData = bf.Deserialize(stream) as SaveData;

            CustomizerState state = saveData.PlayerCustomization;
            Debug.Log("Attempting to update player colors");
            playermodel.UpdateColors();

            GameObject newData = Instantiate(SavePrefab);
            newData.GetComponentInChildren<Text>().text = saveData.name;
            //Yeah this is the only way I could find to get the name of a scene

            string scenepath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(saveData.sceneID);
            int lastslash = scenepath.LastIndexOf('/');
            string scenename = scenepath.Substring(lastslash + 1, scenepath.LastIndexOf(".") - lastslash - 1);
            string name = saveData.name;

            newData.GetComponentInChildren<Text>().text = name + '\n' +
                scenename + '\n' +
                "Checkpoint X = " + saveData.checkpoint[(int)vectorVal.x] + '\n' +
                "Date: " + saveData.statValues[saveData.lastScene]["savetime"];

            FileObjects.Add(newData);
            newData.transform.SetParent(this.transform);
            Image profileImage = newData.transform.GetChild(0).GetComponent<Image>();
            profilestateimage.Add(state, profileImage);

            stream.Close();

            Button loadbutton = newData.GetComponentInChildren<Button>();
            loadbutton.onClick.AddListener(delegate { GameManager.instance.LoadProgress(name); } ) ;

            Debug.Log("Loadmenu Object created for " + stream.Name);
        }

        //Apply each customization manager state to the manager and player and then take a photograph for each
        //Techinically doesn't do it right away, but assigns each coroutine a frame to operate on
        int currentframe = Time.frameCount;
        foreach(CustomizerState state in profilestateimage.Keys)
        {
            Image image = profilestateimage[state];
            //Give 2 frames to load image before taking picture
            StartCoroutine(TakePhotoOnFrame(currentframe*2,image,state));
            currentframe++;
        }
    }

    private IEnumerator TakePhotoOnFrame(int frame, Image image, CustomizerState state)
    {
        yield return new WaitUntil(() => Time.frameCount >= frame);
        CustomizationManager.instance.SetState(state);
        yield return new WaitForEndOfFrame();
        image.sprite = profilecamera.TakePhotograph((int)image.rectTransform.rect.width, (int)image.rectTransform.rect.height);
    }
}
