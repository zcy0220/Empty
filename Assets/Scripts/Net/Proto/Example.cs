/**
 * Tool generation, do not modify!!!
 * Generated from: Example.proto
 */

using ProtoBuf;
using System.Collections.Generic;

namespace Proto {

	[ProtoContract]
	public partial class Example {
		[ProtoMember(1)]
		public int ExampleInt;
		[ProtoMember(2)]
		public float ExampleFloat;
		[ProtoMember(3)]
		public string ExampleString;
		[ProtoMember(4)]
		public List<Item> ExampleArray;
	}

	[ProtoContract]
	public partial class Item {
		[ProtoMember(1)]
		public double ItemDouble;
		[ProtoMember(2)]
		public bool ItemBool;
	}

}
