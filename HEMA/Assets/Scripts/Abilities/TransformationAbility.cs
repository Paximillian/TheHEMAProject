using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Scripts/Abilities/Transformation Ability")]
public class TransformationAbility : AbilityBase
{
    protected override void Activate()
    {
        ObjectPoolManager.PullObject("TransformationLine").transform.SetParent(transform);
    }
}
