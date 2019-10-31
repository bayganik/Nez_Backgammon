using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.UI;
using Nez_Backgammon.Scenes;
using Nez_Backgammon.ECS.Components;

namespace Nez_Backgammon.ECS.Systems
{
    public class MouseClickSystem : EntityProcessingSystem
    {
        MouseState CurrentMouse;
        MouseState PrevMouse;
        bool Dragging = false;
        MainScene MainGameScene;

        Vector2 MousePos;
        public MouseClickSystem(Matcher matcher) : base(matcher)
        {

        }
        public override void Process(Entity entity)
        {
            //
            // ONLY MOUSE entity comes here, Process if White Checkers turn
            // hint: If white player clicks on "Dice Roll" then its his turn
            //
            MainGameScene = entity.Scene as MainScene;              //hand entity belongs to MainScene
            if (!MainGameScene.WhiteTurn)
                return;

            //
            // Mouse working area (find if it clicks on anything)
            //
            var _mouseCollider = entity.GetComponent<BoxCollider>();
            PrevMouse = CurrentMouse;
            CurrentMouse = Mouse.GetState();
            //
            // Current location of the mouse 
            //
            entity.Transform.Position = Scene.Camera.ScreenToWorldPoint(new Vector2(CurrentMouse.Position.X, CurrentMouse.Position.Y));
            MousePos = new Vector2(CurrentMouse.Position.X, CurrentMouse.Position.Y);
            //
            // Check the mouse input action
            //
            if (Input.LeftMouseButtonPressed)
            {
                MainGameScene.Dragging = false;
                //
                // If mouse click is not colliding with anything (do nothing)
                //
                if (!_mouseCollider.CollidesWithAny(out CollisionResult collisionResult))
                    return;

                Entity collidedEntity = collisionResult.Collider.Entity;
                //
                // Collided entity is a stack of checkers
                // Test it to be empty or black checkers (do nothing)
                //
                StackComponent sc = collidedEntity.GetComponent<StackComponent>();
                if (sc == null)
                    return;                         //no stack of checkers

                if (sc.CheckersInStack.Count == 0)
                    return;                         //empty stack of checkers

                if (sc.CheckersInStack[0].Tag < 0)  //test first checker 
                    return;                         //black checkers
                //
                // This is the human player, White checker is grabbed 
                // Find out where it can go, is it allowed to move
                //
                MainGameScene.Dragging = true;
                Entity dragEnt = sc.CheckersInStack[0];
                sc.CheckersInStack.RemoveAt(0);                 //Remove from original stack

                DragComponent dc = new DragComponent();         
                dc.FromStack = collidedEntity;                  //remember original stack
                //dc.PrevPosition = dragEnt.Transform.Position;
                dragEnt.AddComponent<DragComponent>(dc);        //add component so drag system can see it

                MainGameScene.CheckerBeingDragged = dragEnt;    //make sure we know, the checker being dragged
                
            }
            if (Input.LeftMouseButtonReleased)
            {
                //
                // Find the collider entity that checker is being dropped
                //
                if (!_mouseCollider.CollidesWithAny(out CollisionResult collisionResult))
                {
                    if (MainGameScene.Dragging)
                        MainGameScene.DropChecker2PreviousPosition();
                    return;
                }

                Entity collidedEntity = collisionResult.Collider.Entity;

                if (MainGameScene.Dragging)
                {
                    //
                    // Drop location must either be Empty or have one or more White checkers
                    //
                    StackComponent sc = collidedEntity.GetComponent<StackComponent>();
                    if ((sc.CheckersInStack.Count == 0) || (sc.CheckersInStack[0].Tag > 0))
                    {
                        MainGameScene.DropChecker2NewPosition(collidedEntity);
                    }
                    else
                        MainGameScene.DropChecker2PreviousPosition();

                }
            }
        }
    }
}
