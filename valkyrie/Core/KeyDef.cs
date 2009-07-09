using Microsoft.Xna.Framework.Input;
using System;

namespace valkyrie
{
    class KeyDef
    {    
		public KeyDef(string action, Keys key)
            : this(action, key, false, false, false) { }

        public KeyDef(string action, Keys key, Boolean control, bool alt, bool shift)
        {
            this.Action = action;
            this.Key = key;
            this.Control = control;
            this.Alt = alt;
            this.Shift = shift;
        }

		public KeyDef()
		{
			// Insert generic constructor stuff here
		}

		#region Public Properties
        public bool Control
        {
            get;
            set;
        }

        public bool Alt
        {
            get;
            set;
        }

        public bool Shift
        {
            get;
            set;
        }

        public string Action
        {
            get;
            set;
        }

		public Keys Key
		{

			get;
			set;
		}
		#endregion
	}
}
