using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;
using Nez.Textures;

namespace Nez_Backgammon.ECS.Components
{
    public class CheckerComponent : Component
    {
        public StackComponent HoldingStack;                 //Stack component holding this card
        public string CName = "Checker";
        public bool IsWhite = true;                         //default color of checker
        public Sprite CheckerFace;                          //image of checker
        //
        // entity moving on its own
        //
        public bool IsMoving;
        public CheckerComponent()
        { }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
        }
    }
}
