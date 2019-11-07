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


namespace Nez_Backgammon.ECS.Systems
{
    public class EndOfGameSystem : ProcessingSystem
    {
        //
        // This system runs every frame to determine if end of game has reached
        //
        MainScene MainGameScene;
        public EndOfGameSystem()
        {

        }
        public override void Process()
        {
            MainGameScene = Scene as MainScene;              //mouse entity belongs to MainScene
            MainGameScene.GameEnded = false;
            MainGameScene.PlayerWon = 0;

            if (MainGameScene.GameBoard.WhiteWinsGame())
            {
                MainGameScene.PlayerWon = 1;
                MainGameScene.GameEnded = true;
            }
            else if (MainGameScene.GameBoard.BlackWinsGame())
            {
                MainGameScene.PlayerWon = 2;
                MainGameScene.GameEnded = true;
            }


        }
    }
}

