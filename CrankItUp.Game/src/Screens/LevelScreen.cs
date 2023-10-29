using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Input.Events;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Platform;

namespace CrankItUp.Game
{
    public class LevelScreen : Screen
    {
        Crank crank;
        NoteManager manager;
        private Beatmap beatmap;
        string map;
        string difficulty;

        public LevelScreen(string mapname, string difficultyname)
        {
            map = mapname;
            difficulty = difficultyname;
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
            manager = new NoteManager(this, 50, 100, beatmap);
            InternalChildren = new Drawable[] { crank, manager, };
            beatmap.track.Start();
        }

        public void addNote(BaseNote note)
        {
            crank.ScaleTo(manager.dilation);
            AddInternal(note);
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            Vector2 dPos = e.MousePosition - Constants.CORNER_TO_CENTER_TRANSFORMATION;
            crank.updateRotation(dPos);
            return base.OnMouseMove(e);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == osuTK.Input.Key.Escape)
            {
                beatmap.track.Dispose();
                this.Exit();
            }
            return base.OnKeyDown(e);
        }

        public override void OnEntering(ScreenTransitionEvent e)
        {
            this.FadeInFromZero(500, Easing.OutQuint);
        }
    }
}
