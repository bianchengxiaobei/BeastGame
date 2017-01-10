using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SomeTools 
// 创建者：chen
// 修改者列表：
// 创建日期：
// 模块描述：
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 
/// </summary>
public class SomeTools 
{
    [MenuItem("Tools/Rename From 0")]
    public static void RenameFromZero()
    {
        string selectForderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        string[] files = Directory.GetFiles(selectForderPath, "*.*", SearchOption.AllDirectories);
        int i = 0;
        foreach (var file in files)
        {
            if (BuildCommon.getFileSuffix(file) == "meta")
            {
                continue;
            }        
            AssetDatabase.RenameAsset(file,i.ToString());
            AssetDatabase.Refresh();
            i++;
        }
    }
}
