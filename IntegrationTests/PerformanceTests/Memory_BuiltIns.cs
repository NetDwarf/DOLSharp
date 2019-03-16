using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DOL.PerformanceTests.Memory
{
	[TestFixture]
	class BuiltIns
	{
		[Test]
		public void Primitive_object_12Bytes()
		{
			long before = GC.GetTotalMemory(true);
			var obj = new object();
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("Object uses " + memoryConsumption + " bytes");
		}

		[Test]
		public void Array_Empty_12Bytes()
		{
			long before = GC.GetTotalMemory(true);
			byte[] array = { };
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("Empty Array uses " + memoryConsumption + " bytes");
		}

		[Test]
		public void ArrayOfInt_WithTenElements_52Bytes()
		{
			long before = GC.GetTotalMemory(true);
			int[] array = new int[10];
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("int array uses " + memoryConsumption + " bytes");
		}

		[Test]
		public void ListOfInt_Init_36Bytes()
		{
			long before = GC.GetTotalMemory(true);
			var list = new List<int>();
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("Initialized List of Ints uses " + memoryConsumption + " bytes");
		}

		[Test]
		public void ListOfInt_OneIntAdded_40Bytes()
		{
			long before = GC.GetTotalMemory(true);
			var list = new List<int>();
			list.Add(1);
			list.TrimExcess();
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("List with one Int uses " + memoryConsumption + " bytes");
		}

		[Test]
		public void ListOfInt_TenIntAdded_76Bytes()
		{
			long before = GC.GetTotalMemory(true);
			var list = new List<int>();
			for(int i=0; i < 10; i++) { list.Add(1); }
			list.TrimExcess();
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("List with 10 Integers uses " + memoryConsumption + " bytes");
		}

		[Test]
		public void ListOfObjects_10ObjectsAdded_196Bytes()
		{
			long before = GC.GetTotalMemory(true);
			var list = new List<object>();
			for (int i = 0; i < 10; i++) { list.Add(new object()); }
			list.TrimExcess();
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			//36BytesInit + 10 * ( 4BytesReference + 12BytesObject )
			Console.WriteLine("List with 10 Objects uses " + memoryConsumption + " bytes");
		}
	}
}
