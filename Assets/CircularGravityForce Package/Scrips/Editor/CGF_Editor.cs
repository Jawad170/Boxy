﻿/*******************************************************************************************
* Author: Lane Gresham, AKA LaneMax
* Websites: http://resurgamstudios.com
* Description: Used overwriting the Inspector GUI, and Scene GUI
*******************************************************************************************/
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using CircularGravityForce;

namespace CircularGravityForce
{
    [Serializable]
    public class TitleImage
    {
        public Texture2D image;
        public GUIContent content;

        public TitleImage(string imagePath)
        {
            content = new GUIContent("");
            this.image = (Texture2D)AssetDatabase.LoadAssetAtPath(imagePath, typeof(Texture2D));
            content.image = this.image;
        }
    }

    [CustomEditor(typeof(CGF)), CanEditMultipleObjects]
    public class CGF_Editor : Editor
    {
        private GUIStyle panelStyle;
        private GUIStyle titleStyle;
        private TitleImage titleImage;

        private SerializedProperty enable_property;
        private SerializedProperty shape_property;
        private SerializedProperty forceType_property;
        private SerializedProperty forceMode_property;
        private SerializedProperty forcePower_property;
        private SerializedProperty size_property;
        private SerializedProperty torqueMaxAngularVelocity_property;
        private SerializedProperty explosionForceUpwardsModifier_property;
        private SerializedProperty capsuleRadius_property;
        private SerializedProperty boxSize_property;
        private SerializedProperty toggleFilterOptions_property;
        private SerializedProperty toggleMemoryProperties_property;
        private SerializedProperty memoryProperties_seeAffectedColliders_property;
        private SerializedProperty memoryProperties_seeAffectedRaycastHits_property;
        private SerializedProperty memoryProperties_nonAllocPhysics_property;
        private SerializedProperty memoryProperties_colliderBuffer_property;
        private SerializedProperty memoryProperties_raycastHitBuffer_property;
        private SerializedProperty colliderListCount_property;
        private SerializedProperty raycastHitListCount_property;

        private CGF cgf;

        private bool change = false;
        private float spacing = 10f;

        private Color redBar = new Color(1f, 0f, 0f, .4f);

        public void OnEnable()
        {
            //Title Image Resource
            titleImage = new TitleImage("Assets/CircularGravityForce Package/Gizmos/CGF Title.png");

            enable_property = serializedObject.FindProperty("enable");
            shape_property = serializedObject.FindProperty("shape");
            forceType_property = serializedObject.FindProperty("forceType");
            forceMode_property = serializedObject.FindProperty("forceMode");
            forcePower_property = serializedObject.FindProperty("forcePower");
            size_property = serializedObject.FindProperty("size");
            torqueMaxAngularVelocity_property = serializedObject.FindProperty("forceTypeProperties.torqueMaxAngularVelocity");
            explosionForceUpwardsModifier_property = serializedObject.FindProperty("forceTypeProperties.explosionForceUpwardsModifier");
            capsuleRadius_property = serializedObject.FindProperty("capsuleRadius");
            boxSize_property = serializedObject.FindProperty("boxSize");
            toggleFilterOptions_property = serializedObject.FindProperty("toggleFilterOptions");
            memoryProperties_seeAffectedColliders_property = serializedObject.FindProperty("memoryProperties.seeColliders");
            memoryProperties_seeAffectedRaycastHits_property = serializedObject.FindProperty("memoryProperties.seeRaycastHits");
            toggleMemoryProperties_property = serializedObject.FindProperty("memoryProperties.toggleMemoryProperties");
            memoryProperties_nonAllocPhysics_property = serializedObject.FindProperty("memoryProperties.nonAllocPhysics");
            memoryProperties_colliderBuffer_property = serializedObject.FindProperty("memoryProperties.colliderBuffer");
            memoryProperties_raycastHitBuffer_property = serializedObject.FindProperty("memoryProperties.raycastHitBuffer");
            colliderListCount_property = serializedObject.FindProperty("colliderListCount");
            raycastHitListCount_property = serializedObject.FindProperty("raycastHitListCount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            panelStyle = new GUIStyle(GUI.skin.box);
            panelStyle.padding = new RectOffset(panelStyle.padding.left + 10, panelStyle.padding.right, panelStyle.padding.top, panelStyle.padding.bottom);
            EditorGUILayout.BeginVertical(panelStyle);

            /*<----------------------------------------------------------------------------------------------------------*/
            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.alignment = TextAnchor.UpperCenter;
            GUILayout.BeginVertical();
            GUILayout.Box(titleImage.content, titleStyle);
            GUILayout.EndVertical();
            /*<----------------------------------------------------------------------------------------------------------*/

#if !(UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9)
            if (shape_property.enumValueIndex == (int)CGF.Shape.Box)
                EditorGUILayout.HelpBox(CGF.WarningMessageBoxUnity_5_3, MessageType.Warning, true);

            if (memoryProperties_nonAllocPhysics_property.boolValue)
                EditorGUILayout.HelpBox(CGF.WarningMessageNonAllocUnity_5_3, MessageType.Warning, true);
#endif

            /*<----------------------------------------------------------------------------------------------------------*/
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(enable_property, new GUIContent("Enable"));
            if (EditorGUI.EndChangeCheck())
            {
                enable_property.boolValue = EditorGUILayout.Toggle(enable_property.boolValue);
                change = true;
            }
            /*<----------------------------------------------------------------------------------------------------------*/

            /*<----------------------------------------------------------------------------------------------------------*/
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(shape_property, new GUIContent("Shape"));
            if (EditorGUI.EndChangeCheck())
            {
                shape_property.enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup((CGF.Shape)shape_property.enumValueIndex));
                change = true;
            }
            /*<----------------------------------------------------------------------------------------------------------*/

            /*<----------------------------------------------------------------------------------------------------------*/
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(forceType_property, new GUIContent("Force Type"));
            if (EditorGUI.EndChangeCheck())
            {
                forceType_property.enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup((CGF.ForceType)forceType_property.enumValueIndex));
                change = true;
            }
            /*<----------------------------------------------------------------------------------------------------------*/

            /*<----------------------------------------------------------------------------------------------------------*/
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(forceMode_property, new GUIContent("Force Mode"));
            if (EditorGUI.EndChangeCheck())
            {
                forceMode_property.enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup((ForceMode)forceMode_property.enumValueIndex));
                change = true;
            }
            /*<----------------------------------------------------------------------------------------------------------*/

            /*<----------------------------------------------------------------------------------------------------------*/
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(forcePower_property, new GUIContent("Force Power"));
            if (EditorGUI.EndChangeCheck())
            {
                forcePower_property.floatValue = EditorGUILayout.FloatField(forcePower_property.floatValue);
                change = true;
            }
            /*<----------------------------------------------------------------------------------------------------------*/

            switch (forceType_property.enumValueIndex)
            {
                case (int)CGF.ForceType.Torque:

                    /*<----------------------------------------------------------------------------------------------------------*/
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(torqueMaxAngularVelocity_property, new GUIContent("Max Angular Velocity"));
                    if (EditorGUI.EndChangeCheck())
                    {
                        torqueMaxAngularVelocity_property.floatValue = EditorGUILayout.FloatField(torqueMaxAngularVelocity_property.floatValue);
                        change = true;
                    }
                    /*<----------------------------------------------------------------------------------------------------------*/

                    break;

                case (int)CGF.ForceType.ExplosionForce:

                    /*<----------------------------------------------------------------------------------------------------------*/
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(explosionForceUpwardsModifier_property, new GUIContent("Upwards Modifier"));
                    if (EditorGUI.EndChangeCheck())
                    {
                        explosionForceUpwardsModifier_property.floatValue = EditorGUILayout.FloatField(explosionForceUpwardsModifier_property.floatValue);
                        change = true;
                    }
                    /*<----------------------------------------------------------------------------------------------------------*/

                    break;
            }

            if (shape_property.enumValueIndex != (int)CGF.Shape.Box)
            {
                /*<----------------------------------------------------------------------------------------------------------*/
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(size_property, new GUIContent("Size"));
                if (EditorGUI.EndChangeCheck())
                {
                    if (size_property.floatValue >= 0)
                    {
                        size_property.floatValue = EditorGUILayout.FloatField(size_property.floatValue);
                    }
                    else
                    {
                        size_property.floatValue = 0;
                    }
                }
                /*<----------------------------------------------------------------------------------------------------------*/
            }

            switch (shape_property.enumValueIndex)
            {
                case (int)CGF.Shape.Capsule:

                    /*<----------------------------------------------------------------------------------------------------------*/
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(capsuleRadius_property, new GUIContent("Capsule Radius"));
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (capsuleRadius_property.floatValue >= 0)
                        {
                            capsuleRadius_property.floatValue = EditorGUILayout.FloatField(capsuleRadius_property.floatValue);
                        }
                        else
                        {
                            capsuleRadius_property.floatValue = 0;
                        }

                        change = true;
                    }
                    /*<----------------------------------------------------------------------------------------------------------*/

                    break;
                case (int)CGF.Shape.Box:

                    /*<----------------------------------------------------------------------------------------------------------*/
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(boxSize_property, new GUIContent("Box Size"));
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();

                        change = true;
                    }
                    /*<----------------------------------------------------------------------------------------------------------*/

                    break;

            }

            /*<----------------------------------------------------------------------------------------------------------*/
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("constraintProperties"), true, GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                change = true;
            }
            /*<----------------------------------------------------------------------------------------------------------*/

            toggleFilterOptions_property.boolValue = EditorGUILayout.Foldout(toggleFilterOptions_property.boolValue, "Filter Properties");
            if (toggleFilterOptions_property.boolValue)
            {
                /*<----------------------------------------------------------------------------------------------------------*/
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gameobjectFilter"), true, GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    change = true;
                }
                GUILayout.EndHorizontal();
                /*<----------------------------------------------------------------------------------------------------------*/

                /*<----------------------------------------------------------------------------------------------------------*/
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("tagFilter"), true, GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    change = true;
                }
                GUILayout.EndHorizontal();
                /*<----------------------------------------------------------------------------------------------------------*/

                /*<----------------------------------------------------------------------------------------------------------*/
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("layerFilter"), true, GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    change = true;
                }
                GUILayout.EndHorizontal();
                /*<----------------------------------------------------------------------------------------------------------*/

                /*<----------------------------------------------------------------------------------------------------------*/
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("triggerAreaFilter"), true, GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    change = true;
                }
                GUILayout.EndHorizontal();
                /*<----------------------------------------------------------------------------------------------------------*/
            }

            /*<----------------------------------------------------------------------------------------------------------*/
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("specialEffect"), true, GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                change = true;
            }
            /*<----------------------------------------------------------------------------------------------------------*/

            /*<----------------------------------------------------------------------------------------------------------*/
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawGravityProperties"), true, GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                change = true;
            }
            /*<----------------------------------------------------------------------------------------------------------*/

            toggleMemoryProperties_property.boolValue = EditorGUILayout.Foldout(toggleMemoryProperties_property.boolValue, "Memory Properties");
            if (toggleMemoryProperties_property.boolValue)
            {
                if (shape_property.enumValueIndex == (int)CGF.Shape.Sphere || shape_property.enumValueIndex == (int)CGF.Shape.Box)
                {
                    /*<----------------------------------------------------------------------------------------------------------*/
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(memoryProperties_seeAffectedColliders_property, true, GUILayout.ExpandWidth(true));
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                        change = true;
                    }
                    GUILayout.EndHorizontal();
                    /*<----------------------------------------------------------------------------------------------------------*/
                }

                if (shape_property.enumValueIndex == (int)CGF.Shape.Capsule || shape_property.enumValueIndex == (int)CGF.Shape.Raycast)
                {
                    /*<----------------------------------------------------------------------------------------------------------*/
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.PropertyField(memoryProperties_seeAffectedRaycastHits_property, true, GUILayout.ExpandWidth(true));
                    if (EditorGUI.EndChangeCheck())
                    {
                        serializedObject.ApplyModifiedProperties();
                        change = true;
                    }
                    GUILayout.EndHorizontal();
                    /*<----------------------------------------------------------------------------------------------------------*/
                }

                if (Application.isPlaying)
                    GUI.enabled = false;
                
                /*<----------------------------------------------------------------------------------------------------------*/
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(memoryProperties_nonAllocPhysics_property, true, GUILayout.ExpandWidth(true));
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    change = true;
                }
                GUILayout.EndHorizontal();
                /*<----------------------------------------------------------------------------------------------------------*/

                GUI.enabled = true;

                if (memoryProperties_nonAllocPhysics_property.boolValue)
                {

                    if (shape_property.enumValueIndex == (int)CGF.Shape.Sphere || shape_property.enumValueIndex == (int)CGF.Shape.Box)
                    {
                        if (Application.isPlaying)
                            GUI.enabled = false;
                        
                        /*<----------------------------------------------------------------------------------------------------------*/
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(memoryProperties_colliderBuffer_property, true, GUILayout.ExpandWidth(true));
                        if (EditorGUI.EndChangeCheck())
                        {
                            if (memoryProperties_colliderBuffer_property.intValue >= 1)
                            {
                                memoryProperties_colliderBuffer_property.intValue = EditorGUILayout.IntField(memoryProperties_colliderBuffer_property.intValue);
                            }
                            else
                            {
                                memoryProperties_colliderBuffer_property.intValue = 1;
                            }

                            change = true;
                        }
                        GUILayout.EndHorizontal();
                        /*<----------------------------------------------------------------------------------------------------------*/

                        GUI.enabled = true;

                        float collidersUsed = (float)colliderListCount_property.intValue;
                        float collidersNotUsed = (float)memoryProperties_colliderBuffer_property.intValue;
                        float collidersUsedPercent = collidersUsed / collidersNotUsed;

                        if (collidersUsedPercent == 1f)
                            GUI.backgroundColor = redBar;

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.ProgressBar(EditorGUILayout.BeginVertical(), collidersUsedPercent, string.Format("{0} : {1}", collidersUsed, collidersNotUsed));
                        GUILayout.Space(16);
                        EditorGUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }

                    if (shape_property.enumValueIndex == (int)CGF.Shape.Raycast || shape_property.enumValueIndex == (int)CGF.Shape.Capsule)
                    {
                        if (Application.isPlaying)
                            GUI.enabled = false;

                        /*<----------------------------------------------------------------------------------------------------------*/
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(memoryProperties_raycastHitBuffer_property, true, GUILayout.ExpandWidth(true));
                        if (EditorGUI.EndChangeCheck())
                        {
                            if (memoryProperties_raycastHitBuffer_property.intValue >= 1)
                            {
                                memoryProperties_raycastHitBuffer_property.intValue = EditorGUILayout.IntField(memoryProperties_raycastHitBuffer_property.intValue);
                            }
                            else
                            {
                                memoryProperties_raycastHitBuffer_property.intValue = 1;
                            }

                            change = true;
                        }
                        GUILayout.EndHorizontal();
                        /*<----------------------------------------------------------------------------------------------------------*/

                        GUI.enabled = true;

                        float raycastHitsUsed = (float)raycastHitListCount_property.intValue;
                        float raycastHitsNotUsed = (float)memoryProperties_raycastHitBuffer_property.intValue;
                        float raycastHitsUsedPercent = raycastHitsUsed / raycastHitsNotUsed;

                        if (raycastHitsUsedPercent == 1f)
                            GUI.backgroundColor = redBar;

                        if (raycastHitsUsedPercent == 1f)
                            GUI.color = Color.magenta;

                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("", GUILayout.Width(spacing));
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.ProgressBar(EditorGUILayout.BeginVertical(), raycastHitsUsedPercent, string.Format("{0} : {1}", raycastHitsUsed, raycastHitsNotUsed));
                        GUILayout.Space(16);
                        EditorGUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }
                }
            }

            EditorGUILayout.EndVertical();

            if (change)
            {
                change = false;
            }

            serializedObject.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {
            cgf = (CGF)target;

            Color mainColor;
            Color tranMainColor;

            if (cgf.Enable)
            {
                if (cgf.ForcePower == 0)
                {
                    mainColor = Color.white;
                    tranMainColor = Color.white;
                }
                else if (cgf.ForcePower > 0)
                {
                    mainColor = Color.green;
                    tranMainColor = Color.green;
                }
                else
                {
                    mainColor = Color.red;
                    tranMainColor = Color.red;
                }
            }
            else
            {
                mainColor = Color.white;
                tranMainColor = Color.white;
            }

            tranMainColor.a = .1f;

            Handles.color = mainColor;

            float gizmoSize = 0f;
            float gizmoOffset = 0f;

            if (mainColor == Color.green)
            {
                gizmoSize = (cgf.Size / 8f);
                if (gizmoSize > .5f)
                    gizmoSize = .5f;
                else if (gizmoSize < -.5f)
                    gizmoSize = -.5f;
                gizmoOffset = -gizmoSize / 1.5f;
            }
            else if (mainColor == Color.red)
            {
                gizmoSize = -(cgf.Size / 8f);
                if (gizmoSize > .5f)
                    gizmoSize = .5f;
                else if (gizmoSize < -.5f)
                    gizmoSize = -.5f;
                gizmoOffset = gizmoSize / 1.5f;
            }

            Quaternion qUp = cgf.transform.transform.rotation;
            qUp.SetLookRotation(cgf.transform.rotation * Vector3.up);
            Quaternion qDown = cgf.transform.transform.rotation;
            qDown.SetLookRotation(cgf.transform.rotation * Vector3.down);
            Quaternion qLeft = cgf.transform.transform.rotation;
            qLeft.SetLookRotation(cgf.transform.rotation * Vector3.left);
            Quaternion qRight = cgf.transform.transform.rotation;
            qRight.SetLookRotation(cgf.transform.rotation * Vector3.right);
            Quaternion qForward = cgf.transform.transform.rotation;
            qForward.SetLookRotation(cgf.transform.rotation * Vector3.forward);
            Quaternion qBack = cgf.transform.transform.rotation;
            qBack.SetLookRotation(cgf.transform.rotation * Vector3.back);

            float dotSpace = 10f;
            float sizeValue = cgf.Size;
            float capsuleRadiusValue = cgf.CapsuleRadius;

            switch (cgf._shape)
            {
                case CGF.Shape.Sphere:

                    Handles.color = mainColor;

                    if (cgf._forceType == CGF.ForceType.ExplosionForce || cgf._forceType == CGF.ForceType.ForceAtPosition || cgf._forceType == CGF.ForceType.GravitationalAttraction)
                    {
                        Handles.ConeCap(0, GetVector(Vector3.up, cgf.Size + gizmoOffset, 1f), qUp, gizmoSize);
                        Handles.ConeCap(0, GetVector(Vector3.down, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                        Handles.ConeCap(0, GetVector(Vector3.left, cgf.Size + gizmoOffset, 1f), qLeft, gizmoSize);
                        Handles.ConeCap(0, GetVector(Vector3.right, cgf.Size + gizmoOffset, 1f), qRight, gizmoSize);
                        Handles.ConeCap(0, GetVector(Vector3.back, cgf.Size + gizmoOffset, 1f), qBack, gizmoSize);
                    }
                    else if (cgf._forceType == CGF.ForceType.Torque)
                    {
                        Handles.ConeCap(0, GetVector(Vector3.up, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                        Handles.ConeCap(0, GetVector(Vector3.down, cgf.Size + gizmoOffset, 1f), qBack, gizmoSize);
                        Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                        Handles.ConeCap(0, GetVector(Vector3.back, cgf.Size + gizmoOffset, 1f), qUp, gizmoSize);
                    }
                    else
                    {
                        Handles.ConeCap(0, GetVector(Vector3.back, cgf.Size + gizmoOffset, 1f), qBack, -gizmoSize);
                    }

                    if (cgf._forceType != CGF.ForceType.Torque)
                    {
                        Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                    }

                    Handles.DrawDottedLine(GetVector(Vector3.up, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.down, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.left, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.right, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.back, cgf.Size, 1), cgf.transform.position, dotSpace);

                    Handles.CircleCap(0, cgf.transform.position, qUp, cgf.Size);
                    Handles.CircleCap(0, cgf.transform.position, qRight, cgf.Size);
                    Handles.CircleCap(0, cgf.transform.position, qForward, cgf.Size);

                    Handles.color = mainColor;
                    sizeValue = cgf.Size;
                    sizeValue = Handles.ScaleValueHandle(sizeValue, GetVector(Vector3.up, cgf.Size, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    sizeValue = Handles.ScaleValueHandle(sizeValue, GetVector(Vector3.down, cgf.Size, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    sizeValue = Handles.ScaleValueHandle(sizeValue, GetVector(Vector3.left, cgf.Size, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    sizeValue = Handles.ScaleValueHandle(sizeValue, GetVector(Vector3.right, cgf.Size, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    sizeValue = Handles.ScaleValueHandle(sizeValue, GetVector(Vector3.forward, cgf.Size, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    sizeValue = Handles.ScaleValueHandle(sizeValue, GetVector(Vector3.back, cgf.Size, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    if (sizeValue < 0)
                        cgf.Size = 0;
                    else
                        cgf.Size = sizeValue;

                    break;
                case CGF.Shape.Capsule:

                    Handles.DrawDottedLine(GetVector(Vector3.up, cgf.CapsuleRadius, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.down, cgf.CapsuleRadius, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.left, cgf.CapsuleRadius, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.right, cgf.CapsuleRadius, 1), cgf.transform.position, dotSpace);

                    Handles.DrawDottedLine(cgf.transform.position, GetVector(Vector3.forward, cgf.Size, 1), dotSpace);

                    Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1) + ((cgf.transform.rotation * Vector3.up) * cgf.CapsuleRadius), GetVector(Vector3.forward, cgf.Size, 1), dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1) + ((cgf.transform.rotation * Vector3.down) * cgf.CapsuleRadius), GetVector(Vector3.forward, cgf.Size, 1), dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1) + ((cgf.transform.rotation * Vector3.left) * cgf.CapsuleRadius), GetVector(Vector3.forward, cgf.Size, 1), dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.Size, 1) + ((cgf.transform.rotation * Vector3.right) * cgf.CapsuleRadius), GetVector(Vector3.forward, cgf.Size, 1), dotSpace);

                    if (cgf._forceType != CGF.ForceType.Torque)
                    {
                        Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                    }
                    else
                    {
                        Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                    }

                    Handles.CircleCap(0, cgf.transform.position, qForward, cgf.CapsuleRadius);
                    Handles.CircleCap(0, GetVector(Vector3.forward, cgf.Size, 1), qForward, cgf.CapsuleRadius);

                    Handles.color = mainColor;
                    sizeValue = cgf.Size;
                    capsuleRadiusValue = cgf.CapsuleRadius;
                    sizeValue = Handles.ScaleValueHandle(sizeValue, GetVector(Vector3.forward, cgf.Size, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    capsuleRadiusValue = Handles.ScaleValueHandle(capsuleRadiusValue, GetVector(Vector3.up, cgf.CapsuleRadius, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    capsuleRadiusValue = Handles.ScaleValueHandle(capsuleRadiusValue, GetVector(Vector3.down, cgf.CapsuleRadius, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    capsuleRadiusValue = Handles.ScaleValueHandle(capsuleRadiusValue, GetVector(Vector3.left, cgf.CapsuleRadius, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    capsuleRadiusValue = Handles.ScaleValueHandle(capsuleRadiusValue, GetVector(Vector3.right, cgf.CapsuleRadius, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    if (sizeValue < 0)
                        cgf.Size = 0;
                    else
                        cgf.Size = sizeValue;
                    if (capsuleRadiusValue < 0)
                        cgf.CapsuleRadius = 0;
                    else
                        cgf.CapsuleRadius = capsuleRadiusValue;

                    break;
                case CGF.Shape.Raycast:

                    Handles.DrawDottedLine(cgf.transform.position + ((cgf.transform.rotation * Vector3.forward) * cgf.Size), cgf.transform.position, dotSpace);

                    if (cgf._forceType != CGF.ForceType.Torque)
                    {
                        Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qForward, gizmoSize);
                    }
                    else
                    {
                        Handles.ConeCap(0, GetVector(Vector3.forward, cgf.Size + gizmoOffset, 1f), qDown, gizmoSize);
                    }

                    Handles.color = mainColor;
                    sizeValue = cgf.Size;
                    sizeValue = Handles.ScaleValueHandle(sizeValue, GetVector(Vector3.forward, cgf.Size, 1f), cgf.transform.rotation, gizmoSize, Handles.DotCap, .25f);
                    if (sizeValue < 0)
                        cgf.Size = 0;
                    else
                        cgf.Size = sizeValue;

                    break;
                case CGF.Shape.Box:

                    if (cgf._forceType == CGF.ForceType.ExplosionForce || cgf._forceType == CGF.ForceType.ForceAtPosition || cgf._forceType == CGF.ForceType.GravitationalAttraction)
                    {
                        Handles.ConeCap(0, GetVector(Vector3.up, GetArrowOffsetForBox(mainColor, cgf.BoxSize.y, gizmoSize), 1f), qUp, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.y));
                        Handles.ConeCap(0, GetVector(Vector3.down, GetArrowOffsetForBox(mainColor, cgf.BoxSize.y, gizmoSize), 1f), qDown, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.y));
                        Handles.ConeCap(0, GetVector(Vector3.left, GetArrowOffsetForBox(mainColor, cgf.BoxSize.x, gizmoSize), 1f), qLeft, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.x));
                        Handles.ConeCap(0, GetVector(Vector3.right, GetArrowOffsetForBox(mainColor, cgf.BoxSize.x, gizmoSize), 1f), qRight, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.x));
                        Handles.ConeCap(0, GetVector(Vector3.back, GetArrowOffsetForBox(mainColor, cgf.BoxSize.z, gizmoSize), 1f), qBack, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.z));
                    }
                    else if (cgf._forceType == CGF.ForceType.Torque)
                    {
                        Handles.DrawDottedLine(GetVector(Vector3.up, cgf.BoxSize.y, 1), cgf.transform.position, dotSpace);
                        Handles.DrawDottedLine(GetVector(Vector3.down, cgf.BoxSize.y, 1), cgf.transform.position, dotSpace);

                        Handles.ConeCap(0, GetVector(Vector3.up, GetArrowOffsetForBox(mainColor, cgf.BoxSize.y, gizmoSize), 1f), qForward, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.y));
                        Handles.ConeCap(0, GetVector(Vector3.down, GetArrowOffsetForBox(mainColor, cgf.BoxSize.y, gizmoSize), 1f), qBack, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.y));
                        Handles.ConeCap(0, GetVector(Vector3.forward, GetArrowOffsetForBox(mainColor, cgf.BoxSize.z, gizmoSize), 1f), qDown, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.z));
                        Handles.ConeCap(0, GetVector(Vector3.back, GetArrowOffsetForBox(mainColor, cgf.BoxSize.z, gizmoSize), 1f), qUp, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.z));
                    }
                    else
                    {
                        Handles.ConeCap(0, GetVector(Vector3.back, GetArrowOffsetForBox(mainColor, cgf.BoxSize.z, gizmoSize), 1f), qBack, -GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.z));
                    }

                    Handles.DrawDottedLine(GetVector(Vector3.forward, cgf.BoxSize.z, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.back, cgf.BoxSize.z, 1), cgf.transform.position, dotSpace);

                    if (cgf._forceType != CGF.ForceType.Torque)
                    {
                        Handles.ConeCap(0, GetVector(Vector3.forward, GetArrowOffsetForBox(mainColor, cgf.BoxSize.z, gizmoSize), 1f), qForward, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.z));
                    }

                    Handles.DrawDottedLine(GetVector(Vector3.up, cgf.BoxSize.y, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.down, cgf.BoxSize.y, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.left, cgf.BoxSize.x, 1), cgf.transform.position, dotSpace);
                    Handles.DrawDottedLine(GetVector(Vector3.right, cgf.BoxSize.x, 1), cgf.transform.position, dotSpace);

                    Handles.color = mainColor;
                    float sizeXValue = cgf.BoxSize.x;
                    float sizeYValue = cgf.BoxSize.y;
                    float sizeZValue = cgf.BoxSize.z;
                    sizeXValue = Handles.ScaleValueHandle(sizeXValue, GetVector(Vector3.left, cgf.BoxSize.x, 1f), cgf.transform.rotation, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.x), Handles.DotCap, .25f);
                    sizeXValue = Handles.ScaleValueHandle(sizeXValue, GetVector(Vector3.right, cgf.BoxSize.x, 1f), cgf.transform.rotation, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.x), Handles.DotCap, .25f);
                    sizeYValue = Handles.ScaleValueHandle(sizeYValue, GetVector(Vector3.up, cgf.BoxSize.y, 1f), cgf.transform.rotation, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.y), Handles.DotCap, .25f);
                    sizeYValue = Handles.ScaleValueHandle(sizeYValue, GetVector(Vector3.down, cgf.BoxSize.y, 1f), cgf.transform.rotation, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.y), Handles.DotCap, .25f);
                    sizeZValue = Handles.ScaleValueHandle(sizeZValue, GetVector(Vector3.forward, cgf.BoxSize.z, 1f), cgf.transform.rotation, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.z), Handles.DotCap, .25f);
                    sizeZValue = Handles.ScaleValueHandle(sizeZValue, GetVector(Vector3.back, cgf.BoxSize.z, 1f), cgf.transform.rotation, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.z), Handles.DotCap, .25f);
                    if (sizeXValue < 0)
                        cgf.BoxSize = new Vector3(0f, cgf.BoxSize.y, cgf.BoxSize.z);
                    else
                        cgf.BoxSize = new Vector3(sizeXValue, cgf.BoxSize.y, cgf.BoxSize.z);
                    if (sizeYValue < 0)
                        cgf.BoxSize = new Vector3(cgf.BoxSize.x, 0f, cgf.BoxSize.z);
                    else
                        cgf.BoxSize = new Vector3(cgf.BoxSize.x, sizeYValue, cgf.BoxSize.z);
                    if (sizeZValue < 0)
                        cgf.BoxSize = new Vector3(cgf.BoxSize.x, cgf.BoxSize.y, 0f);
                    else
                        cgf.BoxSize = new Vector3(cgf.BoxSize.x, cgf.BoxSize.y, sizeZValue);
                    break;

            }

            if (cgf._shape != CGF.Shape.Box)
            {
                Handles.SphereCap(0, cgf.transform.position, cgf.transform.rotation, gizmoSize / 2f);
            }
            else if (cgf._shape == CGF.Shape.Box)
            {
                Handles.SphereCap(0, cgf.transform.position, cgf.transform.rotation, GetGismoSizeForBox(mainColor, gizmoSize, cgf.BoxSize.magnitude) / 2f);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        Vector3 GetVector(Vector3 vector, float size, float times)
        {
            return cgf.transform.position + (((cgf.transform.rotation * vector) * size) / times);
        }

        public static float GetGismoSizeForBox(Color color, float arrowSize, float cgfSize)
        {
            if (color == Color.green)
            {
                arrowSize = (cgfSize / 8f);
                if (arrowSize > .5f)
                    arrowSize = .5f;
                else if (arrowSize < -.5f)
                    arrowSize = -.5f;
            }
            else if (color == Color.red)
            {
                arrowSize = -(cgfSize / 8f);
                if (arrowSize > .5f)
                    arrowSize = .5f;
                else if (arrowSize < -.5f)
                    arrowSize = -.5f;
            }

            return arrowSize;
        }

        public static float GetArrowOffsetForBox(Color color, float boxSize, float gizmoSize)
        {
            float arrowSize = GetGismoSizeForBox(color, gizmoSize, boxSize);

            float offset = 0;

            if (color == Color.green)
            {
                offset = boxSize + (-arrowSize / 1.5f);
            }
            else if (color == Color.red)
            {
                offset = boxSize + (arrowSize / 1.5f);
            }

            return offset;
        }
    }
}