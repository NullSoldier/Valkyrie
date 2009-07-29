using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace ValkyrieLibrary.Input
{
	public class KeybindController
	{
		private readonly Dictionary<Keys, KeyDef> keyDefCollection = new Dictionary<Keys, KeyDef>();

		public Keys[] LastKeys { get; set; }
		public Keys[] CrntKeys { get; set; }

		//public delegate void KeyPressed(object sender,/*   ???  */);
		// Note that EventHandler is a build in delegate
		// To define custom arguments you just create your own and use it instead
		// Which is what I did

		public event EventHandler<KeyPressedEventArgs> KeyAction;

		public string GetKeyAction(Keys key)
		{
    		return (keyDefCollection.ContainsKey(key)) ? keyDefCollection[key].Action : String.Empty;
		}

		public void AddKey(Keys key, string action)
		{
			this.AddKey(key, action, false, false, false);
		}

		public void AddKey(string keyCode, string action)
		{
			this.AddKey(keyCode, action, false, false, false);
		}

		public void AddKey(string keyCode, string action, bool control, bool alt, bool shift)
		{
			this.AddKey((Keys)Convert.ToInt32(keyCode), action, control, alt, shift);
		}

		public void AddKey(Keys key, string action, bool control, bool alt, bool shift)
		{
			var keyDef = new KeyDef(action, key, control, alt, shift);
			this.keyDefCollection.Add(key, keyDef);
		}

		public void LoadKeys()
		{
			XmlDocument doc = new XmlDocument();
			doc.Load("Data/Keys.xml");

			XmlNodeList nodes = doc.GetElementsByTagName("Key");

			for (int n = 0; n < nodes.Count; n++)
			{
				KeyDef key = new KeyDef();

				XmlNodeList innerNodes = nodes[n].ChildNodes;

				for (int i = 0; i < innerNodes.Count; i++)
				{
					if (innerNodes[i].Name == "KeyPressed")
					{
						key.Key = (Keys)Convert.ToInt32(innerNodes[i].InnerText);
					}
					else if (innerNodes[i].Name == "Action")
					{
						key.Action = innerNodes[i].InnerText;
					}
					else if (innerNodes[i].Name == "Control")
					{
						key.Control = Convert.ToBoolean(Convert.ToInt32(innerNodes[i].InnerText));
					}
					else if (innerNodes[i].Name == "Alt")
					{
						key.Alt = Convert.ToBoolean(Convert.ToInt32(innerNodes[i].InnerText));
					}
					else if (innerNodes[i].Name == "Shift")
					{
						key.Shift = Convert.ToBoolean(Convert.ToInt32(innerNodes[i].InnerText));
					}
				}

				keyDefCollection.Add(key.Key, key);
			}
		}

		public void Update()
		{
			KeyboardState keyState = Keyboard.GetState();

			CrntKeys = keyState.GetPressedKeys();

			foreach (Keys key in CrntKeys)
			{
				if (!key.IsDirection() && LastKeys != null)
				{
					bool doBreak = false;

					foreach (Keys lastKey in LastKeys)
					{
						if (key == lastKey)
						{
							doBreak = true;
							break;
						}
					}

					if (doBreak)
						break;
				}

				if (keyDefCollection.ContainsKey(key))
					OnKeyAction (new KeyPressedEventArgs(key, keyDefCollection[key].Action));
			}

			LastKeys = CrntKeys;
		}

		private void OnKeyAction(KeyPressedEventArgs e)
		{
			var action = this.KeyAction;
			if (action != null)
				action(this, e);
		}
	}

	public class KeyPressedEventArgs : EventArgs
	{
		public KeyPressedEventArgs(Keys key, string action)
		{
			this.KeyPressed = key;
			this.Action = action;
		}

		public Keys KeyPressed { get; private set; }
		public string Action { get; private set; }
	}
}