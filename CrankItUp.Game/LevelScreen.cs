using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using System.Collections.Generic;

namespace CrankItUp.Game
{
    public class LevelScreen : Screen
    {
        Crank crank;
        NoteManager manager;

        [BackgroundDependencyLoader]
        private void load()
        {
            crank = new Crank
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Position = new osuTK.Vector2(0, 0),
            };
            manager = new NoteManager(this, 50, 100);
            InternalChildren = new Drawable[] { crank, manager };
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
    }
}
