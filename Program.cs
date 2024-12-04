
using System.Text.Json;

namespace taskCli
{
    internal class Program
    {
        private const string fileName = "data.json";
        private static EnumCommand _currentCommand;
        private static List<MyDailyTask> _listOfMyDailyTasks = new List<MyDailyTask>();

        static void Main(string[] args)
        {
            LoadFromJsonFile();

            string commandArg = args[0].ToLower();
            if(!ValidateCommandArg(commandArg))
            {
                Console.WriteLine("Invalid command");
                return;
            }

            int taskId;
            switch (_currentCommand)
            {
                case EnumCommand.Add:
                    taskId = _listOfMyDailyTasks.Count() + 1;

                    string taskName = args[1];
                    if (string.IsNullOrEmpty(taskName))
                    {
                        Console.WriteLine("Add: Task name cannot be null or empty");
                        return;
                    }

                    MyDailyTask MyNewTask = new MyDailyTask(taskId: taskId, taskName);
                    _listOfMyDailyTasks.Add(MyNewTask);
                    SaveToJsonFile(_listOfMyDailyTasks);

                    Console.WriteLine($"Task added successfully (ID: {taskId})");

                    ShowListContent();
                    break;
                case EnumCommand.update:
                    taskId = int.Parse(args[1]);

                    MyDailyTask existentTaskToUpdate = _listOfMyDailyTasks.Find(t => t.TaskId == taskId);
                    if(existentTaskToUpdate == null)
                    {
                        Console.WriteLine($"Update: Task not found by (ID: {taskId})");
                        return;
                    }

                    string newTaskName = args[2];
                    if (string.IsNullOrEmpty(newTaskName))
                    {
                        Console.WriteLine("Update: New Task name cannot be null or empty");
                        return;
                    }

                    existentTaskToUpdate.TaskName = newTaskName;
                    existentTaskToUpdate.LastUpdateDate = DateTime.Now;
                    SaveToJsonFile(_listOfMyDailyTasks);

                    Console.WriteLine($"Task updated successfully (ID: {taskId})");

                    ShowListContent();
                    break;
                case EnumCommand.delete:
                    taskId = int.Parse(args[1]);

                    MyDailyTask existentTask = _listOfMyDailyTasks.Find(t => t.TaskId == taskId);
                    if (existentTask == null)
                    {
                        Console.WriteLine($"Delete: Task not found by (ID: {taskId})");
                        return;
                    }

                    _listOfMyDailyTasks.Remove(existentTask);
                    SaveToJsonFile(_listOfMyDailyTasks);

                    Console.WriteLine($"Task deleted successfully (ID: {taskId})");

                    ShowListContent();
                    break;
                case EnumCommand.markInProgress:
                    taskId = int.Parse(args[1]);

                    MyDailyTask existentTaskToMark = _listOfMyDailyTasks.Find(t => t.TaskId == taskId);
                    if (existentTaskToMark == null)
                    {
                        Console.WriteLine($"Mark-in-progress: Task not found by (ID: {taskId})");
                        return;
                    }

                    existentTaskToMark.TaskStatus = "in-progress";
                    existentTaskToMark.LastUpdateDate = DateTime.Now;
                    SaveToJsonFile(_listOfMyDailyTasks);

                    Console.WriteLine($"Task marked as in progress successfully (ID: {taskId})");

                    ShowListContent();
                    break;
                case EnumCommand.markDone:
                    taskId = int.Parse(args[1]);

                    MyDailyTask existentTaskToMarkDone = _listOfMyDailyTasks.Find(t => t.TaskId == taskId);
                    if (existentTaskToMarkDone == null)
                    {
                        Console.WriteLine($"Update: Task not found by (ID: {taskId})");
                        return;
                    }

                    existentTaskToMarkDone.TaskStatus = "done";
                    existentTaskToMarkDone.LastUpdateDate = DateTime.Now;
                    SaveToJsonFile(_listOfMyDailyTasks);

                    Console.WriteLine($"Task marked as done successfully (ID: {taskId})");

                    ShowListContent();
                    break;
                case EnumCommand.list:
                    if(args.Length > 1)
                    {
                        string statusFilter = args[1].ToLower();
                        if (!string.IsNullOrEmpty(statusFilter))
                            ShowFilteredListContent(statusFilter);
                    }
                    else
                        ShowListContent();
                    break;
                case EnumCommand.cleanup:
                    _listOfMyDailyTasks.Clear();
                    SaveToJsonFile(_listOfMyDailyTasks);
                    Console.WriteLine("List cleaned up successfully");
                    ShowListContent();
                    break;
                default:
                    break;
            }
        }

        private static void ShowListContent()
        {
            if (!_listOfMyDailyTasks.Any())
            {
                Console.WriteLine("Task List is current empty");
                return;
            }
            foreach (var task in _listOfMyDailyTasks)
            {
                Console.WriteLine("\n--------------------------------------");
                Console.WriteLine($"TaskId: {task.TaskId}");
                Console.WriteLine($"TaskName: {task.TaskName}");
                Console.WriteLine($"TaskStatus: {task.TaskStatus}");
                Console.WriteLine($"CreationDate: {task.CreationDate}");
                Console.WriteLine($"LastUpdateDate: {task.LastUpdateDate}");
                Console.WriteLine("--------------------------------------");
            }
        }

        private static void ShowFilteredListContent(string statusFilter)
        {
            if(statusFilter != "todo" && statusFilter != "in-progress" && statusFilter != "done")
            {
                Console.WriteLine("Invalid status filter");
                return;
            }

            if (!_listOfMyDailyTasks.Any(items => items.TaskStatus == statusFilter))
            {
                Console.WriteLine($"Task List with status {statusFilter} is currently empty");
                return;
            }
            foreach (var task in _listOfMyDailyTasks.Where(items => items.TaskStatus == statusFilter))
            {
                Console.WriteLine("\n--------------------------------------");
                Console.WriteLine($"TaskId: {task.TaskId}");
                Console.WriteLine($"TaskName: {task.TaskName}");
                Console.WriteLine($"TaskStatus: {task.TaskStatus}");
                Console.WriteLine($"CreationDate: {task.CreationDate}");
                Console.WriteLine($"LastUpdateDate: {task.LastUpdateDate}");
                Console.WriteLine("--------------------------------------");
            }
        }

        private static void SaveToJsonFile(List<MyDailyTask> list)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(list);
                File.WriteAllText(fileName, jsonContent);
                Console.WriteLine($"{fileName} saved successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fail to save {fileName} file, Details: {ex.Message}");
            }
        }

        private static void LoadFromJsonFile()
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"File {fileName} does not exists yet, so it could not be found at this time.");
                return;
            }

            string jsonContent = File.ReadAllText(fileName);
            if (jsonContent == null)
            {
                Console.WriteLine($"{fileName} is empty and could not be loaded");
                return;
            }
            _listOfMyDailyTasks = JsonSerializer.Deserialize<List<MyDailyTask>>(jsonContent);
        }

        private static bool ValidateCommandArg(string commandArg)
        {
            switch (commandArg)
            {
                case "add":
                    _currentCommand = EnumCommand.Add;
                    return true;
                case "update":
                    _currentCommand = EnumCommand.update;
                    return true;
                case "delete":
                    _currentCommand = EnumCommand.delete;
                    return true;
                case "mark-in-progress":
                    _currentCommand = EnumCommand.markInProgress;
                    return true;
                case "mark-done":
                    _currentCommand = EnumCommand.markDone;
                    return true;
                case "list":
                    _currentCommand = EnumCommand.list;
                    return true;
                case "cleanup":
                    _currentCommand = EnumCommand.cleanup;
                    return true;
                default:
                    break;
            }

            return false;
        }

        private enum EnumCommand
        {
            None = 0,
            Add,
            update,
            delete,
            markInProgress,
            markDone,
            list,
            cleanup
        }
    }
}
