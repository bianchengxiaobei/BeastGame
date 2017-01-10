using UnityEngine;
using System.Collections;
using Utility;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ConfigCS
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.4
// 模块描述：电脑配置信息
//----------------------------------------------------------------*/
#endregion
public class ConfigCS : Singleton<ConfigCS>
{
    private const int cosShaderModel = 30;
    public string m_DeviceModel;//电脑cpu型号
    public string m_deviceName;//电脑名称
    public DeviceType m_deviceType;//程序运行在设备的桌面、掌上、命令窗口
    public string m_graphicsDeviceName;
    public string m_graphicsDeviceVersion;
    public int m_graphicsPixelFillrate;//显卡像素填充率，百万像素/秒
    public int m_graphicsMemorySize;//显存大小
    public int m_graphicsShaderLevel;
    public int m_maxTextureSize;
    public NPOTSupport m_npotSupport;
    public string m_operatingSystem;
    public int m_processorCount;
    public string m_processorType;
    public bool m_supports3DTextures;
    public bool m_supportsImageEffects;
    public bool m_supportsRenderTextures;
    public int m_systemMemorySize;
    private IXLog m_log = XLog.GetLog<ConfigCS>();
    public void Init()
    {
        this.GetPlayerPC(true);
        if (this.IsLowPC())
        {
            this.CloseFA();
        }
    }
    /// <summary>
    /// 得到PC电脑的配置信息
    /// </summary>
    /// <param name="IsLog"></param>
    public void GetPlayerPC(bool IsLog = true)
    {
        this.m_DeviceModel = SystemInfo.deviceModel;
        this.m_deviceName = SystemInfo.deviceName;
        this.m_deviceType = SystemInfo.deviceType;
        this.m_graphicsDeviceName = SystemInfo.graphicsDeviceName;
        this.m_graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
        this.m_graphicsMemorySize = SystemInfo.graphicsMemorySize;
        this.m_graphicsPixelFillrate = SystemInfo.graphicsPixelFillrate;
        this.m_graphicsShaderLevel = SystemInfo.graphicsShaderLevel;
        this.m_maxTextureSize = SystemInfo.maxTextureSize;
        this.m_npotSupport = SystemInfo.npotSupport;
        this.m_operatingSystem = SystemInfo.operatingSystem;
        this.m_processorCount = SystemInfo.processorCount;
        this.m_processorType = SystemInfo.processorType;
        this.m_supports3DTextures = SystemInfo.supports3DTextures;
        this.m_supportsImageEffects = SystemInfo.supportsImageEffects;
        this.m_supportsRenderTextures = SystemInfo.supportsRenderTextures;
        this.m_systemMemorySize = SystemInfo.systemMemorySize;
        if (IsLog)
        {
            this.m_log.Debug(string.Concat(new object[]
			{
				"  DeviceModel   ",
				this.m_DeviceModel,
				"\r\n deviceName   ",
				this.m_deviceName,
				"\r\n deviceType   ",
				this.m_deviceType,
				"\r\n graphicsDeviceName   ",
				this.m_graphicsDeviceName,
				"\r\n  graphicsDeviceVersion   ",
				this.m_graphicsDeviceVersion,
				"\r\n  graphicsMemorySize   ",
				this.m_graphicsMemorySize,
				"\r\n  graphicsPixelFillrate   ",
				this.m_graphicsPixelFillrate,
				"\r\n  graphicsShaderLevel   ",
				this.m_graphicsShaderLevel,
				"\r\n  maxTextureSize   ",
				this.m_maxTextureSize,
				"\r\n  npotSupport   ",
				this.m_npotSupport,
				"\r\n  operatingSystem   ",
				this.m_operatingSystem,
				"\r\n  processorCount   ",
				this.m_processorCount,
				"\r\n  processorType   ",
				this.m_processorType,
				"\r\n  supports3DTextures   ",
				this.m_supports3DTextures,
				"\r\n  supportsImageEffects   ",
				this.m_supportsImageEffects,
				"\r\n  supportsRenderTextures   ",
				this.m_supportsRenderTextures,
				"\r\n  systemMemorySize   ",
				this.m_systemMemorySize
			}));
        }
    }
    /// <summary>
    /// 获取图像质量的等级
    /// </summary>
    /// <returns></returns>
    public int GetQaLevel()
    {
        int result;
        if (this.m_graphicsShaderLevel < 30 || this.m_graphicsMemorySize < 512 || this.m_systemMemorySize < 2000 || !this.m_supportsImageEffects)
        {
            result = 0;
        }
        else
        {
            result = 1;
        }
        return result;
    }
    /// <summary>
    /// 是否是低等级的电脑配置
    /// </summary>
    /// <returns></returns>
    public bool IsLowPC()
    {
        return this.m_graphicsShaderLevel < 30 || this.m_graphicsMemorySize < 512 || this.m_systemMemorySize < 2000 || !this.m_supportsImageEffects;
    }
    /// <summary>
    /// 关闭抗锯齿
    /// </summary>
    public void CloseFA()
    {
        if (!(Camera.main == null))
        {
            MonoBehaviour monoBehaviour = Camera.main.GetComponent("AntialiasingAsPostEffect") as MonoBehaviour;
            if (!(monoBehaviour == null))
            {
                monoBehaviour.enabled = !monoBehaviour.enabled;
            }
        }
    }
    /// <summary>
    /// 获取电脑配置信息的字符串
    /// </summary>
    /// <returns></returns>
    public string GetConfigAsString()
    {
        return string.Concat(new object[]
		{
			"\r\n DeviceModel   ",
			this.m_DeviceModel,
			"\r\n deviceName   ",
			this.m_deviceName,
			"\r\n deviceType   ",
			this.m_deviceType,
			"\r\n graphicsDeviceName   ",
			this.m_graphicsDeviceName,
			"\r\n graphicsDeviceVersion   ",
			this.m_graphicsDeviceVersion,
			"\r\n graphicsMemorySize   ",
			this.m_graphicsMemorySize,
			"\r\n graphicsPixelFillrate   ",
			this.m_graphicsPixelFillrate,
			"\r\n graphicsShaderLevel   ",
			this.m_graphicsShaderLevel,
			"\r\n maxTextureSize   ",
			this.m_maxTextureSize,
			"\r\n npotSupport   ",
			this.m_npotSupport,
			"\r\n operatingSystem   ",
			this.m_operatingSystem,
			"\r\n processorCount   ",
			this.m_processorCount,
			"\r\n processorType   ",
			this.m_processorType,
			"\r\n supports3DTextures   ",
			this.m_supports3DTextures,
			"\r\n supportsImageEffects   ",
			this.m_supportsImageEffects,
			"\r\n supportsRenderTextures   ",
			this.m_supportsRenderTextures,
			"\r\n systemMemorySize   ",
			this.m_systemMemorySize
		});
    }
}
