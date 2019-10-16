using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    [SerializeField]
    private Text tutorialText;

    /*[SerializeField]
    private float displayTime = 5.0f;*/

    [SerializeField]
    private Checkpoint initialCheckpoint;
    private PlayerStatistics player;

    private bool hpLow = false;
    private bool hpRecover = false;
    private bool frustration = false;
    private bool staminaDrain = false;

    [Header("Text displays")]
    public DialogueTree hpLowText;
    public DialogueTree hpRecoverText;
    public DialogueTree frustrationText;
    public DialogueTree staminaDrainText;


    void Start()
    {
        player = GameManager.instance.GetPlayerStatistics();
        tutorialText.canvasRenderer.SetAlpha(0);
    }


    // Update is called once per frame
    void Update () {

        if(!hpLow && player.stamina <= 20.0f)
        {
            hpLow = true;
            DialogueProcessor.instance.StartDialogue(hpLowText, true);
        }

        if (!staminaDrain && player.stamina <= 95.0f)
        {
            staminaDrain = true;
            DialogueProcessor.instance.StartDialogue(staminaDrainText, true);
        }

        if(!hpRecover && player.checkpoint == initialCheckpoint)
        {
            hpRecover = true;
            DialogueProcessor.instance.StartDialogue(hpRecoverText, true);
        }

        // We need to introduce frustration. Should this be done if they touch the bar, if they die X times, or both?
        if (!frustration && player.numPlayerDeaths > 1)
        {
            frustration = true;
            DialogueProcessor.instance.StartDialogue(frustrationText, true);
        }

    }


    /*private IEnumerator TutorialTextDisplay(DialogueTree dt)
    {
        displayingText = true;

        //tutorialText.text = n;
        tutorialText.CrossFadeAlpha(1.0f, 1.0f, false);

        DialogueProcessor.instance.StartDialogue(dt, true);
        

        yield return new WaitForSeconds(displayTime);

        tutorialText.CrossFadeAlpha(0.0f, 1.0f, false);

        displayingText = false;
    }*/
}
