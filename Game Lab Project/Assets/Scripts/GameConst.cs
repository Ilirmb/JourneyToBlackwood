using UnityEngine;

public static class GameConst
{
    //The name of the player object. THIS IS VERY IMPORTANT TO KEEP ACCURATE. Many scripts use this string to search for the player object
    public const string PLAYER_OBJECT_NAME = "CharacterRobotBoy";
    //These control stamina drain from various activities
    //Stamina is by default 100
    public const float DAMAGE_FROM_FALL = 30.0f;
    public const float DAMAGE_FROM_HIT = 10.0f;
    public const float STAMINA_DRAIN_PER_MOVING_FRAME = .05f;
    public const float STAMINA_DRAIN_PER_DISTANCE_WALKED = 1.0f;
    public const float STAMINA_TO_JUMP = 3f;
    //Other constants go down here
}
