using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KAPatients.Controllers
{
    [Authorize(Roles = "administrators")]

    public class KAUsersController : Controller
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public KAUsersController(
              UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

       [HttpGet]
        public async Task<IActionResult> Index(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {

                    TempData["errorMessage"] = "Role donot exists";
                    return RedirectToAction("Index", "KARole");

                }
            }

            if (!string.IsNullOrWhiteSpace(roleName))
            {
                HttpContext.Session.SetString("sessionRole", roleName);

            }
            else if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("sessionRole")))
            {
                roleName = HttpContext.Session.GetString("sessionRole");

            }
            else
            {
                TempData["errorMessage"] = "Please select a role by clicking against the userlist near the role ";
                RedirectToAction("Index", "KARole");
            }

            //var users = userManager.Users.OrderBy(a => a.UserName);
            List<IdentityUser> usersInRole = new List<IdentityUser>();
            List<IdentityUser> usersNotInRole = new List<IdentityUser>();
            List<IdentityUser> usersNotInRole2 = new List<IdentityUser>();

            foreach (var user in userManager.Users)
            {
                var userRoles = await userManager.GetRolesAsync(user);

                if(! userRoles.Any())
                {
                    usersNotInRole.Add(user);

                }

                else
                {
                    foreach (var role in userRoles)
                    {
                        int x = 0;
                        if (role == roleName)
                        {
                            usersInRole.Add(user);
                            usersNotInRole.Remove(user);
                            x++;

                        }
                        else if (x == 0)
                        {
                            usersNotInRole.Add(user);
                            x++;



                        }


                    }

                }

             

             

            }

           
                foreach(var user in usersInRole)
                {


                    usersNotInRole.Remove(user);
                }

            usersNotInRole2 = usersNotInRole.Except(usersInRole).ToList();
       

            //ViewBag.UserName = new SelectList(userManager.Users.OrderBy(a => a.UserName), "UserName", "UserName");
            ViewBag.Id  = new SelectList(usersNotInRole2.Distinct().OrderBy(a => a.UserName), "Id", "UserName");

          


            TempData["role"] = roleName;
            return View(usersInRole.Distinct().OrderBy(a=>a.UserName));
        }

        [HttpPost]
        //[validateantiforgerytoken]
        public async Task<IActionResult> Index(string Id, string value)
        {
            try
            {
                IdentityUser user = await userManager.FindByIdAsync(Id);
                IdentityResult addResult = await userManager.AddToRoleAsync(user, HttpContext.Session.GetString("sessionRole"));

                if (!addResult.Succeeded)
                    throw new Exception(addResult.Errors.FirstOrDefault().Description);

                else
                { TempData["message"] = $"The user- {user.UserName} was sucessfully ADDED to {HttpContext.Session.GetString("sessionRole")} role "; }



                return RedirectToAction(nameof(Index));
                    

            }
            catch (Exception ex)
            {

                TempData["errorMessage"] = $"Exception occured while adding the user. Details: {ex.GetBaseException().Message}";
                return RedirectToAction(nameof(Index));

            }



        }



        public async Task<IActionResult> Delete(string id)

        {
            try
            {
                IdentityUser user = await userManager.FindByIdAsync(id);


                IdentityResult removeResult = await userManager.RemoveFromRoleAsync(user, HttpContext.Session.GetString("sessionRole"));

                if (!removeResult.Succeeded)
                    throw new Exception(removeResult.Errors.FirstOrDefault().Description);

                else
                    TempData["message"] = $"The user-{user.UserName} has been succesfully removed from the role {HttpContext.Session.GetString("sessionRole")} ";

                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Exception occured while removing the user. Details: {ex.GetBaseException().Message}";
                return RedirectToAction(nameof(Index), new { roleName = id });

            }


        



        }

    }
}   