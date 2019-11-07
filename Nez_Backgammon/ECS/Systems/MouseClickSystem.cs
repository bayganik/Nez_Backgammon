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
        bool singleBlack;
        bool stackIsEmpty;
        bool stackIsWhite;
        //
        // Mouse movements are processed here using 'MouseComponent'
        //
        MouseState CurrentMouse;
        MainScene MainGameScene;
        Entity gameStack;                   //game stack we clicked on
        Vector2 MousePos;
        public MouseClickSystem(Matcher matcher) : base(matcher)
        {

        }
        public override void Process(Entity entity)
        {
            //
            // Mouse events are processed for White (human) player
            // hint: After white player clicks on "Dice Roll" then its his turn
            //
            MainGameScene = entity.Scene as MainScene;              //hand entity belongs to MainScene

            if (MainGameScene.GameEnded)
            {
                MainGameScene.EndOfGame();
                return;
            }

            if (!MainGameScene.WhiteCanMove)                        //Can white move?
                return;

            //
            // Mouse working area (find if it clicks on anything)
            //
            var _mouseCollider = entity.GetComponent<BoxCollider>();
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

                gameStack = collisionResult.Collider.Entity;
                if (MainGameScene.TestGraveYardForCheckers(24))          //if true, then grave yard checkers go first
                {
                    gameStack = MainGameScene.GameStacks[24];                //automatically, use checkers from White grave yard stack
                }
                //
                // Collided entity is a stack of checkers
                // Test it to be empty or black checkers (do nothing)
                //
                StackComponent sc = gameStack.GetComponent<StackComponent>();
                if (sc == null)
                    return;                         //no stack of checkers

                if (sc.CheckersInStack.Count == 0)
                    return;                         //empty stack of checkers
                //
                // black checers tag are < 0 and white are > 0
                //
                if (sc.CheckersInStack[0].Tag < 0)
                    return;                         //test first checker, if black checker leave

                //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
                // Human player has grabbed a White checker 
                //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn

                MainGameScene.Dragging = true;
                Entity chkerDragEntity = sc.CheckersInStack[0];         //checker being dragged is saved
                sc.CheckersInStack.RemoveAt(0);                         //checker being dragges is Removed from original stack

                DragComponent dc = new DragComponent();         
                dc.FromStack = gameStack;                               //remember original stack
                chkerDragEntity.AddComponent<DragComponent>(dc);        //add component so CheckerDragSystem can see it

                MainGameScene.CheckerBeingDragged = chkerDragEntity;    //make sure we know, the checker being dragged

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
                //
                // We have dropped on top of a Stack
                //
                gameStack = collisionResult.Collider.Entity;
                 
                if (MainGameScene.Dragging)
                {
                    //
                    // You can't drop White checkers on graveyard stacks
                    //if (stack.Tag > 23)
                    //{
                    //    MainGameScene.DropChecker2PreviousPosition();
                    //    MainGameScene.Dragging = false;
                    //    return;
                    //}

                    //
                    // Drop location must either be Empty or have one or more White checkers, or be collector
                    //
                    StackComponent sc = gameStack.GetComponent<StackComponent>();
                    singleBlack = ((sc.CheckersInStack.Count() == 1) && (sc.CheckersInStack[0].Tag < 0));   //single black
                    stackIsEmpty = (sc.CheckersInStack.Count == 0);

                    if (stackIsEmpty)
                        stackIsWhite = true;
                    else
                        stackIsWhite = (sc.CheckersInStack[0].Tag > 0);
                    

                    if ( stackIsEmpty || stackIsWhite || singleBlack)
                    {
                        MainGameScene.DropChecker2NewPosition(gameStack);
                    }
                    else
                        MainGameScene.DropChecker2PreviousPosition();

                    MainGameScene.Dragging = false;
                }
            }
        }
    }
}
