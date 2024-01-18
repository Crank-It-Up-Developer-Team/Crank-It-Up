using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Localisation;

namespace CrankItUp.Game.Elements
{
    public partial class CiuButton : Button
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
        private readonly SpriteText text;

        public CiuButton(TextureStore textureStore)
        {
            if (Texture == null)
            {
                Texture = textureStore.Get("Button");
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
    }
}
