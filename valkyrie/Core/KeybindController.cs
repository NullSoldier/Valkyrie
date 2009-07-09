using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Input;

namespace valkyrie
{

   public class KeybindController
    {

        private Dictionary<Keys, KeyDef> KeyDefCollection = new Dictionary<Keys, KeyDef>();

        //public delegate void KeyPressed(object sender,/*   ???  */);
       // Note that EventHandler is a build in delegate
       // To define custom arguments you just create your own and use it instead
       // Which is what I did

        public event EventHandler<KeyPressedEventArgs> KeyAction;

        public void AddKey(Keys key, string Action)
        {
            this.AddKey(key, Action, false, false, false);
        }

        public void AddKey(string KeyCode, string Action)
        {
            this.AddKey(KeyCode, Action, false, false, false);
        }

        public void AddKey(string KeyCode, string Action, bool Control, bool Alt, bool Shift)
        {
            this.AddKey((Keys)Convert.ToInt32(KeyCode), Action, Control, Alt, Shift);
        }

        public void AddKey(Keys key, string Action, bool Control, bool Alt, bool Shift)
        {
            var keyDef = new KeyDef(Action, key, Control, Alt, Shift);
            this.KeyDefCollection.Add(key, keyDef);
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
                KeyDefCollection.Add(key.Key, key);
            }
        
        }

        public void Update()
        {
            KeyboardState KeyState = Keyboard.GetState();

			Keys[] KeysPressed = KeyState.GetPressedKeys();


            foreach (Keys key in KeysPressed)
            {
                if( KeyDefCollection.ContainsKey(key))
                {
                    //event
                    KeyAction( this, new KeyPressedEventArgs(key, KeyDefCollection[key].Action ));
                
                }
         
            }
        }

    }

    public class KeyPressedEventArgs
        : EventArgs
    {
        public KeyPressedEventArgs(Keys key, string Action)
        {
            this.KeyPressed = key;
            this.Action = Action;
        }

        public Keys KeyPressed;
        public string Action;
    }
}
