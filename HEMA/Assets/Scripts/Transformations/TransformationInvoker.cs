using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformationInvoker : MonoBehaviour {

    [SerializeField]
    private float m_Cooldown = 1.5f;

    [SerializeField]
    private Image m_CooldownTimer;

    private float? m_TimeSinceLastUse;
    	
	// Update is called once per frame
	void Update ()
    {
        m_CooldownTimer.fillAmount = m_TimeSinceLastUse == null ? 1 : ((Time.time - m_TimeSinceLastUse.Value) / m_Cooldown);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateTransformationLine();
        }
    }

    public void GenerateTransformationLine()
    {
        if (m_TimeSinceLastUse == null || Time.time > m_TimeSinceLastUse.Value + m_Cooldown)
        {
            ObjectPoolManager.PullObject("TransformationLine").transform.SetParent(transform);

            m_TimeSinceLastUse = Time.time;
        }
    }
}
