using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PCGameInputManager 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.10.17
// 模块描述：PC端输入管理器
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// PC端输入管理器
/// </summary>
namespace Game
{
    public class PCGameInputManager : MonoBehaviour, IGameInputManager
    {
        #region 字段
        private bool m_bIsMoving = false;
        private EntityParent m_theOwner;
        private Vector2 m_direction = Vector2.zero;
        private Vector3 m_orginPos = Vector3.zero;
        private Vector3 m_desPos = Vector3.zero;
        #endregion
        #region 属性
        public Camera RelativeCamera;
        public bool IsMoving
        {
            get
            {
                return m_bIsMoving;
            }
        }
        public Vector3 OrginPos
        {
            get { return this.m_theOwner != null ? this.m_theOwner.Transform.position : Vector3.zero; }
            set { this.m_orginPos = value; }
        }
        public Vector2 Direction
        {
            get { return this.m_direction; }
            set { this.m_direction = value; }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公共方法
        public void Init(EntityParent theOwner)
        {
            this.m_theOwner = theOwner;
            this.m_orginPos = this.m_theOwner.Transform.position;
            this.RelativeCamera = Camera.main;
        }
        #endregion
        #region 私有方法
        void Awake()
        {

        }
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray r = RelativeCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, 100))
                {
                    if (hit.collider.CompareTag("Terrain"))
                    {
                        Vector3 point = new Vector3(hit.point.x, this.m_orginPos.y, hit.point.z);
                        Vector3 orginPos = new Vector3(this.m_orginPos.x, this.m_orginPos.y, this.m_orginPos.z);
                        if (Vector3.Distance(hit.point, this.m_orginPos) >= 0.01)
                        {
                            this.m_bIsMoving = true;
                            this.m_direction = (new Vector2(point.x, point.z) - new Vector2(orginPos.x, orginPos.z)).normalized;
                            this.m_desPos = hit.point;
                        }
                    }
                }
            }
            this.m_orginPos = this.m_theOwner.Transform.position;
            if (this.m_bIsMoving && Vector3.Distance(this.m_desPos, this.m_orginPos) < 0.5f)
            {
                Reset();
            }
        }
        public void Reset()
        {
            this.m_bIsMoving = false;
            this.m_orginPos = this.m_theOwner.Transform.position;
            this.m_direction = Vector3.one;
        }
        #endregion
        #region 析构方法
        #endregion
    }
}