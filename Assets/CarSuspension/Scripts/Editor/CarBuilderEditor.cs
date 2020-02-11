using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SuspensionBuilder))]
public class CarBuilderEditor : Editor
{
    //master class
    static SuspensionBuilder suspensionBuilder;




    #region Tabs
    private enum Tab { SuspensionConstructor, Lights, EditorSettings }
    Tab tab = new Tab();



    [System.Serializable]
    private class TabSuspension
    {

        //textures for drawing axis UI
        private Texture2D texture_TwoWheels, texture_NoWheels, texture_LeftWheel, texture_RightWheel, texture_middlePart;

        //textures for axis buttons
        private Texture2D texture_SettingsButton, texture_DriveButton, texture_SteerButton;

        //size of UI content
        private float contentSize = 70;





        /// <summary>
        /// Load UI textures from assets
        /// </summary>
        public void LoadTextures()
        {
            //load axis textures
            texture_TwoWheels = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/CarSuspension/Textures/Suspension/EditorTwoWheels.png", typeof(Texture2D));
            texture_NoWheels = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/CarSuspension/Textures/Suspension/EditorNoWheels.png", typeof(Texture2D));
            texture_LeftWheel = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/CarSuspension/Textures/Suspension/EditorOneWheelL.png", typeof(Texture2D));
            texture_RightWheel = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/CarSuspension/Textures/Suspension/EditorOneWheelR.png", typeof(Texture2D));
            texture_middlePart = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/CarSuspension/Textures/Suspension/EditorMiddlePart.png", typeof(Texture2D));


            //load buttons
            texture_SettingsButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/CarSuspension/Textures/Suspension/Buttons/SettingsButton.png", typeof(Texture2D));
            texture_DriveButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/CarSuspension/Textures/Suspension/Buttons/DriveButton.png", typeof(Texture2D));
            texture_SteerButton = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/CarSuspension/Textures/Suspension/Buttons/SteerButton.png", typeof(Texture2D));
        }




        /// <summary>
        /// Show UI
        /// </summary>
        public void Show()
        {
            //no wheel pare
            if (suspensionBuilder.suspension.wheelPareList.Count == 0)
            {
                GUILayout.Space(20);

                //label style
                GUIStyle middleText = new GUIStyle();
                middleText.alignment = TextAnchor.MiddleCenter;
                middleText.fontSize = 15;

                //draw label
                GUILayout.Label("there is no any wheel pare", middleText);
                GUILayout.Space(10);


                //button "add"
                Content_AddButton();
            }


            //more than one wheel pare
            else
            {
                WheelPare[] wheelPareArr = suspensionBuilder.suspension.wheelPareList.ToArray();
                for (int i = 0; i < wheelPareArr.Length; i++)
                {
                    //draw buttons under wheels
                    GUILayout.BeginArea(new Rect(20, 70 + 2f * contentSize * i, 300, 300));
                    Content_WheelButtons(wheelPareArr[i]);
                    GUILayout.EndArea();


                    //draw wheel axis
                    GUILayout.BeginArea(new Rect(20, 70 + 2f * contentSize * i, 300, 300));
                    Content_WheelAxis(wheelPareArr[i]);
                    GUILayout.EndArea();


                    //draw buttons
                    GUILayout.BeginArea(new Rect(20, 70 + 2f * contentSize * i, 300, 300));
                    Content_AxisButtons(wheelPareArr[i]);
                    GUILayout.EndArea();


                    //draw middle part
                    if (i < wheelPareArr.Length - 1)
                    {
                        GUILayout.BeginArea(new Rect(20, 70 + 2f * contentSize * i, 300, 300));
                        Content_MiddlePart();
                        GUILayout.EndArea();
                    }
                }
                suspensionBuilder.suspension.wheelPareList.Clear();
                suspensionBuilder.suspension.wheelPareList.AddRange(wheelPareArr);


                GUILayout.Space(2 * wheelPareArr.Length * contentSize);


                //button "add"
                Content_AddRemoveButtons();
            }
        }






        #region Content
        void Content_AddButton()
        {
            int _width = 50;
            int _height = 16;


            GUILayout.BeginHorizontal();


            GUILayout.Space((Screen.width / 2) - _width);


            if (GUILayout.Button("Add", GUILayout.Width(_width), GUILayout.Height(_height)))
                suspensionBuilder.suspension.AddWheelPare();


            GUILayout.Space((Screen.width / 2));


            GUILayout.EndHorizontal();
        }




        void Content_AddRemoveButtons()
        {
            int _width = 80;
            int _height = 16;


            GUILayout.BeginHorizontal();

            GUILayout.Space((Screen.width / 2) - _width);

            //add button
            if (GUILayout.Button("Add", GUILayout.Width(_width), GUILayout.Height(_height)))
                suspensionBuilder.suspension.AddWheelPare();


            //remove button
            if (GUILayout.Button("Remove", GUILayout.Width(_width), GUILayout.Height(_height)))
            {
                int count = suspensionBuilder.suspension.wheelPareList.Count;
                WheelPare wheelPare = suspensionBuilder.suspension.wheelPareList[count - 1];
                suspensionBuilder.suspension.wheelPareList.Remove(wheelPare);
            }


            GUILayout.Space((Screen.width / 2));

            GUILayout.EndHorizontal();
        }




        //draw emty wheel axis
        void Content_WheelAxis(WheelPare _wheelPare)
        {
            float width = 4 * contentSize;
            float height = 1 * contentSize;


            bool IsLeftWheelPlaced()
            {
                if (_wheelPare.col_leftWheel != null && _wheelPare.mesh_leftWheel != null)
                    return true;
                else
                    return false;
            }

            bool IsRightWheelPlaced()
            {
                if (_wheelPare.col_rightWheel != null && _wheelPare.mesh_rightWheel != null)
                    return true;
                else
                    return false;
            }



            //all wheels placed
            if (IsLeftWheelPlaced() && IsRightWheelPlaced())
                GUILayout.Label(texture_TwoWheels, GUILayout.Width(width), GUILayout.Height(height));


            //left wheel placed
            else if (IsLeftWheelPlaced() && !IsRightWheelPlaced())
                GUILayout.Label(texture_LeftWheel, GUILayout.Width(width), GUILayout.Height(height));


            //right wheel placed
            else if (!IsLeftWheelPlaced() && IsRightWheelPlaced())
                GUILayout.Label(texture_RightWheel, GUILayout.Width(width), GUILayout.Height(height));


            //no wheel placed
            else if (!IsLeftWheelPlaced() && !IsRightWheelPlaced())
                GUILayout.Label(texture_NoWheels, GUILayout.Width(width), GUILayout.Height(height));

        }



        //buttons under wheels
        void Content_WheelButtons(WheelPare _wheelPare)
        {
            float width = 0.5f * 70;
            float height = 1f * 70;

            Color defaultColor = GUI.backgroundColor;
            Color pressedColor = Color.white;
            Color unpressedColor = new Color(255, 255, 255, 0.4f);
            Color hoverColor = new Color(255, 255, 255, 0.6f);

            GUIStyle style_Button = new GUIStyle("Box");



            void DrawButton(WheelSide _wheel)
            {
                //check if that wheel already choosen
                bool IsWheelChoosen()
                {
                    if (choosenWheel.wheelPare == _wheelPare && choosenWheel.wheel == _wheel)
                        return true;
                    else
                        return false;
                }



                void SetButtonColor()
                {
                    if (IsWheelChoosen())
                        GUI.backgroundColor = pressedColor;
                    else
                        GUI.backgroundColor = unpressedColor;
                }
                SetButtonColor();



                //draw button
                if (GUILayout.Button("", style_Button, GUILayout.Width(width), GUILayout.Height(height)))
                {

                    //unpress the button
                    if (IsWheelChoosen())
                    {

                        ResetChoosenWheel();

                        //close wheel window
                        CloseWheelWindow();
                    }


                    //press the button
                    else
                    {
                        choosenWheel.wheelPare = _wheelPare;
                        choosenWheel.wheel = _wheel;


                        //open and refresh wheel window
                        OpenWheelWindow();
                    }
                }


                //return color to default
                GUI.backgroundColor = defaultColor;
            }




            GUI.backgroundColor = unpressedColor;



            GUILayout.BeginHorizontal();


            DrawButton(WheelSide.left);


            GUILayout.Space(1.7f * 70);


            DrawButton(WheelSide.right);


            GUILayout.EndHorizontal();
        }



        //buttons on wheel axis
        void Content_AxisButtons(WheelPare _wheelPare)
        {
            float width = 0.4f * contentSize;
            float height = 0.4f * contentSize;


            Color defaultColor = GUI.backgroundColor;
            Color pressedColor = Color.white;
            Color unpressedColor = new Color(255, 255, 255, 0.4f);
            Color hoverColor = new Color(255, 255, 255, 0.6f);




            GUILayout.BeginHorizontal();


            //horizontal space
            GUILayout.Space(0.7f * contentSize);


            void SteerAngleButton()
            {
                void SetButtonColor()
                {
                    if (_wheelPare.steerAngle > 0)
                        GUI.backgroundColor = pressedColor;
                    else
                        GUI.backgroundColor = unpressedColor;
                }
                SetButtonColor();



                if (GUILayout.Button(texture_SteerButton, GUILayout.Width(width), GUILayout.Height(height)))
                {
                    if (_wheelPare.steerAngle == 0)
                        _wheelPare.steerAngle = 30;
                    else
                        _wheelPare.steerAngle = 0;
                }

                GUI.backgroundColor = defaultColor;
            }
            SteerAngleButton();



            void DriveWheelButton()
            {
                void SetButtonColor()
                {
                    if (_wheelPare.isDrive)
                        GUI.backgroundColor = pressedColor;
                    else
                        GUI.backgroundColor = unpressedColor;
                }
                SetButtonColor();



                if (GUILayout.Button(texture_DriveButton, GUILayout.Width(width), GUILayout.Height(height)))
                {
                    if (_wheelPare.isDrive)
                        _wheelPare.isDrive = false;
                    else
                        _wheelPare.isDrive = true;
                }

                GUI.backgroundColor = defaultColor;
            }
            DriveWheelButton();



            void SettingsButton()
            {
                void SetButtonColor()
                {
                    if (choosenWheelPare == _wheelPare)
                        GUI.backgroundColor = pressedColor;
                    else
                        GUI.backgroundColor = unpressedColor;
                }
                SetButtonColor();



                if (GUILayout.Button(texture_SettingsButton, GUILayout.Width(width), GUILayout.Height(height)))
                {
                    if (choosenWheelPare == _wheelPare)
                    {
                        choosenWheelPare = null;

                        CloseAxislWindow();
                    }
                    else
                    {
                        choosenWheelPare = _wheelPare;

                        OpenAxisWindow();
                    }
                }

                GUI.backgroundColor = defaultColor;
            }
            SettingsButton();


            GUILayout.EndHorizontal();
        }



        //draw middle part between wheel axis
        void Content_MiddlePart()
        {
            float width = 4f * 70;
            float height = 1f * 70;


            GUILayout.Space(0.7f * 70);
            GUILayout.Label(texture_middlePart, GUILayout.Width(width), GUILayout.Height(height));
        }
        #endregion




        #region Wheel window
        private struct ChoosenWheel
        {
            public WheelPare wheelPare;
            public WheelSide wheel;
        }
        ChoosenWheel choosenWheel;



        private void OpenWheelWindow()
        {
            WheelWindow wheelWindow = (WheelWindow)EditorWindow.GetWindow(typeof(WheelWindow));
            wheelWindow.SetWheel(choosenWheel.wheelPare, choosenWheel.wheel);
            wheelWindow.Show();


            wheelWindow.OnWindowClose += ResetChoosenWheel;
        }



        private void CloseWheelWindow()
        {
            WheelWindow wheelWindow = (WheelWindow)EditorWindow.GetWindow(typeof(WheelWindow));
            wheelWindow.Close();
        }



        private void ResetChoosenWheel()
        {
            choosenWheel = new ChoosenWheel();
        }
        #endregion




        #region Axis window
        WheelPare choosenWheelPare;



        private void OpenAxisWindow()
        {
            AxisWindow axisWindow = (AxisWindow)EditorWindow.GetWindow(typeof(AxisWindow));
            axisWindow.SetWheelPare(choosenWheelPare);
            axisWindow.Show();


            axisWindow.OnWindowClose += ResetChoosenWheelPare;
        }


        private void CloseAxislWindow()
        {
            AxisWindow axisWindow = (AxisWindow)EditorWindow.GetWindow(typeof(AxisWindow));
            axisWindow.Close();
        }


        private void ResetChoosenWheelPare()
        {
            choosenWheelPare = null;
        }
        #endregion


    }
    private TabSuspension tabSuspension = new TabSuspension();



    [System.Serializable]
    private class TabLights
    {

    }
    private static TabLights tabLights = new TabLights();



    [System.Serializable]
    private class TabSettings
    {

    }
    private static TabSettings tabSettings = new TabSettings();
    #endregion






    private void OnEnable()
    {
        suspensionBuilder = (SuspensionBuilder)target;

        tabSuspension.LoadTextures();
    }



    public override void OnInspectorGUI()
    {
        //draw tabs on top
        void Content_Tabs()
        {
            GUILayout.BeginHorizontal();

            void TabSuspensionConstructor()
            {
                int width = 90;
                int height = 20;

                GUIStyle tab_style = new GUIStyle("Box");

                if (GUILayout.Button("Constructor", tab_style, GUILayout.Width(width), GUILayout.Height(height)))
                {
                    tab = Tab.SuspensionConstructor;
                }
            }
            TabSuspensionConstructor();

            void TabLights()
            {
                int width = 90;
                int height = 20;

                GUIStyle tab_style = new GUIStyle("Box");

                if (GUILayout.Button("Lights", tab_style, GUILayout.Width(width), GUILayout.Height(height)))
                {
                    tab = Tab.Lights;
                }
            }
            TabLights();

            void TabEditorSettings()
            {
                int width = 90;
                int height = 20;

                GUIStyle tab_style = new GUIStyle("Box");

                if (GUILayout.Button("Settings", tab_style, GUILayout.Width(width), GUILayout.Height(height)))
                {
                    tab = Tab.EditorSettings;
                }
            }
            TabEditorSettings();

            GUILayout.EndHorizontal();
        }
        Content_Tabs();


        tabSuspension.Show();
    }
}
