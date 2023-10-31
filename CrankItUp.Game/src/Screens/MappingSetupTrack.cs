using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using System.Diagnostics;
using System.IO;
using osu.Framework.Platform;

namespace CrankItUp.Game
{
    public partial class MappingSetupTrack : Screen
    {
        CIUButton backButton;
        CIUButton continueButton;
        CIUButton mapsFolderButton;

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
                Position = new Vector2(0, 110),
                Action = () => PushMenu(),
            };
            continueButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Next",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 60),
                Action = () => PushSetupDifficulty(),
            };
            mapsFolderButton = new CIUButton(textures)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "Open maps folder",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Position = new Vector2(0, 10),
                Action = () => storage.GetStorageForDirectory("maps").PresentExternally(),
            };
            InternalChildren = new Drawable[]
            {
                new SpriteText
                {
                    Y = 20,
                    Text = "Mapping Setup",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
                mapsFolderButton,
                continueButton,
                backButton,
            };

            string[] info =
            {
                "Welcome to Mapping Setup!",
                "To get started:",
                "1: open the 'maps' folder using the button below",
                "2: create a folder called 'WIP'",
                "3: Put the mp3 of the song wish to map inside this folder",
                "4: rename the mp3 file to `music` (keep the file extension intact)",
                "5: return here and select the map you wish to add a difficulty for!",
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

        void PushSetupDifficulty()
        {
            this.Push(new MappingSetupDifficulty());
        }
    }
}
