using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Common;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class GroupController : Controller
    {
        // GET: Group
        public async Task<ActionResult> Index()
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;

            if (String.IsNullOrWhiteSpace(accessToken))
                return RedirectToAction("Login", "Account");     

            var groupService = new GroupService(accessToken);
            var groupsForUser = await groupService.GetGroupsAvailableForUser(Session["UserId"] as string);
            var groups = groupsForUser.Select(g => GetGroupModel(g));
            return View(groups);
        }

        // GET: Group/Details/5
        public async Task<ActionResult> Details(string id)
        {
            return RedirectToAction("Index", "GroupUser", new { groupId = id });

        }

        // GET: Group/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: Group/Create
        [HttpPost]
        public async Task<ActionResult> Create(GroupModel groupModel)
        {
            try
            {
                string accessToken = Session["MicrosoftAccessToken"] as string;
                var groupService = new GroupService(accessToken);
                var group = GetGroupFromModel(groupModel);
                var userService = new UserService(accessToken);
                var user = await userService.GetUser(Session["UserId"] as string, UserDomainEnum.Microsoft);
                await groupService.AddUserGroup(group, user);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        // GET: Group/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var groupService = new GroupService(accessToken);
            var group = await groupService.GetGroupById(id);
            var groupModel = GetGroupModel(group);
            

            if (await AccessAdmin(id) && !(group.GroupNameTruncated.Contains("Default group for user")))
            {
                return View(groupModel);
            }
            else if (await AccessEditor(id) && !(group.GroupNameTruncated.Contains("Default group for user")))
            {
                return View(groupModel);
            }
            else
            {
                return View("AccessDenied");
            }


        }

        // POST: Group/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(string id, GroupModel groupModel)
        {
            try
            {
                string accessToken = Session["MicrosoftAccessToken"] as string;
                var groupService = new GroupService(accessToken);
                var group = await groupService.GetGroupById(id);
                await groupService.UpdateGroup(group);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Group/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var groupService = new GroupService(accessToken);
            var group = await groupService.GetGroupById(id);
            var groupModel = GetGroupModel(group);

            
          

            //warunek o nazwie
            if (await AccessAdmin(id) && !(groupModel.GroupName.Contains("Default group for user")))
            {
                return View(groupModel);
            }
            else
            {
                return View("AccessDenied");
            }
            
        }

        // POST: Group/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string id, GroupModel groupModel)
        {
            try
            {
                string accessToken = Session["MicrosoftAccessToken"] as string;
                var groupService = new GroupService(accessToken);
                var group = await groupService.GetGroupById(id);
                await groupService.DeleteGroup(group);

                return RedirectToAction("Index");
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

        private Group GetGroupFromModel(GroupModel groupModel)
        {
            Group group = new Group();
            group.GroupName = groupModel.GroupName;
            group.Id = groupModel.Id;
            return group;

        }

        private GroupModel GetGroupModel(Group group)
        {
            GroupModel groupModel = new GroupModel();
            groupModel.Id = group.Id;
            groupModel.GroupName = group.GroupNameTruncated;
            return groupModel;

        }
    }
}
