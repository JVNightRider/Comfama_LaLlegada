using System;
using System.Collections.Generic;

namespace DreamHouseStudios.Comfama
{
	[Serializable]
	public class PendingUsersToSend
	{
		public List<UserMailInfo> remainingUsers;
	}

	[Serializable]
	public class UserMailInfo
	{
		public string userMail;
		public List<string> photoPaths;
	}
}