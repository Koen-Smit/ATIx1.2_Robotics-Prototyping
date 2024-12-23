using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Robot_App.Repositories;

namespace Robot_App.Components.Pages
{
    public partial class Robot
    {
        [Inject]
        private IUserRepository? UserRepository { get; set; }

        public string Name { get; set; } = "Koen Smit";

        private bool isSubmitted = false;
        private UserFormModel model = new UserFormModel();
         private List<User> users = new List<User>();

        private void HandleValidSubmit()
        {
            var user = new User
            {
                Name = model.Naam,
                Age = model.Leeftijd,
                IsActive = model.IsActief
            };

            if (UserRepository != null)
            {
                UserRepository.InsertUser(user);
                isSubmitted = true;
                LoadUsers();
            }
        }


        protected override async Task OnInitializedAsync()
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (UserRepository != null)
            {
                users = UserRepository.GetUsers();
            }
        }

        public class UserFormModel
        {
            [Required(ErrorMessage = "Naam is verplicht")]
            public string? Naam { get; set; }

            [Range(18, 100, ErrorMessage = "Leeftijd moet tussen de 18 en 100 zijn")]
            public int Leeftijd { get; set; }

            public bool IsActief { get; set; }
        }
    }
}
