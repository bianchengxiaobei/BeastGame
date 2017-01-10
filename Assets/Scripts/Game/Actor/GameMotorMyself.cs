using UnityEngine;
using Utility.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GameMotorMyself 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.17
// 模块描述：自己游戏角色行为驱动器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 游戏主角行为驱动器
/// </summary>
namespace Game
{
    public class GameMotorMyself : GameMotor
    {
        #region 字段
        private Animator m_animator;
        private CharacterController m_characterController;
        private float m_time = 0;
        private uint m_cornersIdx = 0;
        private uint m_timerIdForNav;
        private NavMeshPath path;
        private GameNavmeshHelper m_navmeshHelper;
        private bool m_isMovingOn = false;
        protected float m_stopDistance = 0f;
        #endregion
        #region 属性
        public CharacterController CharacterController
        {
            get
            {
                return this.m_characterController;
            }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公共方法
        #endregion
        #region 私有方法
        void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_characterController = GetComponent<CharacterController>();
            enableStick = true;
            m_navmeshHelper = new GameNavmeshHelper(transform);
        }
        void Start()
        {
            InvokeRepeating("AdjustPosition", 0, 1);
            SetAngularSpeed(100000);
        }
        void OnDestory()
        {
            CancelInvoke("AdjuestPosition");
        }
        void Update()
        {
            ApplyGravity();
            if (!canMove)
            {
                return;
            }
            if (!m_animator.runtimeAnimatorController)
            {
                return;
            }
            if (isLookingAtTarget)
            {
                transform.LookAt(new Vector3(targetToLookAt.x, transform.position.y, targetToLookAt.z));
            }
            if (enableStick && GameInputManager.singleton != null && GameInputManager.singleton.IsMoving)
            {
                StopNav();
                ///如果正在移动中
                if (GameInputManager.singleton.IsMoving)
                {
                    if (enableRotation)
                    {
                        ApplyRotation();
                        moveDirection = transform.forward;
                    }
                    else
                    {
                        int i = GameInputManager.singleton.Direction.x > 0 ? 1 : -1;
                        float targetAngleY = i * Vector2.Angle(new Vector2(0, -1), GameInputManager.singleton.Direction);
                        Vector3 original = transform.eulerAngles;
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngleY, transform.eulerAngles.z);
                        moveDirection = transform.forward;
                        transform.eulerAngles = original;
                    }
                }
                else
                {
                    if (enableRotation)
                    {
                        ApplyRotation();
                        moveDirection = transform.forward;
                        GameWorld.thePlayer.Idle();
                    }
                }
            }
            else if (isMovingToTarget)
            {
                MoveTo(targetToMoveTo, false);
            }
            speed = AccelerateSpeed(speed, targetSpeed);
            m_animator.SetFloat("Speed", speed);
            Move();
        }
        /// <summary>
        /// 应用重力
        /// </summary>
        private void ApplyGravity()
        {
            if (IsGrounded())
            {
                if (isFlying)
                {
                    if (Time.time - m_time < 0.2f)
                    {
                        return;
                    }
                    SetFlying(false);
                    verticalSpeed = 0.0f;
                }
                else
                {
                    verticalSpeed = 0.0f;
                }
            }
            else
            {
                verticalSpeed -= gravity * Time.deltaTime;
            }          
        }
        /// <summary>
        /// 应用旋转
        /// </summary>
        private void ApplyRotation()
        {
            if (Camera.main == null || GameWorld.thePlayer == null)
            {
                return;
            }
            /*float targetAngleY = Vector2.Angle(GameInputManager.singleton.Direction, new Vector2(0,-1));
            Debug.Log("TargetAngleY:" + targetAngleY);
            */
            Vector3 dir = new Vector3(GameInputManager.singleton.Direction.x, 0, GameInputManager.singleton.Direction.y);
            Quaternion rotation = Quaternion.LookRotation(dir);
            if (GameWorld.thePlayer.isBackDirection) rotation = Quaternion.Inverse(rotation);         
            base.ApplyRotation(rotation);
        }
        private void Move()
        {
            collisionFlags = m_characterController.Move((extraSpeed * moveDirection + new Vector3(0, verticalSpeed, 0)) * Time.deltaTime);
        }
        #endregion


        #region 子类重写方法
        /// <summary>
        /// 设置是否在飞行状态，如果飞行的话，不使用虚拟摇杆控制，不能旋转，然后动画不能改变坐标
        /// </summary>
        /// <param name="value"></param>
        public override void SetFlying(bool value)
        {
            base.SetFlying(value);
            if (value)
            {
                m_time = Time.time;
                enableStick = false;
                enableRotation = false;
                m_animator.applyRootMotion = false;
            }
            else
            {
                extraSpeed = 0f;
                enableRotation = true;
                m_animator.applyRootMotion = true;
                enableStick = true;
            }
        }
        public override void StopNav()
        {
            m_cornersIdx = 1;
            isMovingToTarget = false;
            m_isMovingOn = false;
            TimerHeap.DelTimer(m_timerIdForNav);
        }
        /// <summary>
        /// 移动到指定目的
        /// </summary>
        /// <param name="target"></param>
        /// <param name="needToAdjustPosY"></param>
        public override void MoveTo(Vector3 target, bool needToAdjustPosY = true)
        {
            if (path != null && m_cornersIdx < path.corners.Length)
            {
                if (!canMove)
                    return;
            }
            //如果已经到达目的地，并且不需要调整位置
            if (m_isMovingOn && !needToAdjustPosY)
            {
                return;
            }
            else
            {
                //停止上个还没有寻完的路
                StopNav();
            }
            if (needToAdjustPosY)
            {
                bool hasHit = UnityTools.GetPointInTerrain(target.x, target.z, out target);
                if (hasHit == false)
                {
                    return;
                }
            }
            if (!isMovingToTarget)
            {
                path = m_navmeshHelper.GetPathByTarget(target);
                targetToMoveTo = target;
                m_cornersIdx = 1;
            }
            if (path.corners.Length < 2)
            {
                StopNav();
                GameWorld.thePlayer.Idle();
                return;
            }
            isMovingToTarget = true;
            moveDirection = (path.corners[m_cornersIdx] - transform.position).normalized;
            transform.LookAt(new Vector3(path.corners[m_cornersIdx].x, transform.position.y, path.corners[m_cornersIdx].z));
            float dis = Vector3.Distance(transform.position, path.corners[m_cornersIdx]);
            float step = 8 * Time.deltaTime;
            if (step + 0.1f > dis && m_cornersIdx < path.corners.Length - 1)
            {
                collisionFlags = m_characterController.Move(transform.forward * dis);
                m_cornersIdx++;
                transform.LookAt(new Vector3(path.corners[m_cornersIdx].x, transform.position.y, path.corners[m_cornersIdx].z));
            }
            else if (m_cornersIdx == path.corners.Length - 1 && step + 0.1f + m_stopDistance > dis)
            {
                float tempDis;
                tempDis = dis - m_stopDistance;
                tempDis = tempDis > 0 ? tempDis : 0;
                collisionFlags = m_characterController.Move((path.corners[m_cornersIdx] - transform.position).normalized * tempDis);
                StopNav();
                tempDis = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetToMoveTo.x, targetToMoveTo.z));
                if (tempDis > 0.2 + m_stopDistance)
                {
                    GameWorld.thePlayer.Idle();
                    //EventDispatcher.TriggerEvent(ON_MOVE_TO_FALSE, transform.gameObject, targetToMoveTo, tempDis);
                }
                else
                {
                    GameWorld.thePlayer.Idle();
                    //EventDispatcher.TriggerEvent(ON_MOVE_TO, transform.gameObject, targetToMoveTo);
                }
            }
        }
        public override void SetSpeed(float value)
        {
            targetSpeed = value;
        }
        #endregion
    }
}