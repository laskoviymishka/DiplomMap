﻿using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Notification;
using Invest.Common.Model.Project;
using Invest.Common.Repository;
using Invest.Common.State;
using Invest.Common.State.StateAttributes;

namespace BusinessLogic.Wokflow.UnitsOfWork.Realization
{
    [State(typeof (ProjectWorkflow.State), "test", ProjectStatesConstants.OnIspolcom)]
    internal class OnIspolcomUoW : BaseProjectUoW, IOnIspolcomUoW, IState
    {
        public OnIspolcomUoW(Project currentProject,
            IRepository repository,
            IUserNotification userNotification,
            IAdminNotification adminNotification,
            IInvestorNotification investorNotification,
            string userName,
            IEnumerable<string> roles)
            : this(new ProjectStateContext
            {
                UserNotification = userNotification,
                AdminNotification = adminNotification,
                InvestorNotification = investorNotification,
                CurrentProject = currentProject,
                Repository = repository,
                Roles = roles,
                UserName = userName
            })
        {
            if (CurrentProject != null)
            {
                if (currentProject.Responses == null)
                {
                    currentProject.Responses = new List<InvestorResponse>();
                }
            }
        }

        public OnIspolcomUoW(ProjectStateContext context)
            : base(context.CurrentProject,
                context.Repository,
                context.UserNotification,
                context.AdminNotification,
                context.InvestorNotification,
                context.UserName,
                context.Roles)
        {
            Context = context;
        }

        public void OnOnIspolcomExit()
        {
        }

        public void OnOnIspolcomEntry()
        {
            Comission comission =
                Repository.All<Comission>(c => c.CommissionTime > DateTime.Now && c.Type == ComissionType.Ispolcom)
                    .First();
            if (comission.ProjectIds == null)
            {
                comission.ProjectIds = new List<string>();
            }

            if (!comission.ProjectIds.Contains(CurrentProject._id))
            {
                comission.ProjectIds.Add(CurrentProject._id);
                Repository.Update(comission);
            }

            AdminNotification.OnIspolcom(comission, CurrentProject);
            InvestorNotification.OnIspolcom(comission, CurrentProject);
            ProcessMoving(ProjectWorkflow.State.OnIspolcom, "Переход в состояние на исполкоме");
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.ToMinEconomy, ProjectStatesConstants.OnIspolcom,
            ProjectStatesConstants.InMinEconomy)]
        public bool CouldToMinEconomy()
        {
            return Roles.Contains("Admin") &&
                   !CurrentProject.Tasks.Any(p => (p.Step == ProjectWorkflow.State.WaitIspolcomFixes && !p.IsComplete));
        }

        [Trigger(typeof (ProjectWorkflow.Trigger), typeof (ProjectWorkflow.State), "test",
            ProjectTriggersConstants.ToIspolcomFix, ProjectStatesConstants.OnIspolcom,
            ProjectStatesConstants.WaitIspolcomFixes)]
        public bool CouldToIspolcomFix()
        {
            return Roles.Contains("Admin") &&
                   CurrentProject.Tasks.Any(p => (p.Step == ProjectWorkflow.State.WaitIspolcomFixes && !p.IsComplete));
        }

        public IStateContext Context { get; set; }

        public void OnEntry()
        {
            OnOnIspolcomEntry();
        }

        public void OnExit()
        {
            OnOnIspolcomExit();
        }
    }
}