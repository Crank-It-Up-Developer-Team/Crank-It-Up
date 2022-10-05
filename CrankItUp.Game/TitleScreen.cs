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

        BasicButton button;

    [BackgroundDependencyLoader]
    private void load(){
            button =new BasicButton{
                Anchor = Anchor.Centre,
                Text = "Begin Test Levels",
                BackgroundColour = Color4.AntiqueWhite,
                Size = new Vector2(200, 40),
                Margin = new MarginPadding(10),
                Action = () => pushLevel(),          
            };
            


            

            InternalChildren = new Drawable[] {
            
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
    public void test(){
        button.Text = "button has been pushed";
    }

    public void pushLevel(){
        this.Push(new LevelScreen());
    }
    
    
    }
    
}
