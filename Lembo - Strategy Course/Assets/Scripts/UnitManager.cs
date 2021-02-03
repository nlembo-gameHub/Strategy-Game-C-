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

    public int PlayerNumber; //What Turn does this player move their units on

    public Animator UnitAnim;

    //Unit Attack Variables
    public int AttackRange; //The attack Range of the Unit
    public List<UnitManager> EnemiesInRange = new List<UnitManager>(); //This list will be filled and contain any enemy units in range.
    public bool HasAttacked; //Is the flag to determine if the player has or hasn't attacked yet
    public GameObject WeaponIcon;
    //Unit Stats
    public int Health;
    public int AttackDamage;
    public int DefensneDamage;
    public int Armor;

    //Damage Effects
    public DamageIcon damagedIcon;
    public GameObject DeathEffect;

    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameMaster>(); //Us calling out to find the GameMaster Object
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Function that registers if the mouse button has been clicked
    private void OnMouseDown()
    {
        ResetWeaponIcons(); //Resets the weapon icons on the units when a new unit is selected or if a unit is deselected

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
                GetEnemies(); //The function that tells the player what enemy units are within range
            }
        }

        Collider2D UnitCollider = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        UnitManager unit = UnitCollider.GetComponent<UnitManager>();
        if(GM.SelectedUnit != null)
        {
            if(GM.SelectedUnit.EnemiesInRange.Contains(unit) && GM.SelectedUnit.HasAttacked == false)
            {
                GM.SelectedUnit.Attack(unit);
            }
        }
    }

    void Attack(UnitManager enemy)
    {
        HasAttacked = true;

        int EnemyDamage = AttackDamage - enemy.Armor; //The Amount of Damage we are afflicting on the unit that this unit is attacking
        int MyDamage = enemy.DefensneDamage - Armor; //The amount of Damage that the defending unit inflicts on the attakcing unit

        if(EnemyDamage >= 1) //The reduction of the enemy health
        {
            DamageIcon instance = Instantiate(damagedIcon, enemy.transform.position, Quaternion.identity);
            instance.Setup(EnemyDamage);
            enemy.Health -= EnemyDamage;
        }

        if(MyDamage >= 1) //Reduction to this unit's health 
        {
            DamageIcon instance = Instantiate(damagedIcon, transform.position, Quaternion.identity);
            instance.Setup(MyDamage);
            Health -= MyDamage;
        }

        if(enemy.Health <= 0)
        {
            Instantiate(DeathEffect, enemy.transform.position, Quaternion.identity);
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }

        if(Health <= 0)
        {
            Instantiate(DeathEffect, transform.position, Quaternion.identity);
            GM.ReseltTiles();
            Destroy(this.gameObject);
        }
    }

    void GetWalkableTiles()
    {
        //Debug.Log("Getting Tiles");
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

    //Grabs the information of nearby units that are considered enemies to the player whose turn it is
    void GetEnemies()
    {
        //Debug.Log("Getting Enemies Function Calledd");
        //Clears out the enemies in range list, makes sure it's update and doesn't remember enemies who may no long be in range
        EnemiesInRange.Clear();
        UnitManager[] enemies = FindObjectsOfType<UnitManager>();
        foreach (UnitManager enemy in enemies)
        {
            //We are getting the absolute distance values of the X and Y Position of our Unit to the range of the Enemy Units, we are then comparing this with our attack range to consider how far we can move on an X by Y grid
            //This is a coordinate-based system that allows us to use the absolute values for these two object positions. 
            if (Mathf.Abs(transform.position.x - enemy.transform.position.x) + Mathf.Abs(transform.position.y - enemy.transform.position.y) <= AttackRange)
            {
                //Debug.Log("Getting Enemies 1st If Function Working");
                //Checks to see if the unnits that we are adding to the list aren't our ownn ally and that the unit hasn't already attacked
                if (enemy.PlayerNumber != GM.PlayerTurn && HasAttacked == false) 
                {
                    //Debug.Log("Getting Enemies 2nd If Function Working");
                    EnemiesInRange.Add(enemy);
                    enemy.WeaponIcon.SetActive(true);
                }
            }
        }
    }

    public void ResetWeaponIcons()
    {
        foreach(UnitManager unit in FindObjectsOfType<UnitManager>())
        {
            unit.WeaponIcon.SetActive(false);
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
            yield return null;
        }
        //After the x-axis is finished, the y axis is then moved to
        while (transform.position.y != TilePos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, TilePos.y), MoveSpeed * Time.deltaTime);
            yield return null;
        }
        HasMoved = true;
        ResetWeaponIcons(); //Since once we finish moving, we are recalculating who we can attack
        GetEnemies(); //recalculates what enemies are in range of the moved unit
    }

   //void OnDrawGizmosSelected()
   // {
   //     Gizmos.color = Color.yellow;
   //     Gizmos.DrawSphere(transform.position, AttackRange);
   // }
}
