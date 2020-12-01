namespace ELearn.Web.Areas.Administration.Controllers
{
    using ELearn.Common;
    using ELearn.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName + ","
        + GlobalConstants.LecturerRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
