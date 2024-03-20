﻿using HalloDoc.BussinessAccess.Repository.Implementation;
using HalloDoc.DataAccess.Models;
using HalloDoc.DataAccess.ViewModel;
using HalloDoc.DataAccess.ViewModel.AdminViewModel;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.BussinessAccess.Repository.Interface
{
    public interface IAdminDashboardRepository
    {
        public int GetAdminId(string AspId);
        public AdminDashboardViewModel adminDashboard();

        public IEnumerable<newReqViewModel> newReq();
        public IEnumerable<pendingReqViewModel> pendingReq();
        public IEnumerable<activeReqViewModel> activeReq();
        public IEnumerable<concludeReqViewModel> concludeReq();
        public IEnumerable<closeReqViewModel> closeReq();
        public IEnumerable<unpaidReqViewModel> unpaidReq();

        public AdminDashboardViewModel searchPatient(searchViewModel obj, AdminDashboardViewModel data);

        public viewCaseViewModel viewCase(int reqClientId);

        public bool viewCase(viewCaseViewModel obj);

        public viewNoteViewModel ViewNoteGet(int reqClientId);

        public void ViewNotePost(int reqClientId, string adminNote, int adminId);

        public void CancelCase(int CaseTag, string addNote, int reqClientId, int adminId);

        public void BlockCase(int reqClientId, string addNote, int adminId);

        public object FilterPhysician(int Region);

        public void AssignCase(int reqClientId, string addNote, int PhysicianSelect, string RegionSelect, int adminId, string AspId);

        public DocumentViewModel ViewUpload(int reqClientId);

        public void DeleteFile(int ReqClientId, string FileName);

        public SendOrderViewModel SendOrders(int reqClientId);

        public void SendOrders(SendOrderViewModel obj, string AspId);

        public object FilterProfession(int ProfessionId);

        public object ShowVendorDetail(int selectVendor);

        public void ClearCase(int reqClientId);
        public CloseCaseViewModel CloseCase(int reqClientId);
        public void CloseCase(CloseCaseViewModel obj);
        public void CloseToUnpaidCase(int reqClientId);

        public Encounter Encounter(int reqClientId);
        public void Encounter(Encounter obj);

        public int GetStatus(int reqClientId);

        public Profile MyProfile(string AspId);
        public void MyProfile(Profile obj, string AspId);

        public ProviderViewModel Provider();

        public void StopNoty(int Physicianid);
        public ProviderViewModel FilterProvider(int RegionId);
    }
}
