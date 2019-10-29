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
using Backgammon.Model;
using TestMiniMax.ECS.Components;

namespace TestMiniMax.Scenes
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
        // Mouse var, so we can track what it clicks on
        //
        Entity MouseCursor;
        //
        // backgammon board back ground 
        //
        Entity Background;
        //
        // Stacks of checkers
        //
        List<Entity> BoardStacks = new List<Entity>();
        Vector2 PosStacks = new Vector2(100, 50);
        UICanvas UIC;

        public BKBoard Board { get; set; }
        public ImageButton PlayButton { get; set; }
        public TextButton ExitButton { get; set; }
        public TextButton NewButton { get; set; }
        public Label Msg { get; set; }
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
            ExitButton.SetPosition(800f, 30f);
            ExitButton.SetSize(60f, 20f);
            ExitButton.OnClicked += ExitButton_OnClicked;

            NewButton = UIC.Stage.AddElement(new TextButton("Play !", Skin.CreateDefaultSkin()));
            NewButton.SetPosition(800f, 60f);
            NewButton.SetSize(60f, 20f);
            NewButton.OnClicked += NewButton_OnClicked;

            Msg = UIC.Stage.AddElement(new Label("Label Msg"));
            Msg.SetPosition(800f, 90f);
            Msg.SetSize(100f, 50f);

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
            Background.SetPosition(PosStacks);
            //
            // Setup the board (create stacks where checkers go into)
            //
            for (int i = 0; i < 26; i++)
            {
                var ent = CreateEntity("chkstack" + i.ToString(), )
            }
            Board = new BKBoard();

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
            txt.SetColor(Color.Black);
        }
        private void NewButton_OnClicked(Button button)
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
            txt.SetText("GAME is new again !");
            txt.SetColor(Color.Black);

        }
    }
}
