/**
 * Tool generation, do not modify!!!
 */

using ProtoBuf;
using System.Collections.Generic;

namespace Sheet
{
	[ProtoContract]
	public class Example
	{
		[ProtoMember(1)]
		public int exampleInt;
		[ProtoMember(2)]
		public string exampleString;
		[ProtoMember(3)]
		public float exampleFloat;
		[ProtoMember(4)]
		public bool exampleBool;
		[ProtoMember(5)]
		public List<int> exampleArray;
		[ProtoMember(6)]
		public int test1;
		[ProtoMember(7)]
		public int test2;
	}
}
