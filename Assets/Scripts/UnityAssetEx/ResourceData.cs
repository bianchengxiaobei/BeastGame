using UnityEngine;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ResourceData
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：资源数据（包括引用资源）
//----------------------------------------------------------------*/
#endregion
namespace UnityAssetEx.Export
{
    public class ResourceData
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string mResourceName;
        /// <summary>
        /// 资源路径
        /// </summary>
        public string mPath;
        /// <summary>
        /// 资源大小
        /// </summary>
        public int mSize;//资源大小
        /// <summary>
        /// 资源类型，声音，文件，视频，字体等等
        /// </summary>
        public EnumAssetType mType;
        /// <summary>
        /// 被其他资源引用的次数
        /// </summary>
        public int mRefCount;
        /// <summary>
        /// 是否开始检测引用
        /// </summary>
        public bool mHasCheckRef;
        /// <summary>
        /// 创建资源
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="size"></param>
        /// <param name="eResourceType"></param>
        /// <returns>资源对象实例</returns>
        public static ResourceData Create(string name, string path, int size, EnumAssetType eResourceType)
        {
            return new ResourceData
            {
                mResourceName = name,
                mPath = path,
                mSize = size,
                mType = eResourceType,
                mRefCount = 1,
                mHasCheckRef = false
            };
        }
    }
}
