﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustGraphics : MonoBehaviour
{
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}

