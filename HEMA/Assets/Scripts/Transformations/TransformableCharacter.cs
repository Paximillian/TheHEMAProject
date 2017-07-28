using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class TransformableCharacter : MonoBehaviour
{
    [Tooltip("A list of prefabs representing the forms of this character.\nFor this to work, they need to have the exact same form in all sprites and animations, differing only by skin")]
    [SerializeField]
    private CharacterForm[] m_CharacterForms;

    //The last form to have completed transformation (Having 1 fill ratio)
    private CharacterForm m_CurrentBaseForm;

    //Holds a 1-1 dictionary between a form object of this character and the line responsible for transforming it.
    private readonly BiDirectionalDictionary<CharacterForm, TransformationLine> r_FormLineDict = new BiDirectionalDictionary<CharacterForm, TransformationLine>();
    
    //Controls flow of next form of character.
    private int m_CurrentFormId = -1;
    private int m_NextFormId => m_CurrentFormId = (m_CurrentFormId + 1 == m_CharacterForms.Length ? 0 : m_CurrentFormId + 1);
    private CharacterForm m_NextForm => Instantiate(m_CharacterForms[m_NextFormId], transform.position, Quaternion.identity, transform).GetComponent<CharacterForm>();

    private void Awake()
    {
        initBaseForm();
    }

    //The first base form of the character will be the first one in its form list.
    private void initBaseForm()
    {
        //This object holds an image so that during level construction we'll be able to see what the level would look like, in runtime this is managed by forms on the children of this object, so
        //the static image is no longer required.
        Destroy(GetComponent<Image>());

        m_CurrentBaseForm = m_NextForm;
        m_CurrentBaseForm.TransformationRatio = 1;
    }

    //Invoked when the given form completes transformation, sets it as the new base form of this character and removes the last base form.
    private void CharacterForm_TransformationComplete(CharacterForm i_CompletedForm)
    {
        Destroy(m_CurrentBaseForm.gameObject);

        r_FormLineDict.Remove(i_CompletedForm);
        i_CompletedForm.OnTransformationComplete -= CharacterForm_TransformationComplete;
        m_CurrentBaseForm = i_CompletedForm;
    }

    /// <summary>
    /// Transforms a character in its current transformation sequence up to the x point of the given point.
    /// </summary>
    /// <param name="i_TransformationLine">The transformation line that invoked this transformation. Each unique line that affects this character will be linked to a unique specific form object.</param>
    /// <param name="i_WorldPointHit">The world point where the transformation line hit. We'll transform the character with this point's x as the seperator.</param>
    public void TransformByPoint(TransformationLine i_TransformationLine, Vector2 i_WorldPointHit)
    {
        CharacterForm affectedForm = r_FormLineDict[i_TransformationLine];

        if (affectedForm == null)
        {
            affectedForm = createNewTransformation(i_TransformationLine);
        }

        //First we'll get the x point of the hit in local spaces (-sprite width / 2 <= Pixel in sprite <= sprite width / 2).
        float centerRelativeHitX = transform.InverseTransformPoint(i_WorldPointHit).x;
        //Then we offset it to sprite size (0 <= pixel in sprite <= sprite width), since sprite local spaces in unity can be negative.
        float pixelHitX = centerRelativeHitX + affectedForm.Sprite.rectTransform.rect.width / 2;
        //And finally we got the normal of the hit (What % of the transformation is complete).
        float hitFillNormal = pixelHitX / affectedForm.Sprite.rectTransform.rect.width;

        affectedForm.TransformRate = hitFillNormal;
    }

    //Creates a new transformation sequence on this character and maps it to the invoking transformation line.
    private CharacterForm createNewTransformation(TransformationLine i_TransformationLine)
    {
        if (r_FormLineDict[i_TransformationLine])
        {
            throw new ArgumentException("A form already exists for given transformation line on this character");
        }

        CharacterForm newForm = m_NextForm;
        i_TransformationLine.OnLineFinished += TransformationLine_LineFinished;
        newForm.OnTransformationComplete += CharacterForm_TransformationComplete;

        r_FormLineDict.Add(newForm, i_TransformationLine);

        return newForm;
    }

    private void TransformationLine_LineFinished(TransformationLine i_TransformationLine)
    {
        CharacterForm affectedForm = r_FormLineDict[i_TransformationLine];
        i_TransformationLine.OnLineFinished -= TransformationLine_LineFinished;

        affectedForm.TransformRate = 1;
    }
}
