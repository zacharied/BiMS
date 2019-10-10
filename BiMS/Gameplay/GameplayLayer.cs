using System;
using System.Collections.Generic;
using System.Text;
using theori;
using theori.Charting.Playback;
using BiMS.Charting;

namespace BiMS.Gameplay
{
    class GameplayLayer : Layer
    {
        private SlidingChartPlayback playback;
        private HighwayView highwayView;

        public override void Initialize()
        {
            base.Initialize();
            playback = new SlidingChartPlayback(BimsChartFactory.Instance.TestChart());
            highwayView = new HighwayView(playback, 50, 0);

            playback.DefaultViewTime = 4 * 60.0 / 120;
            playback.ObjectHeadCrossPrimary += (dir, entity) =>
            {
                if (entity is NoteEntity note)
                {
                    highwayView.renderableEntities[entity.Lane].Add(note);
                }
            };
            playback.ObjectHeadCrossSecondary += (dir, entity) =>
            {
                if (entity is NoteEntity note)
                    highwayView.renderableEntities[entity.Lane].Remove(note);
            };
            playback.Position = 0;
        }

        public override void Update(float delta, float total)
        {
            base.Update(delta, total);

            // TODO Tie this to audio playback.
            playback.Position += delta;
        }

        public override void Render()
        {
            base.Render();

            highwayView.Render();
        }
    }
}
