using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez_Backgammon.Scenes;
using Nez_Backgammon.ECS.Components;

namespace Nez_Backgammon.ECS.Systems
{
    /*
     * ONLY white checkers are dragged by the mouse
     * Update the position of the white checker (same as mouse position)
     * Find all legal moves (according to Dice roll)
     */
    public class CheckerDragSystem : EntityProcessingSystem
    {
        //
        // Any checker that has 'DragComponent' will be processed here
        //
        MouseState CurrentMouse;
        MainScene MainGameScene;
        Entity gameStack;                   //game stack white checker came frome

        int boardLoc = 0;
        BGBoard gameBoad;
        //int[] legalMoves;
        public CheckerDragSystem(Matcher matcher) : base(matcher)
        {
        }
        public override void Process(Entity entity)
        {
            //
            // This updates the Checker position (entity) that is being dragged by mouse
            //
            CurrentMouse = Mouse.GetState();
            entity.Transform.Position = Scene.Camera.ScreenToWorldPoint(new Vector2(CurrentMouse.Position.X, CurrentMouse.Position.Y));

            MainGameScene = entity.Scene as MainScene;              //mouse entity belongs to MainScene
            gameBoad = MainGameScene.GameBoard;
            //
            // Get the from location of the white checker
            // 
            DragComponent dc = entity.GetComponent<DragComponent>();
            gameStack = dc.FromStack;

            boardLoc = gameStack.Tag;                               //white checker came from this location on the board
            MainGameScene.LegalMoves = gameBoad.GetWhiteLegalMoves(MainGameScene.DiceRoll, boardLoc);
            //StackComponent sc = gameStack.GetComponent<StackComponent>();
        }
    }
}
