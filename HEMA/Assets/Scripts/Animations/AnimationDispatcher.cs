using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to pass events from the Animator of the object using this component to children of this object.
/// </summary>
public class AnimationDispatcher : MonoBehaviour
{
    public void SetSpriteId(int i_SpriteId)
    {
        foreach (AnimationHandler handler in GetComponentsInChildren<AnimationHandler>())
        {
            handler.SetSpriteId(i_SpriteId);
        }
    }
}
