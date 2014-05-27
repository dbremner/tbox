﻿using System;
using System.Collections.Generic;
using Mnk.Library.Common.Data;
using NUnit.Core;

namespace Mnk.Library.ParallelNUnit.Core
{
    [Serializable]
    public class Result : IHasChildren, IRefreshable
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public ResultState State { get; set; }
        public string[] Categories { get; set; }
        public double Time { get; set; }
        public bool Executed { get; set; }
        public int AssertCount { get; set; }
        public bool IsTest { get { return string.Equals(Type, "TestMethod"); } }
        public IList<IHasChildren> Children { get; set; }
        public string Output { get; set; }
        public event Action OnRefresh;

        public Result()
        {
            Children = new List<IHasChildren>();
            State = ResultState.NotRunnable;
        }

        public virtual void Refresh()
        {
            if (OnRefresh != null) OnRefresh();
        }
    }
}