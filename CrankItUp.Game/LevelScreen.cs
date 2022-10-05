using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using System;

namespace CrankItUp.Game{
    public class LevelScreen : Screen {
        Crank crank;
       [BackgroundDependencyLoader]
        private void load()
        {
            crank = new Crank{
                    Sensitivity = 0.6f, // 6 pixels per degree, for 600ppr
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Position = new osuTK.Vector2(0,0),
                };
            InternalChildren = new Drawable[]
            {
                crank
            };
        }
        Vector2 CORNER_TO_CENTER_TRANSFORMATION = new Vector2(740, 375);
        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            Vector2 dPos = e.MousePosition - CORNER_TO_CENTER_TRANSFORMATION;
            crank.updateRotation(dPos);
            return base.OnMouseMove(e);
        }

    }
}