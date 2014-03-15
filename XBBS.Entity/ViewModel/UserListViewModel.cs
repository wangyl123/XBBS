using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XBBS.Models.ViewModel
{
    public class UserListViewModel
    {
        [PetaPoco.Column(Name="uid")]
        public int Uid { get; set; }

        [PetaPoco.Column(Name="username")]
        public string Name { get; set; }

        [PetaPoco.Column(Name="email")]
        public string Email { get; set; }

        [PetaPoco.Column(Name="group_name")]
        public string RoleName { get; set; }

        [PetaPoco.Column(Name = "money")]
        public int Money { get; set; }
    }
}
