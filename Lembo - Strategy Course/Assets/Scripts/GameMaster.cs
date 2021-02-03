using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMaster : MonoBehaviour
{
    //Accesses the UnitManager of the selected units
    public UnitManager SelectedUnit;
    public GameObject SelectedUnitSquare;

    //UI Variables
    public Image PlayerIndicator;
    public Sprite Player1Indicator;
    public Sprite Player2Indicator;

    public int PlayerTurn = 1; //An interger to represent the player turn order. 1 = Blue, 2 = Black

    //Player Gold Variables
    public int Player1Gold = 100;
    public int Player2Gold = 100;

    public TextMeshProUGUI Player1GoldText;
    public TextMeshProUGUI Player2GoldText;

    //Barrack Items
    public BarrackItem PurchasedItem;

    //Unit Stat Variables
    public GameObject StatsPanel;
    public Vector2 StatsPanelShift;
    public UnitManager ViewedUnit;

    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI ArmorText;
    public TextMeshProUGUI AttackDamageText;
    public TextMeshProUGUI RetaliateDamageText;


    public void Start()
    {
        GetGoldIncome(PlayerTurn);
    }

    public void ToggleStatsPanel(UnitManager unit)
    {
        if(unit.Equals(ViewedUnit) == false)
        {
            StatsPanel.SetActive(true);
            StatsPanel.transform.position = (Vector2)unit.transform.position + StatsPanelShift;
            ViewedUnit = unit;
            UpdateStatsPanel();
        }
        else
        {
            StatsPanel.SetActive(false);
            ViewedUnit = null;
        }
    }

    public void UpdateStatsPanel()
    {
        if(ViewedUnit != null)
        {
            HealthText.text = ViewedUnit.Health.ToString();
            ArmorText.text = ViewedUnit.Armor.ToString();
            AttackDamageText.text = ViewedUnit.AttackDamage.ToString();
            RetaliateDamageText.text = ViewedUnit.DefensneDamage.ToString();
        }
    }

    public void MoveStatsPanel(UnitManager unit)
    {
        if(unit.Equals(ViewedUnit))
        {
            StatsPanel.transform.position = (Vector2)unit.transform.position + StatsPanelShift;
        }
    }

    public void RemoveStatsPanel(UnitManager unit)
    {
        if(unit.Equals(ViewedUnit))
        {
            StatsPanel.SetActive(false);
            ViewedUnit = null;
        }
    }

    public void UpdateGoldText()
    {
        Player1GoldText.text = Player1Gold.ToString();
        Player2GoldText.text = Player2Gold.ToString();
    }

    void GetGoldIncome(int playerTurn)
    {
        foreach(VillageManager village in FindObjectsOfType<VillageManager>())
        {
            if(village.PlayerNumber == playerTurn)
            {
                if(playerTurn == 1)
                {
                    Player1Gold += village.GoldProduction;
                }
                else
                {
                    Player2Gold += village.GoldProduction;
                }
            }
            else
            {
                //This is for when I make unclaimed villages
            }
        }

        UpdateGoldText();
    }

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
            PlayerIndicator.sprite = Player2Indicator;
        }
        else if(PlayerTurn == 2)
        {
            PlayerTurn = 1;
            PlayerIndicator.sprite = Player1Indicator;
        }
        GetGoldIncome(PlayerTurn);
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

        GetComponent<BarracksManager>().CloseMenus();
    }
  
}
