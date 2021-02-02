using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //Accesses the UnitManager of the selected units
    public UnitManager SelectedUnit;
    public GameObject SelectedUnitSquare;

    public int PlayerTurn = 1; //An interger to represent the player turn order. 1 = Blue, 2 = Black

    public void ReseltTiles()
    {
        foreach(TileMapper tile in FindObjectsOfType<TileMapper>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        //Get's the Player's input to end their turn
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }

        if(SelectedUnit != null)
        {
            SelectedUnitSquare.SetActive(true);
            SelectedUnitSquare.transform.position = SelectedUnit.transform.position;
        }
        else
        {
            SelectedUnitSquare.SetActive(false);
        }
    }

    private void EndTurn()
    {
        //Switches between Two players, will need to make it more fluid so that it can take on more players
        if(PlayerTurn == 1)
        {
            PlayerTurn = 2;
        }
        else if(PlayerTurn == 2)
        {
            PlayerTurn = 1;
        }
        //If there is a unit selected when the turn is ending, it will deselect that unit
        if(SelectedUnit != null)
        {
            SelectedUnit.IsSelected = false;
            SelectedUnit = null;
        }

        ReseltTiles();
        //Goes through each of the units and make sure their 'Has Moved' bool is reset to false so that they can move again next turn
        foreach(UnitManager unit in FindObjectsOfType<UnitManager>())
        {
            unit.HasMoved = false;
            unit.WeaponIcon.SetActive(false);
            unit.HasAttacked = false;
        }
    }
  
}
