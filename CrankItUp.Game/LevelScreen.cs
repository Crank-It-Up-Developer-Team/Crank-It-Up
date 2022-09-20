using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;


namespace CrankItUp.Game{
    public class LevelScreen : Screen {
       [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Crank{
                    
                }
            };
        }

    }
}