using UnityEngine;
using System;
using System.Collections.Generic;
using System.Timers;
using Game;
using LayerMask = Game.LayerMask;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UnityTools
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility.Export
{
    public class UnityTools
    {
        public static Color32 s_colorHighlight = new Color32(1, 50, 255, 255);
        public static Color32 s_colorGray = new Color32(1, 0, 255, 255);
        private static CByteStream s_byteStream = new CByteStream();
        private static IXLog s_log = new Logger("UnityTools");
        private static long m_lStartTime = 0;
        public UnityTools()
        {
            m_lStartTime = GetElapsedTimeUs();
        }
        public static Color GrayColor
        {
            get
            {
                return UnityTools.s_colorGray;
            }
        }
        public static Color HighlightColor
        {
            get
            {
                return UnityTools.s_colorHighlight;
            }
        }
        public static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            obj.layer = newLayer;
            foreach (Transform transform in obj.transform)
            {
                UnityTools.SetLayerRecursively(transform.gameObject, newLayer);
            }
        }
        public static void SetColor(GameObject obj, Color color)
        {
            if (obj == null)
            {
                return;
            }
            Renderer[] componentsInChildren = obj.GetComponentsInChildren<Renderer>(true);
            if (componentsInChildren == null)
            {
                return;
            }
            Renderer[] array = componentsInChildren;
            for (int i = 0; i < array.Length; i++)
            {
                Renderer renderer = array[i];
                if (!(renderer == null))
                {
                    Material[] materials = renderer.materials;
                    for (int j = 0; j < materials.Length; j++)
                    {
                        Material material = materials[j];
                        if (material.HasProperty("_Color"))
                        {
                            material.SetVector("_Color", color);
                        }
                        if (material.HasProperty("_TintColor"))
                        {
                            material.SetVector("_TintColor", color);
                        }
                    }
                }
            }
        }
        public static CVector3 String2CVector3(string strText)
        {
            CVector3 cVector = new CVector3();
            try
            {
                string[] array = strText.Split(new char[]
				{
					','
				});
                if (array.Length == 3)
                {
                    cVector.m_nX = Convert.ToInt32(array[0]);
                    cVector.m_nY = Convert.ToInt32(array[1]);
                    cVector.m_nU = Convert.ToInt32(array[2]);
                }
            }
            catch (Exception ex)
            {
                UnityTools.s_log.Fatal(ex.ToString());
            }
            return cVector;
        }
        /// <summary>
        /// 字符串坐标转换成Vector3坐标
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static Vector3 String2Vector3(string strText)
        {
            Vector3 result = default(Vector3);
            try
            {
                string[] array = strText.Split(new char[]
				{
					','
				});
                if (array.Length == 3)
                {
                    result.x = (float)Convert.ToDouble(array[0]);
                    result.y = (float)Convert.ToDouble(array[1]);
                    result.z = (float)Convert.ToDouble(array[2]);
                }
            }
            catch (Exception ex)
            {
                UnityTools.s_log.Fatal(ex.ToString());
            }
            return result;
        }
        public static Vector2 String2Vector2(string strText)
        {
            Vector2 result = default(Vector2);
            try
            {
                string[] array = strText.Split(new char[]
				{
					','
				});
                if (array.Length == 2)
                {
                    result.x = (float)Convert.ToDouble(array[0]);
                    result.y = (float)Convert.ToDouble(array[1]);
                }
            }
            catch (Exception ex)
            {
                UnityTools.s_log.Fatal(ex.ToString());
            }
            return result;
        }
        public static T Clone<T>(T obj) where T : IData, new()
        {
            UnityTools.s_byteStream.Clear();
            obj.Serialize(UnityTools.s_byteStream);
            T result = Activator.CreateInstance<T>();
            result.DeSerialize(UnityTools.s_byteStream);
            return result;
        }
        public static string GetHierarchy(Transform trans)
        {
            string text = trans.name;
            while (trans.parent != null)
            {
                trans = trans.parent;
                text = trans.name + "/" + text;
            }
            return text;
        }
        /// <summary>
        /// 将字符串转化成为对应类型的值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetValue(string value, Type type)
        {
            if (null == type)
            {
                return null;
            }
            else if (type == typeof(int))
            {
                return Convert.ToInt32(value);
            }
            else if (type == typeof(float))
            {
                return float.Parse(value);
            }
            else if (type == typeof(byte))
            {
                return Convert.ToByte(value);
            }
            else if (type == typeof(double))
            {
                return Convert.ToDouble(value);
            }
            else if (type == typeof(bool))
            {
                if (value == "0")
                {
                    return false;
                }
                else if (value == "1")
                {
                    return true;
                }
            }
            else if (type == typeof(string))
            {
                return value;
            }
            return null;
        }
        #region Time
        public static long GetLocalMilliseconds()
        {
            return GetElapsedTimeUs() - m_lStartTime;
        }
        public static long GetElapsedTimeUs()
        {
            return DateTime.Now.Ticks / 10;
        }
        #endregion
        #region Navmesh
        /// <summary>
        /// 根据物体的xz坐标，跟地面碰撞的点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool GetPointInTerrain(float x, float z, out Vector3 point)
        {
            RaycastHit hit;
            //跟地面线性碰撞，取得碰撞点，然后更新y坐标
            var flag = Physics.Linecast(new Vector3(x, 1000, z), new Vector3(x, -1000, z), out hit, (int)Game.LayerMask.Terrain);
            if (flag)
            {
                point = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
                return true;
            }
            else
            {
                point = new Vector3(x, 50, z);
                return false;
            }
        }
        #endregion
        #region GameObject
        /// <summary>
        /// 改变物体的父亲节点，但是保持原先物体的本地Transform
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        public static void ChangeObjectParentDontChangeLocalTransform(Transform child,Transform parent)
        {
            Vector3 scale = child.localScale;
            Vector3 position = child.localPosition;
            Vector3 angle = child.localEulerAngles;
            child.parent = parent;
            child.localScale = scale;
            child.localEulerAngles = angle;
            child.localPosition = position;
        }
        /// <summary>
        /// 递归遍历t下面所有的子Transform信息
        /// </summary>
        /// <param name="t"></param>
        /// <param name="lst"></param>
        public static void FindAllTransform(Transform t, List<Transform> lst)
        {
            for (int i = 0; i < t.childCount; i++)
            {
                Transform child = t.GetChild(i);
                lst.Add(child);
                FindAllTransform(child, lst);
            }
        }
        /// <summary>
        /// 找到指定名字的子节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform FindChildTransform(Transform parent, string childName)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform childTrans = parent.GetChild(i);
                if (childTrans.name == childName)
                {
                    return childTrans;
                }
                else
                {
                    Transform t = FindChildTransform(childTrans, childName);
                    if (t != null)
                    {
                        return t;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 新的Transform取代旧的Transform
        /// </summary>
        /// <param name="newTran"></param>
        /// <param name="oldTran"></param>
        public static void ReplaceNewTran(Transform newTran, Transform oldTran)
        {
            for (int i = 0; i < newTran.childCount; i++)
            {
                Transform newChild = newTran.GetChild(i);
                bool bFind = false;
                for (int j = 0; j < oldTran.childCount; j++)
                {
                    Transform oldChild = oldTran.GetChild(j);
                    if (oldChild.name == newChild.name)
                    {
                        ReplaceNewTran(newChild, oldChild);
                        bFind = true;
                        break;
                    }
                }
                if (!bFind)
                {
                    GameObject newObj = new GameObject();
                    newObj.transform.parent = oldTran;
                    newObj.name = newChild.name;
                    newObj.transform.localEulerAngles = newChild.localEulerAngles;
                    newObj.transform.localPosition = newChild.localPosition;
                    newObj.transform.localRotation = newChild.localRotation;
                    newObj.transform.localScale = newChild.localScale;
                    ReplaceNewTran(newChild, newObj.transform);
                }
            }
        }
        #endregion
        #region Rect
        /// <summary>
        ///  判断点p是否在四点构成多边形中
        /// </summary>
        /// <param name="p"></param>
        /// <param name="v0"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static bool InRect(Vector3 p, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
        {
            bool result = false;
            int cnt = 0;//交点个数
            Vector3 p1 = p + new Vector3(0, 0, 50);
            if (CrossPoint(p, p1, v0, v1))
            {
                cnt++;
            }
            if (CrossPoint(p, p1, v1, v2))
            {
                cnt++;
            }
            if (CrossPoint(p, p1, v2, v3))
            {
                cnt++;
            }
            if (CrossPoint(p, p1, v3, v0))
            {
                cnt++;
            }
            if (cnt == 1)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 是否顶点交叉
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <param name="v4"></param>
        /// <returns></returns>
        public static bool CrossPoint(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            if (Math.Max(v1.x, v2.x) >= Math.Min(v3.x, v4.x) &&
               Math.Max(v3.x, v4.x) >= Math.Min(v1.x, v2.x) &&
               Math.Max(v1.z, v2.z) >= Math.Min(v3.z, v4.z) &&
               Math.Max(v3.z, v4.z) >= Math.Min(v1.z, v2.z) &&
               mulpti(v1, v2, v3) * mulpti(v1, v2, v4) <= 0 &&
               mulpti(v3, v4, v1) * mulpti(v3, v4, v2) <= 0)
                return true;
            else
                return false;
        }
        static private float mulpti(Vector3 ps, Vector3 pe, Vector3 p)
        {
            float m;
            m = (pe.x - ps.x) * (p.z - ps.z) - (p.x - ps.x) * (pe.z - ps.z);
            return m;
        }
        #endregion
        #region Entity
        /// <summary>
        /// 取得范围内所有的实体（坐标）
        /// </summary>
        /// <returns></returns>
        public static List<List<uint>> GetEntityInRange(Vector3 position,float radius,float offsetX=0,float offsetY=0,float angleOffset=0,LayerMask layerMask = LayerMask.Monster | LayerMask.Character | LayerMask.Trap)
        {
            List<List<uint>> list = new List<List<uint>>();
            List<uint> listDummy = new List<uint>();
            List<uint> listMonster = new List<uint>();
            List<uint> listPlayer = new List<uint>();
            if (!GameWorld.Entities.ContainsKey(GameWorld.thePlayer.ID))
            {
                GameWorld.Entities.Add(GameWorld.thePlayer.ID, GameWorld.thePlayer);
            }
            foreach (var pair in GameWorld.Entities)
            {
                EntityParent entity = pair.Value;
                if (!entity.Transform)
                {
                    continue;
                }
                ///如果实体不是在这个层内的话，直接过滤
                if (((1 << entity.Transform.gameObject.layer) & (int)layerMask) == 0)
                {
                    continue;
                }
                float entityRadius = ((float)entity.GetInAttr("scaleRadius")) / 100f;
                if ((position - entity.Transform.position).magnitude < entityRadius + radius)
                {
                    continue;
                }
                if (pair.Value is EntityDummy)
                {
                    listDummy.Add(pair.Key);
                }
                else if (pair.Value is EntityPlayer)
                {
                    listPlayer.Add(pair.Key);
                }
                else if (pair.Value is EntityBeast)
                {
                    listMonster.Add(pair.Key);
                }
            }
            GameWorld.Entities.Remove(GameWorld.thePlayer.ID);
            list.Add(listDummy);
            list.Add(listMonster);
            list.Add(listPlayer);
            return list;
        }
        /// <summary>
        /// 取得该区域内所有的实体(矩阵)
        /// </summary>
        /// <param name="ltwm"></param>
        /// <param name="rotation"></param>
        /// <param name="forward"></param>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="angleOffset"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static List<List<uint>> GetEntityInRange(Matrix4x4 ltwm, Quaternion rotation, Vector3 forward, Vector3 position, float radius, float offsetX = 0, float offsetY = 0, float angleOffset = 0, LayerMask layerMask = LayerMask.Monster | LayerMask.Character | LayerMask.Trap)
        {
            List<List<uint>> list = new List<List<uint>>();
            List<uint> listDummy = new List<uint>();
            List<uint> listMonster = new List<uint>();
            List<uint> listPlayer = new List<uint>();
            if (!GameWorld.Entities.ContainsKey(GameWorld.thePlayer.ID))
            {
                GameWorld.Entities.Add(GameWorld.thePlayer.ID, GameWorld.thePlayer);
            }
            Matrix4x4 m = ltwm;
            Matrix4x4 m1 = new Matrix4x4();
            m1.SetRow(0,new Vector4(0, 0, offsetX, 0));
            m1.SetRow(1, new Vector4(0, 1, 0, 0));
            m1.SetRow(2, new Vector4(0, 0, 0, offsetX));
            m1.SetRow(3, new Vector4(0, 0, 0, 1));
            m = m * m1;
            Vector3 pos = new Vector3(m.m03, m.m13, m.m23);
            foreach (var pair in GameWorld.Entities)
            {
                EntityParent entity = pair.Value;
                if (!entity.Transform)
                {
                    continue;
                }
                ///如果实体不是在这个层内的话，直接过滤
                if (((1 << entity.Transform.gameObject.layer) & (int)layerMask) == 0)
                {
                    continue;
                }
                float entityRadius = ((float)entity.GetInAttr("scaleRadius")) / 100f;
                if ((pos - entity.Transform.position).magnitude < entityRadius + radius)
                {
                    continue;
                }
                if (pair.Value is EntityDummy)
                {
                    listDummy.Add(pair.Key);
                }
                else if (pair.Value is EntityPlayer)
                {
                    listPlayer.Add(pair.Key);
                }
                else if (pair.Value is EntityBeast)
                {
                    listMonster.Add(pair.Key);
                }
            }
            GameWorld.Entities.Remove(GameWorld.thePlayer.ID);
            list.Add(listDummy);
            list.Add(listMonster);
            list.Add(listPlayer);
            return list;
        }
        /// <summary>
        /// 取得直线上的所有实体
        /// </summary>
        /// <param name="ltwm"></param>
        /// <param name="rotation"></param>
        /// <param name="forward"></param>
        /// <param name="position"></param>
        /// <param name="length"></param>
        /// <param name="direction"></param>
        /// <param name="width"></param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="angleOffset"></param>
        /// <param name="layerMask"></param>
        /// <returns></returns>
        public static List<List<uint>> GetEnitiesForntLineNew(Matrix4x4 ltwm, Quaternion rotation, Vector3 forward, Vector3 position, float length,Vector3  direction,float width,float offsetX = 0, float offsetY = 0, float angleOffset = 0, LayerMask layerMask = LayerMask.Monster | LayerMask.Character | LayerMask.Trap)
        {
            List<List<uint>> list = new List<List<uint>>();
            List<uint> listDummy = new List<uint>();
            List<uint> listMonster = new List<uint>();
            List<uint> listPlayer = new List<uint>();
            if (!GameWorld.Entities.ContainsKey(GameWorld.thePlayer.ID))
            {
                GameWorld.Entities.Add(GameWorld.thePlayer.ID, GameWorld.thePlayer);
            }
            foreach (var pair in GameWorld.Entities)
            {
                if (pair.Value.Transform == null)
                {
                    continue;
                }
                float entityRadius = ((float)pair.Value.GetInAttr("scaleRadius")) / 100f;
                Matrix4x4 m = ltwm;
                Matrix4x4 m1 = new Matrix4x4();
                m1.SetRow(0, new Vector4(0, 0, 0,(width + entityRadius)*0.5f + offsetY));
                m1.SetRow(1, new Vector4(0, 1, 0, 0));
                m1.SetRow(2, new Vector4(0, 0, 0, 0));
                m1.SetRow(3, new Vector4(0, 0, 0, 1));
                m = m * m1;
                Vector3 pos0 = new Vector3(m.m03, m.m13, m.m23);

                m = ltwm;
                m1.SetRow(2, new Vector4(0, 0, 0, length + entityRadius + offsetX));
                m = m * m1;
                Vector3 pos1 = new Vector3(m.m03, m.m13, m.m23);

                m = ltwm;
                m1.SetRow(0, new Vector4(0, 0, 0 - (width + entityRadius) * 0.5f + offsetY));
                m = m * m1;
                Vector3 pos2 = new Vector3(m.m03, m.m13, m.m23);

                m = ltwm;
                m1.SetRow(2, new Vector4(0, 0, 0, offsetX));
                m = m * m1;
                Vector3 pos3 = new Vector3(m.m03, m.m13, m.m23);
                Vector3 postion = pair.Value.Transform.position;
                if (!InRect(position, pos0, pos1, pos2, pos3))
                {
                    continue;
                }
                if (pair.Value is EntityDummy)
                {
                    listDummy.Add(pair.Key);
                }
                else if (pair.Value is EntityPlayer)
                {
                    listPlayer.Add(pair.Key);
                }
                else if (pair.Value is EntityBeast)
                {
                    listMonster.Add(pair.Key);
                }
            }
            GameWorld.Entities.Remove(GameWorld.thePlayer.ID);
            return list;
        }
        #endregion
        #region StateBit
        /// <summary>
        /// 标记位判定，相等返回1，否则返回0
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="otherFlag"></param>
        /// <returns></returns>
        public static int BitFlag(ulong flag, int otherFlag)
        {
            int result = 0;
            if (otherFlag >= 0 && otherFlag < sizeof(ulong) * 8)
            {
                flag &= (ulong)(1 << otherFlag);
                if (flag != 0)
                {
                    result = 1;
                }
            }
            return result;
        }
        #endregion
    }

}
