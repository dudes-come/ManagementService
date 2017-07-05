﻿using AutoMapper;
using JoyOI.ManagementService.Configuration;
using JoyOI.ManagementService.Model.MapperProfiles;
using JoyOI.ManagementService.Services;
using JoyOI.ManagementService.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 配置管理服务的扩展函数
    /// </summary>
    public static class JoyOIManagementServiceCollectionExtensions
    {
        private static bool StaticFunctionsInitialized = false;
        private static object StaticFunctionsInitializeLock = new object();

        /// <summary>
        /// 初始化静态功能
        /// </summary>
        internal static void InitializeStaticFunctions()
        {
            if (StaticFunctionsInitialized)
            {
                return;
            }
            lock (StaticFunctionsInitializeLock)
            {
                Mapper.Initialize(c =>
                {
                    c.AddProfile<BaseMapperProfile>();
                    c.AddProfile<ActorMapperProfile>();
                    c.AddProfile<BlobMapperProfile>();
                    c.AddProfile<StateMachineMapperProfile>();
                    c.AddProfile<StateMachineInstanceMapperProfile>();
                });
                StaticFunctionsInitialized = true;
            }
        }

        /// <summary>
        /// 初始化管理服务
        /// </summary>
        public static void AddJoyOIManagement(
            this IServiceCollection services, JoyOIManagementConfiguration configuration)
        {
            // 检查配置
            if ((configuration.Nodes?.Count ?? 0) <= 0)
            {
                throw new ArgumentNullException("Please provide atleast 1 docker nodes");
            }

            // 注册服务
            services.AddTransient<IActorService, ActorService>();
            services.AddTransient<IBlobService, BlobService>();
            services.AddTransient<IStateMachineService, StateMachineService>();
            services.AddTransient<IStateMachineInstanceService, StateMachineInstanceService>();

            // 静态功能
            InitializeStaticFunctions();
        }
    }
}
