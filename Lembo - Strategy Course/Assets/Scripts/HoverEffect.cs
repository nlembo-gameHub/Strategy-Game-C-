using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{

    public float HoverAmount; //Will control the intensity of the mouse hoving on the tile

    private void OnMouseEnter()
    {
       transform.localScale += Vector3.one * HoverAmount; //Sizes the scale of the tile based on the hover amount to show a simple graphic 
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * HoverAmount; //Returnns the tile to its default height
    }
}
