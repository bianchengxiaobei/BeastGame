using UnityEngine;
using System.Collections.Generic;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataSceneConfig
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.13
// 模块描述：场景数据配置
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 场景数据配置
/// </summary>
public class DataSceneConfig : GameData<DataSceneConfig>
{
    public static readonly string fileName = "sceneConfig";
    public int ID
    {
        get;
        private set;
    }
    public string SceneName
    {
        get;
        private set;
    }
    public string ChapterName
    {
        get;
        private set;
    }
    public string StoryDSLFile
    {
        get;
        private set;
    }
    public float EnterX
    {
        get;
        private set;
    }
    public float EnterZ
    {
        get;
        private set;
    }
}
