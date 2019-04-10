/**
 * Tool generation, do not modify!!!
 * Generated from: User.proto
 */

using ProtoBuf;
using System.Collections.Generic;

namespace User {

	[ProtoContract]
	public class LoginRequest {
		[ProtoMember(1)]
		public string Account;
	}

	[ProtoContract]
	public class LoginResponse {
		[ProtoMember(1)]
		public int Result;
		[ProtoMember(2)]
		public User User;
	}

	[ProtoContract]
	public class User {
		[ProtoMember(1)]
		public Base Base;
		[ProtoMember(2)]
		public List<Item> Items;
	}

	[ProtoContract]
	public class Base {
		[ProtoMember(1)]
		public int UID;
		[ProtoMember(2)]
		public string Name;
	}

	[ProtoContract]
	public class Item {
		[ProtoMember(1)]
		public int Id;
		[ProtoMember(2)]
		public int Num;
	}

}
