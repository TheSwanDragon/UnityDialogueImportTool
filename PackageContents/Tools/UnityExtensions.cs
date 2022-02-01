/***************************************************
File:           UnityExtensions.cs
Authors:        Kevin Jacobson
Last Updated:   2/16/2021

Description:
  A collection of utility functions for scripting
  A collection of Unity Extensions

Copyright 2019-2022, Kevin Jacobson
***************************************************/
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public static class UnityExtensions
{
    #region COMPONENT EXTENSIONS

    //Generics
    /// <summary>
    /// Returns the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns></returns>
    public static T GetComponentInParentOrChild<T>(this Component self, int priority = 0) where T : Component
    {
        T toReturn = null;

        switch (priority)
        {
            case 0:
                if (!self.TryGetComponent(out toReturn))
                {
                    if (!self.TryGetComponentInParent(out toReturn))
                    {
                        self.TryGetComponentInChildren(out toReturn);
                    }
                }
                break;
            case 1:
                if (!self.TryGetComponentInParent(out toReturn))
                {
                    if (!self.TryGetComponent(out toReturn))
                    {
                        self.TryGetComponentInChildren(out toReturn);
                    }
                }
                break;
            case 2:
                if (!self.TryGetComponentInChildren(out toReturn))
                {
                    if (!self.TryGetComponent(out toReturn))
                    {
                        self.TryGetComponentInParent(out toReturn);
                    }
                }
                break;
        }

        return toReturn;
    }

    /// <summary>
    /// This actually retrieves from the child, unlike GetComponentFromChildren
    /// </summary>
    /// <returns></returns>
    public static T GetComponentInChild<T>(this Component self) where T : Component
    {
        return (T)self.GetComponentInChildren(typeof(T).Name);
    }

    public static bool TryGetComponentInParent<T>(this Component self, out T component) where T : Component
    {
        component = self.GetComponentInParent<T>();

        return component != null;
    }

    public static bool TryGetComponentInChildren<T>(this Component self, out T component) where T : Component
    {
        component = self.GetComponentInChild<T>();

        //Debug.Log("Type name: " + typeof(T).Name);

        return component != null;
    }

    /// <summary>
    /// Outputs the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns>If a component was found</returns>
    public static bool TryGetComponentInParentOrChild<T>(this Component self, out T component, int priority = 0) where T : Component
    {
        component = self.GetComponentInParentOrChild<T>(priority);

        return component != null;
    }

    //String-Typed
    /// <summary>
    /// Returns the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns></returns>
    public static Component GetComponentInParentOrChild(this Component self, string type, int priority = 0)
    {
        Component toReturn = null;

        switch (priority)
        {
            case 0:
                if (!self.TryGetComponent(type, out toReturn))
                {
                    if (!self.TryGetComponentInParent(type, out toReturn))
                    {
                        self.TryGetComponentInChildren(type, out toReturn);
                    }
                }
                break;
            case 1:
                if (!self.TryGetComponentInParent(type, out toReturn))
                {
                    if (!self.TryGetComponent(type, out toReturn))
                    {
                        self.TryGetComponentInChildren(type, out toReturn);
                    }
                }
                break;
            case 2:
                if (!self.TryGetComponentInChildren(type, out toReturn))
                {
                    if (!self.TryGetComponent(type, out toReturn))
                    {
                        self.TryGetComponentInParent(type, out toReturn);
                    }
                }
                break;
        }

        return toReturn;
    }

    public static bool TryGetComponentInParent(this Component self, string type, out Component component)
    {
        component = self.GetComponentInParent(type);

        return component != null;
    }

    public static bool TryGetComponentInChildren(this Component self, string type, out Component component)
    {
        component = self.GetComponentInChildren(type);

        return component != null;
    }

    /// <summary>
    /// Outputs the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns>If a component was found</returns>
    public static bool TryGetComponentInParentOrChild(this Component self, string type, out Component component, int priority = 0)
    {
        component = self.GetComponentInParentOrChild(type, priority);

        return component != null;
    }

    public static bool TryGetComponent(this Component self, string type, out Component component)
    {
        component = self.GetComponent(type);

        return component != null;
    }

    public static Component GetComponentInParent(this Component self, string type)
    {
        try
        {
            return self.transform.parent.GetComponent(type);
        }
        catch
        {
            return null;
        }
    }

    public static Component GetComponentInChildren(this Component self, string type)
    {
        Transform[] children = self.transform.GetChildren();

        Component toRet;

        foreach (Transform child in children)
        {
            if (toRet = child.GetComponent(type))
            {
                return toRet;
            }
        }

        return null;
    }
    #endregion

    #region GAMEOBJECT EXTENSIONS

    //Generics
    /// <summary>
    /// Returns the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns></returns>
    public static T GetComponentInParentOrChild<T>(this GameObject self, int priority = 0) where T : Component
    {
        T toReturn = null;

        switch (priority)
        {
            case 0:
                if (!self.TryGetComponent(out toReturn))
                {
                    if (!self.TryGetComponentInParent(out toReturn))
                    {
                        self.TryGetComponentInChildren(out toReturn);
                    }
                }
                break;
            case 1:
                if (!self.TryGetComponentInParent(out toReturn))
                {
                    if (!self.TryGetComponent(out toReturn))
                    {
                        self.TryGetComponentInChildren(out toReturn);
                    }
                }
                break;
            case 2:
                if (!self.TryGetComponentInChildren(out toReturn))
                {
                    if (!self.TryGetComponent(out toReturn))
                    {
                        self.TryGetComponentInParent(out toReturn);
                    }
                }
                break;
        }

        return toReturn;
    }

    /// <summary>
    /// This actually retrieves from the child, unlike GetComponentFromChildren
    /// </summary>
    /// <returns></returns>
    public static T GetComponentInChild<T>(this GameObject self) where T : Component
    {
        return (T)self.GetComponentInChildren(typeof(T).Name);
    }

    public static bool TryGetComponentInParent<T>(this GameObject self, out T component) where T : Component
    {
        component = self.GetComponentInParent<T>();

        return component != null;
    }

    public static bool TryGetComponentInChildren<T>(this GameObject self, out T component) where T : Component
    {
        component = self.GetComponentInChild<T>();

        return component != null;
    }

    /// <summary>
    /// Outputs the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns>If a component was found</returns>
    public static bool TryGetComponentInParentOrChild<T>(this GameObject self, out T component, int priority = 0) where T : Component
    {
        component = self.GetComponentInParentOrChild<T>(priority);

        return component != null;
    }

    //String-Typed
    /// <summary>
    /// Returns the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns></returns>
    public static Component GetComponentInParentOrChild(this GameObject self, string type, int priority = 0)
    {
        Component toReturn = null;

        switch (priority)
        {
            case 0:
                if (!self.TryGetComponent(type, out toReturn))
                {
                    if (!self.TryGetComponentInParent(type, out toReturn))
                    {
                        self.TryGetComponentInChildren(type, out toReturn);
                    }
                }
                break;
            case 1:
                if (!self.TryGetComponentInParent(type, out toReturn))
                {
                    if (!self.TryGetComponent(type, out toReturn))
                    {
                        self.TryGetComponentInChildren(type, out toReturn);
                    }
                }
                break;
            case 2:
                if (!self.TryGetComponentInChildren(type, out toReturn))
                {
                    if (!self.TryGetComponent(type, out toReturn))
                    {
                        self.TryGetComponentInParent(type, out toReturn);
                    }
                }
                break;
        }

        return toReturn;
    }

    public static bool TryGetComponentInParent(this GameObject self, string type, out Component component)
    {
        component = self.GetComponentInParent(type);

        return component != null;
    }

    public static bool TryGetComponentInChildren(this GameObject self, string type, out Component component)
    {
        component = self.GetComponentInChildren(type);

        return component != null;
    }

    /// <summary>
    /// Outputs the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns>If a component was found</returns>
    public static bool TryGetComponentInParentOrChild(this GameObject self, string type, out Component component, int priority = 0)
    {
        component = self.GetComponentInParentOrChild(type, priority);

        return component != null;
    }

    public static bool TryGetComponent(this GameObject self, string type, out Component component)
    {
        component = self.GetComponent(type);

        return component != null;
    }

    public static Component GetComponentInParent(this GameObject self, string type)
    {
        try
        {
            return self.transform.parent.GetComponent(type);
        }
        catch
        {
            return null;
        }
    }

    public static Component GetComponentInChildren(this GameObject self, string type)
    {
        Transform[] children = self.transform.GetChildren();

        Component toRet;

        foreach (Transform child in children)
        {
            if (toRet = child.GetComponent(type))
            {
                return toRet;
            }
        }

        return null;
    }
    #endregion

    #region MONOBEHAVIOUR EXTENSIONS

    //Generic
    /// <summary>
    /// Returns the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns></returns>
    public static T GetComponentInParentOrChild<T>(this MonoBehaviour self, int priority = 0) where T : Component
    {
        T toReturn = null;

        switch (priority)
        {
            case 0:
                if (!self.TryGetComponent(out toReturn))
                {
                    if (!self.TryGetComponentInParent(out toReturn))
                    {
                        self.TryGetComponentInChildren(out toReturn);
                    }
                }
                break;
            case 1:
                if (!self.TryGetComponentInParent(out toReturn))
                {
                    if (!self.TryGetComponent(out toReturn))
                    {
                        self.TryGetComponentInChildren(out toReturn);
                    }
                }
                break;
            case 2:
                if (!self.TryGetComponentInChildren(out toReturn))
                {
                    if (!self.TryGetComponent(out toReturn))
                    {
                        self.TryGetComponentInParent(out toReturn);
                    }
                }
                break;
        }

        return toReturn;
    }

    /// <summary>
    /// This actually retrieves from the child, unlike GetComponentFromChildren
    /// </summary>
    /// <returns></returns>
    public static T GetComponentInChild<T>(this MonoBehaviour self) where T : Component
    {
        return (T)self.GetComponentInChildren(typeof(T).Name);
    }

    public static bool TryGetComponentInParent<T>(this MonoBehaviour self, out T component) where T : Component
    {
        component = self.GetComponentInParent<T>();

        return component != null;
    }

    public static bool TryGetComponentInChildren<T>(this MonoBehaviour self, out T component) where T : Component
    {
        component = self.GetComponentInChild<T>();

        return component != null;
    }

    /// <summary>
    /// Outputs the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns>If a component was found</returns>
    public static bool TryGetComponentInParentOrChild<T>(this MonoBehaviour self, out T component, int priority = 0) where T : Component
    {
        component = self.GetComponentInParentOrChild<T>(priority);

        return component != null;
    }

    //String-Typed
    /// <summary>
    /// Returns the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns></returns>
    public static Component GetComponentInParentOrChild(this MonoBehaviour self, string type, int priority = 0)
    {
        Component toReturn = null;

        switch (priority)
        {
            case 0:
                if (!self.TryGetComponent(type, out toReturn))
                {
                    if (!self.TryGetComponentInParent(type, out toReturn))
                    {
                        self.TryGetComponentInChildren(type, out toReturn);
                    }
                }
                break;
            case 1:
                if (!self.TryGetComponentInParent(type, out toReturn))
                {
                    if (!self.TryGetComponent(type, out toReturn))
                    {
                        self.TryGetComponentInChildren(type, out toReturn);
                    }
                }
                break;
            case 2:
                if (!self.TryGetComponentInChildren(type, out toReturn))
                {
                    if (!self.TryGetComponent(type, out toReturn))
                    {
                        self.TryGetComponentInParent(type, out toReturn);
                    }
                }
                break;
        }

        return toReturn;
    }

    public static bool TryGetComponentInParent(this MonoBehaviour self, string type, out Component component)
    {
        component = self.GetComponentInParent(type);

        return component != null;
    }

    public static bool TryGetComponentInChildren(this MonoBehaviour self, string type, out Component component)
    {
        component = self.GetComponentInChildren(type);

        return component != null;
    }

    /// <summary>
    /// Outputs the first component found.
    /// By default, will find the first component on the object itself,
    /// then check the Parent,
    /// then check the child.
    /// </summary>
    /// <param name="priority">0 = Self, Parent, Child. 1 = Parent, Self, Child. 2 = Child, Self, Parent.</param>
    /// <returns>If a component was found</returns>
    public static bool TryGetComponentInParentOrChild(this MonoBehaviour self, string type, out Component component, int priority = 0)
    {
        component = self.GetComponentInParentOrChild(type, priority);

        return component != null;
    }

    public static bool TryGetComponent(this MonoBehaviour self, string type, out Component component)
    {
        component = self.GetComponent(type);

        return component != null;
    }

    public static Component GetComponentInParent(this MonoBehaviour self, string type)
    {
        try
        {
            return self.transform.parent.GetComponent(type);
        }
        catch
        {
            return null;
        }
    }

    public static Component GetComponentInChildren(this MonoBehaviour self, string type)
    {
        Transform[] children = self.transform.GetChildren();

        Component toRet;

        foreach (Transform child in children)
        {
            //If GetComponent returns something other than null
            if (toRet = child.GetComponent(type))
            {
                //Return that component
                return toRet;
            }
        }

        return null;
    }
    #endregion

    #region TRANSFORM EXTENSIONS

    public static Transform[] GetChildren(this Transform self, bool includeInactive = false)
    {
        Transform[] toRet;

        if (!includeInactive)
        {
            int numActive = self.childCount;

            for (int i = 0; i < self.childCount; i++)
            {
                if (!self.GetChild(i).gameObject.activeSelf)
                {
                    numActive--;
                }
            }

            toRet = new Transform[numActive];

            int numSkipped = 0;

            for (int i = 0; i < self.childCount; i++)
            {
                if (self.GetChild(i).gameObject.activeSelf)
                {
                    toRet[i - numSkipped] = self.GetChild(i);
                }
                else
                {
                    numSkipped++;
                }
            }
        }
        else
        {
            toRet = new Transform[self.childCount];

            for (int i = 0; i < self.childCount; i++)
            {
                toRet[i] = self.GetChild(i);
            }
        }


        return toRet;
    }

    #endregion
}