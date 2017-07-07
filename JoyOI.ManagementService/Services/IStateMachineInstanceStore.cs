﻿using JoyOI.ManagementService.Core;
using JoyOI.ManagementService.DbContexts;
using JoyOI.ManagementService.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JoyOI.ManagementService.Services
{
    /// <summary>
    /// 管理状态机实例的仓库, 应该为单例
    /// </summary>
    internal interface IStateMachineInstanceStore
    {
        /// <summary>
        /// 初始化仓库
        /// </summary>
        void Initialize(Func<JoyOIManagementContext> contextFactory);

        /// <summary>
        /// 编译状态机代码并返回实例
        /// </summary>
        Task<StateMachineBase> CreateInstance(
            StateMachineEntity stateMachineEntity,
            StateMachineInstanceEntity stateMachineInstanceEntity);

        /// <summary>
        /// 运行状态机实例
        /// </summary>
        Task RunInstance(StateMachineBase instance);
    }
}
