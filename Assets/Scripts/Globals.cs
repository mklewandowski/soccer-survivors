using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals
{
    public static bool DebugMode = false;

    // audio and music
    public static bool AudioOn = true;
    public static bool MusicOn = true;

    public static bool IsPaused = false;


    public enum PlayerTypes {
        Survivor,
    }

    public static string[] PlayerNames = {
        "Soccer Survivor",
    };

    public static string[] PlayerUnlockTexts = {
        "",
    };

    public static string[] AnimationSuffixes = {
        "",
    };

    public static PlayerTypes currentPlayerType = PlayerTypes.Survivor;
    public static int MaxPlayerTypes = 1;
    public static int[] CharacterUnlockStates = new int[MaxPlayerTypes];
}
