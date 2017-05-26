using System;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider
{
    public class MainProvider
    {
        #region Field

        private string connectionString;
        DataBaseHelper _dataBaseHelper;

        ConfigModel _myConfig;

        private FileTransferFtp _myFtpInstance;

        #endregion

        #region Property

        public DataBaseHelper DataBaseHelperInstance
        {
            get { return _dataBaseHelper; }
        }

        public ConfigModel ConfigInstance
        {
            get
            {
                return _myConfig;
            }
        }
        #endregion
        #region Constructor

        public MainProvider(string dbConnectionString)
        {
            try
            {
                _dataBaseHelper = new DataBaseHelper(dbConnectionString);
                _myConfig = ConfigModel.LoadConfig();
                _instance = this;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        public MainProvider(DataBaseHelper dataBase, ConfigModel config, FileTransferFtp ftp)
        {
            _dataBaseHelper = dataBase;
            _myConfig = config;
            _myFtpInstance = ftp;
            _instance = this;
        }
        #endregion

        #region Instance

        private static MainProvider _instance;

        public static MainProvider GetInstance()
        {
           return _instance;
        }
        #endregion
    }
}
