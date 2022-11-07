using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Input.Events;
using osuTK;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;

namespace CrankItUp.Game
{
    public class LevelScreen : Screen
    {
        Crank crank;
        NoteManager manager;
        private Track song;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            crank = new Crank
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new osuTK.Vector2(0, 0),
            };

            song = audio.Tracks.Get("Flamewall.mp3");
            manager = new NoteManager(this, 50, 100);
            InternalChildren = new Drawable[] { crank, manager, };
            song.Start();
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
                song.Dispose();
                this.Push(new TitleScreen());
            }
            return base.OnKeyDown(e);
        }
    }
}
