using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez.ImGuiTools;
using Nez;

namespace Nez_Backgammon
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Core
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1() : base(1000, 700, false, true, "Backgammon MiniMax - Monogame version")
        { }
        protected override void Initialize()
        {

            base.Initialize();
            //System.Diagnostics.Debug.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(System.Console.Out));

            IsMouseVisible = true;
            DebugRenderEnabled = true;
            Window.AllowUserResizing = true;
            DebugRenderEnabled = false;
            //
            // ImGui doesn't work as a service
            //
            //var service = Core.GetGlobalManager<ImGuiManager>();
            //if (service == null)
            //{
            //    service = new ImGuiManager();
            //    Core.RegisterGlobalManager(service);
            //}
            //else
            //{
            //    service.SetEnabled(!service.Enabled);
            //}

            Scene = new Scenes.MainScene();
        }

    }
}
