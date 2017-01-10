using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PVESetting
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class PVESetting
{
    public string OurBaseModelFile
    {
        get;
        set;
    }
    public Vector3 OurBaseLocalPos
    {
        get;
        set;
    }
    public Vector3 OurBaseLocalRotation
    {
        get;
        set;
    }
    public string EnemyBaseModelFile
    {
        get;
        set;
    }
    public Vector3 EnemyBaseLocalPos
    {
        get;
        set;
    }
    public Vector3 EnemyBaseLocalRotation
    {
        get;
        set;
    }
}