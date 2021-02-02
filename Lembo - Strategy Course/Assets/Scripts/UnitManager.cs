using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public bool IsSelected; //Acts as the tracker for the individual owners of this script
    GameMaster GM; //Variable we will use to find the GameMaster Object 

    public int TileSpeed; //Decides how many tiles this unit can move
    public bool HasMoved; //Keeps track on if the unit has moved
    public float MoveSpeed; //The unit's movement speed towards the tile

    public int PlayerNumber;

    public Animator UnitAnim;

    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameMaster>(); //Us calling out to find the GameMaster Object
        UnitAnim = GetComponent<Animator>();
        if(UnitAnim != null)
        {
            UnitAnim.SetBool("IsIdle", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function that registers if the mouse button has been clicked
    private void OnMouseDown()
    {
        if (IsSelected == true) //Checks to see if the Unit is already selected
        {
            IsSelected = false; //If so, we make that selection false and Deselect the Unit
            GM.SelectedUnit = null; //Registers that there are now no currently selected units in the game
            GM.ReseltTiles(); //Reset the tiles when clicking on the unit already selected
        }
        else
        {
            if(PlayerNumber == GM.PlayerTurn)
            {
                if (GM.SelectedUnit != null) //Checking to see if there is already a currently selected unit in the game when clicking on another unit
                {
                    GM.SelectedUnit.IsSelected = false; //If so, we will deselect that unit
                }

                IsSelected = true; //We now set the currently selected unit bool to true
                GM.SelectedUnit = this; //And we then tell the Game Manager that this unit is now selected
                GM.ReseltTiles(); //We reset all the tiles when we click on a non-selected unit
                GetWalkableTiles(); //The function that tells the player what tiles are walkable to
            }
        }

    }

    void GetWalkableTiles()
    {
        //If the unit has already moved, we will ignore the rest of the function
        if (HasMoved == true)
        {
            return;
        }
        //This Foreach loop will let our unit search and access every tile object with the TileMapper script
        foreach (TileMapper tile in FindObjectsOfType<TileMapper>())
        {
            //We are getting the absolute distance values of the X and Y Position of our Unit to our Tile, we are then comparing this with our tile speed to consider how far we can move on an X by Y grid
            //This is a coordinate-based system that allows us to use the absolute values for these two object positions. 
            if(Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= TileSpeed) 
            {
                //We call the "IsClear" function from out TileMapper script. This will allow us to access that scripts spotter for any obstcle tiles.
                if(tile.IsClear() == true)
                {
                    tile.Hightlights();
                }
            }
        }
    }

    //The Unit Move Function, will call an Enum to move the unit towards the desired tile
    public void Move(Vector2 TilePos)
    {
        GM.ReseltTiles(); //We reset all the tiles right before we move to the selected location
        StartCoroutine(StartMovement(TilePos));
    }
    IEnumerator StartMovement(Vector2 TilePos)
    {
        //This while loops first ensures our unit is moving along the x-axis
        while(transform.position.x != TilePos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(TilePos.x, transform.position.y), MoveSpeed * Time.deltaTime);
            UnitAnim.SetBool("IsRunning", true);
            UnitAnim.SetBool("IsIdle", false);
            yield return null;
        }
        //After the x-axis is finished, the y axis is then moved to
        while (transform.position.y != TilePos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, TilePos.y), MoveSpeed * Time.deltaTime);
            UnitAnim.SetBool("IsRunning", true);
            UnitAnim.SetBool("IsIdle", false);
            yield return null;
        }
        UnitAnim.SetBool("IsRunning", false);
        UnitAnim.SetBool("IsIdle", true);
        HasMoved = true;
    }
}
