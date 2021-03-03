using UnityEngine;
using UnityEngine.UI;

public class StatTabContentHandler : MonoBehaviour
{
    public GameObject MainContent;
    public StatisticLoader stats;
    public Text TabName;


    public void RenameTab(string scenename)
    {
        TabName.text = scenename;
    }
    public void Enable()
    {
        MainContent.SetActive(true);
    }

    public void Disable()
    {
        MainContent.SetActive(false);
    }

    public void Reposition(Transform target , Transform content)
    {
        Debug.Log("Target content local position " + content.localPosition + "\n"
                + "Target Tab Local position " + target.localPosition + "\n"
                + "Tab Local position " + this.transform.localPosition + "\n"
                + "Main content local position" + MainContent.transform.localPosition);
        Debug.Log("Target content global position " + content.position + "\n"
             + "Target Tab global position " + target.position + "\n"
             + "Tab global position " + this.transform.position + "\n"
             + "Main content gobal position" + MainContent.transform.position);
        MainContent.transform.localPosition = content.localPosition - this.transform.position;
        Debug.Log("Main Content is now global " + MainContent.transform.position + "\n and local " + MainContent.transform.localPosition);
    }

    public void Reparent(Transform parent)
    {
        MainContent.transform.parent = parent;
    }

    public void Deparent()
    {
        MainContent.transform.parent = null;
    }
    public void Resetparent(Transform parent)
    {
        MainContent.transform.SetParent(parent, true);
    }
}
