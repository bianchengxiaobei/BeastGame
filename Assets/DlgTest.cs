using UnityEngine;
using System.Collections;
using Client.UI.UICommon;
using Client.UI;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgTest
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.12
// 模块描述：
----------------------------------------------------------------*/
#endregion
/// <summary>
/// 
/// </summary>
public class DlgTest : DlgBase<DlgTest,DlgTestBehaviour>
{
	#region 字段
	#endregion
	#region 属性
    public override string fileName
    {
        get
        {
            return "DlgTest";
        }
    }
	#endregion
	#region 构造方法
	#endregion
	#region 公有方法
    protected override void OnShow()
    {
        base.OnShow();
        
    }
	#endregion
	#region 私有方法
	#endregion
}
