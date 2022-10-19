using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public Material newMaterial;
    public int ColourIndex;
    public List<Color> Colours;
    public List<Color> BodyColours;
    public List<Color> HairColours;

    // Update is called once per frame
    void Start()
    {
        SetPantsColour();
        SetShirtColour();
        SetBodyColour();
    }

    public void SetPantsColour()
    {
        var newColour = Colours[ColourIndex];
        meshRenderer.materials[2].color = newColour;
        GameManager.Instance.Prefs.PantsColour = newColour;
    }
    
    public void SetShirtColour()
    {
        var newColour = Colours[ColourIndex];
        meshRenderer.materials[3].color = newColour;
        GameManager.Instance.Prefs.ShirtColour = newColour;
    }
    
    public void SetBodyColour()
    {
        var newColour = BodyColours[ColourIndex];
        meshRenderer.materials[4].color = newColour;
        GameManager.Instance.Prefs.BodyColour = newColour;
    }
    
    public void SetCapColour()
    {
        var newColour = Colours[ColourIndex];
        meshRenderer.materials[7].color = newColour;
        GameManager.Instance.Prefs.CapColour = newColour;
    }
    
    public void SetHairColour()
    {
        var newColour = HairColours[ColourIndex];
        meshRenderer.materials[6].color = newColour;
        GameManager.Instance.Prefs.PantsColour = newColour;
    }

    public void IncreaseIndex(int count)
    {
        ColourIndex = ColourIndex + 1;
        ColourIndex = ColourIndex %= count;
    }

    public void DecreaseIndex(int count)
    {
        if (ColourIndex != 0)
        {
            ColourIndex--;
            ColourIndex %= count;   
        }
        else
        {
            ColourIndex = count - 1;
        }
    }
}
