using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapper : MonoBehaviour
{

    private SpriteRenderer RendSwapper; //This will act as the owner tile's render graphic that will be swapped
    public Sprite[] TileSwapArray; //This will hold all our possible tile arrays

    public float HoverAmount; //Will control the intensity of the mouse hoving on the tile

    public LayerMask ObstacleLayer; //This will act as our variable to check for any obstcles on the tile

    // Start is called before the first frame update
    void Start()
    {
        //Gets the Tile's render component, aka the image itself
        RendSwapper = GetComponent<SpriteRenderer>();
        //Generates a random tile graphic based on the elements in the TileSwap Array
        int RandTile = Random.Range(0, TileSwapArray.Length);
        //Sets the new render graphic to that element
        RendSwapper.sprite = TileSwapArray[RandTile];
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnMouseEnter()
    {
        if(IsClear())
        {
            transform.localScale += Vector3.one * HoverAmount; //Sizes the scale of the tile based on the hover amount to show a simple graphic 
        }
        
    }

    private void OnMouseExit()
    {
        if(IsClear())
        {
            transform.localScale -= Vector3.one * HoverAmount; //Returnns the tile to its default height
        }
        
    }

    //Bool Func that checks and sees if the tile is clear or has an obstacle
    public bool IsClear()
    {
        Collider2D Obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, ObstacleLayer); //Sets the obstacle equal to any Obstacle layer object

        if(Obstacle != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
