using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GameInputManager 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.17
// 模块描述：游戏世界输入设备管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏世界输入设备管理器
/// </summary>
namespace Game
{
    public class GameInputManager
    {
        private IGameInputManager m_inputManager;
        private static GameInputManager m_oInstance;
        public static GameInputManager singleton
        {
            get
            {
                if (GameInputManager.m_oInstance == null)
                {
                    GameInputManager.m_oInstance = new GameInputManager();
                }
                return GameInputManager.m_oInstance;
            }
        }
        public void Init(IGameInputManager editorInputManager,EntityParent theOwner)
        {
            if (RuntimePlatform.WindowsEditor == Application.platform)
            {
                this.m_inputManager = editorInputManager;
            }
            if (this.m_inputManager != null)
            {
                this.m_inputManager.Init(theOwner);
            }
        }
        /// <summary>
        /// 是否输入正在移动
        /// </summary>
        public bool IsMoving
        {
            get
            {
                if (this.m_inputManager == null)
                {
                    Debug.LogError("InputManager == null");
                }
                return this.m_inputManager.IsMoving;
            }
        }
        public Vector2 Direction
        {
            get
            {
                if (this.m_inputManager == null)
                {
                    Debug.LogError("InputManager == null");
                }
                return this.m_inputManager.Direction;
            }
            set
            {
                if (this.m_inputManager == null)
                {
                    Debug.LogError("InputManager == null");
                }
                this.m_inputManager.Direction = value;
            }
        }
        public Vector3 OrginPos
        {
            get
            {
                if (this.m_inputManager == null)
                {
                    Debug.LogError("InputManager == null");
                }
                return this.m_inputManager.OrginPos;
            }
            set
            {
                if (this.m_inputManager == null)
                {
                    Debug.LogError("InputManager == null");
                }
                this.m_inputManager.OrginPos = value;
            }
        }

        public void Reset()
        {
            if (this.m_inputManager == null)
            {
                Debug.LogError("InputManager == null");
            }
            this.m_inputManager.Reset();
        }
    }
}
