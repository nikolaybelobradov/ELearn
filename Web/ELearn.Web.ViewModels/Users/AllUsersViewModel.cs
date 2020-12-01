namespace ELearn.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using ELearn.Data.Models;
    using ELearn.Services.Mapping;

    public class AllUsersViewModel
    {
        public AllUsersViewModel()
        {
            this.Users = new HashSet<UserViewModel>();
        }

        public IEnumerable<UserViewModel> Users { get; set; }

        public string Keyword { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public int NextPage
        {
            get
            {
                if (this.CurrentPage >= this.PagesCount)
                {
                    return 1;
                }

                return this.CurrentPage + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.CurrentPage <= 1)
                {
                    return this.PagesCount;
                }

                return this.CurrentPage - 1;
            }
        }
    }
}
