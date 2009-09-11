using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ValkyrieLibrary.Input;
using Microsoft.Xna.Framework.Input;
using System.Net;
using Gablarski;
using ValkyrieLibrary.Core.Messages;
using ValkyrieLibrary.Network;
using Valkyrie.Characters;
using ValkyrieServerLibrary.Network.Messages.Valkyrie;
using ValkyrieLibrary.Core;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Runtime.InteropServices;
using ValkyrieLibrary.Characters;
using Microsoft.Xna.Framework.Content;
using System.Threading;

namespace ValkyrieLibrary.States
{
	public class LoginModule
		: IModule
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

		#region IModule Members
		private Texture2D Background;
		private bool Loaded;
		private KeybindController keycontroller;
		private bool drawing = false;

		private bool connecting = false;
		
		private string backgroundfile = "PokeBackground.png";

		public void Update(GameTime gameTime)
		{
			if (!this.Loaded)
				return;

			this.keycontroller.Update();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (!this.Loaded)
				return;

			this.drawing = true;

			spriteBatch.Begin();
			spriteBatch.Draw(this.Background, TileEngine.Camera.Screen, Color.White);
			spriteBatch.End();

			this.drawing = false;
		}

		public void Load()
		{
			this.Background = TileEngine.TextureManager.GetTexture(this.backgroundfile);
			
			this.keycontroller = new KeybindController();
			this.keycontroller.AddKey(Keys.Enter, "Login");
			this.keycontroller.KeyUp += keycontroller_KeyUp;

			this.Loaded = true;
		}

		public void Unload()
		{
			if (this.Loaded)
			{
				this.Loaded = false;

				while (this.drawing)
					Thread.Sleep(1); // Wait for the module to finish drawing to dispose the textures

				TileEngine.TextureManager.ClearFromCache(this.backgroundfile);
			}
		}

		public void Activate()
		{
			if (!this.Loaded)
				this.Load();
		}

		public void Deactivate()
		{
			if(this.Loaded)
				this.Unload();
		}

		#endregion

		private void keycontroller_KeyUp(object sender, KeyPressedEventArgs ev)
		{
			if (ev.Action == "Login")
			{
				if (this.connecting)
					return;

				FileInfo file = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "ClientSettings.xml"));

				XmlDocument doc = new XmlDocument();
				doc.Load(file.FullName);

				string address = doc.GetElementsByTagName("Address")[0].InnerText;
				string port = doc.GetElementsByTagName("Port")[0].InnerText;
				string username = doc.GetElementsByTagName("Username")[0].InnerText;
				string password = doc.GetElementsByTagName("Password")[0].InnerText;

				TileEngine.NetworkManager.Disconnected += this.TestDisconnected;
				TileEngine.NetworkManager.MessageReceived += this.TestMessageReceived;

				try
				{
					this.connecting = true;
					TileEngine.NetworkManager.Connect(new IPEndPoint(IPAddress.Parse(address), Convert.ToInt32(port)));
				}
				catch (SocketException)
				{
					// Failed to connect
					TileEngine.NetworkManager.Disconnected -= this.TestDisconnected;
					TileEngine.NetworkManager.MessageReceived -= this.TestMessageReceived;

					MessageBox(new IntPtr(0), "Cannot connect to server.", "Server down", 0);
					this.connecting = false;
					return;
				}

				LoginMessage msg = new LoginMessage();
				msg.Username = username;
				msg.Password = password;
				TileEngine.NetworkManager.Send(msg);
			}
		}

		private void TestDisconnected(object sender, EventArgs ev)
		{
			Environment.Exit(0);
		}

		public void TestMessageReceived(object sender, MessageReceivedEventArgs ev)
		{
			if (ev.Message is LoginSuccessMessage)
			{
				TileEngine.NetworkID = ((LoginSuccessMessage)ev.Message).NetworkIDAssigned;

				PlayerRequestMessage msg = new PlayerRequestMessage();
				msg.RequestedPlayerNetworkID = TileEngine.NetworkID;

				TileEngine.NetworkManager.Send(msg);
			}
			else if (ev.Message is LoginFailedMessage)
			{
				LoginFailedMessage msg = (LoginFailedMessage)ev.Message;

				if (msg.Reason == ConnectionRejectedReason.BadLogin)
					MessageBox(new IntPtr(0), "Incorrect username or password.", "Login Failed", 0);

				this.connecting = false;
			}
			else if (ev.Message is PlayerInfoMessage)
			{
				PlayerInfoMessage msg = (PlayerInfoMessage)ev.Message;
				if (msg.NetworkID != TileEngine.NetworkID)
					return;

				TileEngine.NetworkManager.Disconnected -= this.TestDisconnected;
				TileEngine.NetworkManager.MessageReceived -= this.TestMessageReceived;

				// Get character info
				PokePlayer player = new PokePlayer();
				player.Gender = Genders.Male;
				player.Sprite = TileEngine.TextureManager.GetTexture(msg.TileSheet);
				
				if ((msg.Location.Y % 32) != 0) 
					player.Location = new ScreenPoint(msg.Location.X, msg.Location.Y);
				else
					player.Location = new ScreenPoint(msg.Location.X, msg.Location.Y);
				player.CurrentAnimationName = msg.Animation;
				player.Name = msg.Name;
				player.Loaded = true;

				TileEngine.Player = player;

				TileEngine.ModuleManager.PushModuleToScreen("Game");
			}
		}
	}
}
