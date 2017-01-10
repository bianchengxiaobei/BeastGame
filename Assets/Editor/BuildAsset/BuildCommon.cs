using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.IO;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：BuildCommon
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class BuildCommon 
{
    public static string getFolder(string path)
    {
        path = path.Replace("\\", "/");
        int index = path.LastIndexOf("/");
        if (-1 == index)
            throw new Exception("can not find /!!!");
        return path.Substring(index + 1, path.Length - index - 1);
    }

    public static string getFileName(string fileName)
    {
        int index = fileName.IndexOf(".");
        if (-1 == index)
            throw new Exception("can not find .!!!");
        return fileName.Substring(0, index);
    }
    /// <summary>
    /// 根据文件路径获取文件名，是否带后缀
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="suffix">是否带后最</param>
    /// <returns></returns>
    public static string getFileName(string filePath, bool suffix)
    {
        if (!suffix)
        {
            string path = filePath.Replace("\\", "/");
            int index = path.LastIndexOf("/");
            if (-1 == index)
                throw new Exception("can not find .!!!");
            int index2 = path.LastIndexOf(".");
            if (-1 == index2)
                throw new Exception("can not find /!!!");
            return path.Substring(index + 1, index2 - index - 1);
        }
        else
        {
            string path = filePath.Replace("\\", "/");
            int index = path.LastIndexOf("/");
            if (-1 == index)
                throw new Exception("can not find /!!!");
            return path.Substring(index + 1, path.Length - index - 1);
        }
    }
    /// <summary>
    /// 获取文件后缀名
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string getFileSuffix(string filePath)
    {
        int index = filePath.LastIndexOf(".");
        if (-1 == index)
            throw new Exception("can not find Suffix!!! the filePath is : " + filePath);
        return filePath.Substring(index + 1, filePath.Length - index - 1);
    }

    public static void getFiles(string path, ref Dictionary<string, AssetUnit> list)
    {
        string[] dirs = Directory.GetDirectories(path);
        foreach (string dir in dirs)
        {
            if (getFolder(dir) == ".svn")
                continue;
            getFiles(dir, ref list);
        }

        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            string suffix = getFileSuffix(file);
            if (suffix == "meta" || suffix == "dll")
                continue;
            string realFile = file.Replace("\\", "/");
            realFile = "Assets" + realFile.Replace(Application.dataPath, "");
            list.Add(realFile, new AssetUnit(realFile));
        }
    }

    public static void GetFiles(string path, List<string> list, bool recursion)
    {
        if (recursion)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                if (getFolder(dir) == ".svn")
                    continue;
                GetFiles(dir, list, recursion);
            }
        }

        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            string suffix = getFileSuffix(file);
            if (suffix == "meta")
                continue;
            string realFile = file.Replace("\\", "/");
            realFile = realFile.Replace(Application.streamingAssetsPath + "/", "");
            list.Add(realFile);
        }
    }

    public static void getFloder(string path, string speicalName, bool recursion, List<string> allPath)
    {
        if (recursion)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                if (getFolder(dir) == speicalName)
                    allPath.Add(dir);
                getFloder(dir, speicalName, recursion, allPath);
            }
        }
    }
    /// <summary>
    /// 资源所在层级
    /// </summary>
    /// <param name="filePath">资源路径</param>
    /// <returns></returns>
    public static int getAssetLevel(string filePath)
    {
        //取得资源所有引用，包括脚本cs
        string[] depencys = AssetDatabase.GetDependencies(new string[] { filePath });
        List<string> deps = new List<string>();
        foreach (string file in depencys)
        {
            //排除关联脚本
            string suffix = BuildCommon.getFileSuffix(file);//后缀名
            if (suffix == "dll")//也就说只要是脚本就舍弃，不加到引用资源里面
                continue;
            deps.Add(file);
        }
        if (deps.Count == 1)
            return 1;
        int maxLevel = 0;
        foreach (string file in deps)
        {
            if (file == filePath)
                continue;
            int level = getAssetLevel(file);
            maxLevel = maxLevel > level + 1 ? maxLevel : level + 1;
        }
        return maxLevel;
    }
    /// <summary>
    /// 检测是否存在该目录，如没有就创建
    /// </summary>
    /// <param name="path"></param>
    public static void CheckFolder(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }

    public static string getPath(string filePath)
    {
        Debug.Log(filePath);
        string path = filePath.Replace("\\", "/");
        int index = path.LastIndexOf("/");
        if (-1 == index)
            throw new Exception("can not find /!!!");
        return path.Substring(0, index);
    }

    public static bool isDependenced(string asset, string sourceAsset)
    {
        string[] deps = AssetDatabase.GetDependencies(new string[] { sourceAsset });
        bool isDep = false;
        foreach (string path in deps)
        {
            if (path == sourceAsset)
                continue;

            if (path == asset)
                return true;
            isDep = isDependenced(asset, path);
        }
        return isDep;
    }

    public static bool isSingleDependenced(AssetUnit asset)
    {
        if (asset.mDirectUpperDependences.Count > 1)
            return false;
        else if (asset.mDirectUpperDependences.Count == 1)
            return isSingleDependenced(asset.mDirectUpperDependences[0]);
        else
            return true;
    }
    public static string GetLevelABPathName(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
        if (!path.Contains("Assets/Resources"))
        {
            string assetPath = getFileName(path.Replace("/", "_").ToLower());
            //return assetPath.Insert(assetPath.Length, ResourceCommon.assetbundleFileSuffix);
            return assetPath;
        }
        else 
        {
            string removePath = path.Remove(0, 17);
            string assetPath = getFileName(removePath.Replace("/", "_").ToLower());
            //return assetPath.Insert(assetPath.Length, ResourceCommon.assetbundleFileSuffix);
            return assetPath;
        }
    }
    public static string GetLevelABPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }
        if (!path.Contains("Assets/Resources"))
        {
            string name = GetLevelABPathName(path);
            return name.Insert(name.Length, ResourceCommon.assetbundleFileSuffix);
        }
        else 
        {
            string removePath = path.Remove(0, 17);
            string assetName = GetLevelABPathName(removePath);
            string path1 =  Path.GetDirectoryName(removePath) +"/"+ assetName;
            return path1.Insert(path1.Length, ResourceCommon.assetbundleFileSuffix);
        }
    }
}
