using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Sprites;
using Nez_Backgammon.ECS.Components;
using Nez_Backgammon.Scenes;
using Nez_Backgammon.Models;

namespace Nez_Backgammon.ECS.Systems
{
    public class CheckTurnEOGSystem : ProcessingSystem
    {
        //
        // This system checks for Black player turn or End of Game
        //
        MainScene MainGameScene;
        public CheckTurnEOGSystem()
        {

        }
        public override void Process()
        {
            MainGameScene = Scene as MainScene;              //mouse entity belongs to MainScene

            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // E N D  O F  G A M E 
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn

            MainGameScene.GameEnded = false;
            MainGameScene.PlayerWon = -1;

            if (MainGameScene.GameBoard.WhiteWinsGame())
            {
                MainGameScene.PlayerWon = 0;            //white wins
                MainGameScene.GameEnded = true;
                return;
            }
            else if (MainGameScene.GameBoard.BlackWinsGame())
            {
                MainGameScene.PlayerWon = 1;            //black wins
                MainGameScene.GameEnded = true;
                return;
            }

            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // WHOSE TURN  IS IT
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn

            //if (MainGameScene.WhoseTurn == 0)
            //    return;
            //
            // Black player turn
            //
            //MainGameScene.Roll_The_Dice();
            //MainGameScene.DispBlackDiceValues();

            //MainGameScene.DiceRoll[0] = 1;
            //MainGameScene.DiceRoll[1] = 1;
            //GameState init = new GameState(MainGameScene.GameBoard.DispBoard, MainGameScene.DiceRoll);
            //GameState res = MainGameScene.Eminmax.chooseMove(init);
            //if (res == null) // no moves to play
            //{
            //    //MainGameScene.Dice.useAllDices();
            //    return;
            //}
            //Console.WriteLine(res);
            //Console.Read();

            //MainGameScene.UpdateGameBoardFromCPU(res.board);
            //MainGameScene.UpdateStacksFromGameBoard();

            //MainGameScene.WhoseTurn = 0;


        }
    }
}

