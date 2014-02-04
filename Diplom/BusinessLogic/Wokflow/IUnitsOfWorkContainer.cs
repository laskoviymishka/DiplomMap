﻿using Investmogilev.Infrastructure.BusinessLogic.Wokflow.UnitsOfWork.Interfaces;

namespace Investmogilev.Infrastructure.BusinessLogic.Wokflow
{
    public interface IUnitsOfWorkContainer
    {
        IComissionFixesUoW ComissionFixesUoW { get; }
        IDocumentSendingUoW DocumentSendingUoW { get; }
        IDoneUoW DoneUoW { get; }
        IInvestorApproveUoW InvestorApproveUoW { get; }
        IInvolvedorganizationsUoW InvolvedorganizationsUoW { get; }
        IIspolcomFixesUoW IspolcomFixesUoW { get; }
        IMinEconomyUoW MinEconomyUoW { get; }
        IOnComissionUoW OnComissionUoW { get; }
        IOnIspolcomUoW OnIspolcomUoW { get; }
        IOnMapUoW OnMapUoW { get; }
        IOpenUoW OpenUoW { get; }
        IPlanCreatingUoW PlanCreatingUoW { get; }
        IRealizationUoW RealizationUoW { get; }
        IWaitComissionUoW WaitComissionUoW { get; }
        IWaitInvolvedUoW WaitInvolvedUoW { get; }
        IWaitIspolcomUoW WaitIspolcomUoW { get; }
    }
}