using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    #region InitiativeFields
    public Unit SelectedUnit;
    public Unit[] AllUnits;
    public int SelectedUnitIndex;
    public Unit[] initiativeOrder;
    #endregion

    #region TimeScaleFields
    public List<TimeScaleAction> TimeScaleOrder = new List<TimeScaleAction>();
    public int CurrentTime;
    public TimeScaleAction[] ActionsToActivate;
    #endregion

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    #region //Round Flow Methods
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

        initiativeOrder = AllUnits.OrderByDescending(x => x.InitiativeValue).ToArray();

        StartRound();
    }

    //Initial round set up
    void StartRound()
    {
        SelectedUnit = null;

        SelectedUnitIndex = 0;

        SelectedUnit = initiativeOrder[SelectedUnitIndex];

        foreach (Unit x in initiativeOrder)
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

    //Selects next unit
    void SelectNextUnit()
    {
        SelectedUnitIndex++;

        if (SelectedUnitIndex >= initiativeOrder.Length)
        {
            SelectedUnitIndex = 0;
        }

        initiativeOrder = initiativeOrder.ToArray();

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
    }

    #endregion

    void AddAction(TimeScaleAction TSA_ToBeAdded)
    {
        int priorityOffset = 0;

        TSA_ToBeAdded.timeScalePosition = TSA_ToBeAdded.timeScaleOffSet + CurrentTime;

        foreach (TimeScaleAction x in TimeScaleOrder)
        {
            if (x.timeScalePosition == TSA_ToBeAdded.timeScalePosition)
            {
                priorityOffset++;
            }
        }

        TSA_ToBeAdded.timeScalePriority = priorityOffset;

        TimeScaleOrder.Add(TSA_ToBeAdded);
    }

    void FindNextActionsToActivate()
    {
        List<TimeScaleAction> QueuedActions = new List<TimeScaleAction>();

        foreach (TimeScaleAction x in TimeScaleOrder)
        {
            if (x.timeScalePosition == CurrentTime)
            {
                QueuedActions.Add(x);
            }
        }

        ActionsToActivate = QueuedActions.OrderBy(x => x.timeScalePriority).ToArray();
    }

    void ActivateActions()
    {
        foreach (TimeScaleAction x in ActionsToActivate)
        {
            x.ActionEffect();
            // Some type of wait until the effect resolves
        }

        // Clear the ActionsToActivate array
    }

    void IncrememntTime()
    {
        CurrentTime++;
    }
}   