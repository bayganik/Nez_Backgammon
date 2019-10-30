using System;
using Nez;
using Nez.UI;

namespace Nez_Backgammon.Scenes
{
    public abstract class BaseScene : Scene
    {
        public BaseScene() { }
        public void SetupScene()
        {
            AddRenderer(new DefaultRenderer());

        }
    }
}
