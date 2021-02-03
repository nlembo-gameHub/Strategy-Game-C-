using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapper : MonoBehaviour
{

    private SpriteRenderer RendSwapper; //This will act as the owner tile's render graphic that will be swapped
    public Sprite[] TileSwapArray; //This will hold all our possible tile arrays

    public float HoverAmount; //Will control the intensity of the mouse hoving on the tile

    public LayerMask ObstacleLayer; //This will act as our variable to check for any obstcles on the tile
    public LayerMask VillageLayer;

    public Color HighlightedColor;
    public bool IsWalkable;

    GameMaster GM;
    //Variables for creating unnits
    public Color CreatableColor;
    public bool IsCreatable;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the Tile's render component, aka the image itself
        RendSwapper = GetComponent<SpriteRenderer>();
        //Generates a random tile graphic based on the elements in the TileSwap Array
        int RandTile = Random.Range(0, TileSwapArray.Length);
        //Sets the new render graphic to that element
        RendSwapper.sprite = TileSwapArray[RandTile];

        GM = FindObjectOfType<GameMaster>();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnMouseEnter()
    {
        if(IsClear() == true)
        {
            transform.localScale += Vector3.one * HoverAmount; //Sizes the scale of the tile based on the hover amount to show a simple graphic 
        }
        
    }

    private void OnMouseExit()
    {
        if(IsClear() == true)
        {
            transform.localScale -= Vector3.one * HoverAmount; //Returnns the tile to its default height
        }
        
    }

    //Bool Func that checks and sees if the tile is clear or has an obstacle
    public bool IsClear()
    {
        Collider2D Obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, ObstacleLayer); //Sets the obstacle equal to any Obstacle layer object

        if (Obstacle != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool NoVillage()
    {
        Collider2D Village = Physics2D.OverlapCircle(transform.position, 0.2f, VillageLayer); //Sets the obstacle equal to any Obstacle layer object

        if(Village != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Hightlights()
    {
        RendSwapper.color = HighlightedColor;
        IsWalkable = true;
    }

    public void Reset()
    {
        RendSwapper.color = Color.white;
        IsWalkable = false;
        IsCreatable = false;
    }

    public void SetCreatable()
    {
        RendSwapper.color = CreatableColor;
        IsCreatable = true;
    }

    private void OnMouseDown()
    {
        if(IsWalkable == true && GM.SelectedUnit != null)
        {
            GM.SelectedUnit.Move(this.transform.position);
        }
        else if(IsCreatable == true)
        {
            BarrackItem item = Instantiate(GM.PurchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); //Lets us place down the purchased item
            GM.ReseltTiles();
            UnitManager unit = item.GetComponent<UnitManager>();
            if(unit != null)
            {
                unit.HasMoved = true;
                unit.HasAttacked = true;
            }
        }
    }
}
