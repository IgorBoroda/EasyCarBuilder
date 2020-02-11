using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WheelSide { right, left};


[System.Serializable]
public class WheelPare 
{
    public WheelCollider col_rightWheel, col_leftWheel;
    public MeshRenderer mesh_rightWheel, mesh_leftWheel;


    public float steerAngle
    {
        get => m_steerAngle;

        set
        {
            if (value <= 90)
                m_steerAngle = value;
        }
    }
    [SerializeField]
    private float m_steerAngle;

    public float distanceBetweenWheels
    {
        get => m_distanceBetweenWheels;
        set
        {
            if (value > 0)
                m_distanceBetweenWheels = value;
        }
    }
    private float m_distanceBetweenWheels = 4;

    public float brakeForce
    {
        get => m_brakeForce;

        set
        {
            if (value >= 0)
                m_brakeForce = value;
        }
    }
    [SerializeField]
    private float m_brakeForce = 50;

    public bool isDrive = true;
    public bool blockDifferential;

}
