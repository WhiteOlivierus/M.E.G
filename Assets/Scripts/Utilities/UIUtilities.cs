﻿using UnityEngine.UI;
using UnityEngine;

public class UIUtilities : MonoBehaviour
{
    public void AddOne(Text t)
    {
        t.text = (int.Parse(t.text) + 1).ToString();
    }
}