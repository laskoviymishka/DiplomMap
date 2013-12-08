﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Notification;
using Telerik.Web.Mvc;
using Invest.Common.Model.Common;

namespace InvestPortal.Controllers
{
    public class ProfileController : Controller
    {
        private readonly PortalNotificationHub _notificationHub;

        public ProfileController()
        {
            _notificationHub = new PortalNotificationHub();
        }

        public ActionResult ProfilePartial()
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = _notificationHub.Notification(User.Identity.Name);
                return PartialView(new PortalNotification());
            }

            return PartialView();
        }

        public ActionResult MenuPartial()
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = _notificationHub.Notification(User.Identity.Name);
                return PartialView(new PortalNotification());
            }

            return PartialView();
        }
    }
}
