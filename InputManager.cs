using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseController))]
public class InputManager : MonoBehaviour
{

    private BaseController baseController;

    //vertical input
    private float m_acceleration;
    private float m_braking;

    //horizontal input
    private float m_turning;

    //gear events
    public event Action Event_OnInputGearUP = () => { };
    public event Action Event_OnInputGearDown = () => { };


    private void Awake()
    {
        //get base controller script as component
        void GetBaseController()
        {
            baseController = GetComponent<BaseController>();
            if (baseController == null)
                Debug.LogError(this.name + "Base Controller script not found");
        }
        GetBaseController();


        //subscribe base controller on gears input events
        Event_OnInputGearUP += baseController.OnGearUP;
        Event_OnInputGearDown += baseController.OnGearDOWN;
    }


    private void Update()
    {
        //get input 
        void GetInput()
        {

            // accelerate and brakes input
            m_acceleration = Input.GetAxis("[CC] Acceleration");
            m_braking = Input.GetAxis("[CC] Brakes");

       


            // horizontal input
            m_turning = Input.GetAxis("[CC] Turning");



            void ChangeGears()
            {
                // gear up
                if (Input.GetButtonDown("[CC] ChangeGears") && Input.GetAxisRaw("[CC] ChangeGears") > 0)
                    Event_OnInputGearUP.Invoke();

                //gear down
                if (Input.GetButtonDown("[CC] ChangeGears") && Input.GetAxisRaw("[CC] ChangeGears") < 0)
                    Event_OnInputGearDown.Invoke();
            }
            ChangeGears();
        }
        GetInput();


        //send input data to base controller
        void SendData()
        {
            baseController.inputAcceleration = m_acceleration;
            baseController.inputBraking = m_braking;

            baseController.inputTurning = m_turning;
        }
        SendData();
    }


}
