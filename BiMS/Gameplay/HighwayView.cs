using BiMS.Charting;
using System;
using System.Collections.Generic;
using theori;
using theori.Charting;
using theori.Charting.Playback;
using theori.Graphics;
using BiMS;

namespace TheoriExtensions
{
    static class BasicSpriteRendererExtension
    {
        public static void FillRectCroppedToScreenMask(this BasicSpriteRenderer r, float x,  float y, float w, float h, Rect screenMask)
        {
            Rect rect = new Rect(x, y, w, h);
            if (rect.Left > screenMask.Right || rect.Right < screenMask.Left
                || rect.Top > screenMask.Bottom || rect.Bottom < screenMask.Top)
            {
                // Just don't draw anything.
                return;
            }

            r.FillRect(Math.Max(rect.Left, screenMask.Left), Math.Max(rect.Top, screenMask.Top),
                Math.Min(rect.Right, screenMask.Right) - rect.Left, Math.Min(rect.Bottom, screenMask.Bottom) - rect.Top);
        }

        public static void SetColor(this BasicSpriteRenderer r, (float, float, float) color)
        {
            r.SetColor(color.Item1, color.Item2, color.Item3);
        }
    }
}

namespace BiMS.Gameplay
{
    using TheoriExtensions;

    struct Lane
    {
        public (float, float, float) NoteColor { get; }
        public (float, float, float) BackgroundColor { get; }
        public Rect Rect { get; }

        public Lane((float, float, float) noteColor, (float, float, float) backgroundColor, Rect rect)
        {
            this.NoteColor = noteColor;
            this.BackgroundColor = backgroundColor;
            this.Rect = rect;
        }
    }

    class HighwayView
    {
        private const float LANE_WIDTH_SCRATCH = 60;
        private const float LANE_WIDTH_SMALL = 26;
        private const float LANE_WIDTH_LARGE = 34;
        private const float LANE_SEPARATOR_WIDTH = 2;
        private const float LANES_HEIGHT = 480;
        private const float NOTE_HEIGHT = 10;
        private readonly (float, float, float) NOTE_COLOR_SCRATCH = (200, 0, 0);
        private readonly (float, float, float) NOTE_COLOR_SMALL = (0, 0, 200);
        private readonly (float, float, float) NOTE_COLOR_LARGE = (175, 170, 180);
        private readonly (float, float, float) BACKGROUND_COLOR_SCRATCH = (10, 10, 10);
        private readonly (float, float, float) BACKGROUND_COLOR_SMALL = (10, 10, 10);
        private readonly (float, float, float) BACKGROUND_COLOR_LARGE = (30, 30, 30);


        private readonly Lane[] lanes;

        public Dictionary<LaneLabel, List<NoteEntity>> renderableEntities;

        private readonly BasicSpriteRenderer renderer;
        private readonly float xPos, yPos;

        private readonly SlidingChartPlayback playback;

        private readonly Rect highwayScreenMask;

        private bool isDrawing = false;

        public HighwayView(SlidingChartPlayback playback, float x, float y)
        {
            this.playback = playback;

            renderer = new BasicSpriteRenderer();

            renderableEntities = new Dictionary<LaneLabel, List<NoteEntity>>();
            for (int i = 0; i < BimsUtil.NUM_LANES; i++)
                renderableEntities[i] = new List<NoteEntity>();

            xPos = x; yPos = y;

            static float LanePos(int laneNum)
            {
                float width = 0;
                if (laneNum == 0)
                    return LANE_SEPARATOR_WIDTH;
                width += LANE_WIDTH_SCRATCH;
                width += (laneNum / 2) * LANE_WIDTH_LARGE;
                width += ((laneNum - 1) / 2) * LANE_WIDTH_SMALL;
                width += (laneNum + 1) * LANE_SEPARATOR_WIDTH;
                return width;
            }

            lanes = new Lane[BimsUtil.NUM_LANES];
            for (int i = 0; i < lanes.Length; i++)
            {
                if (i == 0)
                    lanes[i] = new Lane(NOTE_COLOR_SCRATCH, BACKGROUND_COLOR_SCRATCH, new Rect(LanePos(0), 0, LANE_WIDTH_SCRATCH, LANES_HEIGHT));
                else if (i % 2 == 1)
                    lanes[i] = new Lane(NOTE_COLOR_LARGE, BACKGROUND_COLOR_LARGE, new Rect(LanePos(i), 0, LANE_WIDTH_LARGE, LANES_HEIGHT));
                else
                    lanes[i] = new Lane(NOTE_COLOR_SMALL, BACKGROUND_COLOR_SMALL, new Rect(LanePos(i), 0, LANE_WIDTH_SMALL, LANES_HEIGHT));
            }

            highwayScreenMask = new Rect(xPos, yPos, Width, Height);
        }

        public float Width
        {
            get { return lanes[^1].Rect.Right; }
        }

        public float Height
        {
            get { return LANES_HEIGHT; }
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

            // Draw separators.
            for (int i = 0; i < BimsUtil.NUM_LANES; i++)
            {
                renderer.SetColor(87, 87, 87);
                renderer.FillRect(xPos + lanes[i].Rect.Left - LANE_SEPARATOR_WIDTH, yPos, LANE_SEPARATOR_WIDTH, LANES_HEIGHT);
            }

            for (int i = 0; i < BimsUtil.NUM_LANES; i++)
            {
                renderer.SetColor(lanes[i].BackgroundColor);
                renderer.FillRect(xPos + lanes[i].Rect.Left, yPos, lanes[i].Rect.Width, LANES_HEIGHT);
            }
        }

        private void DrawNote(int i, float y)
        {
            renderer.SetColor(lanes[i].NoteColor);
            renderer.FillRectCroppedToScreenMask(xPos + lanes[i].Rect.Left, yPos + y, lanes[i].Rect.Width, NOTE_HEIGHT, this.highwayScreenMask);
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
