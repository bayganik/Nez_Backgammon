using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace TestMiniMax.ECS.Components
{
    public class ComputerComponent : Component
    {
        //
        // Designate one player as computer (auto play)
        //
        public ComputerComponent()
        {

        }
        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity(); 
        }
    }
}
