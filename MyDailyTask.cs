namespace taskCli
{
    internal class MyDailyTask
    {
        public MyDailyTask(int taskId, string taskName)
        {
            TaskId = taskId;
            TaskName = taskName;
            TaskStatus = "todo";
        }

        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public string TaskStatus { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime? LastUpdateDate { get; set; } = null;
    }
}