using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Localisation;
using osu.Framework.Graphics.UserInterface;

namespace CrankItUp.Game
{
    public partial class CIUButton : Button
    {
        public LocalisableString Text // Most of this is from BasicButton
        {
            get => text?.Text ?? default;
            set
            {
                if (text != null)
                    text.Text = value;
            }
        }

        public Texture Texture;
        public Colour4 TextColour = Colour4.White;
        private SpriteText text;

        public CIUButton(TextureStore texturestore)
        {
            if (Texture == null)
            {
                Texture = texturestore.Get("Button");
            }
            AddRange(
                new Drawable[]
                {
                    new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Texture = Texture,
                    },
                    text = new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = TextColour,
                        Text = Text,
                        Depth = -1
                    },
                }
            );
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
        }
    }
}
