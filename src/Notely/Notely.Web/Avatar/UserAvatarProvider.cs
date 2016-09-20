using System.IO;
using System.Web;

using Umbraco.Core.Models.Membership;
using Umbraco.Web;

namespace Notely.Web.Avatar
{
    public class UserAvatarProvider
    {
        public static string GetAvatarUrl(IUser user)
        {
            if (HttpContext.Current.Application["notely_user_" + user.Id] == null)
            {
                var value = string.Format("https://www.gravatar.com/avatar/{0}&s=40", GravatarHelper.HashEmailForGravatar(user.Email));

                HttpContext.Current.Application["notely_user_" + user.Id] = value;
            }

            return HttpContext.Current.Application["notely_user_" + user.Id].ToString();
        }
    }
}