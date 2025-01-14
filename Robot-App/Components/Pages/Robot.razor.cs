using Microsoft.AspNetCore.Components;

namespace Robot_App.Components.Pages
{
    public partial class Robot
    {
        [Inject]
        public IBattery? BatteryService { get; set; }
        [Inject]
        public IStop? StopService { get; set; }
        [Inject]
        public ITask? TaskService { get; set; }
        private bool isTaskAdded = false;
        private bool isTaskRemoved = false;
        private TaskType taskType = new TaskType();

        protected override async Task OnInitializedAsync()
        {
            if (BatteryService != null)
            {
                await BatteryService.LoadBatteries();
                BatteryService.batteries = BatteryService.GetBattery();
            }
            else
            {
                throw new InvalidOperationException("BatteryService is not initialized.");
            }

            // load tasks
            if (TaskService != null)
            {
                TaskService.tasks = new List<TaskList>();
                await TaskService.LoadTasks();
                TaskService.tasks = TaskService.GetTasks();
                TaskService.taskTypes = new List<TaskType>();
                TaskService.taskTypes = TaskService.GetTaskTypes();
            }
            else
            {
                throw new InvalidOperationException("TaskService is not initialized.");
            }
        }

        //Handle tasktype form
        private async Task HandleValidSubmit()
        {
            isTaskAdded = true;
            await Task.Delay(3000);
            isTaskAdded = false;
            await TaskService!.InsertTaskType(taskType);
            taskType = new TaskType();
            RefreshPage();
        }

        private async Task HandleValidDelete(int typeId)
        {
           if (TaskService != null)
            {
                await TaskService.RemoveTaskType(typeId);
                isTaskRemoved = true;
                StateHasChanged();

                await Task.Delay(3000);
                isTaskRemoved = false;
                StateHasChanged(); 
                
                RefreshPage();
            }
            else
            {
            throw new InvalidOperationException("TaskService is not initialized.");
            }
        }

        private void RefreshPage()
        {
            NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
        }

    }
}
