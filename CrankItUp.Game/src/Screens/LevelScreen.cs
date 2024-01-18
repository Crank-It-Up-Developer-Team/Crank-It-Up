using CrankItUp.Game.Elements;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace CrankItUp.Game.Screens
{
    public partial class LevelScreen : Screen
    {
        private Crank crank;
        private NoteManager manager;
        private Beatmap beatmap;
        private readonly string map;
        private readonly string difficulty;

        public LevelScreen(string mapName, string difficultyName)
        {
            map = mapName;
            difficulty = difficultyName;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, Storage storage)
        {
            crank = new Crank
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new Vector2(0, 0),
            };

            beatmap = new Beatmap(map, difficulty, audio, storage);
            manager = new NoteManager(this, beatmap);
            InternalChildren = new Drawable[]
            {
                new DrawSizePreservingFillContainer { crank, manager, }
            };
            beatmap.Track.Start();
        }

        public void AddNote(BaseNote note)
        {
            crank.ScaleTo(manager.Dilation);
            AddInternal(note);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            Vector2 dPos = e.MousePosition - Constants.CORNER_TO_CENTER_TRANSFORMATION;
            crank.UpdateRotation(dPos);
            return base.OnMouseMove(e);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                beatmap.Track.Dispose();
                this.Exit();
            }

            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }

        public override bool OnExiting(ScreenExitEvent e)
        {
            crank.Writer.Dispose();
            return base.OnExiting(e);
        }
    }
}
