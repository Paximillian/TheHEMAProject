using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformationLine : MonoBehaviour
{
    public event Action<TransformationLine> OnLineFinished;

    private RectTransform m_RectTransform;

    [SerializeField]
    private int m_ScannedPixelsPerUpdate = 10;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        m_RectTransform.anchoredPosition3D = new Vector3(-Screen.width / 2, 0, 0);
    }

    private void FixedUpdate()
    {
        advanceLine();
    }

    //Advances the transformation line by a set amount of pixels and then sends transformation messages to each character hit that supports transformations.
    private void advanceLine()
    {
        m_RectTransform.anchoredPosition += Vector2.right * m_ScannedPixelsPerUpdate;

        Vector2 transformationLineLocation = new Vector2(m_RectTransform.anchoredPosition.x, -Screen.height / 2);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transformationLineLocation, Vector2.up, Screen.height);

        foreach (RaycastHit2D hit in hits)
        {
            TransformableCharacter transformableCharacter = hit.transform.GetComponent<TransformableCharacter>();

            if (transformableCharacter)
            {
                transformableCharacter.TransformByPoint(this, hit.point);
            }
        }

        if(m_RectTransform.anchoredPosition.x > Screen.width / 2)
        {
            OnLineFinished?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}
