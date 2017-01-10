using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：MyselfProperty
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Game
{
    public partial class EntityMyself
    {
        #region 字段
        private uint m_atk;
        private uint m_power;
        private uint m_crit;
        private uint m_critExtraAttack;
        private uint m_cdReduce;
        private uint m_physicalResistance;
        private uint m_magicResistance;
        private uint m_attackSpeed;
        private uint propertyBase = 0;
        #endregion
        #region 属性
        public uint Attack 
        {
            get { return this.m_atk; }
            set 
            {
                CheckPropertyBase();
                this.m_atk = value;
                OnPropertyChanged(BattleAttr.Attack, this.Attack);
                UpdatePropertyBase();
            }
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        #endregion
        #region 私有方法
        /// <summary>
        /// 检查基础属性，看是否有玩家作弊修改属性值
        /// </summary>
        private void CheckPropertyBase()
        {
            
        }
        /// <summary>
        /// 更新基础属性，比如提高攻击力就修改更新比较的值
        /// </summary>
        private void UpdatePropertyBase()
        {
            
        }
        #endregion
    }
}