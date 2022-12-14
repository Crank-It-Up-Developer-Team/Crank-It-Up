using osu.Framework.Testing;
using osu.Framework.Graphics;


namespace CrankItUp.Game.Tests.Visual
{
    public class CrankItUpTestScene : TestScene
    {

       
        
        protected override ITestSceneTestRunner CreateRunner() => new CrankItUpTestSceneTestRunner();

        private class CrankItUpTestSceneTestRunner : CrankItUpGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
