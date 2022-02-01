/***************************************************
File:           ArrayTools.cs
Authors:        Kevin Jacobson
Last Updated:   1/18/2022

Description:
  Script tools for arrays

Copyright 2019-2022, Kevin Jacobson
***************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ArrayTools
{
    #region Add and Get Functions
    public static T Get<T>(this T[] array, int index)
    {
        ValidateUnsafe(array, index);

        return array[index];
    }

    public static T Get<T>(this T[,] array, int[] index)
    {
        ValidateUnsafe(array, index);

        return array[index[0], index[1]];
    }

    public static T Get<T>(this T[,,] array, int[] index)
    {
        ValidateUnsafe(array, index);

        return array[index[0], index[1], index[2]];
    }

    public static T GetRandom<T>(this T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length - 1)];
    }

    public static void Add<T>(this T[] array, T newItem)
    {
        T[] temp = new T[array.Length + 1];
        array.CopyTo(temp, 0);
        temp[temp.Length - 1] = newItem;

        array = new T[temp.Length];

        temp.CopyTo(array, 0);
    }

    public static void Add<T>(this T[,] array, T[] newItem)
    {
        if(newItem.Length != array.GetLength(1))
        {
            throw new IndexOutOfRangeException("New array length does not match existing array");
        }

        T[,] temp = new T[array.GetLength(0) + 1, array.GetLength(1)];
        array.CopyTo(temp, 0);

        for(int i = 0; i < temp.GetLength(1); i++)
        {
            temp[temp.GetLength(0), i] = newItem[i];
        }

        array = new T[temp.GetLength(0), temp.GetLength(1)];

        temp.CopyTo(array, 0);
    }
    #endregion

    #region Index Validation Functions
    private static string InvalidIndexMessage(int index, int length) => "Index \"" + index + "\" is invalid for array of length \"" + length + "\"";

    private static bool Validate(int index, int length)
    {
        if (length == 0) return false;
        return index > 0 && index < length - 1;
    }

    public static bool Validate(this Array array, int index)
    {
        return Validate(index, array.Length);
    }

    public static bool ValidateUnsafe(this Array array, int index)
    {
        if(!Validate(index, array.Length))
        {
            throw new IndexOutOfRangeException(InvalidIndexMessage(index, array.Length));
        }

        return true;
    }

    public static bool Validate<T>(this T[] array, int index)
    {
        return Validate(index, array.Length);
    }

    public static bool ValidateUnsafe<T>(this T[] array, int index)
    {
        if (!Validate(index, array.Length))
        {
            throw new IndexOutOfRangeException(InvalidIndexMessage(index, array.Length));
        }

        return true;
    }

    public static bool Validate<T>(this T[,] array, int[] index)
    {
        if (index.Length == array.Rank)
        {
            for (int i = 0; i < array.Rank; i++)
            {
                if (!Validate(index, array.GetLength(i)))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    public static bool ValidateUnsafe<T>(this T[,] array, int[] index)
    {
        if (index.Length == array.Rank)
        {
            for (int i = 0; i < array.Rank; i++)
            {
                if (!Validate(index, array.GetLength(i)))
                {
                    throw new IndexOutOfRangeException(InvalidIndexMessage(index[i], array.GetLength(i)));
                }
            }

            return true;
        }

        return false;
    }

    public static bool Validate<T>(this T[,,] array, int[] index)
    {
        if (index.Length == array.Rank)
        {
            for(int i = 0; i < array.Rank; i++)
            {
                if (!Validate(index, array.GetLength(i)))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
    }

    public static bool ValidateUnsafe<T>(this T[,,] array, int[] index)
    {
        if (index.Length == array.Rank)
        {
            for (int i = 0; i < array.Rank; i++)
            {
                if (!Validate(index, array.GetLength(i)))
                {
                    throw new IndexOutOfRangeException(InvalidIndexMessage(index[i], array.GetLength(i)));
                }
            }

            return true;
        }

        return false;
    }
    #endregion

    #region IndexOf Functions
    /// <summary>
    /// Simple linear search function. O(n) complexity.
    /// </summary>
    /// <param name="value">The value being searched for</param>
    /// <returns>The index of that value, or -1 if it doesn't exist</returns>
    public static int IndexOf<T>(this T[] array, T value)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if(value.Equals(array[i]))
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Simple linear search function. O(n) complexity.
    /// </summary>
    /// <param name="value">The value being searched for</param>
    /// <returns>The index of that value, or -1 for all indexes if it doesn't exist</returns>
    public static int[] IndexOf<T>(this T[,] array, T value)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for(int j = 0; j < array.GetLength(1); j++)
            {
                if (value.Equals(array[i,j]))
                {
                    return new int[] { i, j };
                }
            }
        }

        return new int[] { -1, -1 };
    }

    /// <summary>
    /// Simple linear search function. O(n) complexity.
    /// </summary>
    /// <param name="value">The value being searched for</param>
    /// <returns>The index of that value, or -1 for all indexes if it doesn't exist</returns>
    public static int[] IndexOf<T>(this T[,,] array, T value)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                for (int k = 0; k < array.GetLength(2); k++)
                {
                    if (value.Equals(array[i, j, k]))
                    {
                        return new int[] { i, j, k };
                    }
                }
            }
        }

        return new int[] { -1, -1, -1 };
    }
    #endregion

    #region ValueTuple Array and KeyValuePair Array Extensions
    public static object[] GetKeys(this KeyValuePair<object, object>[] keyValuePairs)
    {
        object[] items = new object[keyValuePairs.Length];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = keyValuePairs[i].Key;
        }

        return items;
    }

    public static object[] GetValues(this KeyValuePair<object, object>[] keyValuePairs)
    {
        object[] items = new object[keyValuePairs.Length];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = keyValuePairs[i].Value;
        }

        return items;
    }

    public static object[] GetItems(this ValueTuple<object>[] valueTuple)
    {
        object[] items = new object[valueTuple.Length];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = valueTuple[i].Item1;
        }

        return items;
    }

    public static object[] GetItems(this ValueTuple<object, object>[] valueTuple, int index = 0)
    {
        object[] items = new object[valueTuple.Length];

        for (int i = 0; i < items.Length; i++)
        {
            if (index == 0)
            {
                items[i] = valueTuple[i].Item1;
            }
            else
            {
                items[i] = valueTuple[i].Item2;
            }
        }

        return items;
    }

    public static object[] GetItems(this ValueTuple<object, object, object>[] valueTuple, int index = 0)
    {
        object[] items = new object[valueTuple.Length];

        for (int i = 0; i < items.Length; i++)
        {
            if (index == 0)
            {
                items[i] = valueTuple[i].Item1;
            }
            else if (index == 1)
            {
                items[i] = valueTuple[i].Item2;
            }
            else
            {
                items[i] = valueTuple[i].Item3;
            }
        }

        return items;
    }
    #endregion

    public static string ArrayToString<T>(this T[] array)
    {
        string toRet = "(";

        foreach(T obj in array)
        {
            toRet += obj.ToString() + ", ";
        }

        toRet.Remove(toRet.LastIndexOf(','), 2);

        toRet += ")";

        return toRet;
    }

    public class InverseCapableComparer : IComparer
    {
        public bool Invert;

        public int Compare(object left, object right)
        {
            if (left is IComparable leftComp && right is IComparable rightComp)
            {
                int value = leftComp.CompareTo(rightComp);
                value *= Invert ? -1 : 1;
                return value;
            }

            return 0;
        }
    }
}