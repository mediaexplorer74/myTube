// Decompiled with JetBrains decompiler
// Type: RykenTube.MediaBoxReader
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RykenTube
{
  public class MediaBoxReader
  {
    private int offset = 8;
    private int bitOffset;
    private byte[] bytes;
    private int length;
    private int shiftOffset;
    private uint payloadSize;
    private string boxName;
    private BitArray bits;

    public int RemainingBits => this.bits.Length - this.bitOffset;

    public int ByteOffset => this.bitOffset / 8;

    public MediaBoxReader(byte[] bytes)
    {
      this.bytes = bytes;
      this.bits = new BitArray(bytes);
      this.length = bytes.Length;
      this.payloadSize = !BitConverter.IsLittleEndian ? BitConverter.ToUInt32(bytes, 0) : BitConverter.ToUInt32(this.reverse(bytes, 0, 4), 0);
      this.boxName = Encoding.UTF8.GetString(bytes, 4, 4);
    }

    private byte[] reverse(byte[] bytes, int index, int length)
    {
      byte[] numArray = new byte[length];
      for (int index1 = 0; index1 < length; ++index1)
        numArray[length - index1 - 1] = bytes[index + index1];
      for (int index2 = 0; index2 < length; ++index2)
      {
        BitArray bitArray = new BitArray(bytes);
      }
      return numArray;
    }

    public SIDXInfo ReadSIDX()
    {
      this.bitOffset += 96;
      SIDXInfo sidxInfo = new SIDXInfo();
      sidxInfo.ReferenceID = this.ReadUInt32();
      sidxInfo.TimeScale = this.ReadUInt32();
      sidxInfo.EarliestPresentationTime = (ulong) this.ReadUInt32();
      sidxInfo.FirstOffset = (ulong) this.ReadUInt32();
      sidxInfo.Reserved = this.ReadUInt16();
      sidxInfo.ReferenceCount = this.ReadUInt16();
      for (int index = 0; index < (int) sidxInfo.ReferenceCount; ++index)
        sidxInfo.Segments.Add(new SIDXSegment()
        {
          ReferenceType = this.GetBool(),
          ReferencedSize = this.ReadUInt32((byte) 31),
          SubsegmentDuration = this.ReadUInt32(),
          StartsWithSAP = this.GetBool(),
          SAPType = this.ReadByte((byte) 3),
          SAPDeltaTime = this.ReadUInt32((byte) 28)
        });
      return sidxInfo;
    }

    public bool SeekToBox(string boxTitle)
    {
      int index1 = 0;
      int num = 0;
      for (int index2 = 0; index2 < this.bytes.Length; ++index2)
      {
        if ((int) this.bytes[index2] == (int) boxTitle[index1])
        {
          if (index1 == 0)
            num = index2;
          ++index1;
          if (index1 == boxTitle.Length)
          {
            this.bitOffset = (num - 4) * 8;
            return true;
          }
        }
        else
          index1 = 0;
      }
      return false;
    }

    public List<string> GetBoxNames()
    {
      Match match = Regex.Match(Encoding.UTF8.GetString(this.bytes, 0, this.bytes.Length), "[a-z]{4}");
      List<string> boxNames = new List<string>();
      for (; match.Success; match = match.NextMatch())
        boxNames.Add(match.Value);
      return boxNames;
    }

    public bool GetBool()
    {
      int num = this.bits[this.bitOffset] ? 1 : 0;
      ++this.bitOffset;
      return num != 0;
    }

    private BitArray getBitRange(int offset, int length, bool reverse)
    {
      BitArray bitRange = new BitArray(length);
      if (!reverse)
      {
        int index1 = 0;
        for (int index2 = offset; index2 < offset + length; ++index2)
        {
          bitRange[index1] = this.bits[index2];
          ++index1;
        }
      }
      else
      {
        int num = offset + length - 1;
        while (num <= offset)
          --num;
      }
      return bitRange;
    }

    private byte[] getByteArray(int length, int? trueLength = null, bool reverse = true)
    {
      if (!trueLength.HasValue)
        trueLength = new int?(length);
      int num1 = trueLength.Value;
      byte[] source = new byte[num1 % 8 == 0 ? num1 / 8 : num1 / 8 + 1];
      for (int index = 0; index < source.Length; ++index)
        source[index] = (byte) 0;
      int index1 = 0;
      int num2 = (8 - length % 8) % 8;
      for (int index2 = 0; index2 < length; ++index2)
      {
        if (this.bits[this.bitOffset])
          source[index1] |= (byte) (1 << num2);
        ++this.bitOffset;
        ++num2;
        if (num2 >= 8)
        {
          num2 = 0;
          ++index1;
        }
      }
      if (reverse && BitConverter.IsLittleEndian)
        source = ((IEnumerable<byte>) source).Reverse<byte>().ToArray<byte>();
      return source;
    }

    private void shift(byte shift)
    {
      for (int offset = this.offset; offset < this.bytes.Length; ++offset)
      {
        byte num = this.bytes[offset];
        int index = offset - 1;
        this.bytes[offset] = (byte) ((uint) num << (int) shift);
        if (index > this.offset)
          this.bytes[index] += (byte) ((uint) this.bytes[offset] >> 8 - (int) shift);
      }
      this.shiftOffset += (int) shift;
    }

    private byte readByteShifted(byte shift)
    {
      int num = (int) (byte) ((uint) this.bytes[this.offset] >> 8 - (int) shift);
      this.shift(shift);
      return (byte) num;
    }

    public string ReadString(byte bitLength)
    {
      byte[] byteArray = this.getByteArray((int) bitLength, reverse: false);
      char[] chArray = new char[byteArray.Length];
      for (int index = 0; index < chArray.Length; ++index)
        chArray[index] = (char) byteArray[index];
      return new string(chArray);
    }

    public byte ReadByte(byte length = 8) => this.getByteArray((int) length, new int?(8))[0];

    public ushort ReadUInt16(byte length = 16) => BitConverter.ToUInt16(this.getByteArray((int) length, new int?(16)), 0);

    public uint ReadUInt32(byte length = 32) => BitConverter.ToUInt32(this.getByteArray((int) length, new int?(32)), 0);

    public ulong ReadUInt64(byte length = 64) => BitConverter.ToUInt64(this.getByteArray((int) length, new int?(64)), 0);

    public float ReadFloat(byte bitLength = 32) => BitConverter.ToSingle(this.getByteArray((int) bitLength, new int?(32)), 0);

    public double ReadDouble(byte bitLength = 64) => BitConverter.ToDouble(this.getByteArray((int) bitLength, new int?(64)), 0);

    private bool isEven(byte b) => (int) b % 2 == 0;
  }
}
