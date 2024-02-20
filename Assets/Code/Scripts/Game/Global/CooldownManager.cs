using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// <c>CooldownManager</c> is an utility class used to easily track and manage cooldowns.
/// <example>
/// Adding a cooldown of 1 second to a button press:
/// <code>
/// public readonly int ButtonPressCooldownId = CooldownManager.NewId;
/// void Update()
/// {
///     if (Input.GetKeyDown(KeyCode.A) &amp;&amp; CooldownManager.IsDone(ButtonPressCooldownId))
///     {
///         CooldownManager.Start(ButtonPressCooldownId, 1.0f);
///         // Do something...
///     }
/// }
/// </code>
/// </example>
/// </summary>
public static class CooldownManager
{
    private static Dictionary<int, float> _cooldowns = new();
    private static int _lastId;

    /// <summary>
    /// Returns a new and unique <c>Id</c> to be used within the <c>CooldownManager</c> class.
    /// <example>
    /// Declaring a new cooldown variable:
    /// <code>
    /// public readonly int NamedCooldownId = CooldownManager.NewId;
    /// </code>
    /// </example>
    /// </summary>
    public static int NewId => ++_lastId;

    /// <summary>
    /// Start or overwrite a cooldown.
    /// </summary>
    public static void Start(int cooldownId, float cooldownDuration, float fedTime = -1.0f)
    {
        if (fedTime == -1.0f)
            _cooldowns[cooldownId] = Time.time + cooldownDuration;
        else
            _cooldowns[cooldownId] = fedTime + cooldownDuration;
    }

    /// <remark>
    /// Not started cooldowns are done by default.
    /// </remark>
    public static bool IsDone(int cooldownId, float fedTime = -1.0f)
    {
        if (fedTime == -1.0f)
            return !_cooldowns.TryGetValue(cooldownId, out float finishTime) || Time.time >= finishTime;
        else
            return !_cooldowns.TryGetValue(cooldownId, out float finishTime) || fedTime >= finishTime;
    }

    /// <remark>
    /// Not started cooldowns are done by default (meaning this will return 0.0f).
    /// </remark>
    public static float GetRemainingTime(int cooldownId, float fedTime = -1.0f)
    {
        if (fedTime == -1.0f)
            return _cooldowns.TryGetValue(cooldownId, out float finishTime) ? Mathf.Max(finishTime - Time.time, 0.0f) : 0.0f;
        else 
            return _cooldowns.TryGetValue(cooldownId, out float finishTime) ? Mathf.Max(finishTime - fedTime, 0.0f) : 0.0f;
    }
}