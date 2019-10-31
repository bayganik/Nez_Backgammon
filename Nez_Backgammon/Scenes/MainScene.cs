using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez;
using Nez.UI;
using Nez.Tiled;
using Nez.Sprites;
using Nez.Textures;

using Nez_Backgammon.ECS.Components;
using Nez_Backgammon.ECS.Systems;

namespace Nez_Backgammon.Scenes
{
    public class MainScene : BaseScene
    {
        SceneResolutionPolicy policy;
        NezSpriteFont font;
        Entity TextEntity;

        Entity Play;
        Entity FForward;
        Entity FBackward;
        Entity Pause;
        //
        // Game Board (to be passed around)
        //
        public BKBoard GameBoard { get; set; }
        public int[] DiceRoll { get; set; }
        public bool WhiteTurn { get; set; }
        //public Entity WhiteGraveYard { get; set; }
        //public Entity BlackGraveYard { get; set; }
        //
        // Stacks of checkers (24 for game, 1 white graveyard, 1 black grave yard)
        //
        public Entity[] CheckerStacks = new Entity[26];
        //
        // Mouse var, so we can track what it clicks on
        //
        Entity MouseCursor;                 // mouse cursor to track (could have an image!)
        Entity Background;                  // backgammon board is the background 

        Vector2 PosStacks = new Vector2(860, 505);
        UICanvas UIC;

        public ImageButton PlayButton { get; set; }
        public TextButton ExitButton { get; set; }
        public TextButton DiceButton { get; set; }
        public Label Msg { get; set; }
        public Entity CheckerBeingDragged { get; set; }
        public bool Dragging { get; set; }
        public MainScene()
        {
            policy = Scene.SceneResolutionPolicy.ExactFit;
        }
        public override void Initialize()
        {
            base.Initialize();
            //font = new NezSpriteFont(Content.Load<SpriteFont>("Arial"));
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // Text entity with component (Game name label)
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            TextEntity = CreateEntity("txt");
            TextEntity.Transform.Position = new Vector2(100, 20);
            TextEntity.Transform.Scale = new Vector2(2, 2);
            var txt = new TextComponent(Graphics.Instance.BitmapFont, "MiniMax Test", new Vector2(0, 0), Color.White);
           // txt.SetFont(font);
            TextEntity.AddComponent(txt);

            font = new NezSpriteFont(Content.Load<SpriteFont>("Arial"));

            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // put a Canvas entity on upper right hand side for UI
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            Entity uiCanvas = CreateEntity("ui-canvas");
            UIC = uiCanvas.AddComponent(new UICanvas());

            ExitButton = UIC.Stage.AddElement(new TextButton("End !", Skin.CreateDefaultSkin()));
            ExitButton.SetPosition(800f, 10f);
            ExitButton.SetSize(60f, 20f);
            ExitButton.OnClicked += ExitButton_OnClicked;

            DiceButton = UIC.Stage.AddElement(new TextButton("Roll Dice", Skin.CreateDefaultSkin()));
            DiceButton.SetPosition(900f, 10f);
            DiceButton.SetSize(60f, 20f);
            DiceButton.OnClicked += DiceButton_OnClicked;

            //Msg = UIC.Stage.AddElement(new Label("Label Msg"));
            //Msg.SetPosition(800f, 90f);
            //Msg.SetSize(100f, 50f);

            ImageButtonStyle stl = new ImageButtonStyle();
            PlayButton = UIC.Stage.AddElement(new ImageButton(stl));
            PlayButton.SetPosition(800f, 120);
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // mouse entity (used for tracking of clicks)
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            MouseCursor = CreateEntity("mouse");
            MouseCursor.AddComponent(new BoxCollider());
            MouseCursor.AddComponent(new MouseComponent());
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // Background is the backgammon board
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            Background = CreateEntity("backgammonboard", new Vector2(0, 0));
            Background.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("BKBoard")).SetRenderLayer(100));
            Background.GetComponent<SpriteRenderer>().SetOrigin(new Vector2(0, 0));
            Background.SetPosition(new Vector2(100, 50));
            //
            // positions bottom right
            //
            Entity ent;
            PosStacks = new Vector2(860, 510);
            for (int i = 0; i < 6; i++)
            {
                
                ent = CreateEntity("stack" + i.ToString(), PosStacks);
                PosStacks = PosStacks - new Vector2(62, 0);
                //ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")).SetRenderLayer(-99));
                ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                //ent.AddComponent(new PilePlayComponent());
                CheckerStacks[i] = ent;
            }
            //
            // positions bottom left
            //
            PosStacks = new Vector2(450, 510);
            for (int i = 6; i < 12; i++)
            {

                ent = CreateEntity("stack" + i.ToString(), PosStacks);
                PosStacks = PosStacks - new Vector2(62, 0);

                //ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")).SetRenderLayer(-99));
                ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                //ent.AddComponent(new PilePlayComponent());
                CheckerStacks[i] = ent;
            }
            //
            // positions top left
            //
            PosStacks = new Vector2(140, 190);
            for (int i = 12; i < 18; i++)
            {

                ent = CreateEntity("stack" + i.ToString(), PosStacks);
                PosStacks = PosStacks + new Vector2(62, 0);

                //ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
                ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                //ent.AddComponent(new PilePlayComponent());
                CheckerStacks[i] = ent;
            }
            //
            // positions top right
            //
            PosStacks = new Vector2(550, 190);
            for (int i = 18; i < 24; i++)
            {

                ent = CreateEntity("stack" + i.ToString(), PosStacks);
                PosStacks = PosStacks + new Vector2(62, 0);

                //ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
                ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                //ent.AddComponent(new PilePlayComponent());
                CheckerStacks[i] = ent;
            }
            //
            // Graveyard for white = 24
            //
            PosStacks = new Vector2(500, 505);
            ent = CreateEntity("stack 24" , PosStacks);
            PosStacks = PosStacks - new Vector2(62, 0);

            //ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
            ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
            ent.AddComponent(new StackComponent() { StackID = 24 });
            CheckerStacks[24] = ent;
            //
            // Graveyard for black = 25
            //
            PosStacks = new Vector2(500, 180);
            ent = CreateEntity("stack 25", PosStacks);
            PosStacks = PosStacks - new Vector2(62, 0);

            //ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
            ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
            ent.AddComponent(new StackComponent() { StackID = 25 });
            CheckerStacks[25] = ent;

            //
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // Systems to process our requests
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            //
            this.AddEntityProcessor(new MouseClickSystem(new Matcher().All(typeof(MouseComponent))));
            this.AddEntityProcessor(new BoardDispSystem(new Matcher().All(typeof(StackComponent))));
            this.AddEntityProcessor(new CheckerDragSystem(new Matcher().All(typeof(DragComponent))));

            GameBoard = new BKBoard();
            Dragging = false;
            WhiteTurn = false;               //if true the Mouse Clicks are allowed

            Fill_Stacks();
        }
        public void DropChecker2NewPosition(Entity _dropStack)
        {
            //
            // Drop is good, put checker back in its original stack
            //
            StackComponent sc = _dropStack.GetComponent<StackComponent>();
            sc.CheckersInStack.Add(CheckerBeingDragged);

            ClearDragChecker();
        }
        public void DropChecker2PreviousPosition()
        {
            //
            // Drop failed, put checker back in its original stack
            //
            DragComponent dc = CheckerBeingDragged.GetComponent<DragComponent>();
            Entity FromEnt = dc.FromStack;
            StackComponent sc = FromEnt.GetComponent<StackComponent>();
            sc.CheckersInStack.Add(CheckerBeingDragged);

            ClearDragChecker();
        }
        public void ClearDragChecker()
        {
            DragComponent dc = CheckerBeingDragged.GetComponent<DragComponent>();
            CheckerBeingDragged.RemoveComponent(dc);
        }
        private void Fill_Stacks()
        {
            int fanout = 3;
            int chkNumBlack = 0;
            int chkNumWhite = 0;
            SpriteRenderer sp;
            for (int i = 0; i < 26; i++)
            {
                if (i == 12)
                    fanout = 4;

                Entity stack = CheckerStacks[i];
                StackComponent sc = stack.GetComponent<StackComponent>();
                sc.FannedDirection = fanout;
                sc.CheckersInStack.Clear();

                int _totalchkers = Math.Abs(GameBoard.NumOfCheckers[i]);
                for (int j = 0; j < _totalchkers; j++)
                {
                    Entity _checker = CreateEntity("Checker" + i.ToString());
                    
                    CheckerComponent cc = new CheckerComponent();
                    cc.CName = "Checker" + i.ToString();

                    if (GameBoard.NumOfCheckers[i] < 0)
                    {
                        sp = new SpriteRenderer(Content.Load<Texture2D>("BlackChecker"));
                        cc.IsWhite = false;
                        chkNumBlack -= 1;
                        _checker.Tag = chkNumBlack;
                    }
                    else
                    {
                        sp = new SpriteRenderer(Content.Load<Texture2D>("WhiteChecker"));
                        cc.IsWhite = true;
                        chkNumWhite += 1;
                        _checker.Tag = chkNumWhite;
                    }
                    _checker.AddComponent(sp);     //add sprite component

                    cc.HoldingStack = sc;
                    cc.CheckerFace = sp.Sprite;
                    _checker.AddComponent(cc);      //add check component
                    //_checker.AddComponent(new BoxCollider(-20, -20, 40, 40));   //collider covers the checker

                    sc.CheckersInStack.Add(_checker);
                }

            }



        }
        private void ExitButton_OnClicked(Button button)
        {
            //
            // Exit button is pressed
            //
            TextEntity.Transform.Position = new Vector2(350, 400);
            var txt = TextEntity.GetComponent<TextComponent>();
            txt.RenderLayer = -100;
            txt.SetText("GAME IS OVER !");
            txt.SetColor(Color.White);
        }
        private void DiceButton_OnClicked(Button button)
        {
            //
            // Give a pair of numbers
            //
            TextEntity.Transform.Position = new Vector2(100, 20);
            var txt = TextEntity.GetComponent<TextComponent>();
            txt.RenderLayer = -100;
            //
            // random integer between min (inclusive) and max (exclusive)
            //
            int _dice1 = Nez.Random.Range(1, 7);
            int _dice2 = Nez.Random.Range(1, 7);
            //
            // if a double, then we have 4 chances to move
            //
            if (_dice1 == _dice2)
            {
                DiceRoll = new int[4];
                DiceRoll[0] = _dice1;
                DiceRoll[1] = _dice1;
                DiceRoll[2] = _dice1;
                DiceRoll[3] = _dice1;
            }
            else
            {
                DiceRoll = new int[2];
                DiceRoll[0] = _dice1;
                DiceRoll[1] = _dice2;
            }
            txt.SetText("White Dice Roll: " + _dice1.ToString() + " - " + _dice2.ToString() + "   " + DiceRoll.Count().ToString());
            txt.SetColor(Color.White);
            WhiteTurn = true;           //allow white to click on its checkers

            //UpdateTheBoard();
        }
    }
}
