namespace ITD.ETC.VETC.Synchonization.Controller
{
    public class EnumAndConst
    {
        #region Const

        public const string STORE_GET_SYNC_DATA = "sp_SYNC_GetDataTracking";
        public const string STORE_UPDATE_SYNC_DATA = "sp_SYNC_Update_DataTrackingStatus";
        public const string STORE_ADD_TRP_tblCheckObuAccount_RFID = "sp_SYNC_InsertTRP_tblCheckObuAccount_RFID";
        public const string STORE_UPDATE_TRP_tblCheckObuAccount_RFID = "sp_SYNC_UpdateTRP_tblCheckObuAccount_RFID";
        public const string STORE_DELETE_TRP_tblCheckObuAccount_RFID = "sp_SYNC_DeleteTRP_tblCheckObuAccount_RFID";

        public const string STORE_ADD_EMPLOYYE = "sp_SYNC_InsertNhanvienTable";
        public const string STORE_ADD_VEHICLE = "sp_SYNC_InsertDataSoxeTable";

        
        public const string STORE_GET_IMAGE_DATA = "sp_SYNC_GetImageTrackingData";
        public const string STORE_UPDATE_IMAGE_DATA = "sp_SYNC_UpDateStatusImageTrackingData";
        
        public const string STORE_ADD_FORCEOPEN = "sp_SYNC_Insert_ForceOpen";
        public const string STORE_GET_JOBLIST = "sp_SYNC_GetAllJobList";
        public const string STORE_ADD_ETAGREGISTER = "sp_SYNC_InsertEtagThangTable";
        public const string STORE_ADD_SOATVE_BTC = "sp_SYNC_Insert_SoatVe_BTC";		
        public const string STORE_ADD_COMMUTER_TICKET_TRANSACTION = "sp_SYNC_InsertDatatoSoatVeThang_Qui";
        public const string STORE_ADD_TOLL_TICKET_TRANSACTION = "sp_SYNC_InsertDatatoSoatVeVangLai";

        public const string STORE_ADD_ACTIVE_TOLL_TICKET = "AddBlocTicket_Active";
        public const string STORE_DELETE_ACTIVE_TOLL_TICKET = "AddBlocTicket_Active_Delete";

        public const string STORE_ADD_COMMUTER_TICKET = "sp_SYNC_InsertDatatoVeThang_Qui";
        public const string STORE_UPDATE_COMMUTER_TICKET = "sp_SYNC_UpdateDatatoVeThang_Qui";
        public const string STORE_DELETE_COMMUTER_TICKET = "sp_SYNC_DeleteVeThangQui";

        public const string STORE_UPDATE_EMPLOYEE = "sp_SYNC_UpdateNhanvienTable";
        public const string STORE_DELETE_EMPLOYEE = "sp_SYNC_DeleteNhanvienTable";


        public const string STORE_UPDATE_ETAGREGISTER = "sp_SYNC_UpdateEtagThangTable";
        public const string STORE_DELETE_ETAGREGISTER = "sp_SYNC_DeleteEtagThangTable";


        public const string STORE_UPDATE_VEHICLE = "sp_SYNC_UpdateDataSoxeTable";
        public const string STORE_DELETE_VEHICLE = "sp_SYNC_DeleteDataSoxeTable";

        #endregion
    }
}
