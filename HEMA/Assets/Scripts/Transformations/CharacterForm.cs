using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterForm : MonoBehaviour
{
    /// <summary>
    /// The image component representing this form.
    /// </summary>
    private Image m_Sprite;
    public Image Sprite { get { return m_Sprite; } }

    public event Action<CharacterForm> OnTransformationComplete;

    public float TransformationRatio
    {
        set
        {
            m_Sprite.fillAmount = value;
        }
    }
    
    // Use this for initialization
    private void Awake()
    {
        m_Sprite = GetComponent<Image>();
    }

    /// <summary>
    /// Transforms a character in its current transformation sequence up to the x point of the given point.
    /// </summary>
    /// <param name="i_TransformationLine">The transformation line that invoked this transformation. Each unique line that affects this character will be linked to a unique specific form object.</param>
    public float TransformRate
    {
        set
        {
            m_Sprite.fillAmount = value;

            if (m_Sprite.fillAmount >= 1)
            {
                OnTransformationComplete?.Invoke(this);
            }
        }
    }
}
