using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using Newtonsoft.Json.Linq;
using System.IO;
using NuGet.Protocol;

namespace CrankItUp.Game
{
    public partial class MappingSetupTrack : Screen
    {
        CIUButton backButton;
        CIUButton continueButton;
        CIUButton mapsFolderButton;
        BasicTextBox trackFileNameBox;
        private BasicTextBox trackNameBox;
        private BasicTextBox mapperNameBox;
        private BasicTextBox artistNameBox;
        private BasicTextBox trackPreviewStartBox;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage storage)
        {
            backButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Back to menu",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 170),
                Action = () => PushMenu(),
            };
            continueButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Next",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 110),
                Action = () => PushSetupDifficulty(storage),
            };
            mapsFolderButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Open maps folder",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 60),
                Action = () => storage.GetStorageForDirectory("maps").PresentExternally(),
            };

            trackFileNameBox = new BasicTextBox()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(-420, 10),
                Size = new Vector2(200, 40),
                PlaceholderText = "Song filename"
            };
            trackNameBox = new BasicTextBox()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(-210, 10),
                Size = new Vector2(200, 40),
                PlaceholderText = "Track name"
            };
            mapperNameBox = new BasicTextBox()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 10),
                Size = new Vector2(200, 40),
                PlaceholderText = "Mapper name (you)"
            };
            artistNameBox = new BasicTextBox()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(210, 10),
                Size = new Vector2(200, 40),
                PlaceholderText = "Artist Name"
            };
            trackPreviewStartBox = new BasicTextBox()
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(445, 10),
                Size = new Vector2(250, 40),
                PlaceholderText = "Preview start time (ms)"
            };

            InternalChildren = new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    new SpriteText
                    {
                        Y = 20,
                        Text = "Mapping Setup",
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = FontUsage.Default.With(size: 40)
                    },
                    trackFileNameBox,
                    trackNameBox,
                    mapperNameBox,
                    artistNameBox,
                    trackPreviewStartBox,
                    mapsFolderButton,
                    continueButton,
                    backButton,
                }
            };

            string[] info =
            {
                "Welcome to Mapping Setup!",
                "To get started:",
                "1: open the 'maps' folder using the button below",
                "2: create a folder called 'WIP'",
                "3: Put the mp3 of the song wish to map inside this folder",
                "4: Put the exact name of the file in the box below, case sensitive, include the file extension.",
                "5: Return here and select the map you wish to add a difficulty for!",
                "6: When you are finished mapping in the editor, rename the folder to the name of the map",
                "7: You can then edit the positions of these objects by editing the json file in a text editor"
            };
            for (int i = 0; i < info.Length; i++)
            {
                AddInternal(
                    new SpriteText
                    {
                        Y = 100 + (i * 30),
                        Text = info[i],
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = FontUsage.Default.With(size: 20)
                    }
                );
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                PushMenu();
            }
            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        void PushMenu()
        {
            this.Exit();
        }

        void PushSetupDifficulty(Storage storage)
        {
            JObject meta = new JObject
            {
                { "dataVersion", Constants.METADATA_DATAVERSION },
                { "name", trackNameBox.Text },
                { "mapperName", mapperNameBox.Text },
                { "trackArtistName", artistNameBox.Text },
                { "trackFilename", trackFileNameBox.Text },
                { "trackPreviewStart", trackPreviewStartBox.Text }
            };
            StreamWriter metafile = new StreamWriter(
                storage.CreateFileSafely("maps/WIP/metadata.json")
            );
            metafile.Write(meta.ToJson(Newtonsoft.Json.Formatting.Indented));
            metafile.Dispose();
            this.Push(new MappingSetupDifficulty(trackFileNameBox.Text));
        }
    }
}
