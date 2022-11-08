using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;
using osuTK;
using osu.Framework.Graphics.UserInterface;
namespace CrankItUp.Game
{
    public class CIUButton : Button
    {
        TextureStore texturestore;
        
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

        private Sprite background;
        private SpriteText text;

        public CIUButton(TextureStore texturestore){
            if (Texture == null)
            {
                Texture = texturestore.Get("Button");
            }
            AddRange(new Drawable[]
            {
                background = new Sprite
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