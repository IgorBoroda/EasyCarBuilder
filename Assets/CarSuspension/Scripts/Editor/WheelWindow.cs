using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WheelWindow : EditorWindow
{
    

    #region Events
    public event Action OnWindowClose = () => {};
    #endregion



    #region Wheel pare operations
    private WheelPare m_wheelPare;
    private WheelSide m_wheel;

    public void SetWheel(WheelPare _wheelPare, WheelSide _wheelSide)
    {
        m_wheelPare = _wheelPare;
        m_wheel = _wheelSide;
    }


    private void ResetWheel()
    {
        if(m_wheel == WheelSide.left)
        {
            m_wheelPare.col_leftWheel = null;
            m_wheelPare.mesh_leftWheel = null;
        }
        else
        {
            m_wheelPare.col_rightWheel = null;
            m_wheelPare.mesh_rightWheel = null;
        }
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
                GUILayout.Label(m_wheel + " wheel", style_topText);
                GUILayout.Space(10);
            }
        }
        DrawTopLabel();

        void DrawColliderAndMeshFields()
        {
            if (m_wheelPare != null)
            {

                //left wheel
                if (m_wheel == WheelSide.left)
                {
                    m_wheelPare.col_leftWheel = (WheelCollider)EditorGUILayout.ObjectField("Wheel collider", m_wheelPare.col_leftWheel, typeof(WheelCollider));
                    m_wheelPare.mesh_leftWheel = (MeshRenderer)EditorGUILayout.ObjectField("Wheel mesh", m_wheelPare.mesh_leftWheel, typeof(MeshRenderer));
                }


                //right wheel
                if (m_wheel == WheelSide.right)
                {
                    m_wheelPare.col_rightWheel = (WheelCollider)EditorGUILayout.ObjectField("Wheel collider", m_wheelPare.col_rightWheel, typeof(WheelCollider));
                    m_wheelPare.mesh_rightWheel = (MeshRenderer)EditorGUILayout.ObjectField("Wheel mesh", m_wheelPare.mesh_rightWheel, typeof(MeshRenderer));
                }
            }
        }
        DrawColliderAndMeshFields();

        void DrawResetButton()
        {
            GUILayout.Space(50);
            if (GUILayout.Button("Reset"))
            {
                ResetWheel();
            }
        }
        DrawResetButton();
    }



    private void OnDestroy()
    {
        OnWindowClose.Invoke();
    }
}
