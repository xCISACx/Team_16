using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Prefs", menuName = "Prefs")]
public class Prefs : ScriptableObject
{
    [Header("Audio")] 
    public float MasterValue;
    public float MasterVolume;
    public float MusicValue;
    public float MusicVolume;
    public float SfxValue;
    public float SfxVolume;

    [Header("Graphics")] 
    public int ResolutionIndex;
    public int ResolutionW;
    public int ResolutionH;
    public bool Fullscreen;
    public FullScreenMode FullScreenMode;

    [Header("Stats")] 
    public List<Score> Scores;

    public int HighScore;
    
    [Header("Customisation")]
    
    public Color CapColour;
    public Color HairColour;
    public Color BodyColour;
    public Color ShirtColour;
    public Color PantsColour;

    public string scoreDataPath;
}
