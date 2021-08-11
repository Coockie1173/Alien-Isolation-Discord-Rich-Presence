using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIDRP
{
    public class CurrentData
    {
        public static string CurrentState;
        public static int CurrentMission;
        public static string MapName;

        public static void ReadCurrentData()
        {
            int CurOffset = 0;
            //ReadString Current State
            UInt32 CStateOffset = 0;
            CStateOffset = MemoryHandler.ReadUInt32((UInt32)MemoryHandler.Base, 0x17E4814);
            CStateOffset = MemoryHandler.ReadUInt32(CStateOffset, 0x04);
            CurOffset = 0x430;
            CurrentState = "";
            while(MemoryHandler.ReadByte(CStateOffset, (uint)CurOffset) != 00)
            {
                CurrentState += MemoryHandler.ReadChar(CStateOffset, (uint)CurOffset);
                CurOffset++;
            }

            CurOffset = 0x470;
            MapName = "";
            while (MemoryHandler.ReadByte(CStateOffset, (uint)CurOffset) != 00)
            {
                MapName += MemoryHandler.ReadChar(CStateOffset, (uint)CurOffset);
                CurOffset++;
            }

            CurOffset = 0x4E8;
            CurrentMission = (int)MemoryHandler.ReadUInt32(CStateOffset, (uint)CurOffset);
        }
    }
}
