using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Nez;

namespace Nez_Backgammon.ECS.Components
{
    public class EndOfGameComponent : Component
    {
        public Entity FromStack;                            //which game stack it came from
        public Vector2 PrevPosition;
        public EndOfGameComponent()
        {

        }
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();


        }
    }
}
