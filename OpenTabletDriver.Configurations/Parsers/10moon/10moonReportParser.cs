using System;
using System.Numerics;
using OpenTabletDriver.Tablet;

namespace OpenTabletDriver.Configurations.Parsers.TenMoon
{
    public struct TenMoonReport : ITabletReport, IAuxReport
    {
        public TenMoonReport(byte[] report)
        {
            Raw = report;
            Position = new Vector2
            {
                X = report[1] << 8 | report[2],
                Y = report[3] << 8 | report[4]
            };
            Pressure = (uint)Math.Min(Math.Max((1650 - (report[5] << 8 | report[6])), 0), 860);
            PenButtons = new bool[]
            {
                report[9] == 4,
                report[9] == 6
            };
            AuxButtons = new bool[]
            {
                !report[12].IsBitSet(1),
                !report[12].IsBitSet(4),
                !report[11].IsBitSet(7),
                !report[12].IsBitSet(0),
                !report[11].IsBitSet(6),
                !report[12].IsBitSet(5),
                !report[11].IsBitSet(5),
                !report[11].IsBitSet(0),
                !report[11].IsBitSet(4),
                !report[11].IsBitSet(1),
                !report[11].IsBitSet(3),
                !report[11].IsBitSet(2),
            };
        }

        public bool[] AuxButtons { set; get; }
        public byte[] Raw { set; get; }
        public Vector2 Position { set; get; }
        public uint Pressure { set; get; }
        public bool[] PenButtons { set; get; }
    }

    public class TenMoonReportParser : IReportParser<IDeviceReport>
    {
        public IDeviceReport Parse(byte[] report)
        {
            /*
             DEXP GT-M102:
             PosX: [1][2]
             PosY: [3][4]
             Pressure: [5][6]
             Btn+: [9] == 4
             Btn-: [9] == 6
             Aux1: ![12]bit2
             Aux2: ![12]bit5
             Aux3: ![11]bit8
             Aux4: ![12]bit1
             Aux5: ![11]bit7
             Aux6: ![12]bit6
             Aux7: ![11]bit6
             Aux8: ![11]bit1
             Aux9: ![11]bit5
             Aux10: ![11]bit2
             Aux11: ![11]bit4
             Aux12: ![11]bit3
             */

            return new TenMoonReport(report);
        }
    }
}
