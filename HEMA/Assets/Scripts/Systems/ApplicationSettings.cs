using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Systems/Application Settings")]
public class ApplicationSettings : Singleton<ApplicationSettings>
{
    [SerializeField]
    [Tooltip("The FPS we want to run the game at.")]
    private int m_TargetFPS = 60;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = m_TargetFPS;
    }
}
