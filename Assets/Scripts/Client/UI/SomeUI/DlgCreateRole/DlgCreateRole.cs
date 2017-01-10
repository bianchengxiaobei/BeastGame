using UnityEngine;
using System.Collections.Generic;
using Client.UI.UICommon;
using Utility;
using Client.Logic;
using System.Text.RegularExpressions;
using Client.Common;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DlgCreateRole
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.29
// 模块描述：创建角色事件类
//----------------------------------------------------------------*/
#endregion
namespace Client.UI.UICommon
{
    public class DlgCreateRole : DlgBase<DlgCreateRole,DlgCreateRoleBehaviour>
    {
        #region 字段
        private string strRole;//角色头像
        private int m_iRoleIndex;//角色索引
        private byte m_btGender;//角色性别0-男，1-女
        private int m_iVocation;//角色职业类型
        private static Regex regex = new Regex("^[0-9a-zA-Z一-龥]+$");//正则表达式
        private MovieTexture m_movieTexture;
        private GameObject m_createPanel1;//CreatePanel1界面
        private GameObject m_createPanel2;//CreatePanel2界面
        private bool m_bIsLeaveFirstCreatePanel;//是否离开createPanel1
        private bool m_bIsLeaveSecondCreatePanel;//是否离开CreatePanel2
        private GameObject m_oCreateRoleObj = null;
        private EntityParent m_oEntityShow;
        private SpinObject m_spinObj;//旋转脚本
        #region 角色职业介绍
        private GameObject m_oExplorerIntro = null;
        private GameObject m_oMagicianIntro = null;
        private GameObject m_oEngineerIntro = null;
        private GameObject m_oCultivatorIntro = null;
        #endregion 
        #endregion
        #region 属性
        public override string fileName
        {
            get
            {
                return "DlgCreateRole";
            }
        }
        public override int layer
        {
            get
            {
                return -2;
            }
        }
        public override uint Type
        {
            get
            {
                return 8u;
            }
        }
        public bool IsClickCreateRole
        {
            get;
            set;
        }
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public override void SetVisible(bool bIsVisible)
        {
            base.SetVisible(bIsVisible);
        }
        public override void Update()
        {
            base.Update();
            string name = base.uiBehaviour.m_Input_RoleName.GetText();
            if (string.IsNullOrEmpty(name))
            {
                base.uiBehaviour.m_Button_EnterGame.SetEnable(false);
            }else
            { 
                base.uiBehaviour.m_Button_EnterGame.SetEnable(true);
            }
            if (m_bIsLeaveFirstCreatePanel && this.IsPanelFirstUIAnimationFinished(this.m_createPanel1))
            {
                this.m_createPanel1.SetActive(false);
                this.m_createPanel2.SetActive(true);
                foreach (var t in this.m_createPanel2.GetComponentsInChildren<TweenPosition>())
                {
                    if (!t.enabled)
                    {
                        t.PlayForward();
                    }
                }
                this.m_bIsLeaveFirstCreatePanel = false;
            }
            if (m_bIsLeaveSecondCreatePanel && this.IsPanelFirstUIAnimationFinished(this.m_createPanel2))
            {
                this.m_createPanel1.SetActive(true);
                this.m_createPanel2.SetActive(false);
                foreach (var t in this.m_createPanel1.GetComponentsInChildren<TweenPosition>())
                {
                    if (!t.enabled)
                    {
                        t.PlayForward();
                    }
                }
                this.m_bIsLeaveSecondCreatePanel = false;
            }
        }
        protected override void OnShow()
        {
            base.OnShow();
            this.IsClickCreateRole = false;
            base.uiBehaviour.m_Input_RoleName.SetText("");//默认角色名为空
            base.uiBehaviour.m_Button_EnterGame.SetEnable(false);//进入按钮为不激活
            base.uiBehaviour.m_Sprite_RoleMovie.SetTexture(this.m_movieTexture);//设置Movieture
            base.uiBehaviour.m_List_RoleType.SetSelectedIndex(0);
            this.m_iRoleIndex = 0;
            this.RefreshRoleIntro((EnumRoleTypeIndex)this.m_iRoleIndex);
            this.SelectRoleType();
        }
        public override void Init()
        {
            base.Init();
            this.m_bIsLeaveFirstCreatePanel = false;
            this.m_bIsLeaveSecondCreatePanel = false;
            this.m_createPanel1 = base.uiBehaviour.transform.FindChild("pn_create1").gameObject;
            this.m_createPanel2 = base.uiBehaviour.transform.FindChild("pn_create2").gameObject;
            this.m_movieTexture = Resources.Load("Data/Video/beginAnimation") as MovieTexture;
            this.m_movieTexture.loop = true;
            this.m_oExplorerIntro = base.uiBehaviour.transform.FindChild("pn_create1/sp_intro/explorer_intro").gameObject;
            this.m_oMagicianIntro = base.uiBehaviour.transform.FindChild("pn_create1/sp_intro/magician_intro").gameObject;
            this.m_oEngineerIntro = base.uiBehaviour.transform.FindChild("pn_create1/sp_intro/engineer_intro").gameObject;
            this.m_oCultivatorIntro = base.uiBehaviour.transform.FindChild("pn_create1/sp_intro/cultivator_intro").gameObject;

            this.m_spinObj = this.uiBehaviour.GetComponent<SpinObject>();
        }
        public override void RegisterEvent()
        {
            base.RegisterEvent();
            base.uiBehaviour.m_Button_EnterGame.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnClickButtonEnterGame));
            base.uiBehaviour.m_Button_BackLogin.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnClickButtonBackLogin));
            base.uiBehaviour.m_Button_Next.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnClickButtonNext));
            base.uiBehaviour.m_Button_BackSelectRoleType.RegisterClickEventHandler(new ButtonClickEventHandler(this.OnClickButtonBackSelectRoleType));
            base.uiBehaviour.m_List_RoleType.RegisterListClickEventHandler(new ListClickEventHandler(this.OnClickRoleTypeList));
            /*base.uiBehaviour.m_Button_Explorer.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnClickCheckBoxExplorer));
            base.uiBehaviour.m_Button_Engineer.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnClickCheckBoxEngineer));
            base.uiBehaviour.m_Button_Magician.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnClickCheckBoxMagician));
            base.uiBehaviour.m_Button_SoulHunter.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnClickCheckBoxSoulHunter));
            base.uiBehaviour.m_Button_WitchDoctor.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnClickCheckBoxWitchDoctor));
            base.uiBehaviour.m_Button_Cultivator.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnClickCheckBoxCultivator));
            base.uiBehaviour.m_Buttin_RoleWoman.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnClickCheckBoxWomen));
            base.uiBehaviour.m_Button_RoleMan.RegisterOnCheckEventHandler(new CheckBoxOnCheckEventHandler(this.OnClickCheckBoxMan));
        */
        }
        #endregion
        #region 私有方法
        #region 返回登陆界面按钮
        /// <summary>
        /// 返回登陆界面
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private bool OnClickButtonBackLogin(IXUIButton button)
        {
            Debug.Log("返回按钮");
            return true;
        }
        /// <summary>
        /// 进入创建角色头发等场景
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private bool OnClickButtonNext(IXUIButton button)
        {
            TweenPosition[] tList = this.m_createPanel1.GetComponentsInChildren<TweenPosition>();
            this.StartLeavePlayUIAnimation(tList);
            this.m_bIsLeaveFirstCreatePanel = true;
            return true;
        }
        /// <summary>
        /// 返回进入到选择角色职业界面
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private bool OnClickButtonBackSelectRoleType(IXUIButton button)
        {
            TweenPosition[] tList = this.m_createPanel2.GetComponentsInChildren<TweenPosition>();
            this.StartLeavePlayUIAnimation(tList);
            this.m_bIsLeaveSecondCreatePanel = true;
            return true;
        }
        #endregion
        #region 进入游戏
        /// <summary>
        /// 进入游戏
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        private bool OnClickButtonEnterGame(IXUIButton button)
        {
            string name = base.uiBehaviour.m_Input_RoleName.GetText();//取得input里面的name
            bool result;
            if (string.IsNullOrEmpty(name))
            {
                //提示角色名为空
                result = false;
            }
            else 
            {
                if (!name.Equals(name))
                {
                    //提示包含空格
                    result = false;
                }
                else if (!Singleton<WordFilter>.singleton.FilterString(ref name))//如果名字有非法字符
                {
                    //提示含有非法字符
                    result = false;
                    base.uiBehaviour.m_Input_RoleName.SetText("");
                }
                else 
                {
                    if (!DlgCreateRole.regex.IsMatch(name))//如果正则不能匹配到
                    {
                        //提示不符合要求
                        Debug.Log("提示不能匹配正则");
                        result = false;
                    }
                    else 
                    {
                        if (!this.IsClickCreateRole)
                        {
                            //发送创建角色的请求给服务器
                            NetworkManager.singleton.SendCreateRole(name,strRole,m_btGender,m_iRoleIndex);
                            this.IsClickCreateRole = true;
                        }
                        result = true;
                    }
                }
            }
            return result;
        }
        #endregion
        #region 人物选择
        /// <summary>
        /// 选中探险家
        /// </summary>
        /// <param name="checkBox"></param>
        /// <returns></returns>
        private bool OnClickCheckBoxExplorer(IXUICheckBox checkBox)
        {
            if (checkBox.bChecked)
            {
                Debug.Log("选择探险家");
                this.strRole = "Bright";
                this.m_iRoleIndex = 0;
            }
            return true;
        }
        private bool OnClickCheckBoxEngineer(IXUICheckBox checkBox)
        {
            if (checkBox.bChecked)
            {
                Debug.Log("选择工程师");
                this.strRole = "Window";
                this.m_iRoleIndex = 1;
            }
            return true;
        }
        private bool OnClickCheckBoxMagician(IXUICheckBox checkBox)
        {
            if (checkBox.bChecked)
            {
            }
            return true;
        }
        private bool OnClickCheckBoxSoulHunter(IXUICheckBox checkBox)
        {
            if (checkBox.bChecked)
            {
            }
            return true;
        }
        private bool OnClickCheckBoxWitchDoctor(IXUICheckBox checkBox)
        {
            if (checkBox.bChecked)
            {
            }
            return true;
        }
        private bool OnClickCheckBoxCultivator(IXUICheckBox checkBox)
        {
            if (checkBox.bChecked)
            {
            }
            return true;
        }
        private bool OnClickCheckBoxWomen(IXUICheckBox checkBox)
        {
            if (checkBox.bChecked)
            {
                Debug.Log("女性");
                this.m_btGender = (byte)1;
            }
            return true;
        }
        private bool OnClickCheckBoxMan(IXUICheckBox checkBox)
        {
            if (checkBox.bChecked)
            {
                Debug.Log("男性");
                this.m_btGender = (byte)0;
            }
            return true;
        }
        private bool OnClickRoleTypeList(IXUIListItem item)
        {
            if (null == item)
            {
                return false;
            }
            this.m_iRoleIndex = (int)item.Id;
            RefreshRoleIntro((EnumRoleTypeIndex)this.m_iRoleIndex);
            this.SelectRoleType();
            return true;
        }
        /// <summary>
        /// 刷新角色介绍
        /// </summary>
        /// <param name="roleTypeIndex"></param>
        private void RefreshRoleIntro(EnumRoleTypeIndex roleTypeIndex)
        {
            this.m_oCultivatorIntro.SetActive(false);
            this.m_oEngineerIntro.SetActive(false);
            this.m_oExplorerIntro.SetActive(false);
            this.m_oMagicianIntro.SetActive(false);
            switch (roleTypeIndex)
            {
                case EnumRoleTypeIndex.e_RoleType_Engineer:
                    this.m_oEngineerIntro.SetActive(true);
                    break;
                case EnumRoleTypeIndex.e_RoleType_Explorer:
                    this.m_oExplorerIntro.SetActive(true);
                    break;
                case EnumRoleTypeIndex.e_RoleType_Cultivator:
                    this.m_oCultivatorIntro.SetActive(true);
                    break;
                case EnumRoleTypeIndex.e_RoleType_Magician:
                    this.m_oMagicianIntro.SetActive(true);
                    break;
            }
        }
        private void SelectRoleType()
        {
            switch ((EnumRoleTypeIndex)this.m_iRoleIndex)
            {
                case EnumRoleTypeIndex.e_RoleType_Engineer:
                    this.m_iVocation = (int)EnumRoleTypeIndex.e_RoleType_Engineer;
                    this.strRole = "Bright";
                    this.m_btGender = 0;
                    break;
                case EnumRoleTypeIndex.e_RoleType_Explorer:
                    this.m_iVocation = (int)EnumRoleTypeIndex.e_RoleType_Explorer;
                    this.strRole = "Bright";
                    this.m_btGender = 0;
                    break;
                case EnumRoleTypeIndex.e_RoleType_Cultivator:
                    this.m_iVocation = (int)EnumRoleTypeIndex.e_RoleType_Cultivator;
                    this.strRole = "Bright";
                    this.m_btGender = 0;
                    break;
                case EnumRoleTypeIndex.e_RoleType_Magician:
                    this.m_iVocation = (int)EnumRoleTypeIndex.e_RoleType_Magician;
                    this.strRole = "Bright";
                    this.m_btGender = 1;
                    break;
            }
            if (this.m_oCreateRoleObj != null)
            {
                if (this.m_spinObj)
                {
                    this.m_spinObj.m_target = null;
                }
                GameObject.Destroy(this.m_oCreateRoleObj);
                this.m_oCreateRoleObj = null;
                this.m_oEntityShow = null;
            }
            GameObject obj = GameObject.Find("CharacterStandGround");
            this.m_oCreateRoleObj = new GameObject();
            this.m_oCreateRoleObj.name = "CreateRole";
            this.m_oCreateRoleObj.transform.SetParent(GameObject.Find("pn_characterscene").transform);
            this.m_oCreateRoleObj.transform.localPosition = new Vector3(obj.transform.localPosition.x, -360, obj.transform.localPosition.z);
            this.m_oCreateRoleObj.transform.localRotation = Quaternion.Euler(0, 150, 0);
            this.m_oCreateRoleObj.transform.localScale = new Vector3(300f, 300f, 300f);
            RoleAttachedInfo info = new RoleAttachedInfo();
            info.mGender = this.m_btGender;
            info.mVocation = (byte)this.m_iVocation;
            this.m_oEntityShow = new EntityShow();
            this.m_oEntityShow.SetEntityInfo(info);
            this.m_oEntityShow.Transform = this.m_oCreateRoleObj.transform;
            this.m_oEntityShow.SetVisiableWhenLoad(false);
            this.m_oEntityShow.OnEnterWorld();
            if (this.m_spinObj)
                this.m_spinObj.m_target = (EntityShow)this.m_oEntityShow;
        }
        #endregion
        /// <summary>
        /// 是否UI离开动画已经播放完成
        /// </summary>
        /// <returns></returns>
        private bool IsPanelFirstUIAnimationFinished(GameObject uiParent)
        {
            TweenPosition[] pList = uiParent.GetComponentsInChildren<TweenPosition>();
            if (pList == null || pList.Length == 0)
            {
                return true;
            }
            foreach (var p in pList)
            {
                if (p.enabled)
                {
                    return false;
                }
            }
            return true;
        }      
        /// <summary>
        /// 进入下个界面的时候播放UI离开动画
        /// </summary>
        private void StartLeavePlayUIAnimation(UITweener[] _tweenList)
        {
            foreach (var t in _tweenList)
            {
                //反向播放，因为要离开
                t.PlayReverse();
            }
        }
        #endregion
    }
}