using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osuTK;

namespace CrankItUp.Game
{
    public class Button : ClickableContainer
    {
        public LocalisableString Text;
        public Button(){}
        
        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChild = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
               {
                    new SpriteText
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = Text,
                        Font = new FontUsage(size: 30),
                        Depth = 0
                    },

                    new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = Size,
                        Texture = textures.Get("Button"),
                        Depth = 1
                    },
               }
            };
        }
        protected override void LoadComplete()
        {
            base.LoadComplete();
        }
    }
}