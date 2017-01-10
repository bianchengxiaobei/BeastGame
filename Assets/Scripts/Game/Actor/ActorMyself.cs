using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：ActorMyself 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.17
// 模块描述：角色自己Actor行为对象
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 角色自己Actor行为对象
/// </summary>
namespace Game
{
    public class ActorMyself : ActorPlayer<EntityMyself>
    {
        #region 字段
        public GameMotor m_motor;
        public bool IsMoving = false;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公共方法
        void Start()
        {
            gameObject.layer = 8;
            DontDestroyOnLoad(this);
        }
        void Update()
        {
            ActChange();
            ProcessMotorInput();
            if (this.Entity == null)
            {
                return;
            }

        }
        #endregion
        #region 私有方法
        private void ProcessMotorInput()
        {
            if (this.Entity == null)
            {
                return;
            }
            if (m_motor.enableStick)
            {
                if (GameInputManager.singleton.IsMoving)
                {
                    IsMoving = true;
                    Vector3 direction = GameInputManager.singleton.Direction;
                    Vector3 orginPos = GameInputManager.singleton.OrginPos;
                    this.Entity.Move();
                }
                else if (!m_motor.isMovingToTarget)
                {
                    if (IsMoving)
                    {
                        Debug.Log("Enitty");
                        IsMoving = false;
                        this.Entity.Idle();
                    }
                }             
            }
        }
        #endregion
        #region 析构方法
        #endregion
    }
}