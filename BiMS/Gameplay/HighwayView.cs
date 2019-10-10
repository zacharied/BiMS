using BiMS.Charting;
using System;
using System.Collections.Generic;
using theori;
using theori.Charting;
using theori.Charting.Playback;
using theori.Graphics;

namespace BiMS.Gameplay
{
    class HighwayView
    {
        private const float LANE_WIDTH_SCRATCH = 60;
        private const float LANE_WIDTH_SMALL = 26;
        private const float LANE_WIDTH_LARGE = 34;
        private const float LANE_SEPARATOR_WIDTH = 2;
        private const float LANES_HEIGHT = 480;
        private const float NOTE_HEIGHT = 10;

        public Dictionary<LaneLabel, List<NoteEntity>> renderableEntities;

        private readonly BasicSpriteRenderer renderer;
        private readonly float xPos, yPos;

        private readonly SlidingChartPlayback playback;

        private bool isDrawing = false;

        private float LanePos(int laneNum) {
            float width = 0;
            if (laneNum == 0)
                return LANE_SEPARATOR_WIDTH;
            width += LANE_WIDTH_SCRATCH;
            width += (laneNum / 2) * LANE_WIDTH_LARGE;
            width += ((laneNum - 1) / 2) * LANE_WIDTH_SMALL;
            width += (laneNum + 1) * LANE_SEPARATOR_WIDTH;
            return width;
        }

        public HighwayView(SlidingChartPlayback playback, float x, float y)
        {
            this.playback = playback;

            renderer = new BasicSpriteRenderer();

            renderableEntities = new Dictionary<LaneLabel, List<NoteEntity>>();
            for (int i = 0; i < 8; i++)
                renderableEntities[i] = new List<NoteEntity>();

            xPos = x; yPos = y;
        }

        public void Render()
        {
            renderer.BeginFrame();
            isDrawing = true;
            renderer.SetColor(206, 133, 188);
            renderer.FillRect(0, 0, Window.Width, Window.Height);
            renderer.Translate(xPos, yPos);
            RenderLanes();
            for (int i = 0; i < 8; i++)
                RenderNotes(i);
            renderer.EndFrame();
            isDrawing = false;
        }

        private void RenderLanes()
        {
            if (!isDrawing) throw new InvalidOperationException("cannot render without drawing");

            void DrawSeparator(float x)
            {
                renderer.SetColor(87, 87, 87);
                renderer.FillRect(x, 0, LANE_SEPARATOR_WIDTH, LANES_HEIGHT);
            }

            void DrawScratchLane(float x)
            {
                renderer.SetColor(10, 10, 10);
                renderer.FillRect(x, 0, LANE_WIDTH_SCRATCH, LANES_HEIGHT);
            }

            void DrawLargeLane(float x)
            {
                renderer.SetColor(30, 30, 30);
                renderer.FillRect(x, 0, LANE_WIDTH_LARGE, LANES_HEIGHT);
            }

            void DrawSmallLane(float x)
            {
                renderer.SetColor(10, 10, 10);
                renderer.FillRect(x, 0, LANE_WIDTH_SMALL, LANES_HEIGHT);
            }

            for (int i = 0; i < 7; i++)
                DrawSeparator(LanePos(i + 1) - LANE_SEPARATOR_WIDTH);
            DrawScratchLane(LanePos(0));
            for (int i = 1; i < 8; i += 2)
                DrawLargeLane(LanePos(i));
            for (int i = 2; i < 8; i += 2)
                DrawSmallLane(LanePos(i));
        }

        private void DrawNote(int i, float y)
        {
            if (i == 0)
            {
                renderer.SetColor(200, 0, 0);
                renderer.FillRect(LanePos(i), y, LANE_WIDTH_SCRATCH, NOTE_HEIGHT);
            }
        }
        
        private void RenderNotes(int i)
        {
            if (!isDrawing) throw new InvalidOperationException("cannot render without drawing");
            
            foreach (var entity in renderableEntities[i])
            {
                float y = LANES_HEIGHT * playback.GetRelativeDistance(entity.AbsolutePosition);
                DrawNote(i, LANES_HEIGHT - y);
            }
        }
    }
}
