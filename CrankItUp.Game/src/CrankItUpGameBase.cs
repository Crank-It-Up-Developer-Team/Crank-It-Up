using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osuTK;
using CrankItUp.Resources;
using System.IO;

namespace CrankItUp.Game
{
    public partial class CrankItUpGameBase : osu.Framework.Game
    {
        // Anything in this class is shared between the test browser and the game implementation.
        // It allows for caching global dependencies that should be accessible to tests, or changing
        // the screen scaling for all components including the test browser and framework overlays.

        protected override Container<Drawable> Content { get; }

        protected CrankItUpGameBase()
        {
            // Ensure game and tests scale with window size and screen DPI.
            base.Content.Add(
                Content = new DrawSizePreservingFillContainer
                {
                    // You may want to change TargetDrawSize to your "default" resolution, which will decide how things scale and position when using absolute coordinates.
                    TargetDrawSize = new Vector2(1366, 768)
                }
            );
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Directory.CreateDirectory(Constants.APPDATA_DIR);
            Directory.SetCurrentDirectory(Constants.APPDATA_DIR);
            Resources.AddStore(new DllResourceStore(typeof(CrankItUpResources).Assembly));
            Host.Window.Title = "Crank It Up!";
        }
    }
}
