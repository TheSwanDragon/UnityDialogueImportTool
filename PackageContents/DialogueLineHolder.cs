/*******************************************************************************
File: DialogueLineHolder.cs
Author: Kevin Jacobson
DP Email: kevin.j@digipen.edu
Date: 1/18/2022

Description:
    Editor utility script to assign a line of dialogue to a text mesh

    Also contains a property type usable in any Behavior (DialogueLineSelector)
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class DialogueLineHolder : MonoBehaviour, ISerializationCallbackReceiver
{
    [HideInInspector]
    public bool m_UpdateTextUI;

    public DialogueLineSelector m_DialogueLine;

    [DefaultVariable]
    public TextMeshProUGUI m_Text;

    private void Update()
    {
        if(m_UpdateTextUI)
        {
            m_UpdateTextUI = false;
            UpdateLine();
        }
    }

    public void UpdateLine()
    {
        if(m_Text != null && m_DialogueLine != null)
        {
            if(m_DialogueLine.GetLine() != null)
            {
                m_Text.text = m_DialogueLine.GetLine();
            }
        }
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        m_UpdateTextUI = true;
    }
}

[System.Serializable]
public class DialogueLineSelector : ISerializationCallbackReceiver
{
    [Tooltip("Which imported CSV to retrieve lines from")]
    public ActiveFormatDialogue m_DialoguePack;
    private static readonly string s_DialoguePack = "m_DialoguePack";

    [SerializeField]
    private int m_LineID;
    public static readonly string s_LineID = "m_LineID";
    
    [SerializeField]
    private int m_LineIDIndex;
    private static readonly string s_LineIDIndex = "m_LineIDIndex";

    public ActiveFormatDialogue.DialogueLine m_LineData;
    private static readonly string s_LineData = "m_LineData";

    public string GetLine()
    {
        if (m_LineData != null)
        {
            return m_LineData.m_Line;
        }

        return "";
    }

    [SerializeField]
    private int m_DialogueHeight;

    [SerializeField]
    private int m_VoiceHeight;

    [SerializeField]
    private int m_TriggeredHeight;

    [SerializeField]
    private int m_ContextHeight;

    [SerializeField]
    private int m_TotalHeight;
    private static readonly string s_TotalHeight = "m_TotalHeight";

    public void OnBeforeSerialize()
    {
        //throw new System.NotImplementedException();
    }

    [SerializeField]
    private int LineWidth = 30;
    private static readonly string s_LineWidth = "LineWidth";

    public void OnAfterDeserialize()
    {
        if(m_DialoguePack)
        {
            m_LineData = m_DialoguePack.GetLine(m_LineID);

            if(m_LineData != null)
            {
                m_TotalHeight = m_DialogueHeight = 1 + m_LineData.m_Line.Length / LineWidth;
                m_TotalHeight += m_VoiceHeight = 1 + m_LineData.m_VoiceDirection.Length / LineWidth;
                m_TotalHeight += m_TriggeredHeight = 1 + m_LineData.m_Triggered.Length / LineWidth;
                m_TotalHeight += m_ContextHeight = 1 + m_LineData.m_Context.Length / LineWidth;
            }
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(DialogueLineSelector))]
    public class DialogueSelectorHolder : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.skin.textField.wordWrap = true;

            SerializedProperty sp_DialoguePack;
            ActiveFormatDialogue obj_DialoguePack;

            SerializedProperty sp_Index;
            SerializedProperty sp_LineID;

            //Allows position to be safely edited
            Rect tempPos = position;
            tempPos.height = EditorGUIUtility.singleLineHeight;

            property.isExpanded = EditorGUI.Foldout(tempPos, property.isExpanded, label);

            if(!property.isExpanded)
            {
                return;
            }

            tempPos.y += EditorGUIUtility.singleLineHeight;

            //Updates the stored information for the dialogue pack
            sp_DialoguePack = property.FindPropertyRelative(s_DialoguePack);
            obj_DialoguePack = (ActiveFormatDialogue)sp_DialoguePack.objectReferenceValue;

            //Ensures the dialogue pack exists
            if(sp_DialoguePack.objectReferenceValue == null)
            {
                string[] guid = AssetDatabase.FindAssets("t:ActiveFormatDialogue");
                if(guid.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid[0]);

                    obj_DialoguePack = AssetDatabase.LoadAssetAtPath<ActiveFormatDialogue>(path);
                }

                //If no pack is found, display an error and prevent operations
                if(obj_DialoguePack == null)
                {
                    EditorGUI.ObjectField(tempPos, sp_DialoguePack);
                    tempPos.y += EditorGUIUtility.singleLineHeight;
                    EditorGUI.LabelField(tempPos, "Error: No ActiveFormatDialogue asset found");
                    Debug.LogError("MissingObjectException: No ActiveFormatDialogue asset found");
                    return;
                }

                sp_DialoguePack.objectReferenceValue = obj_DialoguePack;
            }

            //Display a property field for the pack. The user can edit this if they want a different pack
            EditorGUI.ObjectField(tempPos, sp_DialoguePack);

            //Debug.Log(EditorGUIUtility.currentViewWidth);
            //Update the text line width to match the editor view width
            property.FindPropertyRelative(s_LineWidth).intValue = (int)EditorGUIUtility.currentViewWidth / 12;
            //divide by 12 (by 2, by 6) --> 367 => 180 => 30 (current & accurate)

            //Increment the position
            tempPos.y += EditorGUIUtility.singleLineHeight;

            //Display the dialogue line selection popup
            sp_Index = property.FindPropertyRelative(s_LineIDIndex);
            sp_Index.intValue = EditorGUI.Popup(tempPos, sp_Index.displayName, sp_Index.intValue, obj_DialoguePack.GetLineKeys());

            //Update the stored ID value
            sp_LineID = property.FindPropertyRelative(s_LineID);
            sp_LineID.intValue = obj_DialoguePack.GetLineKeysInt()[sp_Index.intValue];

            //Increment the position
            tempPos.y += EditorGUIUtility.singleLineHeight;

            //Draw the dialogue data
            ActiveFormatDialogue.DialogueLine.OnGUI(tempPos, property, new GUIContent("Line Data"));
        }

        private const int CollapsedHeight = 5;
        private const int ExpandedHeight = CollapsedHeight + 4;
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(property.FindPropertyRelative(s_LineData).isExpanded)
            {
                return EditorGUIUtility.singleLineHeight * (ExpandedHeight + property.FindPropertyRelative(s_TotalHeight).intValue);
            }
            else if(property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight * CollapsedHeight;
            }

            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}