using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：AssetUnit
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class AssetUnit 
{
	#region 字段
    public string mPath;                            //文件路径相对Asset      
    public string mName;                            //文件名称,无后缀名称
    public string mSuffix;                          //后缀
    public int mLevel;                              //层数
    public int mIndex;                              //索引
    public int mAssetSize;                          //保存该Asset的打包文件大小
    public int mRefCount;
    public EnumAssetType mType = EnumAssetType.eAssetType_Undefined;
    public List<string> mAllDependencies;           //所有依赖
    public List<AssetUnit> mNextLevelDependencies;  //下层依赖
    public List<AssetUnit> mDirectUpperDependences; //直接上层依赖,用于判定assetbundle加载后是否可以马上删除
	#endregion
	#region 构造方法
    public AssetUnit(string path)
    {
        mPath = path;
        mName = BuildCommon.getFileName(mPath, true);//获取文件名带后缀
        mSuffix = BuildCommon.getFileSuffix(mName);//获取文件的后缀名
        switch (mSuffix)
        {
            case "shader":
                mType = EnumAssetType.eAssetType_AssetBundleShader;
                break;
            case "prefab":
                mType = EnumAssetType.eAssetType_AssetBundlePrefab;
                break;
            case "tex":
                break;
        }
        mLevel = BuildCommon.getAssetLevel(mPath);
        mAllDependencies = new List<string>();
        mNextLevelDependencies = new List<AssetUnit>();
        //获取这个资源的所有引用
        string[] deps = AssetDatabase.GetDependencies(new string[] { mPath });
        //循环遍历所有引用，加入到allDependencies
        foreach (var file in deps)
        {
            string suffix = BuildCommon.getFileSuffix(file);
            if (file == mPath || suffix == "cs" || suffix == "dll")
            {
                continue;
            }
            mAllDependencies.Add(file);
        }
        mDirectUpperDependences = new List<AssetUnit>();
    }
	#endregion
	#region 公有方法
	#endregion
	#region 私有方法
	#endregion
}
