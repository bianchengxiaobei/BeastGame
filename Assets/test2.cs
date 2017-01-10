using UnityEngine;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：test2
// 创建者：chen
// 修改者列表：
// 创建日期：
// 模块描述：
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 
/// </summary>
public class test2 : MonoBehaviour
{
    /*public Transform e;
    public void Start()
    {
        var cube = new GameObject();
        var filter = cube.AddComponent<MeshFilter>();
        cube.AddComponent<MeshRenderer>();
        DrawCircle(cube, filter, 5, 360, 0, 0, 0);
    }
    private void DrawCircle(GameObject canvas, MeshFilter filter, float raidus, float angle, float offsetX = 0, float offsetY = 0, float angleOffset = 0, int posType = 0)
    {
        int ANGLE_STEP = 15;
        var mesh = new Mesh();
        int len = (int)Mathf.Floor(angle / ANGLE_STEP);
        len = len + 2;
        Vector3[] vs = new Vector3[len];
        float sin = (float)Mathf.Sin(angleOffset * Mathf.PI / 180);
        float cos = (float)Mathf.Cos(angleOffset * Mathf.PI / 180);
        //第一个为圆心
        Matrix4x4 m = e.localToWorldMatrix;
        Matrix4x4 m1 = new Matrix4x4();
        m1.SetRow(0, new Vector4(0, 0, 0, offsetY)); //1
        m1.SetRow(1, new Vector4(0, 1, 0, 0));
        m1.SetRow(2, new Vector4(0, 0, 0, offsetX)); //-1
        m1.SetRow(3, new Vector4(0, 0, 0, 1));
        m = m * m1;
        Vector3 v0 = new Vector3(m.m03, m.m13, m.m23);
        //vs[0] = theOwner.Transform.position;
        vs[0] = v0;
        for (int i = 1; i < len; i++)
        {
            //canvas.transform.position = theOwner.Transform.position;
            canvas.transform.position = v0;
            canvas.transform.rotation = e.rotation;
            canvas.transform.Rotate(new Vector3(0, -angle * 0.5f, 0));
            if (i != len - 1)
            {//非最后一个点
                canvas.transform.Rotate(new Vector3(0, ANGLE_STEP * i, 0));
                var v = canvas.transform.position + canvas.transform.forward * raidus;
                vs[i] = v;
            }
            else
            {//最后一个顶点
                //float r = angle - ANGLE_STEP * (i - 1);
                canvas.transform.Rotate(new Vector3(0, angle, 0));
                var v = canvas.transform.position + canvas.transform.forward * raidus;
                vs[i] = v;
            }
        }
        //三角形数
        int tc = len - 2;
        int[] triangles = new int[tc * 3];
        for (int j = 0; j < tc; j++)
        {
            triangles[j * 3] = 0;
            triangles[j * 3 + 1] = j + 1;
            if (j != 23)
            {
                triangles[j * 3 + 2] = j + 2;
            }
            else
            {
                triangles[j * 3 + 2] = 1;
            }
        }
        canvas.transform.position = Vector3.zero;
        canvas.transform.rotation = new Quaternion();
        mesh.vertices = vs;
        mesh.triangles = triangles;
        filter.mesh = mesh;
    }
    */
    void Start()
    {
        GUITexture t = Camera.main.GetComponent<GUITexture>();
        Debug.Log(t == null);
    }
}
