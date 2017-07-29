using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles animation events of the parent object for a specific spritesheet.
/// </summary>
public class AnimationHandler : MonoBehaviour
{
    [SerializeField]
    private string m_SpriteSheetPath;

    private Sprite[] m_Sprites;

    private Image m_CurrentSprite;

    // Use this for initialization
    private void Awake()
    {
        m_Sprites = Resources.LoadAll<Sprite>(m_SpriteSheetPath);
        m_CurrentSprite = GetComponent<Image>();

        m_CurrentSprite.sprite = m_Sprites[0];
    }

    public void SetSpriteId(int i_SpriteId)
    {
        m_CurrentSprite.sprite = m_Sprites[i_SpriteId];
    }
}
