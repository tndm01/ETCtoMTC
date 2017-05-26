using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;

namespace ITD.ETC.VETC.Synchonization.Controller.ETC
{
    public class EtcProcessBase
    {
        #region Field

        private string _databaseTableName;

        private DataBaseHelper _mydatabaseHelper;

        private FileTransferFtp _fileTransferFtp;

        private string _localPath;

        private string _remotePath;
        public EtcProcessBase( string databaseTableName, string localPath, string remotePath)
        {
            _databaseTableName = databaseTableName;
            _localPath = localPath;
            _remotePath = remotePath;

            _mydatabaseHelper = DataBaseHelper.GetInstance();
            _fileTransferFtp = FileTransferFtp.GetInstance();
        }

        #endregion
    }
}
