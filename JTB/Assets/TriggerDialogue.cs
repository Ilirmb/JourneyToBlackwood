using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.tag == "Player")
       {
            GameManager.instance.ShowWaveTip();
       }
        Invoke("EndDialogue", 2f);
    }
    public void EndDialogue()
    {
        DialogueProcessor.instance.dialogueUI.SetActive(false);
        this.gameObject.SetActive(false);
        GameManager.instance.EnablePlayerMovement();


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
