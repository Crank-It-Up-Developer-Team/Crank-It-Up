using System;
using System.IO;
using CrankItUp.Game.Elements;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace CrankItUp.Game.Screens
{
    public partial class DifficultySelect : Screen
    {
        private Container trackContainer;
        private CiuButton backButton;
        private Track track;
        private readonly string map;
        private readonly TrackMetadata trackmeta;

        public DifficultySelect(string mapname, TrackMetadata trackmeta)
        {
            map = mapname;
            this.trackmeta = trackmeta;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures, Storage storage)
        {
            backButton = new CiuButton(textures)
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Position = new Vector2(0, 0),
                Text = "Back",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = pushMenu,
            };
            trackContainer = new Container
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.BottomRight,
                Position = new Vector2(0, 100)
            };

            ITrackStore trackStore = audio.GetTrackStore(
                new StorageBackedResourceStore(
                    storage.GetStorageForDirectory(Path.Combine("maps", map))
                )
            );
            track = trackStore.Get(trackmeta.TrackFilename);

            var difficulties = storage.GetFiles(Path.Combine("maps", map));
            Vector2 position = new Vector2(0, 0);
            Storage mapStorage = storage.GetStorageForDirectory("maps").GetStorageForDirectory(map);
            int invalidDifficultyCount = 0;

            foreach (string difficulty in difficulties)
            {
                if (difficulty.EndsWith(".json"))
                {
                    string difficultyname = difficulty[(difficulty.LastIndexOf("/", StringComparison.Ordinal) + 1)..];
                    JObject beatmap;

                    using (
                        var sr = new StreamReader(
                            mapStorage.GetStream(difficultyname, mode: FileMode.Open)
                        )
                    )
                    {
                        try
                        {
                            beatmap = JObject.Parse(sr.ReadToEnd());
                        }
                        catch (Exception e)
                        {
                            Logger.Error(
                                e,
                                "Error while loading difficuly JSON! Skipping difficulty"
                            );
                            invalidDifficultyCount += 1;
                            continue;
                        }
                    }

                    int dataVersion;

                    try
                    {
                        JToken meta = beatmap.GetValue("meta");
                        dataVersion = meta.GetValue<int>("dataVersion");
                    }
                    catch (Exception e)
                    {
                        Logger.Error(
                            e,
                            "Error while checking difficulty dataVersion! Skipping difficulty"
                        );
                        invalidDifficultyCount += 1;
                        continue;
                    }

                    if (dataVersion != Constants.MAP_DATAVERSION)
                    {
                        Logger.Log(
                            "Difficulty "
                            + difficultyname
                            + " has an invalid dataVersion for this version of the game!",
                            LoggingTarget.Runtime,
                            LogLevel.Important
                        );
                        invalidDifficultyCount += 1;
                        continue;
                    }

                    difficultyname = difficulty[
                        (difficulty.LastIndexOf("/", StringComparison.Ordinal) + 1)..^5
                    ];
                    trackContainer.Add(
                        new CiuButton(textures)
                        {
                            Text = difficultyname,
                            Size = new Vector2(200, 40),
                            Margin = new MarginPadding(10),
                            Position = position,
                            Action = () => PushLevel(difficultyname),
                        }
                    );
                }

                // Create a grid of buttons, same as TrackSelect.cs
                position.Y += 50;

                if (position.Y == 600)
                {
                    position.Y = 0;
                    position.X += 250;
                }
            }

            SpriteText invalidDifficultyText;
            invalidDifficultyCount -= 1; // the metadata file will always be an invalid difficulty

            if (invalidDifficultyCount > 0)
            {
                invalidDifficultyText = new SpriteText
                {
                    X = 10,
                    Y = 10,
                    Text =
                        "Warning: Map contains " + invalidDifficultyCount + " invalid difficulties!"
                };
            }
            else
            {
                // make an empty spritetext, as there is nothing to say
                invalidDifficultyText = new SpriteText { };
            }

            InternalChildren = new Drawable[]
            {
                new DrawSizePreservingFillContainer
                {
                    new SpriteText
                    {
                        Y = 20,
                        Text = "Select a difficulty",
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Font = FontUsage.Default.With(size: 40)
                    },
                    backButton,
                    trackContainer,
                    invalidDifficultyText,
                }
            };
        }

        protected override void LoadComplete()
        {
            track.Start();
            track.Seek(trackmeta.TrackPreviewStart);
        }

        private void pushMenu()
        {
            this.Exit();
        }

        public void PushLevel(string difficultyname)
        {
            this.Push(new LevelScreen(map, difficultyname));
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                pushMenu();
            }

            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            track.Start();
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        public override void OnSuspending(ScreenTransitionEvent e)
        {
            track.Stop();
        }

        public override bool OnExiting(ScreenExitEvent e)
        {
            track.Dispose();
            return base.OnExiting(e);
        }
    }
}
