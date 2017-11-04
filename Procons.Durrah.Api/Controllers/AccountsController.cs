namespace Procons.Durrah.Controllers
{
    using Microsoft.AspNet.Identity;
    using Procons.Durrah.Api.Attributes;
    using Procons.Durrah.Auth;
    using Procons.Durrah.Common;
    using Procons.Durrah.Common.Models;
    using Procons.Durrah.Facade;
    using Procons.Durrah.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    [ApplicationExceptionFilter]
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        #region PRIVATE VARIABLES  
        LoginFacade loginFacade = new LoginFacade();
        #endregion

        [AllowAnonymous]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateUser(ApplicationUser user)
        {
            if (!base.GoogleReCaptcha(user.CaptchaCode))
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Verify Captcha!"); 

            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);
            //var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            var baseUrl = Utilities.GetConfigurationValue(Common.Constants.ConfigurationKeys.BaseUrl);
            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user);

            if (!addUserResult.Succeeded)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, addUserResult.Errors.FirstOrDefault());
                //return GetErrorResult(addUserResult);
            }
                
            else
            {
                idMessage.Destination = user.Email;
                idMessage.Subject = Utilities.GetResourceValue(Common.Constants.Resources.DurraEmailConfirmation);
                var result = loginFacade.GenerateCofnirmationToken(user.Email);

                var tokens = new Dictionary<string, string>();
                tokens.Add(Common.Constants.EmailKeys.BaseUrl, baseUrl);
                tokens.Add(Common.Constants.EmailKeys.Result, result);
                tokens.Add(Common.Constants.EmailKeys.Email, user.Email);

                if (result != string.Empty)
                {
                    var messageBody = Utilities.GetResourceValue(Common.Constants.Resources.ConfirmationBody).GetMessageBody(tokens);
                    idMessage.Body = messageBody;
                    emailService.SendAsync(idMessage);
                    return Request.CreateResponse(HttpStatusCode.OK, Utilities.GetResourceValue(Common.Constants.Resources.ConfirmationSent));
                    //return Ok(Utilities.GetResourceValue(Common.Constants.Resources.ConfirmationSent));
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, result); 
            }
        }

        [AllowAnonymous]
        [Route("confirmEmail")]
        public IHttpActionResult ConfirmEmail(ConfirmationModel model)
        {
            if (model.Email != null && model.ValidationId != null)
            {
              var result=  loginFacade.ConfirmEmail(model.ValidationId, model.Email);
                if (result)
                {
                    return Ok();
                }
                else
                    return InternalServerError();
            }
            return NotFound();
        }

        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset")]
        public IHttpActionResult ResetPassword([FromBody]PasswordModel model)
        {
            var loginFacade = new LoginFacade();
            var result = loginFacade.ResetPassword(model.Password, model.ValidationId, model.EmailAddress);
            if (result)
                return Ok();
            else
                return InternalServerError();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("resetrequest")]
        public IHttpActionResult RequestReset([FromBody]PasswordModel model)
        {
            //var baseUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            var baseUrl = Utilities.GetConfigurationValue(Common.Constants.ConfigurationKeys.BaseUrl);
            idMessage.Destination = model.EmailAddress;
            idMessage.Subject = Utilities.GetResourceValue(Common.Constants.Resources.DurraPasswordReset);
            var result = loginFacade.ResetRequest(model.EmailAddress);
            if (result != string.Empty)
            {
                var tokens = new Dictionary<string, string>();
                tokens.Add(Common.Constants.EmailKeys.BaseUrl, baseUrl);
                tokens.Add(Common.Constants.EmailKeys.Result, result);
                tokens.Add(Common.Constants.EmailKeys.Email, model.EmailAddress);

                var messageBody = Utilities.GetResourceValue(Common.Constants.Resources.PasswordResetBody).GetMessageBody(tokens);
                idMessage.Body = messageBody;
                emailService.SendAsync(idMessage);
                return Ok();
            }

            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }


        #region Private Methods

        #endregion

        #region Unused Methods
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var identity = User.Identity as System.Security.Claims.ClaimsIdentity;

            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }
        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();

        }
        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/assignclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignClaimsToUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToAssign)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToAssign)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }

                await this.AppUserManager.AddClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/removeclaims")]
        [HttpPut]
        public async Task<IHttpActionResult> RemoveClaimsFromUser([FromUri] string id, [FromBody] List<ClaimBindingModel> claimsToRemove)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            foreach (ClaimBindingModel claimModel in claimsToRemove)
            {
                if (appUser.Claims.Any(c => c.ClaimType == claimModel.Type))
                {
                    await this.AppUserManager.RemoveClaimAsync(id, ExtendedClaimsProvider.CreateClaim(claimModel.Type, claimModel.Value));
                }
            }

            return Ok();
        }

        #endregion
    }
}