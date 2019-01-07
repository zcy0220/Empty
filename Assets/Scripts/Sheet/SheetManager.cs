/**
 * Tool generation, do not modify!!!
 */

using Sheet;
using Base.Common;
using System.Collections.Generic;

public partial class SheetManager : Singleton<SheetManager>
{
	//Example
	private List<Example> ExampleList;
	public List<Example> GetExampleList()
	{
		if (ExampleList == null)
		{
			InitExample();
		}
		return ExampleList;
	}
	private Dictionary<int, Example> ExampleDict;
	public Example GetExample(int key)
	{
		if (ExampleDict == null)
		{
			InitExample();
		}
		return ExampleDict[key];
	}
	private void InitExample()
	{
		var items = GetSheetInfo<ExampleList>("Example").Items;
		ExampleList = items;
		ExampleDict = new Dictionary<int, Example>();
		items.ForEach(item => ExampleDict[item.exampleInt] = item);
	}
	
}