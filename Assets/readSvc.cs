using UnityEngine;
using System.Collections;
using System.IO;
using Utility.Export;
using GameData;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：readSvc
// 创建者：chen
// 修改者列表：
// 创建日期：
// 模块描述：
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 
/// </summary>
public class readSvc : MonoBehaviour
{
    public string path;
    void Start()
    {
        StringFileReader sfr = new StringFileReader();
        FileStream fs = new FileStream("F://chenfuling/龙翼编年史/龙翼编年史/bin/"+path, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        Debug.Log(br.ReadString());
        Debug.Log(br.ReadUInt32());
        int a = br.ReadInt32(); Debug.Log(a);
        byte[] a1 = br.ReadBytes(a);
        IDynamicPacket packet = DynamicPacket.Create(a1);
        int num = packet.ReadInt32();
        Debug.Log(num);
        int i = 0;
        while (i < num)
        {
            int count = packet.ReadInt32();
            Debug.Log("count:"+count);
            IDynamicPacket packet2 = DynamicPacket.Create(packet.ReadBytes(count));
            string text = packet2.ReadString();
            switch (text)
            {
                case "table\\herolist.csv":
                    //DataBeastlistManager.Instance.Deserialize(packet2);
                    //DataHerolistManager.Instance.CorrectString(reader);
                   // Debug.Log(DataBeastlistManager.Instance.DataList[0].StrategyDesc);
                    break;
                case "table\\map\\maplist.csv":
                    DataMaplistManager.Instance.Deserialize(packet2);
                    break;
                    
            }
            i++;
        }
    }
}
