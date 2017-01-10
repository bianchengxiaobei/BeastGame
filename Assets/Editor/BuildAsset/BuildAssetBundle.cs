using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using LitJson;
using System;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：BuildAssetBundle
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：打包Assetbundle
//----------------------------------------------------------------*/
#endregion
public class BuildAssetBundle 
{
	#region 字段
    static string SceneCfgFilePath = Application.dataPath + "/Editor/Build/Scene.txt";
    //打包环境设置=>Includes all dependencies | Forces inclusion of the entire asset. | Builds an asset bundle using a hash for the id of the object stored in the asset bundle.
    static BuildAssetBundleOptions options = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle;
    //打包平台设置为window
    static BuildTarget buildPlatform = BuildTarget.StandaloneWindows;
    //保存所有Scene的信息
    static List<string> mScenes = new List<string>();
    //保存所有Resource的信息
    static List<string> mResource = new List<string>();
    //保存所有Asset信息 场景+Resource
    static Dictionary<int, Dictionary<string, AssetUnit>> allLevelAssets = new Dictionary<int, Dictionary<string, AssetUnit>>();
    static Dictionary<string, AssetUnit> allFiles = new Dictionary<string, AssetUnit>();
    #endregion
	#region 公有方法
    [MenuItem("Build/BuildWindows")]
    public static void BuildWindows()
    {
        buildPlatform = BuildTarget.StandaloneWindows;
        BuildResourceFromUnityRule();
    }
    public static void BuildResourceFromUnityRule()
    {
        //清空所有Assetbundle数据
        if (Directory.Exists(ResourceCommon.assetbundleFilePath))
        {
            Debug.Log("存在目录先删除");
            Directory.Delete(ResourceCommon.assetbundleFilePath,true);
        }
        //刷新数据
        Caching.CleanCache();
        AssetDatabase.Refresh();
        //获取资源信息
        //GetBuildScenes();
        GetBuildResource();
        //获取打包的所有Asset
        GetAllAssets();
        BuildResource();
        DumpResourceInfoFile();
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
    }
	#endregion
	#region 私有方法
    private static void GetBuildScenes()
    {
        mScenes.Clear();
        FileStream fs = new FileStream(SceneCfgFilePath, FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        string path = "";
        while (path != null)
        {
            if (path != "")
            {
                mScenes.Add(path);
            }
            path = sr.ReadLine();
        }
        fs.Close();
        sr.Close();
    }
    private static void GetBuildResource()
    {
        mResource.Clear();
        string resourcePath = Application.dataPath + "/Resources/";
        //文件过滤，选取*.*文件
        string[] files = Directory.GetFiles(resourcePath, "*.*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string suffix = BuildCommon.getFileSuffix(file);
            if (suffix == "meta")
            {
                continue;
            }
            string realFile = file.Replace("\\", "/");
            realFile = realFile.Replace(Application.dataPath, "Assets");
            mResource.Add(realFile);
        }
    }
    /// <summary>
    /// 初始化allLevelAssets，全部打包资源
    /// </summary>
    private static void GetAllAssets()
    {
        allLevelAssets.Clear();
        //所有资源路径
        List<string> allAssetPath = new List<string>();
        //添加场景资源路径
        foreach (var scene in mScenes)
        {
            allAssetPath.Add(scene);
        }
        //添加Resources资源路径
        foreach (var resPath in mResource)
        {
            allAssetPath.Add(resPath);
        }
        //所有需要打包的资源路径+引用路径
        string[] allExportAssets = AssetDatabase.GetDependencies(allAssetPath.ToArray());
        
        foreach (var asset in allExportAssets)
        {
            if (asset.Contains(".cs") || asset.Contains("dll"))
            {
                continue;
            }
            AssetUnit unit = new AssetUnit(asset);
            if (!allFiles.ContainsKey(asset))
            {
                allFiles.Add(asset, unit);
            }
            //所在层级
            int level = unit.mLevel;
            if (allLevelAssets.ContainsKey(level))
            {
                allLevelAssets[level].Add(asset, unit);
            }
            else 
            {
                Dictionary<string, AssetUnit> levelAsset = new Dictionary<string, AssetUnit>();
                allLevelAssets.Add(level, levelAsset);
                allLevelAssets[level].Add(asset, unit);
            }
        }
        BuildAssetUnitIndex();
    }
    /// <summary>
    /// 根据层级创建AssetUnit的索引信息
    /// </summary>
    private static void BuildAssetUnitIndex()
    {
        if (allLevelAssets.Count == 0)
        {
            return;
        }
        int index = 0;
        for (int level = 1; level <= allLevelAssets.Count; level++)
        {
            Dictionary<string, AssetUnit> levelAssets = allLevelAssets[level];
            foreach (var pair in levelAssets)
            {
                AssetUnit unit = pair.Value;
                unit.mIndex = index;
                index++;
            }
        }
    }
    /// <summary>
    /// 打包资源为AssetBundle
    /// </summary>
    private static void BuildResource()
    {
        AssetDatabase.Refresh();
        int maxLevel = allLevelAssets.Count;
        if (maxLevel == 0)
        {
            return;
        }
        for (int level = 1; level <= maxLevel; level++)
        {
            BuildPipeline.PushAssetDependencies();
            Dictionary<string, AssetUnit> levelAssets = allLevelAssets[level];
            foreach (var pair in levelAssets)
            {
                UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath(pair.Value.mPath);
                string savePath = "";
                pair.Value.mName = BuildCommon.GetLevelABPathName(pair.Value.mPath);
                if (null == asset)
                {
                    Debug.LogError("Load :" + pair.Value.mPath + "failed");
                }
                Debug.Log(pair.Value.mSuffix);
                if (pair.Value.mSuffix.Equals("shader"))
                {
                    savePath = "Assets/bin/data/shader/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                    pair.Value.mPath = "data/shader/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                }
                else if (pair.Value.mSuffix.Equals("mat")) 
                {
                    savePath = "Assets/bin/data/atlas/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                    pair.Value.mPath = "data/atlas/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                }
                else if (pair.Value.mSuffix.Equals("prefab"))
                {
                    if (pair.Value.mName.Contains("atla"))
                    {
                        savePath = "Assets/bin/data/atlas/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                        pair.Value.mPath = "data/atlas/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                    }
                    else
                    {
                        savePath = "Assets/bin/data/ui/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                        pair.Value.mPath = "data/ui/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                    }
                }
                else if (pair.Value.mSuffix.Equals("ttf"))
                {
                    savePath = "Assets/bin/data/font/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                    pair.Value.mPath = "data/font/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                }
                else if (pair.Value.mSuffix.Equals("png") || pair.Value.mSuffix.Equals("psd") || pair.Value.mSuffix.Equals("dds"))
                {
                    savePath = "Assets/bin/data/texture/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                    pair.Value.mPath = "data/texture/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                }
                else if (pair.Value.mSuffix.Equals("mp4"))
                {
                    savePath = "Assets/bin/data/audio/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                    pair.Value.mPath = "data/audio/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                }
                else if (pair.Value.mSuffix.Equals("unity"))
                {
                    //pair.Value.mPath = "data/scene/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                    savePath = "Assets/bin/data/scene/" + BuildCommon.GetLevelABPath(pair.Value.mPath);
                }
                BuildCommon.CheckFolder(BuildCommon.getPath(savePath));
                //普通资源
                if (pair.Value.mSuffix != "unity")
                {
                    string assetName = pair.Value.mPath.Replace("Assets/", "");
                    uint ver;
                    if (!BuildPipeline.BuildAssetBundle( asset ,null, savePath,out ver, options, buildPlatform))
                    {
                        Debug.LogError("Build Assetbundle:" + savePath + " Failed");
                    }
                }
                //场景资源
                else 
                {
                    AssetDatabase.Refresh();
                    BuildPipeline.PushAssetDependencies();
                    string error = BuildPipeline.BuildStreamedSceneAssetBundle(new string[] { pair.Value.mPath }, savePath, buildPlatform);
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.LogError(error);
                    }
                    BuildPipeline.PopAssetDependencies();
                }
            }
        }
        for (int level = 1; level <= maxLevel; level++)
        {
            BuildPipeline.PopAssetDependencies();
        }
    }
    private static void DumpResourceInfoFile()
    {
        if (allLevelAssets.Count == 0)
        {
            return;
        }
        CollectDepResourceDataMap uiMap = new CollectDepResourceDataMap();
        CollectDepResourceDataMap effectMap = new CollectDepResourceDataMap();
        CollectDepResourceDataMap atlasMap = new CollectDepResourceDataMap();
        for (int level = 1; level <= allLevelAssets.Count; level++)
        {
            Dictionary<string, AssetUnit> levelAssets = allLevelAssets[level];
            foreach (var current in levelAssets)//Assets/Resources/ui/233.prefab
            {
                //取得资源的名称，不包括后缀名
                current.Value.mAssetSize = GetFileSize(current.Value.mPath);
                foreach (var dep in current.Value.mAllDependencies)
                {
                    for (int level1 = 1; level1 <= allLevelAssets.Count; level1++)
                    {
                        Dictionary<string, AssetUnit> levelAssets1 = allLevelAssets[level1];
                        foreach (var current1 in levelAssets1)
                        {
                            if (dep == current1.Key)
                            {
                                current1.Value.mRefCount++;
                            }
                        }
                    }
                }
                if (current.Value.mAllDependencies.Count >= 2)
                {
                    try
                    {
                        current.Value.mAllDependencies.Sort((left, right) =>
                        {
                            if (allFiles[left].mIndex > allFiles[right].mIndex)
                            {
                                return 1;
                            }
                            else if (allFiles[left].mIndex == allFiles[right].mIndex)
                            {
                                return 0;
                            }
                            else
                            {
                                return -1;
                            }
                        });
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }
            }
        }
        for (int level2 = 1; level2 <= allLevelAssets.Count; level2++)
        {
            Dictionary<string, AssetUnit> levelAssets = allLevelAssets[level2];
            foreach (var current in levelAssets)
            {
                Debug.Log(current.Value.mName);
                if (current.Value.mName.Contains("ui_"))
                {
                    uiMap.InitResourceData(current.Value,allFiles);
                    if (level2 > 1)
                    {
                        uiMap.InitCollectDepData(current.Value);
                    }
                }
                if (current.Value.mName.Contains("atla"))
                {
                    atlasMap.InitResourceData(current.Value,allFiles);
                    if (level2 > 1)
                    {
                        atlasMap.InitCollectDepData(current.Value);
                    }
                }
            }
        }
        WirteToFile(JsonMapper.ToJson(uiMap), "ui.config");
        WirteToFile(JsonMapper.ToJson(atlasMap), "atlas.config");
    }
    private static void DumpVerstionFile()
    {
 
    }
    /// <summary>
    /// 取得文件的大小
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static int GetFileSize(string path)
    {
        string realPath = Application.dataPath + "/bin/" + path;
        FileStream fs = new FileStream(realPath,FileMode.Open);
        long length = fs.Length;
        fs.Close();
        return (int)length;
    }
    /// <summary>
    /// 将文本写入到文件
    /// </summary>
    /// <param name="content"></param>
    /// <param name="fileName"></param>
    private static void WirteToFile(string content,string fileName)
    {
        using (FileStream fs = new FileStream(Application.dataPath + "/bin/data/" + fileName, FileMode.OpenOrCreate))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(content);
            }         
        }
    }
	#endregion
}
