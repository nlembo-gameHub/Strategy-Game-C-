using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BarracksManager : MonoBehaviour
{
    public Button Player1ToggleButton;
    public Button Player2ToggleButton;

    public GameObject Player1Menu;
    public GameObject Player2Menu;

    GameMaster GM;

    private void Start()
    {
        GM = GetComponent<GameMaster>();
    }

    private void Update()
    {
        if(GM.PlayerTurn == 1)
        {
            Player1ToggleButton.interactable = true;
            Player2ToggleButton.interactable = false;
        }
        else
        {
            Player1ToggleButton.interactable = false;
            Player2ToggleButton.interactable = true;
        }
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf); //If the menu is already open, the menu will be closed

    }

    public void CloseMenus()
    {
        Player1Menu.SetActive(false);
        Player2Menu.SetActive(false);
    }

    public void BuyItem(BarrackItem Item)
    {
        if(GM.PlayerTurn == 1 && Item.Cost <= GM.Player1Gold)
        {
            GM.Player1Gold -= Item.Cost;
            Player1Menu.SetActive(false);
        }
        else if (GM.PlayerTurn == 2 && Item.Cost <= GM.Player2Gold)
        {
            GM.Player2Gold -= Item.Cost;
            Player2Menu.SetActive(false);
        }
        else
        {
            print("Not ENOUG GOLD!");
            return;
        }

        GM.UpdateGoldText();
        GM.PurchasedItem = Item;
        //Ensures that we do not have any units selected
        if(GM.SelectedUnit != null)
        {
            GM.SelectedUnit.IsSelected = false;
            GM.SelectedUnit = null;
        }

        GetCreatableTiles();
    }

    void GetCreatableTiles()
    {
        //foreach (TileMapper tile in FindObjectsOfType<TileMapper>())
        //{
        //    if (tile.IsClear() && tile.NoVillage())
        //    {
        //       tile.SetCreatable();
        //    }
        //}

        if(GM.PlayerTurn == 1)
        {
            foreach(VillageManager village in FindObjectsOfType<VillageManager>())
            {
                if(village.PlayerNumber == 1)
                {
                    foreach (TileMapper tile in FindObjectsOfType<TileMapper>())
                    {
                        if (Mathf.Abs(village.transform.position.x - tile.transform.position.x) + Mathf.Abs(village.transform.position.y - tile.transform.position.y) <= village.VillageReach)
                        {
                            if (tile.IsClear() && tile.NoVillage())
                            {
                                tile.SetCreatable();
                            }
                        }
                    }
                }
            }
        }
        else if(GM.PlayerTurn == 2)
        {
            foreach (VillageManager village in FindObjectsOfType<VillageManager>())
            {
                if (village.PlayerNumber == 2)
                {
                    foreach (TileMapper tile in FindObjectsOfType<TileMapper>())
                    {
                        if (Mathf.Abs(village.transform.position.x - tile.transform.position.x) + Mathf.Abs(village.transform.position.y - tile.transform.position.y) <= village.VillageReach)
                        {
                            if (tile.IsClear() && tile.NoVillage())
                            {
                                tile.SetCreatable();
                            }
                        }
                    }
                }
            }
        }
    }
}
