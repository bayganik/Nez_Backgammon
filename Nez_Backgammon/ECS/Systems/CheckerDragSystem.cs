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
        //
        // Any checker that has 'DragComponent' will be processed here
        //
        MouseState CurrentMouse;
        public CheckerDragSystem(Matcher matcher) : base(matcher)
        {
        }
        public override void Process(Entity entity)
        {
            //
            // This Draws the Checker (entity) that is being dragged by mouse
            //
            CurrentMouse = Mouse.GetState();
            entity.Transform.Position = Scene.Camera.ScreenToWorldPoint(new Vector2(CurrentMouse.Position.X, CurrentMouse.Position.Y));

        }
    }
}
