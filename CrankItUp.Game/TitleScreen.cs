using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;
using System;
using osu.Framework.Input;
using osuTK;



namespace CrankItUp.Game{
    public class TitleScreen : Screen{

    [BackgroundDependencyLoader]
    private void load(){
            BasicButton button =new BasicButton{
                Anchor = Anchor.Centre,
                Text = "Begin Test Levels",
                BackgroundColour = Color4.AntiqueWhite,
                Alpha = 1.0f,
                Height = 30,
                CornerRadius = 3,
                 Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => pushScreenToStack(new LevelScreen())                
            };
            


            

            InternalChildren = new Drawable[] {
            new Box
                {
                    Colour = Color4.Violet,
                    RelativeSizeAxes = Axes.Both,
                },
            button,
            
            new SpriteText{
                Y = 20,
                Text = "Welcome to Crank it Up",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Font = FontUsage.Default.With(size: 40)
            },
            
        };
    }
    public void pushScreenToStack(Screen screen){
        Program.getGame().pushScreenToStack(screen);
        
    }
    }
    
}
