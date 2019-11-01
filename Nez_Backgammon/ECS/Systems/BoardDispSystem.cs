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
    public class BoardDispSystem : EntityProcessingSystem
    {
        //
        // Entities with 'StackComponent' processed.  Display what is in their 'List<Entity> CheckersInStack' variable
        //
        Vector2 fanOutDistannce;
        public BoardDispSystem(Matcher matcher) : base(matcher)
        {

        }
        public override void Process(Entity entity)
        {
            //
            // entity = Stack entities (there are 26 of them)
            // This process happens for every stack
            //
            StackComponent sc = entity.GetComponent<StackComponent>();
            //Entity lastCardonStack = sc.CheckersInStack.LastOrDefault();
            //
            // fan out is half size of the checker
            //
            switch (sc.FannedDirection)
            {
                case 0:
                    fanOutDistannce = Vector2.Zero;
                    break;
                case 1:
                    fanOutDistannce = new Vector2(22f, 0);
                    break;
                case 2:
                    fanOutDistannce = new Vector2(-22f, 0);
                    break;
                case 3:
                    fanOutDistannce = new Vector2(0, -22f);         //upwards
                    break;
                case 4:
                    fanOutDistannce = new Vector2(0, 22f);          //downwards
                    break;

            }


            //
            // if we have more than 10 checkers in this Stack, make the fanout value smaller so they all fit
            //
            if (sc.CheckersInStack.Count > 10)
            {
                switch (sc.FannedDirection)
                {
                    case 3:
                        fanOutDistannce = new Vector2(0, -12f);
                        break;
                    case 4:
                        fanOutDistannce = new Vector2(0, 12f);
                        break;
                }
            }

            int ind = 0;                            //checker index in stack
            for (int i = 0; i < sc.CheckersInStack.Count; i++)
            {
                Entity checkerEntity = sc.CheckersInStack[i];       //get checker
                Vector2 stackPos = entity.Transform.Position;       //get location of the stack (doesn't change)
                //
                // The offset value upward or downward
                //
                if (sc.FannedDirection == 4)    //downward
                    stackPos.Y -= 123 - 25;     //initial location
                else
                    stackPos.Y += 123 - 25;     //upward
                //
                //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
                // Checker position determined by stack position with offset values
                //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
                //
                
                checkerEntity.Transform.Position = stackPos + fanOutDistannce * new Vector2(ind, ind);
                //
                // Get the CheckerComponent & its sprite shape
                // (we don't need to do this every frame but then the checkers won't look pretty)
                //
                var cComp = checkerEntity.GetComponent<CheckerComponent>();          //CheckerComponent has the data
                var renderComp = checkerEntity.GetComponent<SpriteRenderer>();       //sprite image of the checker
                //
                // -1 is first layer to display and -9 is last layer to display
                //
                renderComp.RenderLayer = ind * -100;
                renderComp.Sprite = cComp.CheckerFace;

                ind += 1;
            }

        }
    }
}

