using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformableCharacter : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_CharacterForms;

    private int m_CurrentFormId = 0;
    private int m_NextFormId
    {
        get
        {
            return m_CurrentFormId + 1 == m_CharacterForms.Length ? 0 : m_CurrentFormId + 1;
        }
    }

    public Tuple<Image, Image> CurrentTransformation
    {
        get
        {
            return new Tuple<Image, Image>() { Value1 = m_CharacterForms[m_CurrentFormId].GetComponent<Image>(), Value2 = m_CharacterForms[m_NextFormId].GetComponent<Image>() };
        }
    }

    /// <summary>
    /// Transforms a character in its current transformation sequence up to the x point of the given point.
    /// </summary>
    /// <param name="i_WorldPointHit">The world point where the transformation line hit. We'll transform the character with this point's x as the seperator.</param>
    public void TransformByPoint(Vector2 i_WorldPointHit)
    {
        CurrentTransformation.Value1.fillOrigin = (int)Image.OriginHorizontal.Left;
        CurrentTransformation.Value2.fillOrigin = (int)Image.OriginHorizontal.Right;

        //First we'll get the x point of the hit in local spaces (-sprite width / 2 <= Pixel in sprite <= sprite width / 2).
        float centerRelativeHitX = transform.InverseTransformPoint(i_WorldPointHit).x;
        //Then we offset it to sprite size (0 <= pixel in sprite <= sprite width), since sprite local spaces in unity can be negative.
        float pixelHitX = centerRelativeHitX + CurrentTransformation.Value1.sprite.rect.width / 2;
        //And finally we got the normal of the hit (What % of the transformation is complete).
        float hitFillNormal = pixelHitX / CurrentTransformation.Value1.sprite.rect.width;

        CurrentTransformation.Value1.fillAmount = hitFillNormal;
        CurrentTransformation.Value2.fillAmount = 1 - hitFillNormal;
        
        if(hitFillNormal <= 0)
        {
            m_CurrentFormId = m_NextFormId;
        }
    }
}
