using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Net;
using Gablarski;
using Valkyrie.Library.Core.Messages;
using Valkyrie.Library.Network;
using Valkyrie.Characters;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using Valkyrie.Library.Core;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using Valkyrie.Library;
using Valkyrie.Engine;
using Valkyrie.Engine.Input;

namespace Valkyrie.Modules
{
	public class LoginModule
		: IModule
	{
		public string Name
		{
			get { return "Login"; }
		}

		public bool IsLoaded
		{
			get { return this.isloaded; }
			set { this.isloaded = value; }
		}

		#region IModule Members

		public void Update (GameTime gameTime)
		{
		    if(!this.IsLoaded)
		        return;

		    this.keycontroller.Update();
		}

		public void Draw (SpriteBatch spriteBatch, GameTime gameTime)
		{
		    if(!this.IsLoaded)
		        return;

		    this.drawing = true;

		    spriteBatch.Begin();
		    spriteBatch.Draw(this.Background, new Rectangle(0, 0, 800, 600), Color.White);
		    spriteBatch.End();

		    this.drawing = false;
		}

		public void Load (IEngineContext enginecontext)
		{
			this.context = enginecontext;

			this.Background = this.context.TextureManager.GetTexture(this.backgroundfile);

			this.keycontroller.AddKey(Keys.Enter, "Login");
			this.keycontroller.KeyUp += keycontroller_KeyUp;

			this.IsLoaded = true;

		}

		public void Unload ()
		{
			if(this.IsLoaded)
			{
				this.IsLoaded = false;

				while(this.drawing)
					Thread.Sleep(1); // Wait for the module to finish drawing to dispose the textures

				this.context.TextureManager.ClearFromCache(this.backgroundfile);
			}
		}

		public void Activate ()
		{
			if(!this.IsLoaded)
				this.Load(this.context);
		}

		public void Deactivate ()
		{
			if(this.IsLoaded)
				this.Unload();
		}

		#endregion

		private void keycontroller_KeyUp (object sender, KeyPressedEventArgs ev)
		{
			if(ev.Action == "Login")
			{
				if(this.connecting)
					return;

				this.context.ModuleProvider.PushModule("Game");
				//FileInfo file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "ClientSettings.xml"));

				//XmlDocument doc = new XmlDocument();
				//doc.Load(file.FullName);

				//string address = doc.GetElementsByTagName("Address")[0].InnerText;
				//string port = doc.GetElementsByTagName("Port")[0].InnerText;
				//string username = doc.GetElementsByTagName("Username")[0].InnerText;
				//string password = doc.GetElementsByTagName("Password")[0].InnerText;

				//TileEngine.NetworkManager.Disconnected += this.TestDisconnected;
				//TileEngine.NetworkManager.MessageReceived += this.TestMessageReceived;

				//try
				//{
				//    this.connecting = true;
				//    TileEngine.NetworkManager.Connect(new IPEndPoint(IPAddress.Parse(address), Convert.ToInt32(port)));
				//}
				//catch(SocketException)
				//{
				//    // Failed to connect
				//    TileEngine.NetworkManager.Disconnected -= this.TestDisconnected;
				//    TileEngine.NetworkManager.MessageReceived -= this.TestMessageReceived;

				//    MessageBox(new IntPtr(0), "Cannot connect to server.", "Server down", 0);
				//    this.connecting = false;
				//    return;
				//}

				//LoginMessage msg = new LoginMessage();
				//msg.Username = username;
				//msg.Password = password;
				//TileEngine.NetworkManager.Send(msg);
			}
		}

		private void TestMessageReceived (object sender, MessageReceivedEventArgs ev)
		{
			//if(ev.Message is LoginSuccessMessage)
			//{
			//    TileEngine.NetworkID = ((LoginSuccessMessage)ev.Message).NetworkIDAssigned;

			//    PlayerRequestMessage msg = new PlayerRequestMessage();
			//    msg.RequestedPlayerNetworkID = TileEngine.NetworkID;

			//    TileEngine.NetworkManager.Send(msg);
			//}
			//else if(ev.Message is LoginFailedMessage)
			//{
			//    LoginFailedMessage msg = (LoginFailedMessage)ev.Message;

			//    if(msg.Reason == ConnectionRejectedReason.BadLogin)
			//        MessageBox(new IntPtr(0), "Incorrect username or password.", "Login Failed", 0);

			//    this.connecting = false;
			//}
			//else if(ev.Message is PlayerInfoMessage)
			//{
			//    PlayerInfoMessage msg = (PlayerInfoMessage)ev.Message;
			//    if(msg.NetworkID != TileEngine.NetworkID)
			//        return;

			//    TileEngine.NetworkManager.Disconnected -= this.TestDisconnected;
			//    TileEngine.NetworkManager.MessageReceived -= this.TestMessageReceived;

			//    // Get character info
			//    PokePlayer player = new PokePlayer();
			//    player.Gender = Genders.Male;
			//    player.Sprite = TileEngine.TextureManager.GetTexture(msg.TileSheet);

			//    player.Location = new ScreenPoint(msg.Location.X, msg.Location.Y);
			//    player.CurrentAnimationName = msg.Animation;
			//    player.Name = msg.Name;
			//    player.Loaded = true;

			//    TileEngine.Player = player;

			//    TileEngine.ModuleManager.PushModuleToScreen("Game");
			//}
		}

		private void TestDisconnected (object sender, EventArgs ev)
		{
			Environment.Exit(0);
		}

		private bool isloaded = false;
		private IEngineContext context = null;
		private Texture2D Background;
		private KeybindController keycontroller = new KeybindController();
		private bool drawing = false;
		private bool connecting = false;
		private string backgroundfile = "PokeBackground.png";

		#region Static internals
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox (IntPtr hWnd, String text, String caption, uint type);
		
		#endregion
	}
}
