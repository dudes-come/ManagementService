﻿using JoyOI.ManagementService.Model.Enums;
using JoyOI.ManagementService.Services;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using JoyOI.ManagementService.Utils;
using Newtonsoft.Json;

namespace JoyOI.ManagementService.Core
{
    /// <summary>
    /// 状态机实例的基础类
    /// </summary>
    public abstract class StateMachineBase : IDisposable
    {
        /// <summary>
        /// 第一个阶段
        /// </summary>
        public const string InitialStage = "Start";
        /// <summary>
        /// 最后一个阶段, 状态机到这个阶段后应该不做任何处理
        /// </summary>
        public const string FinalStage = "Finished";

        /// <summary>
        /// 状态机Id
        /// </summary>
        public Guid Id { get; internal set; }
        /// <summary>
        /// 执行时使用的并发键
        /// </summary>
        public string ExecutionKey { get; internal set; }
        /// <summary>
        /// 状态机的当前状态
        /// </summary>
        public StateMachineStatus Status { get; internal set; }
        /// <summary>
        /// 状态机的当前阶段
        /// </summary>
        public string Stage { get; internal set; }
        /// <summary>
        /// 已开始的任务列表
        /// </summary>
        public IList<ActorInfo> StartedActors { get; internal set; }
        /// <summary>
        /// 初始的文件列表
        /// </summary>
        public IList<BlobInfo> InitialBlobs { get; internal set; }
        /// <summary>
        /// 管理状态机实例的仓库
        /// </summary>
        internal IStateMachineInstanceStore Store { get; set; }
        /// <summary>
        /// 使用的限制参数
        /// </summary>
        public ContainerLimitation Limitation { get; internal set; }
        /// <summary>
        /// 自定义参数
        /// Host等等可以通过这里获取
        /// </summary>
        public IDictionary<string, string> Parameters { get; internal set; }
        /// <summary>
        /// 运行优先级
        /// 默认为0, 越低的值越优先
        /// </summary>
        public int Priority { get; internal set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public StateMachineBase()
        {
        }

        /// <summary>
        /// 状态机运行完毕后执行的函数
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// 发布单个任务到容器并运行
        /// </summary>
        protected Task DeployAndRunActorAsync(RunActorParam parameter)
        {
            return DeployAndRunActorsAsync(parameter);
        }

        /// <summary>
        /// 发布多个任务到容器并同时运行
        /// </summary>
        protected Task DeployAndRunActorsAsync(params RunActorParam[] parameters)
        {
            var runActors = new List<ActorInfo>();
            foreach (var parameter in parameters)
            {
                var actorInfo = new ActorInfo()
                {
                    Name = parameter.Name,
                    StartTime = DateTime.UtcNow,
                    EndTime = null,
                    Inputs = parameter.Inputs,
                    Outputs = new BlobInfo[0],
                    Exceptions = new string[0],
                    Status = ActorStatus.Running,
                    Stage = Stage,
                    Tag = parameter.Tag,
                    UsedNode = null,
                    UsedContainer = null
                };
                runActors.Add(actorInfo);
            }
            return Store.RunActors(this, runActors);
        }

        /// <summary>
        /// 处理状态机抛出异常时的操作
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public virtual Task HandleErrorAsync(Exception ex)
        {
            return Task.FromResult(-1);
        }

        /// <summary>
        /// 切换到新的阶段
        /// </summary>
        protected Task SetStageAsync(string stage)
        {
            return Store.SetInstanceStage(this, stage);
        }

        /// <summary>
        /// 提交内容到远程服务器, 并返回回应的内容
        /// </summary>
        protected Task<string> HttpInvokeAsync(HttpMethod method, string endpoint, object body)
        {
            var host = Parameters["Host"];
            return HttpClientUtils.HttpInvokeAsync(host, method, endpoint, body);
        }

        protected Task<Guid> UploadBlobAsync(string filename, byte[] bytes)
        {
            return Store.PutBlob(filename, bytes, DateTime.Now);
        }

        protected Task<Guid> UploadTextFileAsync(string filename, string text)
        {
            return UploadBlobAsync(filename, Encoding.UTF8.GetBytes(text));
        }

        protected Task<Guid> UploadJsonFileAsync(string filename, object obj)
        {
            return UploadTextFileAsync(filename, JsonConvert.SerializeObject(obj));
        }

        /// <summary>
        /// 运行状态机
        /// </summary>
        public abstract Task RunAsync();
    }
}
