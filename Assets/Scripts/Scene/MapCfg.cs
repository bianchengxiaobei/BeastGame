using UnityEngine;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MapCfg
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：地图格子信息配置器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 专门初始化格子类型的工具
/// </summary>
public class MapCfg
{
    private static Dictionary<string, EMapNodeType> s_oDicNodeType = new Dictionary<string, EMapNodeType>();
    private static Dictionary<string, EMapJudgeType> s_oDicJudegeType = new Dictionary<string, EMapJudgeType>();
    private static Dictionary<string, bool> s_oDicWalkType = new Dictionary<string, bool>();
    private static Dictionary<EMapNodeType, string> s_oDicMapNodeType2String = new Dictionary<EMapNodeType, string>();
    public static void Register(string type, EMapNodeType eNodeType, EMapJudgeType eJudgeType, bool bWalkType)
    {
        MapCfg.s_oDicNodeType.Add(type, eNodeType);
        MapCfg.s_oDicJudegeType.Add(type, eJudgeType);
        MapCfg.s_oDicWalkType.Add(type, bWalkType);
        MapCfg.s_oDicMapNodeType2String.Add(eNodeType, type);
    }
    /// <summary>
    /// 初始化格子类型，从已经存在的字典里面取
    /// </summary>
    /// <param name="oNode"></param>
    /// <returns></returns>
    public static bool MapNodeTransfer(MapNode oNode)
    {
        if (!MapCfg.s_oDicNodeType.ContainsKey(oNode.m_strType))
        {
            string message = string.Format("MapNodeTransfer fail {0}", oNode.m_strType);
            Debug.LogError(message);
            return false;
        }
        oNode.m_eMapNodeType = MapCfg.s_oDicNodeType[oNode.m_strType];
        oNode.m_eMapJudgeType = MapCfg.s_oDicJudegeType[oNode.m_strType];
        oNode.m_bCanWalk = MapCfg.s_oDicWalkType[oNode.m_strType];
        return true;
    }
    public static bool GetNodeWalkType(string strNodeType)
    {
        return MapCfg.s_oDicWalkType.ContainsKey(strNodeType) && MapCfg.s_oDicWalkType[strNodeType];
    }
    public static string MapNodeType2String(EMapNodeType eMapNodeType)
    {
        string result = "";
        if (!MapCfg.s_oDicMapNodeType2String.TryGetValue(eMapNodeType, out result))
        {
            Debug.LogError("m_oDicMapNodeType2String.TryGetValue(eMapNodeType, out strMapNodeString) == false:" + eMapNodeType);
        }
        return result;
    }
    public static int MapNodeTypeToInt(EMapNodeType eMapNodeType)
    {
        return (int)eMapNodeType;
    }
    /// <summary>
    /// 根据格子类型字符，返回格子类型
    /// </summary>
    /// <param name="strType"></param>
    /// <returns></returns>
    public static EMapNodeType String2MapNodeType(string strType)
    {
        if (string.IsNullOrEmpty(strType))
        {
            return EMapNodeType.MAP_NODE_INVALID;
        }
        EMapNodeType result = EMapNodeType.MAP_NODE_INVALID;
        if (MapCfg.s_oDicNodeType.TryGetValue(strType, out result))
        {
            return result;
        }
        return EMapNodeType.MAP_NODE_INVALID;
    }
}
