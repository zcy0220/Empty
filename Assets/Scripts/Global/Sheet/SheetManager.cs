/**
 * Tool generation, do not modify!!!
 */

using Sheet;
using Base.Common;
using System.Collections.Generic;

public partial class SheetManager : Singleton<SheetManager>
{
	//Example
	private Dictionary<int, Sheet.Example> mExampleDict;
	public Sheet.Example GetExample(int key)
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
		mExampleDict = new Dictionary<int, Sheet.Example>();
		items.ForEach(item => mExampleDict[item.exampleInt] = item);
	}
	
	//Preload
	private List<Preload> mPreloadList;
	public List<Preload> GetPreloadList()
	{
		if (mPreloadList == null)
		{
			InitPreload();
		}
		return mPreloadList;
	}
	private void InitPreload()
	{
		var items = GetSheetInfo<PreloadList>("Preload").Items;
		mPreloadList = items;
	}
	
	//Scene
	private Dictionary<int, Sheet.Scene> mSceneDict;
	public Sheet.Scene GetScene(int key)
	{
		if (mSceneDict == null)
		{
			InitScene();
		}
		return mSceneDict[key];
	}
	private void InitScene()
	{
		var items = GetSheetInfo<SceneList>("Scene").Items;
		mSceneDict = new Dictionary<int, Sheet.Scene>();
		items.ForEach(item => mSceneDict[item.Id] = item);
	}
	
}