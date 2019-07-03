using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    [SerializeField]
    private Text tutorialText;

    [SerializeField]
    private float displayTime = 5.0f;

    [SerializeField]
    private Checkpoint initialCheckpoint;

    private bool displayingText = false;
    private IEnumerator DisplayText;
    private PlayerStatistics player;

    private bool hpLow = false;
    private bool hpRecover = false;
    private bool frustration = false;
    private bool staminaDrain = false;


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
            DisplayText = TutorialTextDisplay("You're dying!");
            StartCoroutine(DisplayText);
        }

        if (!staminaDrain && player.stamina <= 95.0f)
        {
            staminaDrain = true;
            DisplayText = TutorialTextDisplay("Stamina drains as you move");
            StartCoroutine(DisplayText);
        }

        if(!hpRecover && player.checkpoint == initialCheckpoint)
        {
            hpRecover = true;
            DisplayText = TutorialTextDisplay("Recover at checkpoints");
            StartCoroutine(DisplayText);
        }

        // We need to introduce frustration. Should this be done if they touch the bar, if they die X times, or both?

    }


    private IEnumerator TutorialTextDisplay(string n)
    {
        displayingText = true;

        tutorialText.text = n;
        tutorialText.CrossFadeAlpha(1.0f, 1.0f, false);

        yield return new WaitForSeconds(displayTime);

        tutorialText.CrossFadeAlpha(0.0f, 1.0f, false);

        displayingText = false;
    }
}
