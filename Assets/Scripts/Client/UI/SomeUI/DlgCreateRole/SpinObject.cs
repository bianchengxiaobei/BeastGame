using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SpinObject 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.16
// 模块描述：旋转物体
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 旋转物体
/// </summary>
public class SpinObject : MonoBehaviour
{
    [HideInInspector]
    public EntityShow m_target;
    public float m_speed;
    public void OnDrag(Vector2 kDelta)
    {
        if (this.m_target != null && this.m_target.IsPlayingShowAnim() == false)
        {
            this.m_target.GameObject.transform.Rotate(new Vector3(0, -kDelta.x , 0) * m_speed);
        }
    }
}
