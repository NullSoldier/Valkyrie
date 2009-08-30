using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using ValkyrieLibrary.Core.Messages;

namespace Gablarski.Messages
{
	public abstract class MessageBase
	{
		public abstract ushort MessageTypeCode
		{
			get;
		}

		public virtual bool Reliable
		{
			get { return true; }
		}

		public virtual int MessageSize
		{
			get { return 0; }
		}

		public abstract void WritePayload (IValueWriter writerm);
		public abstract void ReadPayload (IValueReader reader);

		public static bool GetMessage (ushort messageType, out MessageBase msg)
		{
			msg = null;
			Func<MessageBase> msgctor;
			if (MessageTypes.TryGetValue (messageType, out msgctor))
			{
				msg = msgctor();
				return true;
			}

			return false;
		}

		public static ReadOnlyDictionary<ushort, Func<MessageBase>> MessageTypes
		{
			get;
			private set;
		}

		static MessageBase ()
		{
			// Manual message type checking
			/*MessageTypes = new ReadOnlyDictionary<ushort, Func<MessageBase>>(new Dictionary<ushort, Func<MessageBase>>
			{
				{ (ushort)ServerMessageType.LoginSuccess, () => new LoginSuccessMessage() },
				{ (ushort)ServerMessageType.LoginFailed, () => new LoginFailedMessage() },
				{ (ushort)ServerMessageType.PlayerUpdate, () => new PlayerUpdateMessage() },

				{ (ushort)ClientMessageType.Login, () => new LoginMessage() },
				{ (ushort)ClientMessageType.LocationData, () => new LocationUpdateMessage() },
			});*/


			// Automatic message type stuff
			var messageTypes = new Dictionary<ushort,Func<MessageBase>> (new Dictionary<ushort, Func<MessageBase>>());

			Type msgb = typeof(MessageBase);

			foreach (Type t in Assembly.GetExecutingAssembly ().GetTypes ().Where (t => msgb.IsAssignableFrom (t) && !t.IsAbstract))
			{
				var ctor = t.GetConstructor (Type.EmptyTypes);

				var dctor = new DynamicMethod (t.Name, msgb, null);
				var il = dctor.GetILGenerator ();

				il.Emit (OpCodes.Newobj, ctor);
				il.Emit (OpCodes.Ret);

				Func<MessageBase> dctord = (Func<MessageBase>)dctor.CreateDelegate (typeof (Func<MessageBase>));
				MessageBase dud = dctord();

				messageTypes.Add (dud.MessageTypeCode, dctord);
			}

			MessageTypes = new ReadOnlyDictionary<ushort, Func<MessageBase>> (messageTypes);
		}
	}
}