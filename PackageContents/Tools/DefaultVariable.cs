/***************************************************
File:           DefaultVariable.cs
Authors:        Kevin Jacobson
Last Updated:   4/24/2021

Description:
  Assigns components a default value if the correct
    component type exists on the object.

Copyright 2019-2022, Kevin Jacobson
***************************************************/
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine
{
    public class DefaultVariable : PropertyAttribute
    {
        public bool m_SelfOnly = false;

        public string DefaultCheck
        {
            set
            {
                throw new NotImplementedException();
            }
        }
        /**public static readonly char m_KeyValueSplit = '=';
        public static readonly char m_ValueSplit = ':';
        public static readonly char m_CheckSplit = ',';*/
        
        public Type m_VariableType;

#if UNITY_EDITOR
        public Action<GameObject, SerializedProperty> CheckAction;
#endif

        public static readonly Type[] m_SupportedTypes =
        {
            typeof(Sprite),
            typeof(Mesh)
            //typeof(Gradient) //Still working on a truly generic way to do Gradients
        };

        public DefaultVariable(Type type = null) 
        {
            if(type != null)
            {
                if(!Array.Exists(m_SupportedTypes, x => x == type))
                {
                    //TODO - Use https://stackoverflow.com/a/154296 for a proper warning system later
                    Debug.LogError("Attempting to use unsupported type '" + type.Name + "' as a default variable");
                }
                else
                {
                    /**if(type == typeof(Gradient) && m_DefaultCheck == "")
                    {
                        //Use https://stackoverflow.com/a/154296 for a proper warning system later
                        Debug.LogError("Attempting to use type 'Gradient' as a default variable without providing a default check");
                    }
                    else
                    {
                        CheckAction = (gameObject, property) =>
                        {
                            //DefaultCheck format:
                            //key=0:alpha=0:color=0|0|0

                            try
                            {
                                Gradient grad = property.Value<Gradient>();

                                string[] checkSplit = m_DefaultCheck.Split(m_CheckSplit);

                                foreach (string split in checkSplit)
                                {
                                    string[] values = split.Split(m_ValueSplit);
                                }
                            }
                            catch
                            {

                            }
                            
                        };
                    }*/
                }

                m_VariableType = type;
            }
        }
    }
}

#if UNITY_EDITOR

namespace UnityEditor
{
    [CustomPropertyDrawer(typeof(DefaultVariable))]
    public class DefaultVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DefaultVariable varAtt = attribute as DefaultVariable;

            EditorGUI.PropertyField(position, property, label);

            GameObject gameObject = ((Component)property.serializedObject.targetObject).gameObject;

            try
            {
                //If the variable has a type that requires
                // custom default handling
                if (varAtt.m_VariableType != null)
                {
                    switch (varAtt.m_VariableType)
                    {
                        //Visual Studio was complaining about non-constant type
                        case var _ when varAtt.m_VariableType == typeof(Sprite):
                            if (!property.objectReferenceValue)
                            {
                                property.objectReferenceValue = gameObject.GetComponentInParentOrChild<SpriteRenderer>().sprite;

                                Debug.Log("Property of type 'Sprite' on " + gameObject.name + " assigned a default value");
                            }
                            break;
                        case var _ when varAtt.m_VariableType == typeof(Mesh):
                            if(!property.objectReferenceValue)
                            {
                                property.objectReferenceValue = gameObject.GetComponentInParentOrChild<MeshFilter>().sharedMesh;

                                Debug.Log("Property of type 'Mesh' on " + gameObject.name + " assigned a default value");
                            }
                            break;
                        /*case var _ when varAtt.m_VariableType == typeof(Gradient):
                            varAtt.CheckAction(gameObject, property);

                            Debug.Log("Property of type 'Gradient' on " + gameObject.name + " assigned a default value");
                            break;*/
                        default:
                            Debug.LogWarning("Attempting to use unsupported type '" + varAtt.m_VariableType.Name + "' as a default variable");
                            break;
                    }
                }
                //If the property is another type of object reference
                //This can throw errors for non-component types that
                //  are still object references
                else if (property.propertyType == SerializedPropertyType.ObjectReference)
                {
                    if (!property.objectReferenceValue)
                    {
                        //property.type returns the format
                        //      PPtr<&type>
                        //These substrings remove the excess,
                        // leaving only
                        //      type
                        string type = property.type;
                        type = type.Substring(type.IndexOf("<") + 2);
                        type = type.Substring(0, type.Length - 1);

                        //Debug.Log("Object: " + gameObject.name);

                        if (varAtt.m_SelfOnly)
                        {
                            property.objectReferenceValue = gameObject.GetComponent(type);
                        }
                        else
                        {
                            /*Debug.Log("Type: " + type);
                            Debug.Log("Found: " + gameObject.GetComponentInParentOrChild(type));*/
                            property.objectReferenceValue = gameObject.GetComponentInParentOrChild(type);
                        }

                        //A default property was not found
                        if (!property.objectReferenceValue)
                        {
                            Debug.LogWarning("Failed to assign property '" + property.name 
                                                           + "' of type '" + type 
                                                           + "' on " + gameObject.name 
                                                           + " a default value");
                        }
                        else
                        {
                            Debug.Log("Property '" + property.name + "' of type '" + type + "' on "
                                                   + gameObject.name + " assigned a default value");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to assign property '" + property.name + "' on " 
                                          + gameObject.name  + " a default value\n"
                                          + e.Message + "\n" + e.StackTrace);
            }
        }
    }
}

#endif