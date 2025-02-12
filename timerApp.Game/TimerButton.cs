using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace timerApp.Game
{
    public partial class TimerButton : BasicButton
    {
        private SpriteText buttonText;
        private Action action;

        public TimerButton(string text, Action action)
        {
            this.action = action;
            Children =
            [
                new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                buttonText = new SpriteText
                {
                    Text = text,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Colour4.Gray,
                    Font = new FontUsage(size: 30)
                }
            ];
        }

        protected override bool OnClick(ClickEvent e)
        {
            action.Invoke();
            return base.OnClick(e);
        }

        public void SetAction(Action action) => this.action = action;
        public void SetText(string text) => buttonText.Text = text;
    }
}
