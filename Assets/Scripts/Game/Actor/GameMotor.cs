using UnityEngine;
using System.Collections;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GameMotor
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public class GameMotor : MonoBehaviour
    {
        #region 字段
        /// <summary>
        /// 是否使用输入设备
        /// </summary>
        public bool enableStick = true;
        /// <summary>
        /// 碰撞标记
        /// </summary>
        protected CollisionFlags collisionFlags = CollisionFlags.CollidedBelow;
        /// <summary>
        /// 垂直方向上的速度
        /// </summary>
        public float verticalSpeed = 0.0f;
        /// <summary>
        /// 是否正在跳跃
        /// </summary>
        protected bool isFlying = false;
        /// <summary>
        /// 重力值
        /// </summary>
        public float gravity = 20.0f;
        /// <summary>
        /// 是否能够移动
        /// </summary>
        public bool canMove = true;
        /// <summary>
        /// 是否向目标移动
        /// </summary>
        public bool isMovingToTarget = false;
        /// <summary>
        /// 移动目的地
        /// </summary>
        public Vector3 targetToMoveTo;
        /// <summary>
        /// 是否是在移动
        /// </summary>
        public bool isMovable;
        /// <summary>
        /// 速度
        /// </summary>
        public float speed = 0f;
        /// <summary>
        /// 附加速度
        /// </summary>
        public float extraSpeed = 0f;
        /// <summary>
        /// 目标速度
        /// </summary>
        public float targetSpeed;
        /// <summary>
        /// 加速度
        /// </summary>
        public float acceleration = 2;
        /// <summary>
        /// 转向y的值
        /// </summary>
        public float RotationY = 0f;
        /// <summary>
        /// 旋转速度
        /// </summary>
        private float acturalAngularSpeed = 1440f;

        private float angularSpeed = 1440f;
        /// <summary>
        /// 选择速度比例，默认为1
        /// </summary>
        private float angularSpeedRate = 1.0f;
        /// <summary>
        /// 移动方向
        /// </summary>
        protected Vector3 moveDirection = Vector3.one;
        /// <summary>
        /// 是否启用输入设备控制人物面朝
        /// </summary>
        public bool enableRotation = true;
        /// <summary>
        /// 是否能转向
        /// </summary>
        public bool canTurn = false;
        /// <summary>
        /// 是否能设置转向
        /// </summary>
        public bool IsRotationTo = false;
        /// <summary>
        /// 是否正在旋转
        /// </summary>
        public bool IsTurning = false;
        /// <summary>
        /// 是否看着某个方向
        /// </summary>
        protected bool isLookingAtTarget = false;
        /// <summary>
        /// 看着某个方向值
        /// </summary>
        protected Vector3 targetToLookAt;
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetSpeed(float value)
        {
            this.speed = value;
        }
        /// <summary>
        /// 设置附加速度
        /// </summary>
        /// <param name="extra"></param>
        public void SetExtraSpeed(float extra)
        {
            extraSpeed = extra;
        }
        /// <summary>
        /// 根据加速度acceleration，缓慢增加速度
        /// </summary>
        /// <param name="originalSpeed"></param>
        /// <param name="_targetSpeed"></param>
        /// <returns></returns>
        protected float AccelerateSpeed(float originalSpeed, float _targetSpeed)
        {
            if (Mathf.Abs(originalSpeed - _targetSpeed) < acceleration * Time.deltaTime)
            {
                originalSpeed = _targetSpeed;
            }
            else if (originalSpeed - _targetSpeed > 0)
            {
                originalSpeed -= acceleration * Time.deltaTime;
            }
            else
            {
                originalSpeed += acceleration * Time.deltaTime;
            }
            return originalSpeed;
        }
        public void SetMoveDirection(Vector3 _direction, Space space = Space.World)
        {
            if (space == Space.Self)
            {
                this.moveDirection = transform.TransformDirection(_direction);
            }
            else if(space == Space.World)
            {
                this.moveDirection = _direction;
            }
        }
        /// <summary>
        /// 设置旋转速度
        /// </summary>
        /// <param name="speed"></param>
        public void SetAngularSpeed(float speed)
        {
            angularSpeed = speed;
            acturalAngularSpeed = angularSpeed * angularSpeedRate;
        }
        public void RotateTo(float targetAngleY)
        {
            if (!canTurn)
            {
                return;
            }
            IsRotationTo = true;
            RotationY = targetAngleY;
        }
        /// <summary>
        /// 旋转y轴
        /// </summary>
        /// <param name="targetAngleY"></param>
        protected void ApplyRotation(Quaternion rotation)
        {
            /*float m = targetAngleY % 360;
            float n = transform.eulerAngles.y % 360;
            if (m < 0) m += 360;
            if (n < 0) n += 360;
            if (m - n < -180) m += 360;
            float dY = (m - n);

            float angularStep = Time.deltaTime * acturalAngularSpeed;


            if (Mathf.Abs(dY) < angularStep)
            {
                transform.eulerAngles = new Vector3(0, targetAngleY, 0);
                IsTurning = false;
                IsRotationTo = false;
            }
            else
            {
                int j = (dY > 0 && dY < 180) ? 1 : -1;
                transform.Rotate(new Vector3(0, j * angularStep, 0), Space.Self);
                IsTurning = true;
            }*/
            float angularStep = Time.deltaTime * acturalAngularSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, angularStep);
        }
        /// <summary>
        /// 是否在地面上
        /// </summary>
        /// <returns></returns>
        protected bool IsGrounded()
        {
            return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
            
        }
        /// <summary>
        /// 调整坐标位置，如果坐标的y在地面以下的话，就调整
        /// </summary>
        protected void AdjustPosition()
        {
            if (GameWorld.isLoadingScene)
            {
                return;
            }
            if (transform.position.y < -100 && transform.position.y > -9000)
            {
                Vector3 temp;
                bool hasHit = UnityTools.GetPointInTerrain(transform.position.x, transform.position.z, out temp);
                if (!hasHit)
                {

                }
                else
                {
                    transform.position = temp;
                }
            }
        }
        #endregion
        #region 子类重写方法
        /// <summary>
        /// 通过Navmesh来寻路
        /// </summary>
        /// <param name="v"></param>
        /// <param name="stopDistance"></param>
        /// <param name="needToAdjustPosY"></param>
        /// <returns></returns>
        public virtual bool MoveToByNav(Vector3 v,float stopDistance = 0f,bool needToAdjustPosY = true)
        {
            return false;
        }
        /// <summary>
        /// 移动到指定目的点
        /// </summary>
        /// <param name="v"></param>
        /// <param name="needToAdjustPosY"></param>
        public virtual void MoveTo(Vector3 target, bool needToAdjustPosY = true)
        {

        }
        /// <summary>
        /// 停止寻路
        /// </summary>
        public virtual void StopNav()
        {

        }
        /// <summary>
        /// 设置是否在跳跃状态，子类选择关闭一些输入操作
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetFlying(bool value)
        {
            this.isFlying = value;
        }
        /// <summary>
        /// 传送角色到目的地
        /// </summary>
        /// <param name="destination"></param>
        public virtual void TeleportTo(Vector3 destination)
        {
            transform.position = destination;
        }
        #endregion
    }
}
