using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Net;
using Valkyrie.Library.Core.Messages;
using Valkyrie.Library.Network;
using Valkyrie.Characters;
using Valkyrie;
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
using Valkyrie.Providers;
using Valkyrie.Engine.Providers;
using Valkyrie.Engine.Characters;
using Valkyrie.Engine.Core;
using Microsoft.Xna.Framework.Audio;
using System.Media;
using System.Security.Cryptography;
using System.Diagnostics;

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
			this.context.SoundProvider.Update (gameTime);
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
			this.network = (PokeNetworkProvider)this.context.NetworkProvider;

			this.Background = this.context.TextureManager.GetTexture(this.backgroundfile);

			this.keycontroller.AddKey(Keys.Enter, "Login");
			this.keycontroller.KeyUp += keycontroller_KeyUp;

			this.context.SoundManager.AddSound ("IntroMusic.wav");

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
			this.context.SoundProvider.PlayBGM (this.context.SoundManager.GetSound ("IntroMusic.wav"), true);

			if(!this.IsLoaded)
				this.Load(this.context);
		}

		public void Deactivate ()
		{
			this.context.SoundProvider.StopBGM ();

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

				FileInfo file = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PokeWorldOnline\\ClientSettings.xml"));

				XmlDocument doc = new XmlDocument();
				doc.Load(file.FullName);

				string address = doc.GetElementsByTagName("Address")[0].InnerText;
				string port = doc.GetElementsByTagName("Port")[0].InnerText;
				string username = doc.GetElementsByTagName("Username")[0].InnerText;
				string password = doc.GetElementsByTagName("Password")[0].InnerText;
				string gablarskiaddress = doc.GetElementsByTagName ("GablarskiHost")[0].InnerText;
				string gablarskiport = doc.GetElementsByTagName("GablarskiPort")[0].InnerText;

				this.context.NetworkProvider.Disconnected += this.TestDisconnected;
				this.context.NetworkProvider.MessageReceived += this.TestMessageReceived;

				try
				{
					this.connecting = true;

					this.context.VoiceChatProvider.ConnectAsync (gablarskiaddress, Convert.ToInt32 (gablarskiport), username, Helpers.MD5 (password));
					bool result = this.context.NetworkProvider.Connect(address, Convert.ToInt32(port));
					if(!result)
						throw new SocketException ();
				}
				catch(SocketException)
				{
					// Failed to connect
					this.context.NetworkProvider.Disconnected -= this.TestDisconnected;
					this.context.NetworkProvider.MessageReceived -= this.TestMessageReceived;

					this.context.VoiceChatProvider.Disconnect ();

					MessageBox(new IntPtr(0), "The server is currently offline.", "Server down", 0);
					this.connecting = false;
					return;
				}

				LoginMessage msg = new LoginMessage();
				msg.Username = username;
				msg.Password = Helpers.MD5 (password).Replace ("-", String.Empty);
				this.context.NetworkProvider.Send(msg);
			}
		}

		private void TestMessageReceived (object sender, MessageReceivedEventArgs ev)
		{
			if(ev.Message is LoginSuccessMessage)
			{
				// Store the assigned network ID untill we are ready to create the player
				this.assignednetworkID = ((LoginSuccessMessage)ev.Message).NetworkIDAssigned;

				PlayerRequestMessage msg = new PlayerRequestMessage();
				msg.RequestedPlayerNetworkID = this.assignednetworkID;

				this.network.Send(msg);
			}
			else if(ev.Message is LoginFailedMessage)
			{
				LoginFailedMessage msg = (LoginFailedMessage)ev.Message;

				if(msg.Reason == ConnectionRejectedReason.BadLogin)
					MessageBox(new IntPtr(0), "Incorrect username or password.", "Login Failed", 0);

				this.connecting = false;
			}
			else if(ev.Message is PlayerInfoMessage)
			{
				PlayerInfoMessage msg = (PlayerInfoMessage)ev.Message;
				if(msg.NetworkID != this.assignednetworkID) // If we've just received some player info that isn't us..
					return;

				// Always unsubscribe to avoid memory leaks
				this.network.Disconnected -= this.TestDisconnected;
				this.network.MessageReceived -= this.TestMessageReceived;

				// Get character info
				PokePlayer player = new PokePlayer()
				{
					Name = msg.Name,
					ID = this.assignednetworkID,
					Location = new ScreenPoint(msg.Location.X, msg.Location.Y),
					WorldName = msg.WorldName,
					Gender = Genders.Male,
					Sprite = this.context.TextureManager.GetTexture(msg.TileSheet),
					CurrentAnimationName = msg.Animation,
					IsLoaded = true
				};

				this.context.SceneProvider.AddPlayer("player1", player);
				this.context.ModuleProvider.GetModule("Game").Load(this.context);
				this.context.ModuleProvider.PushModule("Game");
			}
		}

		private void TestDisconnected (object sender, EventArgs ev)
		{
			Environment.Exit(0);
		}

		private bool isloaded = false;
		private IEngineContext context = null;
		private PokeNetworkProvider network = null;
		private Texture2D Background;
		private KeybindController keycontroller = new KeybindController();
		private uint assignednetworkID = 0;
		private bool drawing = false;
		private bool connecting = false;
		private string backgroundfile = "PokeBackground.png";

		#region Static internals
		
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox (IntPtr hWnd, String text, String caption, uint type);
		
		#endregion
	}
}
