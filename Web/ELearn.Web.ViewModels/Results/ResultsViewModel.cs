namespace ELearn.Web.ViewModels.Results
{
    using System.Collections.Generic;

    public class ResultsViewModel
    {
        public ResultsViewModel()
        {
            this.Results = new HashSet<ResultViewModel>();
        }

        public IEnumerable<ResultViewModel> Results { get; set; }

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