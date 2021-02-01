using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    //Accesses the UnitManager of the selected units
    public UnitManager SelectedUnit;

    public void ReseltTiles()
    {
        foreach(TileMapper tile in FindObjectsOfType<TileMapper>())
        {
            tile.Reset();
        }
    }
}
