using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IHero
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：神兽接口
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public interface IBeast
    {
        GameObject Object
        {
            get;
        }
        bool IsError
        {
            get;
        }
        bool IsModelVisible
        {
            get;
        }
        Vector3 RealPos3D
        {
            get;
        }
        Vector3 MovingPos
        {
            get;
        }
        float Height
        {
            get;
        }
        Transform Body
        {
            get;
        }
        Transform LeftHand
        {
            get;
        }
        Transform RightHand
        {
            get;
        }
        Transform RightSpecialTrans
        {
            get;
        }
        Transform LeftSpecialTrans
        {
            get;
        }
        Transform OtherSpecialTrans
        {
            get;
        }
        void AddMaterial(Material material);
        void DelMaterial(Material material);
    }
}