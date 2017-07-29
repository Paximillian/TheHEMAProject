using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    private enum eActivationMethod
    {
        Pressed,
        Held,
        Released
    }

    [SerializeField]
    [Tooltip("The KeyCode that activates this ability")]
    private KeyCode m_Key;

    [SerializeField]
    [Tooltip("The method in which this abilitiy activates given the selected KeyCode")]
    private eActivationMethod m_ActivationMethod;

    [SerializeField]
    [Tooltip("How long do we have to wait between uses of this ability")]
    private float m_Cooldown = 1.5f;
    private float? m_TimeSinceLastUse;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (m_TimeSinceLastUse == null || Time.time > m_TimeSinceLastUse.Value + m_Cooldown)
        {
            if ((m_ActivationMethod == eActivationMethod.Pressed && Input.GetKeyDown(m_Key))
                || (m_ActivationMethod == eActivationMethod.Released && Input.GetKeyUp(m_Key))
                || (m_ActivationMethod == eActivationMethod.Held && Input.GetKey(m_Key)))
            {
                Activate();
                m_TimeSinceLastUse = Time.time;
            }
        }
    }

    protected abstract void Activate();
}
