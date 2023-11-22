using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;
using System.IO;
using osu.Framework.Platform;
using CrankItUp.Resources;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Graphics.Containers;

namespace CrankItUp.Game
{
    public partial class InitialSetup : Screen
    {
        CIUButton startButton;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures, Storage storage)
        {
            startButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Start setup",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 0),
                Action = () => PushMenu(audio, storage),
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
                new DrawSizePreservingFillContainer
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
                }
            };
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        void PushMenu(AudioManager audio, Storage storage)
        {
            Stream exampleMapFileStream = storage.CreateFileSafely("maps/Example/easy.json");
            // there is no *store class for text files, so we have to pull it from the ResourceAssembly manually
            Stream exampleMap = CrankItUpResources.ResourceAssembly.GetManifestResourceStream(
                "CrankItUp.Resources.Beatmaps.Example.easy.json"
            );

            Logger.Log(exampleMap.ToString());

            exampleMap.CopyTo(exampleMapFileStream);

            exampleMapFileStream.Dispose();
            exampleMap.Dispose();

            Stream exampleSong = storage.CreateFileSafely("maps/Example/music.mp3");
            audio.GetTrackStore().GetStream("Tera I_O.mp3").CopyTo(exampleSong);
            exampleSong.Dispose();
            this.Push(new TitleScreen());
        }
    }
}
