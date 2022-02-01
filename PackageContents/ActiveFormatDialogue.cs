/*******************************************************************************
File: ActiveFormatDialogue.cs
Author: Kevin Jacobson
DP Email: kevin.j@digipen.edu
Date: 1/18/2022
Last Updated: 2/1/2022

Description:
    Editor utility to interface with importing a CSV using the Active Format for dialogue
*******************************************************************************/
using UnityEngine;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Dialogue/CSVImport")]
public class ActiveFormatDialogue : ScriptableObject
{
    [SerializeField][HideInInspector]
    private string m_LatestPath;

    public string GetAssetPath() => m_LatestPath;

    public static readonly string[] g_InternalDataTitles = { "m_LineID", "m_Story", "m_Section", "m_Character", "m_Line", "m_VoiceDirection", "m_Triggered", "m_Context" };
    public static readonly string[] g_ColumnTitles =       { "Line ID",  "Story",   "Section",   "Character",   "Line",   "Voice Direction",  "Triggered",   "Context" };

    [Serializable]
    public class DialogueLine
    {
        public int m_LineID;
        public string m_Story;
        public string m_Section;
        public string m_Character;
        public string m_Line;
        public string m_VoiceDirection;
        public string m_Triggered;
        public string m_Context;

        [HideInInspector]
        public string m_Label;

        //There's probaby a cleaner way to do this
        //    but this is simple and functional
        public void SetValue(string value, string column)
        {
            //Debug.Log("Value:" + value + " || col: " + column);

            switch (column)
            {
                case var _ when column == g_ColumnTitles[0]:
                    m_LineID = int.Parse(value);
                    m_Label = value;
                    break;
                case var _ when column == g_ColumnTitles[1]:
                    m_Story = value;
                    break;
                case var _ when column == g_ColumnTitles[2]:
                    m_Section = value;
                    break;
                case var _ when column == g_ColumnTitles[3]:
                    m_Character = value;
                    break;
                case var _ when column == g_ColumnTitles[4]:
                    m_Line = value;
                    break;
                case var _ when column == g_ColumnTitles[5]:
                    m_VoiceDirection = value;
                    break;
                case var _ when column == g_ColumnTitles[6]:
                    m_Triggered = value;
                    break;
                case var _ when column == g_ColumnTitles[7]:
                    m_Context = value;
                    break;
            }
        }

#if UNITY_EDITOR
        public static void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect tempPos = position;
            tempPos.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty lineProp = property.FindPropertyRelative("m_LineData");
            SerializedProperty dialogueHeight = property.FindPropertyRelative("m_DialogueHeight");
            SerializedProperty voiceHeight = property.FindPropertyRelative("m_VoiceHeight");
            SerializedProperty triggeredHeight = property.FindPropertyRelative("m_TriggeredHeight");
            SerializedProperty contextHeight = property.FindPropertyRelative("m_ContextHeight");

            int[] heights = { dialogueHeight.intValue, voiceHeight.intValue, triggeredHeight.intValue, contextHeight.intValue };

            SerializedProperty temp = lineProp.FindPropertyRelative("m_Line");

            EditorGUI.TextField(tempPos, "Line Preview", temp.stringValue);

            //Reset position size
            tempPos.y += tempPos.height;

            //Draw data foldout
            lineProp.isExpanded = EditorGUI.Foldout(tempPos, lineProp.isExpanded, label);

            if(lineProp.isExpanded)
            {
                tempPos = new Rect(tempPos.x + 20, tempPos.y, tempPos.width - 20, tempPos.height);

                //Draw the ID field
                tempPos.y += EditorGUIUtility.singleLineHeight;
                temp = lineProp.FindPropertyRelative(g_InternalDataTitles[0]);
                EditorGUI.IntField(tempPos, new GUIContent(g_ColumnTitles[0]), temp.intValue);

                //Draw the various text fields
                for (int i = 1; i < 4; i++)
                {
                    tempPos.y += EditorGUIUtility.singleLineHeight;
                    temp = lineProp.FindPropertyRelative(g_InternalDataTitles[i]);
                    EditorGUI.TextField(tempPos, new GUIContent(g_ColumnTitles[i]), temp.stringValue);
                }

                tempPos.y += EditorGUIUtility.singleLineHeight;

                //Dynamic text boxes
                for(int i = 4; i < g_InternalDataTitles.Length; i++)
                {
                    temp = lineProp.FindPropertyRelative(g_InternalDataTitles[i]);
                    tempPos = DynamicText(tempPos, g_ColumnTitles[i], temp.stringValue, heights[i - 4]);
                }
            }
        }

        private static Rect DynamicText(Rect position, string label, string text, int height)
        {
            position.height = EditorGUIUtility.singleLineHeight * height;
            EditorGUI.TextField(position, label, text);
            position.y += EditorGUIUtility.singleLineHeight * height;
            return position;
        }
#endif
    }

    //Editor-visible array of all lines and data
    [SerializeField]
    private DialogueLine[] m_Lines;

    //Internal array of line IDs
    [SerializeField][HideInInspector]
    private string[] m_LineKeys;

    [SerializeField][HideInInspector]
    private int[] m_LineKeysInt;

    //Access the array of line IDs--used to create dropdown menus
    public string[] GetLineKeys()
    {
        return m_LineKeys;
    }

    public int[] GetLineKeysInt()
    {
        return m_LineKeysInt;
    }

    ///<summary>Retrieve the data for a given line ID</summary>
    ///<returns>May return null if the dialogue asset hasn't initialized</returns>
    public DialogueLine GetLine(int ID)
    {
        if(m_Lines != null && m_LineKeysInt != null)
        {
            if(m_LineKeysInt.IndexOf(ID) != -1)
            {
                return m_Lines[m_LineKeysInt.IndexOf(ID)];
            }
        }

        return null;
    }

    public void StoreLines(DialogueLine[] lines, string path)
    {
        //Update the latest path
        m_LatestPath = path;

        //Update the stored array of lines
        m_Lines = lines;

        //Reset the array of ID keys
        m_LineKeys = new string[lines.Length];
        m_LineKeysInt = new int[lines.Length];

        //Go through each line
        for (int i = 0; i < m_Lines.Length; i++)
        {
            DialogueLine line = m_Lines[i];

            if(line != null)
            {
                //Store the line ID into the enumeration
                m_LineKeys[i] = line.m_LineID.ToString();
                m_LineKeysInt[i] = line.m_LineID;
            }
        }
    }

#if UNITY_EDITOR
    [Obsolete("Manual refreshing should not be needed")]
    public void RefreshLines()
    {
        DialogueLine[] lines = ActiveFormatDialoguePostprocessor.Deserialize(GetAssetPath());
        if(lines.Length == 0)
        {
            Debug.Log("Nothing at path: " + GetAssetPath());

            string newPath = GetAssetPath().Replace(".asset", ".csv");

            lines = ActiveFormatDialoguePostprocessor.Deserialize(newPath);

            if(lines.Length != 0)
            {
                Debug.Log("Found with path replace: " + newPath);

                m_LatestPath = newPath;
            }
            else
            {
                Debug.Log("Nothing at path replace: " + newPath);
            }
        }
        else
        {
            Debug.Log("Found at path: " + GetAssetPath());
        }

        if(lines.Length != 0)
        {
            StoreLines(lines, GetAssetPath());
            Debug.Log("Updating lines");
        }
        else
        {
            Debug.Log("No lines found");
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(ActiveFormatDialogue))]
public class ActiveFormatDialogueEditor : Editor
{
    SerializedProperty sp_Lines;

    private void OnEnable()
    {
        sp_Lines = serializedObject.FindProperty("m_Lines");
    }

    public override void OnInspectorGUI()
    {
        ActiveFormatDialogue owner = (ActiveFormatDialogue)serializedObject.targetObject;

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Script");
        EditorGUILayout.ObjectField(owner, typeof(ActiveFormatDialogue), false);
        GUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(sp_Lines, true);
    }
}

#endif

#if UNITY_EDITOR
public class ActiveFormatDialoguePostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string assetPath in importedAssets)
        {
            //Check for the naming convention
            if (assetPath.IndexOf("AF.csv") != -1)
            {
                //Load the CSV as text
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);

                //Store the path for the new asset
                string textPath = assetPath.Replace(".csv", ".asset");

                //Attempt to load the asset, if it already exists
                ActiveFormatDialogue dialogueAsset = AssetDatabase.LoadAssetAtPath<ActiveFormatDialogue>(textPath);

                //If it doesn't exist,
                if (dialogueAsset == null)
                {
                    //Create the dialogue asset
                    dialogueAsset = ScriptableObject.CreateInstance<ActiveFormatDialogue>();
                    AssetDatabase.CreateAsset(dialogueAsset, textPath);
                }

                //Retrieve the dialogue lines from the text
                dialogueAsset.StoreLines(Deserialize(data), textPath);

                //Prompt the editor to save the asset
                EditorUtility.SetDirty(dialogueAsset);
                AssetDatabase.SaveAssets();
#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + assetPath);
#endif
            }
        }
    }

    public static ActiveFormatDialogue.DialogueLine[] Deserialize(string path)
    {
        TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

        if(asset)
        {
            return Deserialize(asset);
        }

        return new ActiveFormatDialogue.DialogueLine[0];
    }

    private static ActiveFormatDialogue.DialogueLine[] Deserialize(TextAsset data)
    {
        //This requires the CSVSerializer from the Unity Asset Store
        List<string[]> text = CSVSerializer.ParseCSV(data.text);

        ActiveFormatDialogue.DialogueLine[] lines = new ActiveFormatDialogue.DialogueLine[text.Count - 1];

        //the last non-empty index
        int lastIndex = 0;

        for(int row = 1; row < text.Count; row++)
        {
            if(text[row][0] == "")
            {
                continue;
            }

            lastIndex = row;

            lines[row - 1] = CreateDialogueLine(text[row]);
        }

        ActiveFormatDialogue.DialogueLine[] toRet = new ActiveFormatDialogue.DialogueLine[lastIndex];
        Array.ConstrainedCopy(lines, 0, toRet, 0, lastIndex);

        return toRet;
    }

    private static ActiveFormatDialogue.DialogueLine CreateDialogueLine(string[] row)
    {
        ActiveFormatDialogue.DialogueLine toRet = new ActiveFormatDialogue.DialogueLine();

        //Debug.Log("Row length: " + row.Length);
        string val;

        for(int col = 0; col < ActiveFormatDialogue.g_ColumnTitles.Length; col++)
        {
            try { val = row[col]; }
            catch { val = ""; }
            toRet.SetValue(val, ActiveFormatDialogue.g_ColumnTitles[col]);
        }

        return toRet;
    }
}
#endif