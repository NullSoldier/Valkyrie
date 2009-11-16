using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gablarski.Server
{
	public interface IUserProvider
	{
		/// <summary>
		/// Gets whether a user exists or not.
		/// </summary>
		/// <param name="username">The username to check.</param>
		/// <returns><c>true</c> if the username exists, <c>false</c> otherwise</returns>
		//bool UserExists (string username);

		/// <summary>
		/// Gets the type used to identify users.
		/// </summary>
		Type IdentifyingType { get; }

		/// <summary>
		/// Attempts to login a user using the supplied username and password.
		/// </summary>
		/// <param name="username">The username to login with.</param>
		/// <param name="password">The password to login to the username with.</param>
		/// <returns></returns>
		LoginResult Login (string username, string password);
	}
}