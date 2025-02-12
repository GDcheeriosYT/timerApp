using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace timerApp.Game
{
    public partial class Timer : CompositeDrawable
    {
        private bool onBreak = true;
        private double elapsedSeconds = 0;

        private SpriteText timerText;

        public Timer()
        {
            Masking = true;
            CornerRadius = 10;
            CornerExponent = 1.5f;

            InternalChildren =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(17, 17, 17, 177)
                },
                timerText = new SpriteText
                {
                    Text = "00:00:00",
                    Colour = Colour4.White,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Font = new FontUsage(size: 50)
                }
            ];
        }

        public void Pause() => onBreak = true;
        public void Resume() => onBreak = false;

        public void Reset()
        {
            onBreak = true;
            elapsedSeconds = 0;
            updateTimerDisplay();
        }

        public void Increment(double timePast)
        {
            if (onBreak) return;
            elapsedSeconds += timePast / 1000.0;

            updateTimerDisplay();
        }

        public float GetHours() => (float)elapsedSeconds / 3600;

        private void updateTimerDisplay()
        {
            TimeSpan time = TimeSpan.FromSeconds(elapsedSeconds);
            timerText.Text = time.ToString(@"hh\:mm\:ss");
        }
    }
}
