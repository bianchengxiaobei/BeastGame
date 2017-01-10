using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MakeAssetbundle
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class MakeAssetbundle 
{
	[MenuItem("Custom Editor/Create AssetBunldes Main")]
	static void CreateAssetBunldesMain ()
	{
        //获取在Project视图中选择的所有游戏对象
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
 
        //遍历所有的游戏对象
		foreach (Object obj in SelectedAsset) 
		{
			string sourcePath = AssetDatabase.GetAssetPath (obj);
			//本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径
			//StreamingAssets是只读路径，不能写入
			//服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。
			string targetPath = Application.dataPath + "/" + obj.name + ".ab";
			if (BuildPipeline.BuildAssetBundle (obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies)) {
  				Debug.Log(obj.name +"资源打包成功");
			} 
			else 
			{
 				Debug.Log(obj.name +"资源打包失败");
			}
		}
		//刷新编辑器
		AssetDatabase.Refresh ();	
 
	}
    [MenuItem("Custom Editor/test")]
    public static void test()
    {
        Object[] select = Selection.GetFiltered(typeof(Object), SelectionMode.Deep);
        for (int i = 0; i < select.Length; i++)
        {
            Debug.Log(select[i].name);
        }
        AssetDatabase.Refresh();
    }
    [MenuItem("Custom Editor/Create Material")]
    public static void CreateMaterial()
    {
        Material m = new Material(Shader.Find("Specular"));
        AssetDatabase.CreateAsset(m, "Assets/MyMaterial.mat");
        AnimationClip clip = new AnimationClip();
        clip.name = "MyClip";
        AssetDatabase.AddObjectToAsset(clip, m);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(clip));
        Debug.Log(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(m)));
    }
    [MenuItem("Custom Editor/GetDeepAsset")]
    static void GetDeepAssetPath()
    {
       
    }
    [MenuItem("Custom Editor/FindAssetSerach")]
    static void Find()
    {
        string[] guids = AssetDatabase.FindAssets("B", null);
        Debug.Log(guids.Length);
        foreach (var id in guids)
        {
            Debug.Log(AssetDatabase.GUIDToAssetPath(id));
        }
    }
    [MenuItem("Custom Editor/GetAssetPathOrScene")]
    static void GetAssetOrScene()
    {
        Object asset = Selection.activeObject;
        string path = AssetDatabase.GetAssetOrScenePath(asset);
        Debug.Log(path);
    }
    [MenuItem("Custom Editor/GetDependencies")]
    static void GetDependencies()
    {
        Object[] asset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        string[] path = new string[asset.Length];
        for (int i = 0; i < asset.Length; i++)
        {
            path[i] = AssetDatabase.GetAssetPath(asset[i]);
        }

       string[] depend =  AssetDatabase.GetDependencies(path);
       for (int i = 0; i < depend.Length; i++)
       {
           Debug.Log(depend[i]);
       }
    }
    [MenuItem("Custom Editor/GetTextMetaDataPathFromAssetPath")]
    static void GetTextMetaDataPath()
    {
        Object asset = Selection.activeObject;
        string path = AssetDatabase.GetTextMetaDataPathFromAssetPath(AssetDatabase.GetAssetPath(asset));
        Debug.Log(path);
    }
    [MenuItem("Custom Editor/Make assetbundle into config")]
    public static void MakeAssetbundleIntoConfig()
    {
        Object asset = Selection.activeObject;
        string uiPath = AssetDatabase.GetAssetPath(asset);
        string[] ABPath = FindAsset(new string[1]{uiPath});
    }
    /// <summary>
    /// 在父亲文件夹下查找所需要的文件
    /// </summary>
    /// <param name="parentFolder">在这个文件夹下，查找文件</param>
    /// <returns>所需要的文件路径</returns>
    static string[] FindAsset(string[] parentFolder)
    {
        string[] guid = AssetDatabase.FindAssets("l:concrete", parentFolder);
        string[] assetPath = new string[guid.Length];
        for (int i = 0; i < guid.Length; i++)
        {
            assetPath[i] = AssetDatabase.GUIDToAssetPath(guid[i]);
            Debug.Log(assetPath[i]);
        }
        return assetPath;
    }
    static string GetDicCollectDepResourceData(string[] ABPath)
    {
        string text = "\"mDicCollectDepResourceData\":{";//"mDicCollectDepResourceData:{"
        foreach (var path in ABPath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\"" + Path.GetFileNameWithoutExtension(path) + "\":");//"ui_dlgbeginanimation_dlgbeginanimation"
            sb.AppendLine("{\"mResourceName\":\"" + Path.GetFileNameWithoutExtension(path) + "\","+//"mResourceName":"ui_dlgbeginanimation_dlgbeginanimation",
            "\"mDependResourceName\":["+    "]},");
        }
        return null;
    }
    static string[] GetABDependencies(string filePath)
    {
        string[] path = AssetDatabase.GetDependencies(new string[1]{filePath});
        for (int i = 0; i < path.Length; i++)
        {
            if (path[i].EndsWith(".cs"))
            {
                continue;
            }
        }
        return null;
    }
}
