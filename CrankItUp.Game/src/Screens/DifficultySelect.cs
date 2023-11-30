using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Graphics.Textures;
using osuTK;
using System.IO;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using Newtonsoft.Json.Linq;
using System;
using osu.Framework.Logging;
using NuGet.ProjectModel;
using osu.Framework.Audio;
using osu.Framework.IO.Stores;
using osu.Framework.Audio.Track;

namespace CrankItUp.Game
{
    public partial class DifficultySelect : Screen
    {
        Container trackContainer;
        CIUButton backButton;
        Track track;
        string map;
        TrackMetadata trackmeta;

        public DifficultySelect(string mapname, TrackMetadata trackmeta)
        {
            map = mapname;
            this.trackmeta = trackmeta;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, TextureStore textures, Storage storage)
        {
            backButton = new CIUButton(textures)
            {
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Position = new Vector2(0, 0),
                Text = "Back",
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => pushMenu(),
            };
            trackContainer = new Container()
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
            track = trackStore.Get(trackmeta.trackFilename);

            var difficulties = storage.GetFiles(Path.Combine("maps", map));
            Vector2 position = new Vector2(0, 0);
            Storage mapStorage = storage.GetStorageForDirectory("maps").GetStorageForDirectory(map);
            int invalidDifficultyCount = 0;

            foreach (string difficulty in difficulties)
            {
                JObject beatmap;
                if (difficulty.EndsWith(".json"))
                {
                    string difficultyname = difficulty[(difficulty.LastIndexOf("/") + 1)..];
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
                        (difficulty.LastIndexOf("/") + 1)..(difficulty.Length - 5)
                    ];
                    trackContainer.Add(
                        new CIUButton(textures)
                        {
                            Text = difficultyname,
                            Size = new Vector2(200, 40),
                            Margin = new MarginPadding(10),
                            Position = position,
                            Action = () => pushLevel(difficultyname),
                        }
                    );
                }
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
            track.Seek(trackmeta.trackPreviewStart);
        }

        private void pushMenu()
        {
            this.Exit();
        }

        public void pushLevel(string difficultyname)
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
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        public override bool OnExiting(ScreenExitEvent e)
        {
            track.Stop();
            return base.OnExiting(e);
        }
    }
}
