/**
 * Tool generation, do not modify!!!
 */

using Sheet;
using Base.Common;
using System.Collections.Generic;

public partial class SheetManager : Singleton<SheetManager>
{
	//Example
	private List<Example> mExampleList;
	public List<Example> GetmExampleList()
	{
		if (mExampleList == null)
		{
			InitExample();
		}
		return mExampleList;
	}
	private Dictionary<int, Example> mExampleDict;
	public Example GetExample(int key)
	{
		if (mExampleDict == null)
		{
			InitExample();
		}
		return mExampleDict[key];
	}
	private void InitExample()
	{
		var items = GetSheetInfo<ExampleList>("Example").Items;
		mExampleList = items;
		mExampleDict = new Dictionary<int, Example>();
		items.ForEach(item => mExampleDict[item.exampleInt] = item);
	}
	
}