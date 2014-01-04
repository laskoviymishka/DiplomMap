﻿using System.Collections.Generic;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    class PlanCreatingUoW : BaseProjectUoW, IPlanCreatingUoW
    {
        public PlanCreatingUoW(Project currentProject,
            IRepository repository,
            IUserNotification userNotification,
            IAdminNotification adminNotification,
            IInvestorNotification investorNotification,
            string userName,
            IEnumerable<string> roles)
            : base(currentProject,
           repository,
           userNotification,
           adminNotification,
           investorNotification,
           userName,
           roles)
        {
        }

        public void OnPlanCreatingExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnPlanCreatingEntry()
        {
            throw new System.NotImplementedException();
        }

        public bool CouldUpdatePlan()
        {
            return true;
        }

        public bool CouldApprovePlan()
        {
            return true;
        }
    }
}