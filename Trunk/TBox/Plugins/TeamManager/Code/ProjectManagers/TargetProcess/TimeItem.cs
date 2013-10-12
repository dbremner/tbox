using System;

namespace TeamManager.Code.ProjectManagers.TargetProcess
{
    public class ProjectItem
    {
        public int Id { get; set; }
    }
    public class AssignableItem
    {
        public int Id { get; set; }
    }
    public class UserItem
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
    public class TimeItem
    {
        public double Spent { get; set; }
        public DateTime Date { get; set; }
        public AssignableItem Assignable { get; set; }
        public UserItem User { get; set; }
    }

    public class TimeReport
    {
        public TimeItem[] Items { get; set; }
    }
}
