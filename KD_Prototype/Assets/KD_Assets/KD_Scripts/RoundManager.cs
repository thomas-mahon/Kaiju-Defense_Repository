using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    #region Fields
    public Unit selectedUnit;
    public Unit[] allUnits;
    public List<Unit> initiativeOrder = new List<Unit>();
    public int SelectedUnitIndex;

    public List<TimeScaleAction> timeScaleOrder = new List<TimeScaleAction>();
    #endregion

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Use this for initialization
    void Start ()
    {
        StartBattle();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            EndUnitTurn();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SelectNextUnit();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            MoveTimeScaleForward();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            timeScaleOrder.Add(null);
        }

        if (Input.GetMouseButton(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Begin the inital round and set up, then call startRound
    void StartBattle()
    {
        allUnits = FindObjectsOfType<Unit>();

        foreach (Unit x in allUnits)
        {
            initiativeOrder.Add(x);
        }

        initiativeOrder = initiativeOrder.OrderByDescending(x => x.InitiativeValue)
                  .ThenBy(x => x.InitiativeValue)
                  .ToList();

        StartRound();
    }

    //Initial round set up
    void StartRound()
    {
        selectedUnit = null;

        SelectedUnitIndex = 0;

        selectedUnit = initiativeOrder[SelectedUnitIndex];

        foreach (Unit x in initiativeOrder)
        {
            if (x != selectedUnit)
            {
                x.ToggleControl(false);
            }
        }

        selectedUnit.ToggleControl(true);
    }

    //Activate the unit to be controlled
    void ActivateNewUnit()
    {
        selectedUnit.ToggleControl(true);
    }

    //Selects next unit, if the unit can't be selected skip it, if theres nothing left
    //End the round
    void SelectNextUnit()
    {
        SelectedUnitIndex++;

        if (SelectedUnitIndex >= initiativeOrder.Count)
        {
            SelectedUnitIndex = 0;
        }

        selectedUnit = initiativeOrder[SelectedUnitIndex];

        foreach (Unit x in initiativeOrder)
        {
            if (x != selectedUnit)
            {
                x.ToggleControl(false);
            }
        }

        ActivateNewUnit();
    }

    //Any clean up, then call RoundProcess
    void EndRound()
    {

    }

    //get next unit, move time scale forward
    void EndUnitTurn()
    {
        SelectNextUnit();
        MoveTimeScaleForward();
    }

    //do action effect, remove that action
    void MoveTimeScaleForward()
    {
        if (timeScaleOrder.Count > 0)
        {
            if (timeScaleOrder[0] != null)
            {
                timeScaleOrder[0].ActionEffect();
            }
            timeScaleOrder.RemoveAt(0);
        }
    }
}   
