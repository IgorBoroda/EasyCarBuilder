using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SuspensionBuilder : MonoBehaviour
{
    [System.Serializable]
    public class Suspension
    {
        public List<WheelPare> wheelPareList = new List<WheelPare>();



        /// <summary>
        /// add new wheel pare
        /// </summary>
        public void AddWheelPare()
        {
            WheelPare wheelPare = new WheelPare()
            {
                blockDifferential = false,
                isDrive = false,
                steerAngle = 0,
                col_leftWheel = null,
                mesh_leftWheel = null,
                col_rightWheel = null,
                mesh_rightWheel = null
            };
            wheelPareList.Add(wheelPare);
        }




        /// <summary>
        /// remove choosen wheel pare
        /// </summary>
        /// <param name="_wheelPare"></param>
        public void RemoveWheelPare(WheelPare _wheelPare)
        {
            wheelPareList.Remove(_wheelPare);
        }



    }
    public Suspension suspension;




    [System.Serializable]
    public class Lights
    {
        public List<Light> basicLights = new List<Light>();
        public List<Light> parkingLights = new List<Light>();
        public List<Light> highBeam = new List<Light>();

        public List<Light> leftTurnSignal = new List<Light>();
        public List<Light> rightTurnSignal = new List<Light>();

        public List<Light> stopSignal = new List<Light>();

        public List<Light> reverseLight = new List<Light>();
    }
    public Lights lights;
    
}
