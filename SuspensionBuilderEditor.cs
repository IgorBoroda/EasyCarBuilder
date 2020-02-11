using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SuspensionBuilder))]
public class SuspensionBuilderEditor : Editor
{

    private SuspensionBuilder suspensionBuilder;
    private SuspensionsToolsWindow toolsWindow;

    public Texture2D texture_wheel, texture_wheelAxis, texture_middlePart;

    private void OnEnable()
    {
        toolsWindow = (SuspensionsToolsWindow)EditorWindow.GetWindow(typeof(SuspensionsToolsWindow));
        toolsWindow.Close();
        suspensionBuilder = (SuspensionBuilder)target;
    }

    
    public override void OnInspectorGUI()
    {
        #region Styles
        GUIStyle style_header1 = new GUIStyle() { alignment = TextAnchor.MiddleCenter };
        #endregion

        #region Content
        void Content_AddButton()
        {
            int _width = 50;
            int _height = 16;


            GUILayout.BeginHorizontal();


                GUILayout.Space((Screen.width / 2) - _width);


                if (GUILayout.Button("Add", GUILayout.Width(_width), GUILayout.Height(_height)))
                    suspensionBuilder.AddWheelPare();


                GUILayout.Space((Screen.width / 2));


            GUILayout.EndHorizontal();
        }
        void Content_RemoveButton(WheelPare _wheelPare)
        {

            int _width = 70;
            int _height = 16;

            GUILayout.BeginHorizontal();


                GUILayout.Space((Screen.width / 2) - _width);


                if (GUILayout.Button("Remove", GUILayout.Width(_width), GUILayout.Height(_height)))
                    suspensionBuilder.wheelPareList.Remove(_wheelPare);


                GUILayout.Space((Screen.width / 2));
            
            
            GUILayout.EndHorizontal();
        }
        void Content_AddRemoveButtons()
        {
            int _width = 80;
            int _height = 16;


            GUILayout.BeginHorizontal();

                GUILayout.Space(70);

                //add button
                if (GUILayout.Button("Add", GUILayout.Width(_width), GUILayout.Height(_height)))
                    suspensionBuilder.AddWheelPare();


                //remove button
                if (GUILayout.Button("Remove", GUILayout.Width(_width), GUILayout.Height(_height)))
                {
                    int count = suspensionBuilder.wheelPareList.Count;
                    WheelPare wheelPare = suspensionBuilder.wheelPareList[count - 1];
                    suspensionBuilder.wheelPareList.Remove(wheelPare);
                }

            GUILayout.EndHorizontal();
        }
        void Content_WPsettingsButtons(WheelPare _wheelPare)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(90);


            int _width = 30;
            int _height = 30;



            Color defaultColor = GUI.backgroundColor;
            Color disableColor = Color.white;
            Color enableColor = Color.gray;



            void SteerAngleButton()
            {

                //button enabled
                if (_wheelPare.steerAngle > 0)
                {


                    //set button color
                    GUI.backgroundColor = enableColor;



                    if (GUILayout.Button("St", GUILayout.Width(_width), GUILayout.Height(_height)))
                    {
                        _wheelPare.steerAngle = 0;
                    }


                    //set default color for next elements
                    GUI.backgroundColor = defaultColor;
                }

                //button disabled
                else
                {


                    //set button color 
                    GUI.backgroundColor = disableColor;



                    if (GUILayout.Button("St", GUILayout.Width(_width), GUILayout.Height(_height)))
                    {
                        _wheelPare.steerAngle = 30;
                    }



                    //set default color for next elements
                    GUI.backgroundColor = defaultColor;
                }
            }
            SteerAngleButton();


            void DriveWheelButton()
            {

                //button enabled
                if (_wheelPare.isDrive)
                {


                    //set button color
                    GUI.backgroundColor = enableColor;



                    if (GUILayout.Button("Dr", GUILayout.Width(_width), GUILayout.Height(_height)))
                    {
                        _wheelPare.isDrive = false;
                    }


                    //set default color for next elements
                    GUI.backgroundColor = defaultColor;
                }

                //button disabled
                else
                {


                    //set button color 
                    GUI.backgroundColor = disableColor;



                    if (GUILayout.Button("Dr", GUILayout.Width(_width), GUILayout.Height(_height)))
                    {
                        _wheelPare.isDrive = true;
                    }



                    //set default color for next elements
                    GUI.backgroundColor = defaultColor;
                }
            }
            DriveWheelButton();


            void BlockDiffButton()
            {

                //button enabled
                if (_wheelPare.blockDifferential)
                {


                    //set button color
                    GUI.backgroundColor = enableColor;



                    if (GUILayout.Button("Bd", GUILayout.Width(_width), GUILayout.Height(_height)))
                    {
                        _wheelPare.blockDifferential = false;
                    }


                    //set default color for next elements
                    GUI.backgroundColor = defaultColor;
                }

                //button disabled
                else
                {


                    //set button color 
                    GUI.backgroundColor = disableColor;



                    if (GUILayout.Button("Bd", GUILayout.Width(_width), GUILayout.Height(_height)))
                    {
                        _wheelPare.blockDifferential = true;
                    }



                    //set default color for next elements
                    GUI.backgroundColor = defaultColor;
                }
            }
            BlockDiffButton();


            GUILayout.EndHorizontal();
        }
        void Content_WPsteerAgleSlider(WheelPare _wheelPare)
        {
            if(_wheelPare.steerAngle > 0)
                _wheelPare.steerAngle = EditorGUI.Slider(new Rect(60, 60, 150, 15), _wheelPare.steerAngle, 1, 90);
        }
        void Content_DrawWheelPare(WheelPare _wheelPare)
        {
            

            //draw buttons under wheels 
            void Content_WheelButtons()
            {
                int _width = 400;
                int _height = 100;



                GUILayout.BeginArea(new Rect(0, 0, _width, _height));

                Color defaultColor = GUI.backgroundColor;
                Color buttonColor = new Color(255, 255, 255, 0.4f);


                GUIStyle style_wheelPlacedEnabled = new GUIStyle("Button");
                GUIStyle style_wheelPlacedDisabled = new GUIStyle("Button");






                GUILayout.BeginHorizontal();



                #region Left wheel
                //wheel not placed
                if (_wheelPare.col_leftWheel == null)
                {


                    GUI.backgroundColor = buttonColor;


                    if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(100)))
                    {
                        toolsWindow.Show(); 
                    }


                    GUI.backgroundColor = defaultColor;


                }

                //wheel placed
                else
                {
                    GUI.backgroundColor = buttonColor;

                    if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(100)))
                    {
                        toolsWindow.Show();
                    }

                    GUI.backgroundColor = defaultColor;



                    GUILayout.BeginArea(new Rect(0, 3, 50, 100));

                    GUILayout.Label(texture_wheel, GUILayout.Width(50));

                    GUILayout.EndArea();
                }
                #endregion



                GUILayout.Space(180);



                #region Right wheel
                //wheel not placed
                if (_wheelPare.col_rightWheel == null)
                {


                    GUI.backgroundColor = buttonColor;


                    if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(100)))
                    {
                        toolsWindow.Show();
                    }


                    GUI.backgroundColor = defaultColor;


                }

                //wheel placed
                else
                {
                    GUI.backgroundColor = buttonColor;

                    if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(100)))
                    {
                        toolsWindow.Show();
                    }

                    GUI.backgroundColor = defaultColor;



                    GUILayout.BeginArea(new Rect(234, 3, 50, 100));

                    GUILayout.Label(texture_wheel, GUILayout.Width(50));

                    GUILayout.EndArea();
                }
                #endregion



                GUILayout.EndHorizontal();

                




                GUILayout.EndArea();
            }
            Content_WheelButtons();


            //draw emty wheel axis
            void Content_WheelAxis()
            {
                int _width = 400;
                int _height = 100;


                GUILayout.Label(texture_wheelAxis, GUILayout.Width(_width), GUILayout.Height(_height));
            }
            Content_WheelAxis();


            
        }
        void Content_DrawMiddlePart()
        {
            int _width = 400;
            int _height = 100;

            GUILayout.Label(texture_middlePart, GUILayout.Width(_width), GUILayout.Height(_height));
        }
        #endregion


        GUILayout.Space(20);

        
        //no wheel pare
        if (suspensionBuilder.wheelPareList.Count == 0)
        {
            //label
            GUILayout.Label("there is no any wheel pare", style_header1);
            GUILayout.Space(10);


            //button "add"
            Content_AddButton();
        }


        //one wheel pare
        else if(suspensionBuilder.wheelPareList.Count == 1)
        {


            WheelPare[] wheelPareArr = suspensionBuilder.wheelPareList.ToArray();
            for (int i = 0; i < wheelPareArr.Length; i++)
            {

                GUILayout.BeginArea(new Rect(20, 20, 400, 300));



                #region Wheel pare number
                GUILayout.BeginArea(new Rect(5, 60, 20, 20));

                    GUILayout.Label((i + 1).ToString());

                GUILayout.EndArea();
                #endregion


                #region Visualization of wheel pare 
                Content_DrawWheelPare(wheelPareArr[i]);
                #endregion


                #region Settings buttons and steer angle slider
                GUILayout.BeginArea(new Rect(0, 0, 400, 100));

                    Content_WPsettingsButtons(wheelPareArr[i]);
                    Content_WPsteerAgleSlider(wheelPareArr[i]);

                GUILayout.EndArea();
                #endregion


                //#region Draw object fields for wheels
                //Content_WheelsObjectField(wheelPareArr[i]);
                //#endregion



                GUILayout.EndArea();



            }
            suspensionBuilder.wheelPareList.Clear();
            suspensionBuilder.wheelPareList.AddRange(wheelPareArr);



            GUILayout.Space(150);


            //Add and Remove buttons
            Content_AddRemoveButtons();
        }


        //more then one
        else
        {

            
            WheelPare[] wheelPareArr = suspensionBuilder.wheelPareList.ToArray();
            for (int i = 0; i < wheelPareArr.Length; i++)
            {

                GUILayout.BeginArea(new Rect(20, 200 * i + 20, 400, 300));



                #region Wheel pare number
                GUILayout.BeginArea(new Rect(60, 10, 20, 20));

                    GUILayout.Label((i + 1).ToString());

                GUILayout.EndArea();
                #endregion


                #region Visualization of wheel pare 
                Content_DrawWheelPare(wheelPareArr[i]);
                #endregion


                #region Settings buttons and steer angle slider
                GUILayout.BeginArea(new Rect(0, 0, 400, 100));

                Content_WPsettingsButtons(wheelPareArr[i]);
                Content_WPsteerAgleSlider(wheelPareArr[i]);

                GUILayout.EndArea();
                #endregion


                //#region Draw object fields for wheels
                //Content_WheelsObjectField(wheelPareArr[i]);
                //#endregion


                #region Draw middle part between wheels axes (wheel pares)
                if (i < wheelPareArr.Length - 1)
                {
                    GUILayout.BeginArea(new Rect(0, 80, 400, 100));

                    Content_DrawMiddlePart();

                    GUILayout.EndArea();
                }
                #endregion





                GUILayout.EndArea();

            }
            suspensionBuilder.wheelPareList.Clear();
            suspensionBuilder.wheelPareList.AddRange(wheelPareArr);



            GUILayout.Space(wheelPareArr.Length * 200 - 30);



            //Add and Remove buttons
            Content_AddRemoveButtons();
        }


        GUILayout.Space(20);

    }
}
