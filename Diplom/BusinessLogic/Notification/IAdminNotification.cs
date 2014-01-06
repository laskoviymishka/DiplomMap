﻿using Invest.Common.Model.Project;

namespace BusinessLogic.Notification
{
    public interface IAdminNotification
    {
        void NotificateFill(Project project);
        void MapEntryNotificate();
        void NotificateReOpen();
        void InvestorApprovedNotificate(Project project);
        void InvestorResponsed(Project project);
        void DocumentUpdate(Project project);
        void WaitInvolved(Project project);

        void InvolvedOrganizationUpdate(Project project);

        void Comission(Comission comission, Project project);

        void WaitComission(Project project);

        void UpdateComissionFix(Project project);

        void WaitIspolcom(Project project);

        void OnIspolcom(Comission comission, Project project);
    }
}
