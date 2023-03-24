﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Hutao.Service.Game.Unlocker;

/// <summary>
/// 游戏帧率解锁器
/// </summary>
public interface IGameFpsUnlocker
{
    /// <summary>
    /// 目标FPS,设置以动态更改帧率
    /// </summary>
    int TargetFps { get; set; }

    /// <summary>
    /// 异步的解锁帧数限制
    /// 需要管理员进程等级
    /// </summary>
    /// <param name="findModuleDelay">每次查找UnityPlayer的延时,推荐100毫秒</param>
    /// <param name="findModuleLimit">查找UnityPlayer的最大阈值,推荐10000毫秒</param>
    /// <param name="adjustFpsDelay">每次循环调整的间隔时间，推荐2000毫秒</param>
    /// <returns>一个任务，表示解锁的结果，在游戏进程退出前不会完成</returns>
    Task UnlockAsync(TimeSpan findModuleDelay, TimeSpan findModuleLimit, TimeSpan adjustFpsDelay);
}