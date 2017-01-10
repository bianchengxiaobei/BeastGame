using UnityEngine;
using System.Collections;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataAvatarModel 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.14
// 模块描述：角色模型配置，根据不同的职业不同的数据
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 角色模型配置
/// </summary>
public class DataAvatarModel : GameData<DataAvatarModel>
{
    public static string fileName = "dataAvatarModel";
    public int ID
    {
        get;
        private set;
    }
    public string PrefabPath
    {
        get;
        private set;
    }
    public float Scale
    {
        get;
        private set;
    }
    public string Vocation
    {
        get;
        private set;
    }
}
