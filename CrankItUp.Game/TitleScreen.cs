using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osu.Framework.Graphics.UserInterface;



namespace CrankItUp.Game{
    public class TitleScreen : Screen{
    
    [BackgroundDependencyLoader]
    private void load(){
            InternalChildren = new Drawable[] {
            new BasicButton{
                Anchor = Anchor.TopCentre,
                Text = "Begin Test Level",
                Action = () => pushScreenToStack(new LevelScreen())
            }, 
            new Box{
                Colour = Color4.Violet,
                RelativeSizeAxes = Axes.Both,
                },
            new SpriteText{
                Y = 20,
                Text = "Welcome to Crank it Up",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Font = FontUsage.Default.With(size: 40)
            },
            new SpinningBox{
                Anchor = Anchor.Centre
            }
        };
    }
    public void pushScreenToStack(Screen screen){
        Program.getGame().pushScreenToStack(screen);
        
    }
    }
    
}
