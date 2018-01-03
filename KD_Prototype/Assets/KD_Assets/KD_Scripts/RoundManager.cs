using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    #region Fields
    public Unit SelectedUnit;
    public Unit[] AllUnits;
    public List<Unit> InitiativeOrder = new List<Unit>();
    public int SelectedUnitIndex;

    public List<TimeScaleAction> TimeScaleOrder = new List<TimeScaleAction>();
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
            TimeScaleOrder.Add(null);
        }

        if (Input.GetMouseButton(0) && Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Begin the inital round and set up, then call startRound
    void StartBattle()
    {
        AllUnits = FindObjectsOfType<Unit>();

        foreach (Unit x in AllUnits)
        {
            InitiativeOrder.Add(x);
        }

        InitiativeOrder = InitiativeOrder.OrderByDescending(x => x.InitiativeValue)
                  .ThenBy(x => x.InitiativeValue)
                  .ToList();

        StartRound();
    }

    //Initial round set up
    void StartRound()
    {
        SelectedUnit = null;

        SelectedUnitIndex = 0;

        SelectedUnit = InitiativeOrder[SelectedUnitIndex];

        foreach (Unit x in InitiativeOrder)
        {
            if (x != SelectedUnit)
            {
                x.ToggleControl(false);
            }
        }

        SelectedUnit.ToggleControl(true);
    }

    //Activate the unit to be controlled
    void ActivateNewUnit()
    {
        SelectedUnit.ToggleControl(true);
    }

    //Selects next unit, if the unit can't be selected skip it, if theres nothing left
    //End the round
    void SelectNextUnit()
    {
        SelectedUnitIndex++;

        if (SelectedUnitIndex >= InitiativeOrder.Count)
        {
            SelectedUnitIndex = 0;
        }

        Unit[] initiativeOrder = InitiativeOrder.ToArray();

        SelectedUnit = initiativeOrder[SelectedUnitIndex];

        for (int i = 0; i < initiativeOrder.Length; i++)
        {
            if (initiativeOrder[i] != SelectedUnit)
            {
                initiativeOrder[i].ToggleControl(false);
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
        if (TimeScaleOrder.Count > 0)
        {
            if (TimeScaleOrder[0] != null)
            {
                TimeScaleOrder[0].ActionEffect();
            }
            TimeScaleOrder.RemoveAt(0);
        }
    }
}   
