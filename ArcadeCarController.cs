using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ArcadeCarController : BaseController
{

    


    [Header("Brakes")]
    public float baseBrakeForce = 20;


    [Header("Other")]
    public double speed;



    private void FixedUpdate()
    {
        Engine();
        Brakes();
        Turning();
        UpdateWheelsMeshes();
        GetCarSpeed();
    }



    private void Engine()
    {

        //update wheels torque
        foreach (WheelPare wheelPare in drivePareList)
        {
            WheelCollider l_collider = wheelPare.leftWheel.wheelCollider;
            WheelCollider r_collider = wheelPare.rightWheel.wheelCollider;


            void AddWheelTorque(WheelCollider _wheelCollider)
            {
                if (_wheelCollider.rpm * GetGearForce(m_currentGear) < maxRPM)
                    _wheelCollider.motorTorque = m_engineTorque;
                else
                    _wheelCollider.motorTorque = 0;
            }
            AddWheelTorque(l_collider);
            AddWheelTorque(r_collider);
        }
    }


    float turningSpeed = 5f;
    private void Turning()
    {
        foreach(WheelPare wheelPare in steerPareList)
        {
            WheelCollider left_col = wheelPare.leftWheel.wheelCollider;
            WheelCollider right_col = wheelPare.rightWheel.wheelCollider;

            float maxSteerAngle = wheelPare.steerAngle;

            void TurnWheel(WheelCollider _col)
            {
                float turnTarget = Mathf.Lerp(_col.steerAngle, maxSteerAngle * m_turning, turningSpeed * Time.deltaTime);
                Debug.Log(turnTarget);
                _col.steerAngle = turnTarget;
            }
            TurnWheel(left_col);
            TurnWheel(right_col);
        }
    }



    private void Brakes()
    {
        void SetBrakesForce(float _force = 0)
        {

            foreach (WheelPare wheelPare in allWheelsList)
            {
                WheelCollider left_col = wheelPare.leftWheel.wheelCollider;
                WheelCollider right_col = wheelPare.rightWheel.wheelCollider;


                left_col.brakeTorque = _force;
                right_col.brakeTorque = _force;
            }
        }
        SetBrakesForce(m_braking * baseBrakeForce);
    }



    private void UpdateWheelsMeshes()
    {
        foreach (WheelPare wheelPare in allWheelsList)
        {
            void UpdateMesh(WheelCollider collider, Transform wheel)
            {
                Vector3 position;
                Quaternion rotation;

                collider.GetWorldPose(out position, out rotation);

                wheel.position = position;
                wheel.rotation = rotation;
            }
            UpdateMesh(wheelPare.leftWheel.wheelCollider, wheelPare.leftWheel.pivot);
            UpdateMesh(wheelPare.rightWheel.wheelCollider, wheelPare.rightWheel.pivot);
        }
    }



    private void GetCarSpeed()
    {
        speed = GetSpeed(SpeedMetrics.KMH);
    }
}
