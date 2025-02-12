using System;
using System.Drawing;
using System.Net.Http;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using WindowState = osu.Framework.Platform.WindowState;

namespace timerApp.Game
{
    public partial class timerAppGame : timerAppGameBase
    {
        private ScreenStack screenStack;
        private Bindable<Projects> projects = new(Projects.test);
        private ProjectsDropdown projectsDropdown;
        private Timer timer;
        private TimerButton startButton;
        private TimerButton pauseButton;
        private SpriteText details;
        private bool readyToReset = false;
        private bool isStarted = false;

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            Size windowSize = new Size();
            windowSize.Width = 640;
            windowSize.Height = 400;

            if (host.Window != null)
            {
                host.Window.Title = "timerApp";
                host.Window.WindowState = WindowState.Normal;
                host.Window.MinSize = windowSize;
                host.Window.MaxSize = windowSize;
                host.Window.Resizable = false;
                host.AudioThread.Exit();
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = ColourInfo.GradientVertical(
                    Colour4.White,
                    Colour4.Gray
                )
            });
            Add(projectsDropdown = new ProjectsDropdown(projects)
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.X,
                Margin = new MarginPadding(10),
                Width = 0.3f
            });
            Add(timer = new Timer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(250, 100),
            });
            Add(startButton = new TimerButton("Start", pressStart)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(200, 25),
                Y = 75
            });
            Add(pauseButton = new TimerButton("Pause", pressPause)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(200, 25),
                Y = 110
            });
            Add(details = new SpriteText
            {
                Text = $"Currently working on: {projects.Value.ToString()}",
                Origin = Anchor.TopRight,
                Anchor = Anchor.TopRight,
                Margin = new MarginPadding { Top = 10, Right = 10 },
                Colour = Colour4.Black,
            });
            pauseButton.Hide();
            projectsDropdown.Current.BindValueChanged(e =>
            {
                if (e.NewValue != null)
                {
                    projects.Value = Enum.Parse<Projects>(e.NewValue);
                    details.Text = $"Currently working on: {projects.Value.ToString()}";
                }
            }, true);
        }

        protected override void Update()
        {
            base.Update();
            timer.Increment(Time.Elapsed);
        }

        private void pressStart()
        {
            int projectNumber = (int)projects.Value;

            if (!isStarted)
            {
                isStarted = true;
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:8080");
                client.GetAsync($"/start/{projectNumber}").Wait();
            }

            readyToReset = false;
            timer.Resume();
            pauseButton.Show();
            startButton.SetText("Resume");
            pauseButton.SetText("Pause");
        }

        private void pressPause()
        {
            if (!readyToReset)
            {
                pauseButton.SetText("End");
                timer.Pause();
                readyToReset = true;
            }
            else
            {
                end();
            }
        }

        private void end()
        {
            float hours = timer.GetHours();
            startButton.SetText("Start");
            timer.Reset();
            pauseButton.Hide();
            isStarted = false;

            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080");
            client.GetAsync($"/end/{hours}").Wait();
        }
    }
}
