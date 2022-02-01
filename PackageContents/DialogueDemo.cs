/*******************************************************************************
File: DialogueDemo.cs
Author: Kevin Jacobson
DP Email: kevin.j@digipen.edu
Date: 1/24/2022

Description:
    A basic example of using dialogue line selectors.
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueDemo : MonoBehaviour
{
    public TextMeshProUGUI m_Text;

    public KeyCode m_ToggleSingle = KeyCode.S;

    public KeyCode m_SeriesForward = KeyCode.D;
    private int m_SeriesNext;

    [Header("Dialogue Lines")]
    public DialogueLineSelector m_SingleLine;

    public DialogueLineSelector[] m_LineSeries;

    private void Start()
    {
        m_Text.text = m_SingleLine.GetLine();
        m_Text.enabled = false;
    }

    private void Update()
    {
        UpdateSingleLine();

        UpdateLineSeries();
    }

    void UpdateLineSeries()
    {
        //If the line should be progressed and there are any lines in the series
        if (Input.GetKeyDown(m_SeriesForward) && m_LineSeries.Length > 0)
        {
            //Enable the text
            m_Text.enabled = true;

            //Ensure the index is within the series
            if (m_SeriesNext >= m_LineSeries.Length || m_SeriesNext < 0)
            {
                m_SeriesNext = 0;
            }

            //Update the text
            m_Text.text = m_LineSeries[m_SeriesNext].GetLine();

            //Update the index for the next progression
            m_SeriesNext++;
        }
    }

    void UpdateSingleLine()
    {
        //If the line should be toggled on/off
        if (Input.GetKeyDown(m_ToggleSingle))
        {
            //Toggle the line to the opposite of current state
            m_Text.enabled = !m_Text.enabled;
            //Update the text
            m_Text.text = m_SingleLine.GetLine();
        }
    }
}
