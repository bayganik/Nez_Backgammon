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
        //
        // Mouse var, so we can track what it clicks on
        //
        Entity MouseCursor;
        //
        // backgammon board is the background 
        //
        Entity Background;
        //
        // Stacks of checkers
        //
        Entity[] CheckerStacks = new Entity[26];
        Vector2 PosStacks = new Vector2(860, 505);
        UICanvas UIC;

        //public BKBoard Board { get; set; }
        public ImageButton PlayButton { get; set; }
        public TextButton ExitButton { get; set; }
        public TextButton NewButton { get; set; }
        public Label Msg { get; set; }
        public Entity CheckerBeingDragged { get; set; }
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

            NewButton = UIC.Stage.AddElement(new TextButton("Roll Dice", Skin.CreateDefaultSkin()));
            NewButton.SetPosition(900f, 10f);
            NewButton.SetSize(60f, 20f);
            NewButton.OnClicked += DiceButton_OnClicked;

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
            Background.Tag = 90;
            Background.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("BKBoard")).SetRenderLayer(99));
            Background.GetComponent<SpriteRenderer>().SetOrigin(new Vector2(0, 0));
            Background.SetPosition(new Vector2(100, 50));
            //
            // positions bottom right
            //
            Entity ent;
            PosStacks = new Vector2(860, 505);
            for (int i = 0; i < 6; i++)
            {
                
                ent = CreateEntity("chkstack" + i.ToString(), PosStacks);
                PosStacks = PosStacks - new Vector2(62, 0); 

                ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
                //ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                //ent.AddComponent(new PilePlayComponent());
                CheckerStacks[i] = ent;
            }
            //
            // positions bottom left
            //
            PosStacks = new Vector2(450, 505);
            for (int i = 6; i < 12; i++)
            {

                ent = CreateEntity("chkstack" + i.ToString(), PosStacks);
                PosStacks = PosStacks - new Vector2(62, 0);

                ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
                //ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                //ent.AddComponent(new PilePlayComponent());
                CheckerStacks[i] = ent;
            }
            //
            // positions top left
            //
            PosStacks = new Vector2(140, 180);
            for (int i = 12; i < 18; i++)
            {

                ent = CreateEntity("chkstack" + i.ToString(), PosStacks);
                PosStacks = PosStacks + new Vector2(62, 0);

                ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
                //ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                //ent.AddComponent(new PilePlayComponent());
                CheckerStacks[i] = ent;
            }
            ////
            //// positions top Right
            ////
            //PosStacks = new Vector2(140, 180);
            //for (int i = 18; i < 24; i++)
            //{

            //    var ent = CreateEntity("chkstack" + i.ToString(), PosStacks);
            //    PosStacks = PosStacks + new Vector2(62, 0);

            //    ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
            //    ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
            //    ent.AddComponent(new StackComponent() { StackID = i });
            //    //ent.AddComponent(new PilePlayComponent());
            //    CheckerStacks[i] = ent;
            //}
            //
            // positions top right
            //
            PosStacks = new Vector2(550, 180);
            for (int i = 18; i < 24; i++)
            {

                ent = CreateEntity("chkstack" + i.ToString(), PosStacks);
                PosStacks = PosStacks + new Vector2(62, 0);

                ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
                //ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
                ent.AddComponent(new StackComponent() { StackID = i });
                //ent.AddComponent(new PilePlayComponent());
                CheckerStacks[i] = ent;
            }
            //
            // Graveyard for white = 24
            //
            PosStacks = new Vector2(500, 505);
            ent = CreateEntity("chkstack 24" , PosStacks);
            PosStacks = PosStacks - new Vector2(62, 0);

            ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
            ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
            ent.AddComponent(new StackComponent() { StackID = 24 });
            CheckerStacks[24] = ent;
            //
            // Graveyard for black = 25
            //
            PosStacks = new Vector2(500, 180);
            ent = CreateEntity("chkstack 25", PosStacks);
            PosStacks = PosStacks - new Vector2(62, 0);

            ent.AddComponent(new SpriteRenderer(Content.Load<Texture2D>("EmptyHolder")));
            ent.AddComponent(new BoxCollider(-31, -123, 62, 246));
            ent.AddComponent(new StackComponent() { StackID = 25 });
            CheckerStacks[25] = ent;

            //
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // Systems to process our requests
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            //
            this.AddEntityProcessor(new MouseClickSystem(new Matcher().All(typeof(MouseComponent))));
            this.AddEntityProcessor(new StackDispSystem(new Matcher().All(typeof(StackComponent))));
            this.AddEntityProcessor(new CheckerDragSystem(new Matcher().All(typeof(DragComponent))));

            GameBoard = new BKBoard();
            Fill_Stacks();
        }
        public void DropChecker2PreviousPosition()
        {
            DragComponent dc = CheckerBeingDragged.GetComponent<DragComponent>();
            CheckerBeingDragged.Transform.Position = dc.PrevPosition;
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
                    _checker.Tag = i;
                    
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
                    _checker.AddComponent(new BoxCollider(-20, -20, 40, 40));   //collider covers the checker

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
            //CardDeckManager.CreateDeckOfCards(this);   //create a new deck and shuffle
            //Fill_All_Stacks();
            //
            // New button is pressed
            //
            var msg = UIC.Stage.GetElements();
            foreach (Element el in msg)
            {
                if ((el.GetType() == typeof(Label)))
                {
                    var lbl = (Label)el;
                    lbl.SetText("New Button Pushed");
                }
            }
            //
            // put back the main heading
            //
            TextEntity.Transform.Position = new Vector2(350, 20);
            var txt = TextEntity.GetComponent<TextComponent>();
            txt.RenderLayer = -100;
            txt.SetText("Dice Results !");
            txt.SetColor(Color.White);

        }
    }
}
