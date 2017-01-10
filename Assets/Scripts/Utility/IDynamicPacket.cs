using UnityEngine;
using System.IO;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：IDynamicPacket
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility.Export
{
    public interface IDynamicPacket
    {
        byte[] GetBuffer();
        int GetLength();
        void Read(Dictionary<string, string> diction);
        void Read(out Dictionary<string, string> diction);
        void Read(out byte[] bytes);
        void Read(out char c);
        void Read(out char[] chars);
        void Read(out short[] shorts);
        void Read<TK, TV>(out Dictionary<TK, TV> diction)
            where TK : IDynamicData, new()
            where TV : IDynamicData, new();
        void Read(Dictionary<int, int> diction);
        void Read<T>(out Dictionary<int, T> diction) where T : IDynamicData, new();
        void Read(out Dictionary<int, int> diction);
        void Read(out List<byte> list);
        void Read(out List<short> list);
        void Read<T>(Dictionary<int, T> diction) where T : IDynamicData, new();
        void Read<T>(out List<T> list) where T : IDynamicData, new();
        void Read(out bool b);
        void Read(out List<int> list);
        void Read(out List<float> list);
        void Read(out byte b);
        void Read(out List<string> list);
        void Read<TK, TV>(Dictionary<TK, TV> diction)
            where TK : IDynamicData, new()
            where TV : IDynamicData, new();
        void Read(out List<ushort> list);
        void Read(out List<uint> list);
        void Read(out List<ulong> list);
        void Read(out decimal d);
        void Read(out double d);
        void Read(out short i);
        void Read(out int i);
        void Read<T>(List<T> list) where T : IDynamicData, new();
        void Read(List<string> list);
        void Read(out int[] ints);
        void Read(out long[] longs);
        void Read(out sbyte[] bytes);
        void Read(List<float> list);
        void Read(out ushort[] ushorts);
        void Read(List<ulong> list);
        void Read(out uint i);
        void Read(out float f);
        void Read(out ulong i);
        void Read(List<int> list);
        void Read(out string str);
        void Read(List<uint> list);
        void Read(out sbyte b);
        void Read(out ulong[] ulongs);
        void Read<T>(T data) where T : IDynamicData;
        void Read(List<ushort> list);
        void Read(out long i);
        void Read(List<short> list);
        void Read(out ushort i);
        void Read<T>(out T[] array) where T : IDynamicData, new();
        void Read(List<byte> list);
        void Read(out uint[] uints);
        void Read(out byte[] bytes, int count);
        void Read(out short[] shorts, int count);
        void Read<T>(out T[] array, int count) where T : IDynamicData, new();
        void Read(out int[] ints, int count);
        void Read(out long[] longs, int count);
        void Read(out sbyte[] bytes, int count);
        void Read(out ushort[] ushorts, int count);
        void Read(out uint[] uints, int count);
        void Read(out ulong[] ulongs, int count);
        T[] ReadArray<T>() where T : IDynamicData, new();
        T[] ReadArray<T>(int count) where T : IDynamicData, new();
        bool ReadBoolean();
        byte ReadByte();
        byte[] ReadBytes();
        byte[] ReadBytes(int count);
        char ReadChar();
        char[] ReadChars();
        char[] ReadChars(int count);
        T ReadData<T>() where T : IDynamicData, new();
        decimal ReadDecimal();
        Dictionary<TK, TV> ReadDiction<TK, TV>()
            where TK : IDynamicData, new()
            where TV : IDynamicData, new();
        Dictionary<int, T> ReadDictionInt32<T>() where T : IDynamicData, new();
        Dictionary<int, int> ReadDictionInt32Int32();
        Dictionary<string, string> ReadDictionStringString();
        double ReadDouble();
        float ReadFloat();
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        int[] ReadInts();
        int[] ReadInts(int count);
        List<T> ReadList<T>() where T : IDynamicData, new();
        List<byte> ReadListByte();
        List<float> ReadListFloat();
        List<short> ReadListInt16();
        List<int> ReadListInt32();
        List<string> ReadListString();
        List<ushort> ReadListUInt16();
        List<uint> ReadListUInt32();
        List<ulong> ReadListUInt64();
        long[] ReadLongs();
        long[] ReadLongs(int count);
        sbyte ReadSByte();
        sbyte[] ReadSBytes();
        sbyte[] ReadSBytes(int count);
        short[] ReadShorts();
        short[] ReadShorts(int count);
        string ReadString();
        ushort ReadUInt16();
        uint ReadUInt32();
        ulong ReadUInt64();
        uint[] ReadUInts();
        uint[] ReadUInts(int count);
        ulong[] ReadULongs();
        ulong[] ReadULongs(int count);
        ushort[] ReadUShorts();
        ushort[] ReadUShorts(int count);
        void Seek(long offset, SeekOrigin loc);
        byte[] ToArray();
        void Write<T>(T[] array) where T : IDynamicData, new();
        void Write(int[] ints);
        void Write<T>(T data) where T : IDynamicData;
        void Write(uint[] uints);
        void Write<TK, TV>(Dictionary<TK, TV> diction)
            where TK : IDynamicData, new()
            where TV : IDynamicData, new();
        void Write<T>(Dictionary<int, T> diction) where T : IDynamicData, new();
        void Write(long[] longs);
        void Write(Dictionary<int, int> diction);
        void Write(ushort[] ushorts);
        void Write(ulong[] ulongs);
        void Write(Dictionary<string, string> diction);
        void Write<T>(List<T> list) where T : IDynamicData, new();
        void Write(List<byte> list);
        void Write(List<ushort> list);
        void Write(byte[] b);
        void Write(List<int> list);
        void Write(List<short> list);
        void Write(short[] shorts);
        void Write(char c);
        void Write(List<float> list);
        void Write(List<string> list);
        void Write(bool b);
        void Write(byte b);
        void Write(List<uint> list);
        void Write(List<ulong> list);
        void Write(sbyte[] bytes);
        void Write(decimal d);
        void Write(double d);
        void Write(short i);
        void Write(int i);
        void Write(long i);
        void Write(sbyte b);
        void Write(float f);
        void Write(string str);
        void Write(ushort i);
        void Write(uint i);
        void Write(char[] chars);
        void Write(ulong i);
        void Write<T>(T[] array, int count) where T : IDynamicData, new();
        void Write(byte[] bytes, int count);
        void Write(char[] chars, int count);
        void Write(short[] shorts, int count);
        void Write(int[] ints, int count);
        void Write(long[] longs, int count);
        void Write(sbyte[] bytes, int count);
        void Write(ushort[] ushorts, int count);
        void Write(uint[] uints, int count);
        void Write(ulong[] ulongs, int count);
    }
}
