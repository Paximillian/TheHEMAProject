using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSettings : MonoBehaviour
{
    [SerializeField]
    private int m_TargetFPS = 60;

    // Use this for initialization
    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = m_TargetFPS;
    }
}
