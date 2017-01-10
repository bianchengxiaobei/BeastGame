using UnityEngine;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UIButtonKeyBinding
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
[AddComponentMenu("Game/UI/Button Key Binding")]
public class UIButtonKeyBinding : MonoBehaviour
{
    public KeyCode keyCode;
    private void Update()
    {
        if (!UICamera.inputHasFocus)
        {
            if (this.keyCode == KeyCode.None)
            {
                return;
            }
            if (Input.GetKeyDown(this.keyCode))
            {
                base.SendMessage("OnPress", true, SendMessageOptions.DontRequireReceiver);
            }
            if (Input.GetKeyUp(this.keyCode))
            {
                base.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
                base.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
