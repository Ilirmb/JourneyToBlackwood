using UnityEngine;
using UnityEngine.UI;

public class StatDrawer : MonoBehaviour
{
    public Text text; 
    
    public void Initialize(string t)
    {
        text.text = t;
    }
}
