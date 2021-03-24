using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatTabManager : MonoBehaviour
{
    public GameObject TabPrefab;
    public Transform TabTargetPosition;
    public Transform ContentTargetPosition;
    [Header("")]
    public Image profileImage;
    public Text profileText;
    public PhotoMaker photomaker;
    public ToggleGroup group;

    private List<StatTabContentHandler> contentHandlers = new List<StatTabContentHandler>();

    // Start is called before the first frame update
    void Start()
    {
        GameManager manager = GameManager.instance;

        foreach (string scenename in manager.Stats.Keys)
        {
            StatTabContentHandler handler = Instantiate(TabPrefab).GetComponent<StatTabContentHandler>();
            contentHandlers.Add(handler);
            //The way layout groups work is they force children to go to a certain position, so to find out that position we first introduce the variance to the tab
            //we will later be undoing this for repositioning sake
            //handler.transform.SetParent(this.transform); 

            List<string> statistics = manager.GetStatValueKeys(scenename);
            handler.stats.Initialize(scenename, statistics);
            handler.RenameTab(scenename);

            Toggle toggle = handler.gameObject.GetComponent<Toggle>();
            toggle.group = group;

            toggle.onValueChanged.AddListener(delegate { OnToggle(contentHandlers.IndexOf(handler)); } );
        }

        foreach (StatTabContentHandler handler in contentHandlers)
        {

            handler.Deparent();
            handler.transform.SetParent(this.transform);
            handler.Resetparent(handler.transform);
        }

        if (contentHandlers.Count > 0)
        {
            DisableAllTabs();
            EnableTab(0);
        }
    }

    public void OnToggle(int ID)
    {
        DisableAllTabs();
        EnableTab(ID);
    }

    private void DisableAllTabs()
    {
        foreach(StatTabContentHandler handler in contentHandlers)
        {
            handler.Disable();
        }
    }

    private void EnableTab(int tabID)
    {
        contentHandlers[tabID].Enable();
    }

    private IEnumerator EndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        profileImage.sprite = photomaker.TakePhotograph(100, 100);
    }
}
