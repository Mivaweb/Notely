﻿using Umbraco.Core.Models.Membership;

using Notely.Web.Models;
using Notely.Web.Avatar;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Implement extension methods for the <see cref="UserViewModel"/> class
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Convert the Membrship user to a notely <see cref="User"/> object
        /// </summary>
        /// <param name="user"></param>
        /// <param name="iuser"></param>
        /// <returns></returns>
        public static UserViewModel Convert(this UserViewModel user, IUser iuser)
        {
            // Check if a user is assigned
            if (user == null)
                return null;

            // Create a new UserViewModel
            return new UserViewModel()
            {
                Id = iuser.Id,
                Name = iuser.Name,
                AvatarUrl = UserAvatarProvider.GetAvatarUrl(iuser)
            };
        }
    }
}