using System.Diagnostics;
using System.IO;
using CrankItUp.Game.Elements;
using CrankItUp.Resources;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace CrankItUp.Game.Screens
{
    public partial class InitialSetup : Screen
    {
        private CiuButton startButton;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures, Storage storage)
        {
            startButton = new CiuButton(textures)
            {
                Anchor = Anchor.Centre,
                Text = "Start setup",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 0),
                Action = () => pushMenu(audio, storage),
            };

            string[] intro =
            {
                "Hi! Welcome to Crank It Up!",
                "We need to do some initial setup before we can get started",
                "So just hit that big shiny button below!",
                "If the game freezes after clicking the button, don't panic! It might take a while though"
            };
            var introTextList = new Drawable[intro.Length];

            for (int i = 0; i < intro.Length; i++)
            {
                var tempText = new SpriteText
                {
                    Y = 100 + (i * 30),
                    Text = intro[i],
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 20)
                };
                introTextList[i] = tempText;
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

        private void pushMenu(AudioManager audio, Storage storage)
        {
            Stream exampleMapFileStream = storage.CreateFileSafely("maps/Example/easy.json");
            // there is no *store class for text files, so we have to pull it from the ResourceAssembly manually
            Stream exampleMap = CrankItUpResources.ResourceAssembly.GetManifestResourceStream(
                "CrankItUp.Resources.Beatmaps.Example.easy.json"
            );
            Debug.Assert(exampleMap != null, nameof(exampleMap) + " != null");
            exampleMap.CopyTo(exampleMapFileStream);

            exampleMapFileStream.Dispose();
            exampleMap.Dispose();

            Stream exampleMetadataFileStream = storage.CreateFileSafely(
                "maps/Example/metadata.json"
            );
            // there is no *store class for text files, so we have to pull it from the ResourceAssembly manually
            Stream exampleMetadata = CrankItUpResources.ResourceAssembly.GetManifestResourceStream(
                "CrankItUp.Resources.Beatmaps.Example.metadata.json"
            );
            Debug.Assert(exampleMetadata != null, nameof(exampleMetadata) + " != null");
            exampleMetadata.CopyTo(exampleMetadataFileStream);

            exampleMetadataFileStream.Dispose();
            exampleMetadata.Dispose();

            Stream exampleSong = storage.CreateFileSafely("maps/Example/music.mp3");
            audio.GetTrackStore().GetStream("Tera I_O.mp3").CopyTo(exampleSong);
            exampleSong.Dispose();
            this.Push(new TitleScreen());
        }
    }
}
