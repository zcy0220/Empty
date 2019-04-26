/**
 * 定时器测试
 */

using UnityEngine;
using Base.Timer;

public class TimerManagerTest : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0.5f;
        TimerManager.Instance.CreateTimer(1, () =>
        {
            Debug.Log("Finish1");
        });
        TimerManager.Instance.CreateTimer(1, () =>
        {
            Debug.Log("Finish2");
        }, 2, true);
        TimerManager.Instance.CreateTimer(1, () =>
        {
            Debug.Log("Finish3");
        }, -1);
        TimerManager.Instance.CreateTimer(1, () =>
        {
            Debug.Log("Finish4");
        }, -1, true);
    }
}
