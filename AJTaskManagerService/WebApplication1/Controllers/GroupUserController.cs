using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Twitter.Messages;
using WebApplication1.Common;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class GroupUserController : Controller
    {
        // GET: GroupUser
        public async Task<ActionResult> Index(string groupId)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var roleTypeService = new RoleTypeService(accessToken);
            var userDataService = new UserService(accessToken);
            var userGroupService = new UserGroupService(accessToken);

            var allRoleTypes = await roleTypeService.GetAllRoleTypes();
            var usersFromGroup = await userDataService.GetUsersFromGroup(groupId);
            List<GroupUserModel> guModelsList = new List<GroupUserModel>();
            foreach (var user in usersFromGroup)
            {
                var userGroup = await userGroupService.GetUserGroup(user.Id, groupId);
                GroupUserModel guModel = new GroupUserModel();
                guModel.UserGroupId = userGroup.Id;
                guModel.UserEmail = user.Email;
                guModel.RoleTypeId = userGroup.RoleTypeId;
                guModel.RoleName = allRoleTypes.Single(r => r.Id == userGroup.RoleTypeId).RoleName;
                guModelsList.Add(guModel);
            }

            ViewBag.GroupId = groupId;
            return View(guModelsList);
        }

        // GET: GroupUser/Details/5
        public async Task<ActionResult> Details(string userGroupId)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var roleTypeService = new RoleTypeService(accessToken);
            var userService = new UserService(accessToken);
            var userGroupService = new UserGroupService(accessToken);

            var allRoleTypes = await roleTypeService.GetAllRoleTypes();
            var userGroup = await userGroupService.GetUserGroupById(userGroupId);
            var user = await userService.GetUserById(userGroup.UserId);

            GroupUserModel guModel = new GroupUserModel();
            guModel.UserEmail = user.Email;
            guModel.RoleTypeId = userGroup.RoleTypeId;
            guModel.RoleName = allRoleTypes.Single(r => r.Id == userGroup.RoleTypeId).RoleName;
            guModel.UserGroupId = userGroup.Id;

            ViewBag.GroupId = userGroup.GroupId;
            return View(guModel);
        }

        // GET: GroupUser/Create
        public async Task<ActionResult> Create(string groupId)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var roleTypeService = new RoleTypeService(accessToken);
            var allRoleTypes = await roleTypeService.GetAllRoleTypes();
            var defaultRole = allRoleTypes.Single(r => r.RoleKey == (int)RoleTypeEnum.Viewer);
            ViewBag.RoleTypeId = new SelectList(allRoleTypes, "Id", "RoleName", defaultRole.Id);
            ViewBag.GroupId = groupId;

            GroupService groupService = new GroupService(accessToken);
            string groupName =await groupService.GroupNameByGroupId(groupId);

            if (await AccessAdmin(groupId) && !(groupName.Contains("Default group for user")))
            {
                return View();
            }
            else
            {
                return View("AccessDenied");
            }

            return View();
        }

        // POST: GroupUser/Create
        [HttpPost]
        public async Task<ActionResult> Create(string groupId, GroupUserModel groupUserModel)
        {
            try
            {
                string accessToken = Session["MicrosoftAccessToken"] as string;
                var userService = new UserService(accessToken);
                var user = await userService.GetUsersByEmail(groupUserModel.UserEmail);

                if (user == null)
                {
                    ModelState.AddModelError("Error", "User with this email does not exist!");
                    ViewBag.Error = TempData["Error"];
                    var roleTypeService = new RoleTypeService(accessToken);
                    var allRoleTypes = await roleTypeService.GetAllRoleTypes();
                    var defaultRole = allRoleTypes.Single(r => r.RoleKey == (int)RoleTypeEnum.Viewer);
                    ViewBag.RoleTypeId = new SelectList(allRoleTypes, "Id", "RoleName", defaultRole.Id);
                    ViewBag.GroupId = groupId;
                    return View(groupUserModel);
                }
                else
                {
                    UserGroup userGroup = new UserGroup
                    {
                        Id = Guid.NewGuid().ToString(),
                        GroupId = groupId,
                        RoleTypeId = groupUserModel.RoleTypeId,
                        UserId = user.Id,
                        IsUserDefaultGroup = false
                    };

                    var userGroupService = new UserGroupService(accessToken);
                    await userGroupService.Insert(userGroup);

                    return RedirectToAction("Index", new { groupId });
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: GroupUser/Edit/5
        public async Task<ActionResult> Edit(string userGroupId)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var roleTypeService = new RoleTypeService(accessToken);
            var userService = new UserService(accessToken);
            var userGroupService = new UserGroupService(accessToken);

            var allRoleTypes = await roleTypeService.GetAllRoleTypes();
            var userGroup = await userGroupService.GetUserGroupById(userGroupId);
            var user = await userService.GetUserById(userGroup.UserId);

            GroupUserModel guModel = new GroupUserModel();
            guModel.UserEmail = user.Email;
            guModel.RoleTypeId = userGroup.RoleTypeId;
            guModel.RoleName = allRoleTypes.Single(r => r.Id == userGroup.RoleTypeId).RoleName;
            guModel.UserGroupId = userGroup.Id;

            ViewBag.RoleTypeId = new SelectList(allRoleTypes, "Id", "RoleName", userGroup.RoleTypeId);

            ViewBag.GroupId = userGroup.GroupId;

            GroupService groupService = new GroupService(accessToken);
            string groupName = await groupService.GroupNameByGroupId(userGroup.GroupId);

            if (await AccessAdmin(userGroup.GroupId) && !userGroup.IsUserDefaultGroup)
            {
                return View(guModel);
            }
            else
            {
                return View("AccessDenied");
            }
        }

        

        // POST: GroupUser/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(string userGroupId, GroupUserModel groupUserModel)
        {
            try
            {
                string accessToken = Session["MicrosoftAccessToken"] as string;
                var userGroupService = new UserGroupService(accessToken);
                var userGroup = await userGroupService.GetUserGroupById(userGroupId);

                userGroup.RoleTypeId = groupUserModel.RoleTypeId;
                await userGroupService.Update(userGroup);
                return RedirectToAction("Index", new { groupId = userGroup.GroupId });
            }
            catch
            {
                return View();
            }
        }

        // GET: GroupUser/Delete/5
        public async Task<ActionResult> Delete(string userGroupId)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var roleTypeService = new RoleTypeService(accessToken);
            var userService = new UserService(accessToken);
            var userGroupService = new UserGroupService(accessToken);

            var allRoleTypes = await roleTypeService.GetAllRoleTypes();
            var userGroup = await userGroupService.GetUserGroupById(userGroupId);
            var user = await userService.GetUserById(userGroup.UserId);

            GroupUserModel guModel = new GroupUserModel();
            guModel.UserEmail = user.Email;
            guModel.RoleTypeId = userGroup.RoleTypeId;
            guModel.RoleName = allRoleTypes.Single(r => r.Id == userGroup.RoleTypeId).RoleName;
            guModel.UserGroupId = userGroup.Id;

            ViewBag.GroupId = userGroup.GroupId;

            GroupService groupService = new GroupService(accessToken);
            string groupName = await groupService.GroupNameByGroupId(userGroup.GroupId);

            if (await AccessAdmin(userGroup.GroupId) && !userGroup.IsUserDefaultGroup)
            {
                return View(guModel);
            }
            else
            {
                return View("AccessDenied");
            }


        }

        // POST: GroupUser/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string userGroupId, GroupUserModel groupUserModel)
        {
            try
            {
                string accessToken = Session["MicrosoftAccessToken"] as string;
                var userGroupService = new UserGroupService(accessToken);
                var userGroup = await userGroupService.GetUserGroupById(userGroupId);
                await userGroupService.DeleteUserGroup(userGroup.Id);
                return RedirectToAction("Index", new { groupId = userGroup.GroupId });
            }
            catch
            {
                return View();
            }

        }


        public async Task<bool> AccessEditor(string groupId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;

            UserService userService = new UserService(accessToken);
            string userId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserEditItem(userId, groupId);

        }

        public async Task<bool> AccessAdmin(string groupId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;

            UserService userService = new UserService(accessToken);
            string userId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserAddOrDeleteItem(userId, groupId);

        }
    }
}
