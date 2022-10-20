using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public List<int> ColourIndexes;
    public List<Color> Colours;
    public List<Color> BodyColours;
    public List<Color> HairColours;
    public List<Color> CapColours;
    public List<Color> ShirtColours;
    public List<Color> PantsColours;

    // Update is called once per frame
    void Start()
    {
        ColourIndexes.Clear();
        
        for (int i = 0; i < 7; i++)
        {
            ColourIndexes.Add(0);
        }
        
        SetPantsColour();
        SetShirtColour();
        SetBodyColour();
        SetCapColour();
        SetHairColour();
    }

    public void SetPantsColour()
    {
        var newColour = PantsColours[ColourIndexes[5]];
        meshRenderer.materials[2].color = newColour;
        GameManager.Instance.Prefs.PantsColour = newColour;
    }
    
    public void SetShirtColour()
    {
        var newColour = ShirtColours[ColourIndexes[4]];
        meshRenderer.materials[3].color = newColour;
        GameManager.Instance.Prefs.ShirtColour = newColour;
    }
    
    public void SetBodyColour()
    {
        var newColour = BodyColours[ColourIndexes[3]];
        meshRenderer.materials[4].color = newColour;
        GameManager.Instance.Prefs.BodyColour = newColour;
    }
    
    public void SetCapColour()
    {
        var newColour = CapColours[ColourIndexes[0]];
        meshRenderer.materials[7].color = newColour;
        GameManager.Instance.Prefs.CapColour = newColour;
    }
    
    public void SetHairColour()
    {
        var newColour = HairColours[ColourIndexes[1]];
        meshRenderer.materials[6].color = newColour;
        GameManager.Instance.Prefs.HairColour = newColour;
    }

    public void IncreaseIndex(string text)
    {
        var partText = text.Substring(0, 1);
        var countText = text.Substring(1,1);
        
        var part = int.Parse(partText);
        var count = int.Parse(countText);
        
        if (ColourIndexes[part] <= count - 1)
        {
            ColourIndexes[part]++;
            ColourIndexes[part] %= count;
        }
        else if (ColourIndexes[part] >= count)
        {
            ColourIndexes[part] = 0;
        }
    }

    public void DecreaseIndex(string text)
    {
        var partText = text.Substring(0, 1);
        var countText = text.Substring(1,1);
        
        var part = int.Parse(partText);
        var count = int.Parse(countText);
        
        if (ColourIndexes[part] >= 1)
        {
            ColourIndexes[part]--;
            ColourIndexes[part] %= count;
        }
        else if (ColourIndexes[part] <= 0)
        {
            ColourIndexes[part] = count - 1;
        }
    }
}
