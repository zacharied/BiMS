using System;
using System.Collections.Generic;
using System.Text;
using theori;
using theori.Charting.Playback;
using BiMS.Charting;
using BiMS.IO;

namespace BiMS.Gameplay
{
    class GameplayLayer : BimsLayer
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

            Controller.Instance().ButtonPressed = OnControllerButtonPressed;
            Controller.Instance().ButtonReleased = OnControllerButtonReleased;
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

        public override void OnControllerButtonPressed(ControllerInput input)
        {
            highwayView.OnButtonPressed(input);
        }

        public override void OnControllerButtonReleased(ControllerInput input)
        {
            highwayView.OnButtonReleased(input);
        }
    }
}
