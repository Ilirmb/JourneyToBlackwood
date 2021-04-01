using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHintShuffle : MonoBehaviour
{
    Image hintmenu;
    public Sprite[] MenuCards;

    // Start is called before the first frame update
    void Start()
    {
        hintmenu = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //hintmenu.sprite = current;
    }

    public void Shuffle()
    {
        hintmenu.sprite = MenuCards[Random.Range(0, MenuCards.Length)];
    }
}
