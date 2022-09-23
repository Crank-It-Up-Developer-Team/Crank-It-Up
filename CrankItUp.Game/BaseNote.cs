using osuTK;

namespace CrankItUp.Game{
    public class BaseNote{
        
        /**
        so my thoughts on how mapping should be implemented
        we have a ring where notes can be placed of all the potential spots for the platter to 
        be (may be a bit complicated because mapping a circle to a non circlular object, we can probably 
        make a ring of some thickness tho to account for that if my mental math is right)
        this also makes it so all we have to do to instantiate any given note is pass in its radial direction and then use a
        vector of some static magnitude to be able to project it onto the spawning circle
        **/

        private double radians;
        private static Vector2 PROJECTION_VECTOR2;
        public BaseNote(double radians){
            
        }
    }
}