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

            for (int i = 1; i < 16; i++)
            {
                chart[0].Add(new NoteEntity { Position = i });
                chart[1].Add(new NoteEntity { Position = i });
                chart[7].Add(new NoteEntity { Position = i + 0.5 });
                chart[4].Add(new NoteEntity { Position = i + 0.75 });
            }

            return chart;
        }
    }
}
