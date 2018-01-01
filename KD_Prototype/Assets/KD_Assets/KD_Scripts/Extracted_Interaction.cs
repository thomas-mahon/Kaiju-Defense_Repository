﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extracted_Interaction : MonoBehaviour
{
    #region Interaction Fields
    float InteractiveRange = 3;

    public IInteractable InteractableToActivate;

    public GameObject cameraToDrawFrom;

    Vector3 endPoint;

    bool isNotReadyToInteract;
    float InteractionCooldown;
    #endregion

    [SerializeField]
    Text UIDisplayText;

    #region Interaction Methods
    //Updates our tooltip text with the text displayed by the interactable
    private void InteractableText()
    {
        if (InteractableToActivate == null)
        {
            UIDisplayText.gameObject.SetActive(false);
        }
        else
        {
            UIDisplayText.text = InteractableToActivate.DisplayText;
            UIDisplayText.gameObject.SetActive(true);
        }
    }

    //Checks if we are drawing to any interactables
    private void CheckForInteractables()
    {
        endPoint = (cameraToDrawFrom.transform.forward * InteractiveRange) + cameraToDrawFrom.transform.position;

        RaycastHit raycastHit;
        if (Physics.Raycast(cameraToDrawFrom.transform.position, cameraToDrawFrom.transform.forward,
            out raycastHit, InteractiveRange) && raycastHit.transform.gameObject.layer == 12)
        {
            InteractableToActivate = raycastHit.transform.gameObject.GetComponent<IInteractable>();
        }
        else
        {
            InteractableToActivate = null;
        }
    }
    #endregion

}
