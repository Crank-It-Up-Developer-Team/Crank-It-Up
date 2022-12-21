using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Framework.Input.Events;
using osuTK;
<<<<<<< Updated upstream
using osu.Framework.Audio;
using osu.Framework.Audio.Track;

namespace CrankItUp.Game
{
    public class LevelScreen : Screen
    {
=======
using System;
using System.Collections.Generic;


namespace CrankItUp.Game{
    public class LevelScreen : Screen {
>>>>>>> Stashed changes
        Crank crank;
        NoteManager manager;
        private Track song;

<<<<<<< Updated upstream
        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            crank = new Crank
=======
        bool firstUpdate;

       [BackgroundDependencyLoader]
        private void load()
        {
            firstUpdate = true;
            
            crank = new Crank{
                    
                };
            manager = new NoteManager(this, 50, 100);
            InternalChildren = new Drawable[]
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        public void addNote(BaseNote note)
        {
=======
        protected override void Update()
        {
            base.Update();
        }

        public void addNote(BaseNote note){
>>>>>>> Stashed changes
            crank.ScaleTo(manager.dilation);
            crank.addNote(note);
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
                this.Exit();
            }
            return base.OnKeyDown(e);
        }
    }
}
