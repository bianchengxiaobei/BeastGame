using UnityEngine;
using System.Collections;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ActorBullet 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.4.18
// 模块描述：远程粒子发送器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 远程粒子发送器
/// </summary>
public class ActorBullet : MonoBehaviour
{
	#region 字段
    public Transform target;
    public float speed = 10f;
    public Vector3 targetPos;
    public bool isSetup;
    public System.Action OnDestory = null;
	#endregion
	#region 属性
	#endregion
	#region 构造方法
	#endregion
	#region 公共方法
	#endregion
	#region 私有方法
	#endregion
	#region 析构方法
	#endregion
}
