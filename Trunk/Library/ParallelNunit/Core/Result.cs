using System;
using System.Collections.Generic;
using Mnk.Library.Common.Models;
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
        public FailureSite FailureSite { get; set; }
        public string[] Categories { get; set; }
        public double Time { get; set; }
        public int AssertCount { get; set; }
        public IList<IHasChildren> Children { get; set; }
        public string Output { get; set; }
        public event Action OnRefresh;

        public Result()
        {
            Children = new List<IHasChildren>();
            State = ResultState.NotRunnable;
            FailureSite = FailureSite.Test;
        }

        public virtual void Refresh()
        {
            if (OnRefresh != null) OnRefresh();
        }

        public bool IsTest { get { return string.Equals(Type, "TestMethod"); } }

        public bool Executed
        {
            get
            {
                return State == ResultState.Success ||
                       State == ResultState.Failure ||
                       State == ResultState.Error ||
                       State == ResultState.Inconclusive;
            }
        }

        public virtual bool IsSuccess
        {
            get { return State == ResultState.Success; }
        }

        public virtual bool IsFailure
        {
            get { return State == ResultState.Failure; }
        }

        public virtual bool IsError
	    {
            get { return State == ResultState.Error; }
        }

        public bool HasResults
        {
            get { return Children != null && Children.Count > 0; }
        }
    }
}
