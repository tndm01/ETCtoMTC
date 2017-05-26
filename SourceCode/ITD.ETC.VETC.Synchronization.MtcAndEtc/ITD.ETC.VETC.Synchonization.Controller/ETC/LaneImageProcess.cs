using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;

namespace ITD.ETC.VETC.Synchonization.Controller.ETC
{
    public class LaneImageProcess
    {
        #region Fields
        //object file transfer ftp
        private FileTransferFtp _fileTransferFtp;
        public FileTransferFtp FileTransferFtp
        {
            get { return _fileTransferFtp; }
            set { _fileTransferFtp = value; }
        }
        //địa chỉ thư mục chứa hình mã làn ở local
        private string _localPath;
        public string LocalPath
        {
            get { return _localPath; }
            set { _localPath = value; }
        }
        //địa chỉ thư mục chứa hình mã làn ở server
        private string _remotePath;
        public string RemotePath
        {
            get { return _remotePath; }
            set { _remotePath = value; }
        }
        private bool _isFile;
        public bool IsFile
        {
            get { return _isFile; }
            set { _isFile = value; }
        }
        #endregion
    }
}
