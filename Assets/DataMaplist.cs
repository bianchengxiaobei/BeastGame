using UnityEngine;
using System.Collections;
using Utility.Export;
public class DataMaplist1 : IDynamicData
{
    public int ID
    {
        get;
        private set;
    }
    public string Name
    {
        get;
        private set;
    }
    public int MapType
    {
        get;
        private set;
    }
    public string MapFile
    {
        get;
        private set;
    }
    public string MapFilePath
    {
        get;
        private set;
    }
    public int DeathMatch_OMG
    {
        get;
        private set;
    }
    public string PicFile
    {
        get;
        private set;
    }
    public string ShopFile
    {
        get;
        private set;
    }
    public string PCBgSoundFile
    {
        get;
        private set;
    }
    public string PCWarnSoundFile
    {
        get;
        private set;
    }
    public string PCToughSoundFile
    {
        get;
        private set;
    }
    public string PCNormalSoundFile
    {
        get;
        private set;
    }
    public string UnPCBgSoundFile
    {
        get;
        private set;
    }
    public string UnPCWarnSoundFile
    {
        get;
        private set;
    }
    public string UnPCToughSoundFile
    {
        get;
        private set;
    }
    public string UnPCNormalSoundFile
    {
        get;
        private set;
    }
    public string LCameraPos
    {
        get;
        private set;
    }
    public string LCameraAngle
    {
        get;
        private set;
    }
    public string ECameraPos
    {
        get;
        private set;
    }
    public string ECameraAngle
    {
        get;
        private set;
    }
    public float MapCenterX
    {
        get;
        private set;
    }
    public float MapCenterY
    {
        get;
        private set;
    }
    public float MaxX
    {
        get;
        private set;
    }
    public float MaxY
    {
        get;
        private set;
    }
    public float MinCameraScale
    {
        get;
        private set;
    }
    public float MaxCameraScale
    {
        get;
        private set;
    }
    public float MouseWheelSensitivity
    {
        get;
        private set;
    }
    public int LeagueBaseDeadEft
    {
        get;
        private set;
    }
    public int EmpireBaseDeadEft
    {
        get;
        private set;
    }
    public int MineBaseDeadEft
    {
        get;
        private set;
    }
    public int TheirBaseDeadEft
    {
        get;
        private set;
    }
    public int LeagueAttackedEffect
    {
        get;
        private set;
    }
    public int EmpireAttackedEffect
    {
        get;
        private set;
    }
    public int LeagueEffectWhenHeroDead
    {
        get;
        private set;
    }
    public int EmpireEffectWhenHeroDead
    {
        get;
        private set;
    }
    public int LeagueHighHpEffect
    {
        get;
        private set;
    }
    public int EmpireHighHpEffect
    {
        get;
        private set;
    }
    public int LeagueLowHpEffect
    {
        get;
        private set;
    }
    public int EmpireLowHpEffect
    {
        get;
        private set;
    }
    public string EmpireBasePos
    {
        get;
        private set;
    }
    public string LeagueBasePos
    {
        get;
        private set;
    }
    public string ComPos
    {
        get;
        private set;
    }
    public string ComSmallPos
    {
        get;
        private set;
    }
    public void Serialize(IDynamicPacket packet)
    {
        packet.Write(this.ID);
        packet.Write(this.Name);
        packet.Write(this.MapType);
        packet.Write(this.MapFile);
        packet.Write(this.MapFilePath);
        packet.Write(this.DeathMatch_OMG);
        packet.Write(this.PicFile);
        packet.Write(this.ShopFile);
        packet.Write(this.PCBgSoundFile);
        packet.Write(this.PCWarnSoundFile);
        packet.Write(this.PCToughSoundFile);
        packet.Write(this.PCNormalSoundFile);
        packet.Write(this.UnPCBgSoundFile);
        packet.Write(this.UnPCWarnSoundFile);
        packet.Write(this.UnPCToughSoundFile);
        packet.Write(this.UnPCNormalSoundFile);
        packet.Write(this.LCameraPos);
        packet.Write(this.LCameraAngle);
        packet.Write(this.ECameraPos);
        packet.Write(this.ECameraAngle);
        packet.Write(this.MapCenterX);
        packet.Write(this.MapCenterY);
        packet.Write(this.MaxX);
        packet.Write(this.MaxY);
        packet.Write(this.MinCameraScale);
        packet.Write(this.MaxCameraScale);
        packet.Write(this.MouseWheelSensitivity);
        packet.Write(this.LeagueBaseDeadEft);
        packet.Write(this.EmpireBaseDeadEft);
        packet.Write(this.MineBaseDeadEft);
        packet.Write(this.TheirBaseDeadEft);
        packet.Write(this.LeagueAttackedEffect);
        packet.Write(this.EmpireAttackedEffect);
        packet.Write(this.LeagueEffectWhenHeroDead);
        packet.Write(this.EmpireEffectWhenHeroDead);
        packet.Write(this.LeagueHighHpEffect);
        packet.Write(this.EmpireHighHpEffect);
        packet.Write(this.LeagueLowHpEffect);
        packet.Write(this.EmpireLowHpEffect);
        packet.Write(this.EmpireBasePos);
        packet.Write(this.LeagueBasePos);
        packet.Write(this.ComPos);
        packet.Write(this.ComSmallPos);
    }
    public void Deserialize(IDynamicPacket packet)
    {
        this.ID = packet.ReadInt32();
        this.Name = packet.ReadString();
        this.MapType = packet.ReadInt32();
        this.MapFile = packet.ReadString();
        this.MapFilePath = packet.ReadString();
        this.DeathMatch_OMG = packet.ReadInt32();
        this.PicFile = packet.ReadString();
        this.ShopFile = packet.ReadString();
        this.PCBgSoundFile = packet.ReadString();
        this.PCWarnSoundFile = packet.ReadString();
        this.PCToughSoundFile = packet.ReadString();
        this.PCNormalSoundFile = packet.ReadString();
        this.UnPCBgSoundFile = packet.ReadString();
        this.UnPCWarnSoundFile = packet.ReadString();
        this.UnPCToughSoundFile = packet.ReadString();
        this.UnPCNormalSoundFile = packet.ReadString();
        this.LCameraPos = packet.ReadString();
        this.LCameraAngle = packet.ReadString();
        this.ECameraPos = packet.ReadString();
        this.ECameraAngle = packet.ReadString();
        this.MapCenterX = packet.ReadFloat();
        this.MapCenterY = packet.ReadFloat();
        this.MaxX = packet.ReadFloat();
        this.MaxY = packet.ReadFloat();
        this.MinCameraScale = packet.ReadFloat();
        this.MaxCameraScale = packet.ReadFloat();
        this.MouseWheelSensitivity = packet.ReadFloat();
        this.LeagueBaseDeadEft = packet.ReadInt32();
        this.EmpireBaseDeadEft = packet.ReadInt32();
        this.MineBaseDeadEft = packet.ReadInt32();
        this.TheirBaseDeadEft = packet.ReadInt32();
        this.LeagueAttackedEffect = packet.ReadInt32();
        this.EmpireAttackedEffect = packet.ReadInt32();
        this.LeagueEffectWhenHeroDead = packet.ReadInt32();
        this.EmpireEffectWhenHeroDead = packet.ReadInt32();
        this.LeagueHighHpEffect = packet.ReadInt32();
        this.EmpireHighHpEffect = packet.ReadInt32();
        this.LeagueLowHpEffect = packet.ReadInt32();
        this.EmpireLowHpEffect = packet.ReadInt32();
        this.EmpireBasePos = packet.ReadString();
        this.LeagueBasePos = packet.ReadString();
        this.ComPos = packet.ReadString();
        this.ComSmallPos = packet.ReadString();
    }
}