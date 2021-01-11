using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KAPatients.Controllers
{

    [Authorize(Roles="administrators")]
    public class KARoleController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public KARoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            this.userManager = userManager;
            this.roleManager = roleManager;

        }

        public IActionResult Index()
        {
            var roles = roleManager.Roles.OrderBy(a => a.Name); 
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string role)
        {
           
            try
            {
                role = role.Trim();

                if (String.IsNullOrWhiteSpace(role)) {

                    TempData["errorMessage"] = "The role cannot be empty";
                    return RedirectToAction(nameof(Index));

                }
                else if (await roleManager.RoleExistsAsync(role))
                {
                    TempData["errorMessage"] = "This role already exists. You cannot create another role with same name.";
                    return RedirectToAction(nameof(Index));

                }

                
                IdentityResult identityResult =
                await roleManager.CreateAsync(new IdentityRole(role));

                if (identityResult.Succeeded)
                {
                    TempData["message"] = "Role added- " + role;
                }

                    if (!identityResult.Succeeded)
                    throw new Exception(identityResult.Errors.FirstOrDefault().Description);

                //TempData["message"] = $"role created: '{role.Name}'";

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Exception occured while creating the role. Details: " + ex.GetBaseException().Message; 
                
            }


            //if (!await roleManager.RoleExistsAsync(role.Name))
            //{
            //    IdentityRole _role = new IdentityRole();
            //    _role.Name = role.Name;
            //    IdentityResult identityResult = await roleManager.CreateAsync(_role);
            //}

            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> Delete(string id)

        {
            
            string name = id;
            List<IdentityUser> usersInRole;


            if (string.IsNullOrWhiteSpace(name))//URL EMPTY
            {
                if(!string.IsNullOrWhiteSpace(HttpContext.Session.GetString("sessionRoleName")))
                    {

                    name = HttpContext.Session.GetString("sessionRoleName");

                }


            }

            else
            {
                HttpContext.Session.SetString("sessionRoleName", name);


            }

            if (name.ToLower() == "administrators")
            {
                TempData["errorMessage"] = "Administrators cannot be deleted";
                return RedirectToAction(nameof(Index));

            }

            else
            {
              usersInRole = new List<IdentityUser>();
                //List<IdentityUser> usersNotInRole = new List<IdentityUser>();

                foreach (var user in userManager.Users)
                {
                    int x = 0;
                    var userRoles = await userManager.GetRolesAsync(user);

                    foreach (var UR in userRoles)
                    {

                        if (UR == name)
                        {
                            x++;

                        }

                    }

                    if (x > 0)
                    {
                        usersInRole.Add(user);
                    }

                }

                if (!usersInRole.Any())
                {

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            if (await roleManager.RoleExistsAsync(id))
                            {
                                //IdentityRole roleToDelete = await roleManager.FindByNameAsync(name);
                                IdentityResult deleteResult = await roleManager.DeleteAsync(await roleManager.FindByNameAsync(name));

                                if (deleteResult.Succeeded)
                                {
                                    TempData["message"] = $"The role-{name} was successfully deleted";
                                    return RedirectToAction(nameof(Index));
                                }

                                else if ((! deleteResult.Succeeded))
                                {
                                    throw new Exception(deleteResult.Errors.FirstOrDefault().Description);
                                }

                            }

                            else
                            {
                                TempData["errorMessage"] = "Role donot exists";
                                return RedirectToAction(nameof(Index));

                            }
                        }

                        else
                        {
                            TempData["errorMessage"] = "Role cannot be empty";
                            return RedirectToAction(nameof(Index));

                        }

                    }
                    catch (Exception ex)
                    {
                        TempData["errorMessage"] = "Exception occured while creating the role. Details: " + ex.GetBaseException().Message;

                    }
        
                    //return RedirectToAction("DeleteConfirmed", new { id = name });
                 
                    //RedirectToAction(new { controller = "home", action = "index", id = 123});
                }
            }


            if (! await roleManager.RoleExistsAsync(name))
            {

                TempData["errorMessage"] = "Role donot exists";
                return RedirectToAction(nameof(Index));

            }

            IdentityRole roleToDelete = await roleManager.FindByNameAsync(name);

            TempData["roleToBeDeleted"] = name;

            return View(usersInRole);

        
        }








        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            if (string.IsNullOrWhiteSpace(id))
            {
                if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("sessionRoleName")))
                {

                    TempData["message"] = "Please select a role to delete";


                }

                else {
                    id = HttpContext.Session.GetString("sessionRoleName");
                }

            }

            List<IdentityUser> usersInRole = new List<IdentityUser>();

            foreach (var user in userManager.Users)
            {
                int x = 0;
                var userRoles = await userManager.GetRolesAsync(user);

                foreach (var UR in userRoles)
                {

                    if (UR == id)
                    {
                        x++;

                    }

                }

                if (x > 0)
                {
                    usersInRole.Add(user);
                }

            }

            if (!usersInRole.Any())
            {

                return RedirectToAction("DeleteConfirmed");
            }


            if (!string.IsNullOrWhiteSpace(id))
            {
                if (await roleManager.RoleExistsAsync(id))
                {
                    try
                    {
                        IdentityRole roleToDelete = await roleManager.FindByNameAsync(id);
                        IdentityResult delResult = await roleManager.DeleteAsync(roleToDelete);
                        if (delResult.Succeeded)
                        {

                            TempData["message"] = $"The role -{id} was succesfully deleted";
                        }

                        else if (! delResult.Succeeded)
                        {

                            throw new Exception(delResult.Errors.FirstOrDefault().Description);


                        }
                    }
                    catch (Exception ex)
                    {

                        TempData["errorMessage"] = "Exception occured while creating the role. Details: " + ex.GetBaseException().Message;

                    }



                }

                else
                {
                    TempData["errorMessage"] = "Role donot exists";

                }


            }

            else
            {
                TempData["errorMessage"] = "Role cannot be empty";

            }

            return RedirectToAction(nameof(Index));

        }



    }
}