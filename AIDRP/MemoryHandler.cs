using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDRP
{
    public class MemoryHandler
    {
        public static VAMemory mem;
        public static IntPtr Base;

        public static UInt32 GetBaseAddr()
        {
            return (UInt32)mem.getBaseAddress;
        }

        public static UInt32 ReadUInt32(UInt32 BaseAddr, UInt32 Offset)
        {
            return mem.ReadUInt32((IntPtr)(BaseAddr + Offset));
        }

        public static UInt32 ReadUInt16(UInt32 BaseAddr, UInt32 Offset)
        {
            return mem.ReadUInt16((IntPtr)(BaseAddr + Offset));
        }

        public static float ReadFloat(UInt32 BaseAddr, UInt32 Offset)
        {
            return mem.ReadFloat((IntPtr)(BaseAddr + Offset));
        }

        public static byte ReadByte(UInt32 BaseAddr, UInt32 Offset)
        {
            return mem.ReadByte((IntPtr)(BaseAddr + Offset));
        }

        public static char ReadChar(UInt32 BaseAddr, UInt32 Offset)
        {
            return Convert.ToChar(mem.ReadByte((IntPtr)(BaseAddr + Offset)));
        }
    }
}
