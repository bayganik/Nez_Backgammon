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
    public class StackDispSystem : EntityProcessingSystem
    {
        //
        // Entities with StackComponent system to display what is on their location (Fanned out, in place, etc.)
        //
        Vector2 fanOutDistannce;
        public StackDispSystem(Matcher matcher) : base(matcher)
        {
        }
        public override void Process(Entity entity)
        {
            //
            // entity = PlayStack
            //
            StackComponent sc = entity.GetComponent<StackComponent>();
            Entity lastCardonStack = sc.CheckersInStack.LastOrDefault();
            //
            // fan out is half size of the checker
            //
            switch (sc.FannedDirection)
            {
                case 0:
                    fanOutDistannce = Vector2.Zero;
                    break;
                case 1:
                    fanOutDistannce = new Vector2(31f, 0);
                    break;
                case 2:
                    fanOutDistannce = new Vector2(-31f, 0);
                    break;
                case 3:
                    fanOutDistannce = new Vector2(0, -31f);         //upwards
                    break;
                case 4:
                    fanOutDistannce = new Vector2(0, 31f);          //downwards
                    break;

            }
            //
            // All cards are Entities in this stack
            //
            int ind = 0;                            //checker number in stack

            for (int i = 0; i < sc.CheckersInStack.Count; i++)
            {
                Entity checkerEntity = sc.CheckersInStack[i];
                //checkerEntity.Enabled = true;
                Vector2 stackPos = entity.Transform.Position;

                if (sc.FannedDirection == 4)
                    stackPos.Y -= 123 - 25;
                else
                    stackPos.Y += 123 - 25;

                checkerEntity.Transform.Position = stackPos + fanOutDistannce * new Vector2(ind, ind);
                //
                // Get the sprite (face/back)
                //
                var cardComp = checkerEntity.GetComponent<CheckerComponent>();          //CheckerComponent has the data
                var renderComp = checkerEntity.GetComponent<SpriteRenderer>();               //sprite renderer of the card
                //
                // -1 is first to display and -9 is last layer to display
                //
                renderComp.RenderLayer = ind * -1;
                renderComp.Sprite = cardComp.CheckerFace;

                ind += 1;
            }

        }
    }
}

