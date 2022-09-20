public class TitleScreen: Screen{
    [BackgroundDependencyLoader]
    private void load(){
            InternalChildren = new Drawable[] {
            new Box{
                Colour = Color4.Violet,
                RelativeSizeAxes = Axes.Both,
                },
            new SpriteText{
                Y = 20,
                Text = "Main Screen",
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Font = FontUsage.Default.With(size: 40)
            },
            new SpinningBox{
                Anchor = Anchor.Centre,
            }
        };
    }

}