using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Gablarski;
using Gablarski.Server;
using NHibernate;
using NHibernate.Linq;
using ValkyrieServerLibrary.Core;
using ValkyrieServerLibrary.Entities;

namespace Valkyrie.Gablarski
{
	public class ValkyrieGablarskiAuthProvider
		: IAuthenticationProvider
	{
		public ValkyrieGablarskiAuthProvider()
		{
			factory = Fluently.Configure()
				.Database(PostgreSQLConfiguration.Standard.ConnectionString(s => s
					.Host (ConfigurationManager.AppSettings["valkyriedbhost"])
					.Username (ConfigurationManager.AppSettings["valkyriedbusername"])
					.Password (ConfigurationManager.AppSettings["valkyriedbpassword"])
					.Database (ConfigurationManager.AppSettings["valkyriedbdatabase"])
					.Port (Convert.ToInt32 (ConfigurationManager.AppSettings["valkyriedbport"]))))
				.Mappings (m => m.FluentMappings.AddFromAssemblyOf<Account> ())
				.BuildSessionFactory();
		}

		public Type IdentifyingType
		{
			get { return typeof (Int32); }
		}

		public bool UpdateSupported
		{
			get { return false; }
		}

		public bool UserExists (string username)
		{
			try
			{
				using (var session = factory.OpenSession())
				{
					return session.Linq<Account>().Any (a => a.Username == username);
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		public LoginResult Login (string username, string password)
		{
			try
			{
				using (var session = factory.OpenSession())
				{
					var login = session.Linq<Account>().FirstOrDefault (a => a.Username == username && a.Password == password);
					if (login == null)
						return new LoginResult (0, LoginResultState.FailedUsernameAndPassword);

					return new LoginResult (login.ID, LoginResultState.Success);
				}
			}
			catch (Exception)
			{
				return new LoginResult (0, LoginResultState.FailedUnknown);
			}
		}

		public LoginResult Register (string username, string password)
		{
			throw new NotSupportedException();
		}

		private readonly ISessionFactory factory;
	}
}
