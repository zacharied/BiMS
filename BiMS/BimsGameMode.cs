using BiMS.Charting;
using System;
using System.Collections.Generic;
using System.Text;
using theori.Charting;
using theori.GameModes;

namespace BiMS
{
    public class BimsGameMode : GameMode
    {
        public static GameMode Instance { get; } = new BimsGameMode();

        public BimsGameMode() : base("BiMS") { }

        public override bool SupportsStandaloneUsage => base.SupportsStandaloneUsage;

        public override bool SupportsSharedUsage => base.SupportsSharedUsage;

        public override ChartFactory CreateChartFactory() => new BimsChartFactory();

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void InvokeStandalone(string[] args)
        {
            base.InvokeStandalone(args);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
