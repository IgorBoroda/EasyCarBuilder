using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AxisWindow : EditorWindow
{

    #region Events
    public event Action OnWindowClose = () => { };
    #endregion



    #region Wheel pare operations
    WheelPare m_wheelPare;

    public void SetWheelPare(WheelPare _wheelPare)
    {
        m_wheelPare = _wheelPare;
    }

    private void ResetAxis()
    {
        m_wheelPare.isDrive = true;
        m_wheelPare.blockDifferential = false;
        m_wheelPare.steerAngle = 0;
        m_wheelPare.brakeForce = 50;
    }
    #endregion



    private void OnGUI()
    {
        void DrawTopLabel()
        {
            if (m_wheelPare != null)
            {
                GUIStyle style_topText = new GUIStyle();
                style_topText.alignment = TextAnchor.MiddleCenter;
                style_topText.fontSize = 15;

                GUILayout.Space(10);
                GUILayout.Label("wheel axis", style_topText);
                GUILayout.Space(10);
            }
        }
        DrawTopLabel();

        void DrawParams()
        {
            if(m_wheelPare != null)
            {
                //is drive axis 
                m_wheelPare.isDrive = EditorGUILayout.Toggle("Drive Wheel", m_wheelPare.isDrive);

                //block differential
                m_wheelPare.blockDifferential = EditorGUILayout.Toggle("Block Differential", m_wheelPare.blockDifferential);

                //steer angle slider
                m_wheelPare.steerAngle = EditorGUILayout.Slider("Steer Angle", m_wheelPare.steerAngle, 0, 90);

                //brake force
                m_wheelPare.brakeForce = EditorGUILayout.FloatField("Break Force", m_wheelPare.brakeForce);
            }
        }
        DrawParams();

        void DrawResetButton()
        {
            GUILayout.Space(50);
            if (GUILayout.Button("Reset"))
            {
                ResetAxis();
            }
        }
        DrawResetButton();
    }


    private void OnDestroy()
    {
        OnWindowClose.Invoke();
    }
}
