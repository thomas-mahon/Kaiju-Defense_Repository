using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour {

    [SerializeField]
    public float coolDown;
    [SerializeField]
    public Image skillIcon;

    public float currentCoolDown = 0;


    void Start()
    {
        currentCoolDown = coolDown;
    }
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            if(currentCoolDown >=  coolDown)
            {
                currentCoolDown = 0;
            }
        }
    }

    void Update()
    {
        if(currentCoolDown < coolDown)
        {
            currentCoolDown += Time.deltaTime;
            skillIcon.fillAmount = currentCoolDown / coolDown;
        }
    }


}
