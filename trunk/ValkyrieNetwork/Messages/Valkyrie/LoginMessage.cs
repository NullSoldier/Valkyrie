using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gablarski.Messages;
using Gablarski;

namespace ValkyrieLibrary.Core.Messages
{
	public class LoginMessage
		: ClientMessage
	{
		public LoginMessage ()
			: base (ClientMessageType.Login)
		{
		}

		public string Username
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public override void WritePayload (IValueWriter writer)
		{			
			if (String.IsNullOrEmpty (this.Password))
				throw new InvalidOperationException ("Can not login without a password.");

			writer.WriteString (Username);
			writer.WriteString (Password);
		}

		public override void ReadPayload (IValueReader reader)
		{
			this.Username = reader.ReadString ();
			this.Password = reader.ReadString ();
		}
	}
}