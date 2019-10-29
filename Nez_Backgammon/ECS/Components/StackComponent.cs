using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace TestMiniMax.ECS.Components
{    /*
     * initial stacks for Backgammon
     * negative numbers are computer
     * positive numbers are human player
     * 
     * stack 24 = grave yard for human player checkers
     * stack 25 = grave yard for computer player checkers
     * 
     *  12 13 14 15 16 17 18 19 20 21 22 23
     * |-----------------|------------------|
     * |05          -3   |-5             02 |
     * |                 |                  | 24
     * |                 |                  | 25
     * |-5          03   |05             -2 |
     * |-----------------|------------------|
     *  11 10  9  8  7  6  5  4  3  2  1  0
     */
    public class StackComponent : Component
    {
        public int StackID;                 //0 - 99 
        public string CName = "Stack of Checkers";
        public Vector2 Location;

        public List<Entity> CheckersInStack;    //number of checkers in this stack
        public StackComponent()
        {
            CheckersInStack = new List<Entity>();
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
        }
        public Entity GetTopChecker()
        {
            if (CheckersInStack.Count <= 0)
                return null;

            return CheckersInStack[0];
        }
    }
}
