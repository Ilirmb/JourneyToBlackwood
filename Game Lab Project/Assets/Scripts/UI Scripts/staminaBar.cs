using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class staminaBar : MonoBehaviour {

    //The 'full' and 'empty' positions of the stamina bar. These are found out manually by placing the bar in the 'retracted' state and the fully extended state
    //It should be noted that the stamina bar is child to the sprite mask, so it's transform is relative to the center of the mask, not the world origin
    public const float STAMINA_MIN_POS = -400f;
    public const float STAMINA_MAX_POS = -300f;
    //STAMINA_MAX_POS is used to find the scale, which is used to find the positional value the staminaBar should be at given any value of 'stamina'
    //The scale only works if it's calculated from the parents origin, aka 0, so we offset this using the minimum position of the bar
    private const float POSITION_SCALE = 100 / (STAMINA_MAX_POS - STAMINA_MIN_POS);
    private float stamina = 100;
    private float maxStamina = 100;

    private playerStatistics playerStatistics;
    private Text finalTenText;

    // Use this for initialization
    void Start () {
        //Grabs the instance of playerStatistic that we want from the player character
        //IF THE CHARACTER'S NAME CHANGES FROM 'CharacterRobotBoy' it MUST be changed here and in other instances
        playerStatistics = GameObject.Find(GameConst.PLAYER_OBJECT_NAME).GetComponent<playerStatistics>();
        finalTenText = GameObject.Find("Stamina Last Text").GetComponent<Text>();
    }
	
	// So there's definitely a better way to do this
    //You could have a call whenever the stamina changes instead of updating it every frame
    //But I'm too busy to do this right now, just something to keep in mind if performance is an issue
	void Update () {
        //Get stamina and update the stamina bar's location to reflect that
        stamina = playerStatistics.getStamina();
        maxStamina = playerStatistics.maxStamina;
        //Reverse the offset from parent origin (the calculation for POSITION_SCALE required it to be zero for the scale to work)
        transform.localPosition = new Vector3(STAMINA_MIN_POS + (stamina / POSITION_SCALE), 0, 0);
        //Update stamina bar text
        gameObject.transform.GetChild(0).GetComponent<Text>().text = string.Format("{0:0}", stamina);
        gameObject.transform.GetChild(1).GetComponent<Text>().text = string.Format("/{0:0}", maxStamina);
        //Check if the final 10 stamina point text should show
        if (stamina <= 20 && stamina >= 0)
        {
            finalTenText.enabled = true;
            finalTenText.text = string.Format("{0:0.00}", stamina);
        }
        else
        {
            finalTenText.enabled = false;
        }
    }
}
