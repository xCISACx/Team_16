using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CutoutMaskUI : Image
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public override Material materialForRendering()
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int) CompareFunction.NotEqual);
            return material;
        }
    }*/
}
