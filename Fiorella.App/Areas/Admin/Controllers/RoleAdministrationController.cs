using Fiorella.App.Models;
using Fiorella.App.ViewModels.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.App.Areas.Admin.Controllers
{
    [Area("admin")]
    public class RoleAdministrationController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager) : Controller
    {
        private readonly RoleManager<AppRole> _roleManager = roleManager;
        private readonly UserManager<AppUser> _userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            List<AppRole> roles = await _roleManager.Roles.ToListAsync();
            return View("Roles", roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the role already exists
                bool roleExists = await _roleManager.RoleExistsAsync(roleModel.RoleName);
                if (roleExists)
                {
                    ModelState.AddModelError("", "Role Already Exists");
                }
                else
                {
                    // Create the role
                    // We just need to specify a unique role name to create a new role
                    AppRole identityRole = new()
                    {
                        Name = roleModel.RoleName,
                        Description = roleModel.Description,
                    };

                    // Saves the role in the underlying AspNetRoles table
                    IdentityResult result = await _roleManager.CreateAsync(identityRole);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ListRoles));
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(roleModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRole(int id)
        {
            // var role = await _roleManager.FindByIdAsync(model.Id); // demand id as string
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                // Handle the scenario when the role is not found
                return View("NotFound");
            }

            //Populate the EditRoleViewModel from the data retrived from the database
            var model = new UpdateRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name!,
                Description = role.Description,
                // You can add other properties here if needed
            };

            // Retrieve all the Users
            foreach (var user in await _userManager.Users.ToListAsync())
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. 
                // This model object is then passed to the view for display
                if (!string.IsNullOrEmpty(role.Name) && !string.IsNullOrEmpty(user.UserName))
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        model.Users.Add(user.UserName);
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var role = await _roleManager.FindByIdAsync(model.Id); // demand id as string
                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == model.Id);
                if (role == null)
                {
                    // Handle the scenario when the role is not found
                    ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.RoleName;
                    if (model.Description != null)
                    {
                        role.Description = model.Description;
                    }

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(ListRoles)); // Redirect to the roles list
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveRole(int id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                // Role not found, handle accordingly
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                // Role deletion successful
                return RedirectToAction(nameof(ListRoles)); // Redirect to the roles list page
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            // If we reach here, something went wrong, return to the view
            return RedirectToAction(nameof(ListRoles));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUsersInRole(int id)
        {
            ViewBag.roleId = id;

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            ViewBag.RoleName = role.Name;
            List<UserRoleViewModel> model = [];

            foreach (var user in await _userManager.Users.ToListAsync())
            {
                if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(role.Name))
                {
                    UserRoleViewModel userRoleViewModel = new()
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };

                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleViewModel.IsSelected = false;
                    }

                    model.Add(userRoleViewModel);
                }

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUsersInRole(List<UserRoleViewModel> model, int id)
        {
            //First check whether the Role Exists or not
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                // var user = await _userManager.FindByIdAsync(model[i].UserId);
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == model[i].UserId);

                if (user != null && !string.IsNullOrEmpty(role.Name))
                {
                    IdentityResult? result = null;

                    if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                    {
                        //If IsSelected is true and User is not already in this role, then add the user
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        //If IsSelected is false and User is already in this role, then remove the user
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        //Don't do anything simply continue the loop
                        continue;
                    }

                    //If you add or remove any user, please check the Succeeded of the IdentityResult
                    if (result.Succeeded)
                    {
                        if (i < (model.Count - 1))
                            continue;
                        else
                            return RedirectToAction(nameof(UpdateRole), new { id });
                    }
                }


            }

            return RedirectToAction(nameof(UpdateRole), new { id });
        }
    }
}
