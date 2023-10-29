using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using System.IO;
using osu.Framework.Testing;

namespace CrankItUp.Game
{
    public partial class InitialSetup : Screen
    {
        CIUButton startButton;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures)
        {
            startButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Start setup",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 0),
                Action = () => PushMenu(audio),
            };

            string[] Intro =
            {
                "Hi! Welcome to Crank It Up!",
                "We need to do some initial setup before we can get started",
                "So just hit that big shiny button below!",
                "If the game freezes after clicking the button, don't panic! It might take a while though"
            };
            Drawable[] introTextList;
            introTextList = new SpriteText[Intro.Length];
            SpriteText TempText;
            for (int i = 0; i < Intro.Length; i++)
            {
                TempText = new SpriteText
                {
                    Y = 100 + (i * 30),
                    Text = Intro[i],
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 20)
                };
                introTextList[i] = TempText;
            }

            InternalChildren = new Drawable[]
            {
                startButton,
                new SpriteText
                {
                    Y = 20,
                    Text = "Initial Setup",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
                introTextList[0],
                introTextList[1],
                introTextList[2],
                introTextList[3]
            };
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        void PushMenu(AudioManager audio)
        {
            Directory.CreateDirectory("maps");
            Directory.CreateDirectory("maps/Test");
            StreamWriter testmap = File.CreateText("maps/Test/easy.json");
            // yes I know this creates a really ugly file, but it works
            testmap.WriteLine(
                @"{
                            'BaseNoteQueue':[
                                {'position': 0, 'spawnTime': 0},
                                {'position': 5, 'spawnTime': 200},
                                {'position': 10, 'spawnTime': 400},
                                {'position': 15, 'spawnTime': 600},
                                {'position': 20, 'spawnTime': 800}
                            ]
                        }"
            );
            testmap.Close();
            FileStream testsong = File.Create("maps/Test/music.mp3");
            audio.GetTrackStore().GetStream("Tera I_O.mp3").CopyTo(testsong);
            testsong.Close();
            this.Push(new TitleScreen());
        }
    }
}
