/**
 * lua测试
 */
/**
 * Lua脚本测试
 */

using UnityEngine;

public class LuaTest : MonoBehaviour
{
	private void Awake()
	{
        LuaManager.Instance.Restart();
        LuaManager.Instance.StartGame();
    }
}
