using Effect;
using System;
using System.Collections.Generic;
using UnityEngine;
using Xft;
public class EffectLayerRopeSet : MonoBehaviour, IEffectLayerRopeSet
{
    public List<GameObject> ListTarget;
    private Transform m_Trans;
    public void SetEffectLayerTarget(Transform target)
    {
        if (null == target)
        {
            XLog.Log.Error("EffectLayerRopeSet.Target is null");
            return;
        }
        foreach (GameObject current in this.ListTarget)
        {
            if (null != current)
            {
                EffectLayer component = current.GetComponent<EffectLayer>();
                if (null != component)
                {
                    component.CollisionGoal = target;
                    component.GravityObject = target;
                }
            }
        }
    }
}
