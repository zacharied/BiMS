using System;
using System.Collections.Generic;
using System.Text;
using theori.Charting;

namespace BiMS.Charting
{
    public sealed class BimsChartFactory : ChartFactory
    {
        public static BimsChartFactory Instance = new BimsChartFactory();

        public override Chart CreateNew()
        {
            var chart = new Chart(BimsGameMode.Instance);

            for (int i = 0; i < 8; i++)
                chart.CreateTypedLane<NoteEntity>(i, EntityRelation.Equal);
            chart.CreateTypedLane<NoteTypedEvent>(BimsLaneLabel.NoteEvent, EntityRelation.Subclass);
            chart.CreateTypedLane<HighwayTypedEvent>(BimsLaneLabel.HighwayEvent, EntityRelation.Subclass);

            return chart;
        }

        public Chart TestChart()
        {
            var chart = Instance.CreateNew();

            var e = new NoteEntity
            {
                Position = 3,
                Lane = 0
            };
            chart[0].Add(e);

            return chart;
        }
    }
}
