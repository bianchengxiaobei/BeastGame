using UnityEngine;
using System.Collections.Generic;
using Utility.Export;
using System.IO;
using System;
using System.Text;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DynamicPacketImplement
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.7
// 模块描述：数据包解析器实现类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 数据包解析器实现类
/// </summary>
namespace Utility
{
    public class DynamicPacketImplement : IDynamicPacket
    {
        protected MemoryStream memStream;
        protected BinaryReader reader;
        protected BinaryWriter writer;
        public DynamicPacketImplement()
        {
            this.memStream = new MemoryStream();
            this.reader = new BinaryReader(this.memStream);
            this.writer = new BinaryWriter(this.memStream);
        }
        public DynamicPacketImplement(byte[] bytes)
            : this()
        {
            this.memStream.Write(bytes, 0, bytes.Length);
            this.memStream.Seek(0L, SeekOrigin.Begin);
        }
        public DynamicPacketImplement(byte[] bytes, int offset, int count)
            : this()
        {
            this.memStream.Write(bytes, offset, count);
            this.memStream.Seek(0L, SeekOrigin.Begin);
        }
        public virtual byte[] GetBuffer()
        {
            return this.memStream.GetBuffer();
        }
        public virtual int GetLength()
        {
            return (int)this.memStream.Length;
        }
        public void Read(Dictionary<string, string> diction)
        {
            ushort num = this.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                string key = this.ReadString();
                string value = this.ReadString();
                diction.Add(key, value);
            }
        }
        public void Read(out Dictionary<string, string> diction)
        {
            diction = this.ReadDictionStringString();
        }
        public void Read(out byte[] bytes)
        {
            bytes = this.ReadBytes();
        }
        public void Read(out char c)
        {
            c = this.reader.ReadChar();
        }
        public void Read(out char[] chars)
        {
            ushort count = this.reader.ReadUInt16();
            chars = this.reader.ReadChars((int)count);
        }
        public void Read(out short[] shorts)
        {
            shorts = this.ReadShorts();
        }
        public void Read<TK, TV>(out Dictionary<TK, TV> diction)
            where TK : IDynamicData, new()
            where TV : IDynamicData, new()
        {
            diction = this.ReadDiction<TK, TV>();
        }
        public void Read(Dictionary<int, int> diction)
        {
            diction.Clear();
            ushort num = this.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                int key = this.reader.ReadInt32();
                int value = this.reader.ReadInt32();
                diction.Add(key, value);
            }
        }
        public void Read<T>(out Dictionary<int, T> diction) where T : IDynamicData, new()
        {
            diction = this.ReadDictionInt32<T>();
        }
        public void Read(out Dictionary<int, int> diction)
        {
            diction = this.ReadDictionInt32Int32();
        }
        public void Read(out List<byte> list)
        {
            list = this.ReadListByte();
        }
        public void Read(out List<short> list)
        {
            list = this.ReadListInt16();
        }
        public void Read<T>(Dictionary<int, T> diction) where T : IDynamicData, new()
        {
            diction.Clear();
            ushort num = this.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                int key = this.reader.ReadInt32();
                T value = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
                value.Deserialize(this);
                diction.Add(key, value);
            }
        }
        public void Read<T>(out List<T> list) where T : IDynamicData, new()
        {
            list = this.ReadList<T>();
        }
        public void Read(out bool b)
        {
            b = this.reader.ReadBoolean();
        }
        public void Read(out List<int> list)
        {
            list = this.ReadListInt32();
        }
        public void Read(out List<float> list)
        {
            list = this.ReadListFloat();
        }
        public void Read(out byte b)
        {
            b = this.reader.ReadByte();
        }
        public void Read(out List<string> list)
        {
            list = this.ReadListString();
        }
        public void Read<TK, TV>(Dictionary<TK, TV> diction)
            where TK : IDynamicData, new()
            where TV : IDynamicData, new()
        {
            diction.Clear();
            ushort num = this.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                TK key = (default(TK) == null) ? Activator.CreateInstance<TK>() : default(TK);
                key.Deserialize(this);
                TV value = (default(TV) == null) ? Activator.CreateInstance<TV>() : default(TV);
                value.Deserialize(this);
                diction.Add(key, value);
            }
        }
        public void Read(out List<ushort> list)
        {
            list = this.ReadListUInt16();
        }
        public void Read(out List<uint> list)
        {
            list = this.ReadListUInt32();
        }
        public void Read(out List<ulong> list)
        {
            list = this.ReadListUInt64();
        }
        public void Read(out decimal d)
        {
            d = this.reader.ReadDecimal();
        }
        public void Read(out double d)
        {
            d = this.reader.ReadDouble();
        }
        public void Read(out short i)
        {
            i = this.reader.ReadInt16();
        }
        public void Read(out int i)
        {
            i = this.reader.ReadInt32();
        }
        public void Read<T>(List<T> list) where T : IDynamicData, new()
        {
            list.Clear();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                T item = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
                item.Deserialize(this);
                list.Add(item);
            }
        }
        public void Read(List<string> list)
        {
            list.Clear();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.ReadString());
            }
        }
        public void Read(out int[] ints)
        {
            ints = this.ReadInts();
        }
        public void Read(out long[] longs)
        {
            longs = this.ReadLongs();
        }
        public void Read(out sbyte[] bytes)
        {
            bytes = this.ReadSBytes();
        }
        public void Read(List<float> list)
        {
            list.Clear();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadSingle());
            }
        }
        public void Read(out ushort[] ushorts)
        {
            ushorts = this.ReadUShorts();
        }
        public void Read(List<ulong> list)
        {
            list.Clear();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadUInt64());
            }
        }
        public void Read(out uint i)
        {
            i = this.reader.ReadUInt32();
        }
        public void Read(out float f)
        {
            f = this.reader.ReadSingle();
        }
        public void Read(out ulong i)
        {
            i = this.reader.ReadUInt64();
        }
        public void Read(List<int> list)
        {
            list.Clear();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadInt32());
            }
        }
        public void Read(out string str)
        {
            str = this.ReadString();
        }
        public void Read(List<uint> list)
        {
            list.Clear();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadUInt32());
            }
        }
        public void Read(out sbyte b)
        {
            b = this.reader.ReadSByte();
        }
        public void Read(out ulong[] ulongs)
        {
            ulongs = this.ReadULongs();
        }
        public void Read<T>(T data) where T : IDynamicData
        {
            data.Deserialize(this);
        }
        public void Read(List<ushort> list)
        {
            list.Clear();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadUInt16());
            }
        }
        public void Read(out long i)
        {
            i = this.reader.ReadInt64();
        }
        public void Read(List<short> list)
        {
            list.Clear();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadInt16());
            }
        }
        public void Read(out ushort i)
        {
            i = this.reader.ReadUInt16();
        }
        public void Read<T>(out T[] array) where T : IDynamicData, new()
        {
            array = this.ReadArray<T>();
        }
        public void Read(List<byte> list)
        {
            list.Clear();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadByte());
            }
        }
        public void Read(out uint[] uints)
        {
            uints = this.ReadUInts();
        }
        public void Read(out byte[] bytes, int count)
        {
            bytes = this.ReadBytes(count);
        }
        public void Read(out short[] shorts, int count)
        {
            shorts = this.ReadShorts(count);
        }
        public void Read<T>(out T[] array, int count) where T : IDynamicData, new()
        {
            array = this.ReadArray<T>(count);
        }
        public void Read(out int[] ints, int count)
        {
            ints = this.ReadInts(count);
        }
        public void Read(out long[] longs, int count)
        {
            longs = this.ReadLongs(count);
        }
        public void Read(out sbyte[] bytes, int count)
        {
            bytes = this.ReadSBytes(count);
        }
        public void Read(out ushort[] ushorts, int count)
        {
            ushorts = this.ReadUShorts(count);
        }
        public void Read(out uint[] uints, int count)
        {
            uints = this.ReadUInts(count);
        }
        public void Read(out ulong[] ulongs, int count)
        {
            ulongs = this.ReadULongs(count);
        }
        public T[] ReadArray<T>() where T : IDynamicData, new()
        {
            ushort count = this.reader.ReadUInt16();
            return this.ReadArray<T>((int)count);
        }
        public T[] ReadArray<T>(int count) where T : IDynamicData, new()
        {
            T[] array = new T[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
                array[i].Deserialize(this);
            }
            return array;
        }
        public bool ReadBoolean()
        {
            return this.reader.ReadBoolean();
        }
        public byte ReadByte()
        {
            return this.reader.ReadByte();
        }
        public byte[] ReadBytes()
        {
            ushort count = this.reader.ReadUInt16();
            return this.ReadBytes((int)count);
        }
        public byte[] ReadBytes(int count)
        {
            byte[] array = new byte[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = this.reader.ReadByte();
            }
            return array;
        }
        public char ReadChar()
        {
            return this.reader.ReadChar();
        }
        public char[] ReadChars()
        {
            ushort count = this.reader.ReadUInt16();
            return this.reader.ReadChars((int)count);
        }
        public char[] ReadChars(int count)
        {
            return this.reader.ReadChars(count);
        }
        public T ReadData<T>() where T : IDynamicData, new()
        {
            T result = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
            result.Deserialize(this);
            return result;
        }
        public decimal ReadDecimal()
        {
            return this.reader.ReadDecimal();
        }
        public Dictionary<TK, TV> ReadDiction<TK, TV>()
            where TK : IDynamicData, new()
            where TV : IDynamicData, new()
        {
            Dictionary<TK, TV> dictionary = new Dictionary<TK, TV>();
            this.Read<TK, TV>(dictionary);
            return dictionary;
        }
        public Dictionary<int, T> ReadDictionInt32<T>() where T : IDynamicData, new()
        {
            Dictionary<int, T> dictionary = new Dictionary<int, T>();
            this.Read<T>(dictionary);
            return dictionary;
        }
        public Dictionary<int, int> ReadDictionInt32Int32()
        {
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            ushort num = this.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                int key = this.reader.ReadInt32();
                int value = this.reader.ReadInt32();
                dictionary.Add(key, value);
            }
            return dictionary;
        }
        public Dictionary<string, string> ReadDictionStringString()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            ushort num = this.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                string key = this.ReadString();
                string value = this.ReadString();
                dictionary.Add(key, value);
            }
            return dictionary;
        }
        public double ReadDouble()
        {
            return this.reader.ReadDouble();
        }
        public float ReadFloat()
        {
            return this.reader.ReadSingle();
        }
        public short ReadInt16()
        {
            return this.reader.ReadInt16();
        }
        public int ReadInt32()
        {
            return this.reader.ReadInt32();
        }
        public long ReadInt64()
        {
            return this.reader.ReadInt64();
        }
        public int[] ReadInts()
        {
            ushort count = this.reader.ReadUInt16();
            return this.ReadInts((int)count);
        }
        public int[] ReadInts(int count)
        {
            int[] array = new int[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = this.reader.ReadInt32();
            }
            return array;
        }
        public List<T> ReadList<T>() where T : IDynamicData, new()
        {
            List<T> list = new List<T>();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                T item = (default(T) == null) ? Activator.CreateInstance<T>() : default(T);
                item.Deserialize(this);
                list.Add(item);
            }
            return list;
        }
        public List<byte> ReadListByte()
        {
            List<byte> list = new List<byte>();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadByte());
            }
            return list;
        }
        public List<float> ReadListFloat()
        {
            List<float> list = new List<float>();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadSingle());
            }
            return list;
        }
        public List<short> ReadListInt16()
        {
            List<short> list = new List<short>();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadInt16());
            }
            return list;
        }
        public List<int> ReadListInt32()
        {
            List<int> list = new List<int>();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadInt32());
            }
            return list;
        }
        public List<string> ReadListString()
        {
            List<string> list = new List<string>();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.ReadString());
            }
            return list;
        }
        public List<ushort> ReadListUInt16()
        {
            List<ushort> list = new List<ushort>();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadUInt16());
            }
            return list;
        }
        public List<uint> ReadListUInt32()
        {
            List<uint> list = new List<uint>();
            ushort num = this.reader.ReadUInt16();
            for (int i = 0; i < (int)num; i++)
            {
                list.Add(this.reader.ReadUInt32());
            }
            return list;
        }
        public List<ulong> ReadListUInt64()
        {
            List<ulong> list = new List<ulong>();
            this.Read(list);
            return list;
        }
        public long[] ReadLongs()
        {
            ushort count = this.reader.ReadUInt16();
            return this.ReadLongs((int)count);
        }
        public long[] ReadLongs(int count)
        {
            long[] array = new long[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = this.reader.ReadInt64();
            }
            return array;
        }
        public sbyte ReadSByte()
        {
            return this.reader.ReadSByte();
        }
        public sbyte[] ReadSBytes()
        {
            ushort count = this.reader.ReadUInt16();
            return this.ReadSBytes((int)count);
        }
        public sbyte[] ReadSBytes(int count)
        {
            sbyte[] array = new sbyte[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = this.reader.ReadSByte();
            }
            return array;
        }
        public short[] ReadShorts()
        {
            ushort count = this.reader.ReadUInt16();
            return this.ReadShorts((int)count);
        }
        public short[] ReadShorts(int count)
        {
            short[] array = new short[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = this.reader.ReadInt16();
            }
            return array;
        }
        public string ReadString()
        {
            int count = (int)(this.ReadUInt16() - 1);
            byte[] bytes = this.ReadBytes(count);
            this.ReadByte();
            return Encoding.UTF8.GetString(bytes);
        }
        public ushort ReadUInt16()
        {
            return this.reader.ReadUInt16();
        }
        public uint ReadUInt32()
        {
            return this.reader.ReadUInt32();
        }
        public ulong ReadUInt64()
        {
            return this.reader.ReadUInt64();
        }
        public uint[] ReadUInts()
        {
            ushort count = this.reader.ReadUInt16();
            return this.ReadUInts((int)count);
        }
        public uint[] ReadUInts(int count)
        {
            uint[] array = new uint[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = this.reader.ReadUInt32();
            }
            return array;
        }
        public ulong[] ReadULongs()
        {
            ushort count = this.reader.ReadUInt16();
            return this.ReadULongs((int)count);
        }
        public ulong[] ReadULongs(int count)
        {
            ulong[] array = new ulong[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = this.reader.ReadUInt64();
            }
            return array;
        }
        public ushort[] ReadUShorts()
        {
            ushort count = this.reader.ReadUInt16();
            return this.ReadUShorts((int)count);
        }
        public ushort[] ReadUShorts(int count)
        {
            ushort[] array = new ushort[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = this.reader.ReadUInt16();
            }
            return array;
        }
        public void Seek(long offset, SeekOrigin loc)
        {
            this.memStream.Seek(offset, loc);
        }
        public virtual byte[] ToArray()
        {
            return this.memStream.ToArray();
        }
        public void Write<T>(T[] array) where T : IDynamicData, new()
        {
            this.writer.Write((ushort)array.Length);
            this.Write<T>(array, array.Length);
        }
        public void Write(int[] ints)
        {
            ushort num = (ushort)ints.Length;
            this.writer.Write(num);
            this.Write(ints, (int)num);
        }
        public void Write<T>(T data) where T : IDynamicData
        {
            data.Serialize(this);
        }
        public void Write(uint[] uints)
        {
            ushort num = (ushort)uints.Length;
            this.writer.Write(num);
            this.Write(uints, (int)num);
        }
        public void Write<TK, TV>(Dictionary<TK, TV> diction)
            where TK : IDynamicData, new()
            where TV : IDynamicData, new()
        {
            this.writer.Write((ushort)diction.Count);
            foreach (KeyValuePair<TK, TV> current in diction)
            {
                TK key = current.Key;
                key.Serialize(this);
                TV value = current.Value;
                value.Serialize(this);
            }
        }
        public void Write<T>(Dictionary<int, T> diction) where T : IDynamicData, new()
        {
            this.writer.Write((ushort)diction.Count);
            foreach (KeyValuePair<int, T> current in diction)
            {
                this.writer.Write(current.Key);
                T value = current.Value;
                value.Serialize(this);
            }
        }
        public void Write(long[] longs)
        {
            ushort num = (ushort)longs.Length;
            this.writer.Write(num);
            this.Write(longs, (int)num);
        }
        public void Write(Dictionary<int, int> diction)
        {
            this.writer.Write((ushort)diction.Count);
            foreach (KeyValuePair<int, int> current in diction)
            {
                this.writer.Write(current.Key);
                this.writer.Write(current.Value);
            }
        }
        public void Write(ushort[] ushorts)
        {
            ushort num = (ushort)ushorts.Length;
            this.writer.Write(num);
            this.Write(ushorts, (int)num);
        }
        public void Write(ulong[] ulongs)
        {
            ushort num = (ushort)ulongs.Length;
            this.writer.Write(num);
            this.Write(ulongs, (int)num);
        }
        public void Write(Dictionary<string, string> diction)
        {
            this.writer.Write((ushort)diction.Count);
            foreach (KeyValuePair<string, string> current in diction)
            {
                this.Write(current.Key);
                this.Write(current.Value);
            }
        }
        public void Write<T>(List<T> list) where T : IDynamicData, new()
        {
            this.writer.Write((ushort)list.Count);
            foreach (T current in list)
            {
                current.Serialize(this);
            }
        }
        public void Write(List<byte> list)
        {
            this.writer.Write((ushort)list.Count);
            foreach (byte current in list)
            {
                this.writer.Write(current);
            }
        }
        public void Write(List<ushort> list)
        {
            this.writer.Write((ushort)list.Count);
            foreach (ushort current in list)
            {
                this.writer.Write(current);
            }
        }
        public void Write(byte[] b)
        {
            this.writer.Write((ushort)b.Length);
            this.Write(b, b.Length);
        }
        public void Write(List<int> list)
        {
            this.writer.Write((ushort)list.Count);
            foreach (int current in list)
            {
                this.writer.Write(current);
            }
        }
        public void Write(List<short> list)
        {
            this.writer.Write((ushort)list.Count);
            foreach (short current in list)
            {
                this.writer.Write(current);
            }
        }
        public void Write(short[] shorts)
        {
            ushort num = (ushort)shorts.Length;
            this.writer.Write(num);
            this.Write(shorts, (int)num);
        }
        public void Write(char c)
        {
            this.writer.Write(c);
        }
        public void Write(List<float> list)
        {
            this.writer.Write((ushort)list.Count);
            foreach (float value in list)
            {
                this.writer.Write(value);
            }
        }
        public void Write(List<string> list)
        {
            this.writer.Write((ushort)list.Count);
            foreach (string current in list)
            {
                this.Write(current);
            }
        }
        public void Write(bool b)
        {
            this.writer.Write(b);
        }
        public void Write(byte b)
        {
            this.writer.Write(b);
        }
        public void Write(List<uint> list)
        {
            this.writer.Write((ushort)list.Count);
            foreach (uint current in list)
            {
                this.writer.Write(current);
            }
        }
        public void Write(List<ulong> list)
        {
            this.writer.Write((ushort)list.Count);
            foreach (ulong current in list)
            {
                this.writer.Write(current);
            }
        }
        public void Write(sbyte[] bytes)
        {
            this.writer.Write((ushort)bytes.Length);
            this.Write(bytes, bytes.Length);
        }
        public void Write(decimal d)
        {
            this.writer.Write(d);
        }
        public void Write(double d)
        {
            this.writer.Write(d);
        }
        public void Write(short i)
        {
            this.writer.Write(i);
        }
        public void Write(int i)
        {
            this.writer.Write(i);
        }
        public void Write(long i)
        {
            this.writer.Write(i);
        }
        public void Write(sbyte b)
        {
            this.writer.Write(b);
        }
        public void Write(float f)
        {
            this.writer.Write(f);
        }
        public void Write(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            this.Write((ushort)(bytes.Length + 1));
            this.Write(bytes, bytes.Length);
            this.Write(0);
        }
        public void Write(ushort i)
        {
            this.writer.Write(i);
        }
        public void Write(uint i)
        {
            this.writer.Write(i);
        }
        public void Write(char[] chars)
        {
            this.writer.Write((ushort)chars.Length);
            this.writer.Write(chars);
        }
        public void Write(ulong i)
        {
            this.writer.Write(i);
        }
        public void Write<T>(T[] array, int count) where T : IDynamicData, new()
        {
            for (int i = 0; i < count; i++)
            {
                array[i].Serialize(this);
            }
        }
        public void Write(byte[] bytes, int count)
        {
            int num = Math.Min(bytes.Length, count);
            int i;
            for (i = 0; i < num; i++)
            {
                this.writer.Write(bytes[i]);
            }
            while (i < count)
            {
                this.writer.Write(0);
                i++;
            }
        }
        public void Write(char[] chars, int count)
        {
            this.writer.Write(chars, 0, count);
        }
        public void Write(short[] shorts, int count)
        {
            int num = Math.Min(shorts.Length, count);
            int i;
            for (i = 0; i < num; i++)
            {
                this.writer.Write(shorts[i]);
            }
            while (i < count)
            {
                this.writer.Write(0);
                i++;
            }
        }
        public void Write(int[] ints, int count)
        {
            int num = Math.Min(ints.Length, count);
            int i;
            for (i = 0; i < num; i++)
            {
                this.writer.Write(ints[i]);
            }
            while (i < count)
            {
                this.writer.Write(0);
                i++;
            }
        }
        public void Write(long[] longs, int count)
        {
            int num = Math.Min(longs.Length, count);
            int i;
            for (i = 0; i < num; i++)
            {
                this.writer.Write(longs[i]);
            }
            while (i < count)
            {
                this.writer.Write(0L);
                i++;
            }
        }
        public void Write(sbyte[] bytes, int count)
        {
            int num = Math.Min(bytes.Length, count);
            int i;
            for (i = 0; i < num; i++)
            {
                this.writer.Write(bytes[i]);
            }
            while (i < count)
            {
                this.writer.Write(0);
                i++;
            }
        }
        public void Write(ushort[] ushorts, int count)
        {
            int num = Math.Min(ushorts.Length, count);
            int i;
            for (i = 0; i < num; i++)
            {
                this.writer.Write(ushorts[i]);
            }
            while (i < count)
            {
                this.writer.Write(0);
                i++;
            }
        }
        public void Write(uint[] uints, int count)
        {
            int num = Math.Min(uints.Length, count);
            int i;
            for (i = 0; i < num; i++)
            {
                this.writer.Write(uints[i]);
            }
            while (i < count)
            {
                this.writer.Write(0u);
                i++;
            }
        }
        public void Write(ulong[] ulongs, int count)
        {
            int num = Math.Min(ulongs.Length, count);
            int i;
            for (i = 0; i < num; i++)
            {
                this.writer.Write(ulongs[i]);
            }
            while (i < count)
            {
                this.writer.Write(0uL);
                i++;
            }
        }
    }
}