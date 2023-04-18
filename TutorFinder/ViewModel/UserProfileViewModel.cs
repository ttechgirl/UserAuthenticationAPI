using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorFinder.Model;

namespace TutorFinder.ViewModel
{
    public class UserProfileViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }



        public static explicit operator UserProfileViewModel(RegisterViewModel source)
        {
            var destination = new UserProfileViewModel
            {
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                RoleId = source.RoleId

            };
            return destination;
        }

        public static explicit operator UserProfile(UserProfileViewModel source)
        {
            var destination = new UserProfile
            {
                FullName = source.FullName,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                UserId = source.UserId,
                RoleId = source.RoleId
            };
            return destination;
        }

        public static explicit operator UserProfileViewModel(UserProfile source)
        {
            var destination = new UserProfileViewModel
            {
                FullName = source.FullName,
                Email = source.Email,
                PhoneNumber = source.PhoneNumber,
                UserId = source.UserId,
                RoleId = source.RoleId

            };
            return destination;
        }

    }


}








