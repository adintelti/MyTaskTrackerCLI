# MyTaskTrackerCLI

Example
The list of commands and their usage is given below:

# Adding a new task
task-cli add "Buy groceries"
# Output: Task added successfully (ID: 1)

# Updating and deleting tasks
task-cli update 1 "Buy groceries and cook dinner"<br>
task-cli delete 1

# Marking a task as in progress or done
task-cli mark-in-progress 1<br>
task-cli mark-done 1

# Listing all tasks
task-cli list

# Listing tasks by status
task-cli list done<br>
task-cli list todo<br>
task-cli list in-progress

Exercise can be found at:
https://roadmap.sh/projects/task-tracker
