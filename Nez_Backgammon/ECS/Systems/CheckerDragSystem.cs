using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Nez_Backgammon.ECS.Systems
{
    public class CheckerDragSystem : EntityProcessingSystem
    {
        MouseState CurrentMouse;
        public CheckerDragSystem(Matcher matcher) : base(matcher)
        {
        }
        public override void Process(Entity entity)
        {
            //
            // Current location of the mouse used for the hand icon
            //
            CurrentMouse = Mouse.GetState();
            entity.Transform.Position = Scene.Camera.ScreenToWorldPoint(new Vector2(CurrentMouse.Position.X, CurrentMouse.Position.Y));

        }
    }
}
