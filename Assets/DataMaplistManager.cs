using UnityEngine;
using System.Collections.Generic;
using Utility.Export;
public class DataMaplistManager : IDynamicData
{
    private static readonly DataMaplistManager instance;
    private List<DataMaplist1> dataList = new List<DataMaplist1>();
    public static DataMaplistManager Instance
    {
        get
        {
            return DataMaplistManager.instance;
        }
    }
    public List<DataMaplist1> DataList
    {
        get
        {
            return this.dataList;
        }
    }
    public int Count
    {
        get
        {
            return this.dataList.Count;
        }
    }
    static DataMaplistManager()
    {
        DataMaplistManager.instance = new DataMaplistManager();
    }
    public void Clear()
    {
        this.dataList.Clear();
    }
    public void Serialize(IDynamicPacket packet)
    {
        packet.Write<DataMaplist1>(this.dataList);
    }
    public void Deserialize(IDynamicPacket packet)
    {
        this.dataList = packet.ReadList<DataMaplist1>();
    }
    public DataMaplist1 GetDataByID(int iD)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ID == iD)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByID(int iD)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ID == iD)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByName(string name)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.Name == name)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByName(string name)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.Name == name)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMapType(int mapType)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapType == mapType)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMapType(int mapType)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapType == mapType)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMapFile(string mapFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapFile == mapFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMapFile(string mapFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapFile == mapFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMapFilePath(string mapFilePath)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapFilePath == mapFilePath)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMapFilePath(string mapFilePath)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapFilePath == mapFilePath)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByDeathMatch_OMG(int deathMatch_OMG)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.DeathMatch_OMG == deathMatch_OMG)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByDeathMatch_OMG(int deathMatch_OMG)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.DeathMatch_OMG == deathMatch_OMG)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByPicFile(string picFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PicFile == picFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByPicFile(string picFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PicFile == picFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByShopFile(string shopFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ShopFile == shopFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByShopFile(string shopFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ShopFile == shopFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByPCBgSoundFile(string pCBgSoundFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PCBgSoundFile == pCBgSoundFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByPCBgSoundFile(string pCBgSoundFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PCBgSoundFile == pCBgSoundFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByPCWarnSoundFile(string pCWarnSoundFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PCWarnSoundFile == pCWarnSoundFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByPCWarnSoundFile(string pCWarnSoundFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PCWarnSoundFile == pCWarnSoundFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByPCToughSoundFile(string pCToughSoundFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PCToughSoundFile == pCToughSoundFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByPCToughSoundFile(string pCToughSoundFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PCToughSoundFile == pCToughSoundFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByPCNormalSoundFile(string pCNormalSoundFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PCNormalSoundFile == pCNormalSoundFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByPCNormalSoundFile(string pCNormalSoundFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.PCNormalSoundFile == pCNormalSoundFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByUnPCBgSoundFile(string unPCBgSoundFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.UnPCBgSoundFile == unPCBgSoundFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByUnPCBgSoundFile(string unPCBgSoundFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.UnPCBgSoundFile == unPCBgSoundFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByUnPCWarnSoundFile(string unPCWarnSoundFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.UnPCWarnSoundFile == unPCWarnSoundFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByUnPCWarnSoundFile(string unPCWarnSoundFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.UnPCWarnSoundFile == unPCWarnSoundFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByUnPCToughSoundFile(string unPCToughSoundFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.UnPCToughSoundFile == unPCToughSoundFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByUnPCToughSoundFile(string unPCToughSoundFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.UnPCToughSoundFile == unPCToughSoundFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByUnPCNormalSoundFile(string unPCNormalSoundFile)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.UnPCNormalSoundFile == unPCNormalSoundFile)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByUnPCNormalSoundFile(string unPCNormalSoundFile)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.UnPCNormalSoundFile == unPCNormalSoundFile)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByLCameraPos(string lCameraPos)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LCameraPos == lCameraPos)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByLCameraPos(string lCameraPos)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LCameraPos == lCameraPos)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByLCameraAngle(string lCameraAngle)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LCameraAngle == lCameraAngle)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByLCameraAngle(string lCameraAngle)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LCameraAngle == lCameraAngle)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByECameraPos(string eCameraPos)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ECameraPos == eCameraPos)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByECameraPos(string eCameraPos)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ECameraPos == eCameraPos)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByECameraAngle(string eCameraAngle)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ECameraAngle == eCameraAngle)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByECameraAngle(string eCameraAngle)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ECameraAngle == eCameraAngle)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMapCenterX(float mapCenterX)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapCenterX == mapCenterX)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMapCenterX(float mapCenterX)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapCenterX == mapCenterX)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMapCenterY(float mapCenterY)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapCenterY == mapCenterY)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMapCenterY(float mapCenterY)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MapCenterY == mapCenterY)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMaxX(float maxX)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MaxX == maxX)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMaxX(float maxX)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MaxX == maxX)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMaxY(float maxY)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MaxY == maxY)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMaxY(float maxY)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MaxY == maxY)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMinCameraScale(float minCameraScale)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MinCameraScale == minCameraScale)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMinCameraScale(float minCameraScale)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MinCameraScale == minCameraScale)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMaxCameraScale(float maxCameraScale)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MaxCameraScale == maxCameraScale)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMaxCameraScale(float maxCameraScale)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MaxCameraScale == maxCameraScale)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMouseWheelSensitivity(float mouseWheelSensitivity)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MouseWheelSensitivity == mouseWheelSensitivity)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMouseWheelSensitivity(float mouseWheelSensitivity)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MouseWheelSensitivity == mouseWheelSensitivity)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByLeagueBaseDeadEft(int leagueBaseDeadEft)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueBaseDeadEft == leagueBaseDeadEft)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByLeagueBaseDeadEft(int leagueBaseDeadEft)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueBaseDeadEft == leagueBaseDeadEft)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByEmpireBaseDeadEft(int empireBaseDeadEft)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireBaseDeadEft == empireBaseDeadEft)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByEmpireBaseDeadEft(int empireBaseDeadEft)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireBaseDeadEft == empireBaseDeadEft)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByMineBaseDeadEft(int mineBaseDeadEft)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MineBaseDeadEft == mineBaseDeadEft)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByMineBaseDeadEft(int mineBaseDeadEft)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.MineBaseDeadEft == mineBaseDeadEft)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByTheirBaseDeadEft(int theirBaseDeadEft)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.TheirBaseDeadEft == theirBaseDeadEft)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByTheirBaseDeadEft(int theirBaseDeadEft)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.TheirBaseDeadEft == theirBaseDeadEft)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByLeagueAttackedEffect(int leagueAttackedEffect)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueAttackedEffect == leagueAttackedEffect)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByLeagueAttackedEffect(int leagueAttackedEffect)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueAttackedEffect == leagueAttackedEffect)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByEmpireAttackedEffect(int empireAttackedEffect)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireAttackedEffect == empireAttackedEffect)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByEmpireAttackedEffect(int empireAttackedEffect)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireAttackedEffect == empireAttackedEffect)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByLeagueEffectWhenHeroDead(int leagueEffectWhenHeroDead)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueEffectWhenHeroDead == leagueEffectWhenHeroDead)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByLeagueEffectWhenHeroDead(int leagueEffectWhenHeroDead)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueEffectWhenHeroDead == leagueEffectWhenHeroDead)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByEmpireEffectWhenHeroDead(int empireEffectWhenHeroDead)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireEffectWhenHeroDead == empireEffectWhenHeroDead)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByEmpireEffectWhenHeroDead(int empireEffectWhenHeroDead)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireEffectWhenHeroDead == empireEffectWhenHeroDead)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByLeagueHighHpEffect(int leagueHighHpEffect)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueHighHpEffect == leagueHighHpEffect)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByLeagueHighHpEffect(int leagueHighHpEffect)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueHighHpEffect == leagueHighHpEffect)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByEmpireHighHpEffect(int empireHighHpEffect)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireHighHpEffect == empireHighHpEffect)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByEmpireHighHpEffect(int empireHighHpEffect)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireHighHpEffect == empireHighHpEffect)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByLeagueLowHpEffect(int leagueLowHpEffect)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueLowHpEffect == leagueLowHpEffect)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByLeagueLowHpEffect(int leagueLowHpEffect)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueLowHpEffect == leagueLowHpEffect)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByEmpireLowHpEffect(int empireLowHpEffect)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireLowHpEffect == empireLowHpEffect)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByEmpireLowHpEffect(int empireLowHpEffect)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireLowHpEffect == empireLowHpEffect)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByEmpireBasePos(string empireBasePos)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireBasePos == empireBasePos)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByEmpireBasePos(string empireBasePos)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.EmpireBasePos == empireBasePos)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByLeagueBasePos(string leagueBasePos)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueBasePos == leagueBasePos)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByLeagueBasePos(string leagueBasePos)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.LeagueBasePos == leagueBasePos)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByComPos(string comPos)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ComPos == comPos)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByComPos(string comPos)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ComPos == comPos)
            {
                list.Add(current);
            }
        }
        return list;
    }
    public DataMaplist1 GetDataByComSmallPos(string comSmallPos)
    {
        DataMaplist1 result;
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ComSmallPos == comSmallPos)
            {
                result = current;
                return result;
            }
        }
        result = null;
        return result;
    }
    public List<DataMaplist1> GetDataListByComSmallPos(string comSmallPos)
    {
        List<DataMaplist1> list = new List<DataMaplist1>();
        foreach (DataMaplist1 current in this.dataList)
        {
            if (current.ComSmallPos == comSmallPos)
            {
                list.Add(current);
            }
        }
        return list;
    }
}
