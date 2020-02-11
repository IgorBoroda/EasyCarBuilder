using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct Gear
{
    public string name;
    public float force;
}


public enum SpeedMetrics { KMH, MPH }


public class BaseController : MonoBehaviour
{
    SuspensionBuilder suspensionBuilder = new SuspensionBuilder();

    protected List<WheelPare> allWheelsList = new List<WheelPare>();
    protected List<WheelPare> drivePareList = new List<WheelPare>();
    protected List<WheelPare> steerPareList = new List<WheelPare>();



    public float inputAcceleration
    {
        get
        {
            return m_acceleration;
        }
        set
        {
            if (value >= 0 && value <= 1)
                m_acceleration = value;
        }
    }
    protected float m_acceleration;

    public float inputBraking
    {
        get
        {
            return m_braking;
        }
        set
        {
            if(value >= 0 && value <= 1)
            {
                m_braking = value;
            }
        }
    }
    protected float m_braking;

    public float inputTurning
    {
        get
        {
            return m_turning;
        }
        set
        {
            if (value >= -1 && value <= 1)
                m_turning = value;
        }
    }
    protected float m_turning;




    



    protected virtual void Start()
    {
        void CacheSuspensionBuilder()
        {
            suspensionBuilder = GetComponent<SuspensionBuilder>();
            if (suspensionBuilder == null)
                Debug.LogError(this.name + "SuspensionBuilder script not found!");
        }
        CacheSuspensionBuilder();

        void CacheWheelPares()
        {
            foreach (WheelPare wheelPare in suspensionBuilder.wheelPareList)
            {
                if (wheelPare.isDrive)
                    drivePareList.Add(wheelPare);

                if (wheelPare.steerAngle > 0)
                    steerPareList.Add(wheelPare);

                allWheelsList.Add(wheelPare);
            }
        }
        CacheWheelPares();

        void CacheWheelRadius()
        {
            
            float l_maxRadius = 0;

            foreach(WheelPare wheelPare in suspensionBuilder.wheelPareList)
            {
                float GetWheelRadius(Wheel _wheel)
                {
                    if (_wheel != null)
                        return _wheel.wheelCollider.radius;

                    return 0;
                }

                float leftRadius = GetWheelRadius(wheelPare.leftWheel);
                float rightRadius = GetWheelRadius(wheelPare.rightWheel);


                if (leftRadius > rightRadius)
                    l_maxRadius = leftRadius;
                else
                    l_maxRadius = rightRadius;
            }

            wheelRadius = l_maxRadius;
        }
        CacheWheelRadius();

        UpdateGearName();
    }


    protected virtual void Update()
    {
        CalculateEngineRPM();
        CalculateEngineTorque();
        AtomaticTransmissionSystem();
    }




    #region Utilities
    protected float GetWheelRPM()
    {
        float wheelRPM = 0;

        foreach (WheelPare wheelPare in drivePareList)
        {
            float l_rpm = wheelPare.leftWheel.wheelCollider.rpm;
            float r_rpm = wheelPare.rightWheel.wheelCollider.rpm;
            float largerRpm;



            if (l_rpm > r_rpm)
                largerRpm = l_rpm;
            else
                largerRpm = r_rpm;



            if (largerRpm > wheelRPM)
                wheelRPM = largerRpm;
        }

        return wheelRPM;
    }



    protected float GetGearForce(int _gearNumber)
    {
        if (_gearNumber >= 0 && _gearNumber < gearsArr.Length)
            return gearsArr[m_currentGear].force;
        else
            Debug.LogError(this.name + ": gears array not consist this gear");

        return 0;
    }



    private float wheelRadius;
    protected double GetSpeed(SpeedMetrics speedMetrics = SpeedMetrics.KMH) 
    {
        switch (speedMetrics)
        {
            case SpeedMetrics.KMH: return (2 * Math.PI * GetWheelRPM()) / 60 * wheelRadius * 3.6d;
            case SpeedMetrics.MPH: return (2 * Math.PI * GetWheelRPM()) / 60 * wheelRadius * 2.2d;
        }
        return 0;
    }
    #endregion




    #region Engine
    [Header("Engine")]
    public float baseEngineForce = 100;
    public float maxRPM = 500;
    public float engineRPM
    {
        get 
        {
            return m_engineRPM; 
        }
    }
    protected float m_engineRPM;
    protected float m_engineTorque;


    private void CalculateEngineRPM()
    {
        m_engineRPM = m_engineRPM * 0.9f + (GetWheelRPM() * GetGearForce(m_currentGear)) * 0.1f;
    }

    private void CalculateEngineTorque()
    {
        m_engineTorque = baseEngineForce * m_acceleration * GetGearForce(m_currentGear);
    }
    #endregion




    #region Transmission



    [Header("Transmission")]
    [Tooltip("is transmission must be automatic")]
    public bool automatic = true;
    [Tooltip("(automatic only) gear will shift up when engine RPM will be greather than value")]
    public float shiftUpRPM = 3000;
    [Tooltip("(automatic only) gear will shift down when engine RPM will be less than value")]
    public float shiftDownRPM = 1000;
    protected int m_currentGear = 1;
    public string currentGearName
    {
        get
        {
            return m_currentGearName;
        }
    }
    protected string m_currentGearName;
    public Gear[] gearsArr = new Gear[]
    {
        new Gear{name = "R", force = -5f},
        new Gear{name = "N", force = 0f},
        new Gear{name = "1", force = 5f},
        new Gear{name = "2", force = 3.5f},
        new Gear{name = "3", force = 2.5f},
        new Gear{name = "4", force = 2f},
        new Gear{name = "5", force = 1.5f}
    };


    //gear change events
    public event Action Event_OnGearUP = () => { };
    public event Action Event_OnGearDown = () => { };


    private void AtomaticTransmissionSystem() //TODO
    {
        if(automatic)
        {
            
        }
    }


    /// <summary>
    /// up shift gear
    /// </summary>
    public virtual void OnGearUP()
    {
        if (m_currentGear < gearsArr.Length - 1)
        {
            m_currentGear++;
            UpdateGearName();
            Event_OnGearUP.Invoke();
        }
    }



    /// <summary>
    /// down shift gear
    /// </summary>
    public void OnGearDOWN()
    {
        if (m_currentGear > 0)
        {
            m_currentGear--;
            UpdateGearName();
            Event_OnGearDown.Invoke();
        }
    }


 
    /// <summary>
    /// set gear by name
    /// </summary>
    /// <param name="_name">gear name</param>
    public void SetGear(string _name)
    {
        for (int i = 0; i < gearsArr.Length; i++)
        {
            if (gearsArr[i].name == _name)
            {
                m_currentGear = i;
                return;
            }
        }

        Debug.LogError(this.name + "Gear with current name was not found");
    }



    /// <summary>
    /// set gear by array index
    /// </summary>
    /// <param name="_index">gear index in array</param>
    public void SetGear(int _index)
    {
        if (_index >= 0 && _index < gearsArr.Length)
            m_currentGear = _index;
        else
            Debug.LogError(this.name + "Gear with current index was not found");
    }



    /// <summary>
    /// get gear name by index
    /// </summary>
    /// <param name="_name">gear name</param>
    /// <returns></returns>
    public int GetGearIndex(string _name)
    {
        for (int i = 0; i < gearsArr.Length; i++)
        {
            if (gearsArr[i].name == _name)
                return i;
        }

        Debug.LogError(this.name + "Gear with current name was not found");
        return 0;
    }



    private void UpdateGearName()
    {
        m_currentGearName = gearsArr[m_currentGear].name;
    }
    #endregion
}
