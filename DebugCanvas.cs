using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvas : MonoBehaviour
{
    private ArcadeCarController carController;

    [SerializeField]
    private Text RPM, Gear, Speed;

    private void Start()
    {
        carController = GameObject.FindObjectOfType<ArcadeCarController>();
    }

    private void Update()
    {
        RPM.text = "RPM: " + carController.engineRPM.ToString("0");
        Gear.text = "Gear: " + carController.currentGearName;
        Speed.text = "Speed: " + carController.speed.ToString("0.0");
    }
}
