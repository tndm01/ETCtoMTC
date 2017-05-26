using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITD.ETC.VETC.Synchonization.Controller;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using WinSCP;
using System.IO;


namespace ITD.ETC.VETC.Synchonization.Controller.Ftp
{
    public class FileTransferFtp
    {
        #region Fields
        /// <summary>
        /// mỗi đối tượng FileTransferFtp có thuộc tính _sessionOptions
        /// </summary>
        private SessionOptions _sessionOptions;
        public SessionOptions SessionOptions
        {
            get { return _sessionOptions; }
            set { _sessionOptions = value; }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Khởi tạo FileTransferFtp không tham số
        /// </summary>
        public FileTransferFtp()
        {
            _sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = "10.0.4.255",
                PortNumber = 21,
                UserName = "vietphan",
                Password = "vietphan",
                TimeoutInMilliseconds = 60000
            };
        }
        /// <summary>
        /// Khởi tạo FileTransferFtp có tham số
        /// </summary>
        /// <param name="address">địa chỉ host</param>
        /// <param name="portNumber">port</param>
        /// <param name="userName">tên đăng nhập</param>
        /// <param name="password">mật khẩu</param>
        /// <param name="timeOut">thời gian chờ đáp ứng của máy chủ</param>
        public FileTransferFtp(string address, int portNumber, string userName, string password, int timeOut)
        {
            _sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = address,
                PortNumber = portNumber,
                UserName = userName,
                Password = password,
                TimeoutInMilliseconds = timeOut
            };

            _instance = this;
        }
        #endregion

        #region Instance

        private static FileTransferFtp _instance;
        public static FileTransferFtp GetInstance()
        {
            return _instance;
        }
        #endregion
        /// <summary>
        /// hàm PathCheck kiểm tra đường dẫn là thư mục hay file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true nếu là thư mục, false nếu là file</returns>
        //public bool PathCheck(string path)
        //{
        //    FileAttributes attr = System.IO.File.GetAttributes(path);
        //    if (attr.HasFlag(FileAttributes.Directory))
        //        return true;
        //    else
        //        return false;
        //}
        /// <summary>
        /// hàm download thư mục
        /// </summary>
        /// <param name="localPath">đường dẫn local</param>
        /// <param name="remotePath">đường dẫn server</param>
        /// <returns>list string lưu danh sách những file mới tải về</returns>
        ///
        #region Download
        public List<string> DownloadDirectory(string localPath, string remotePath)
        {
            try
            {
                using(Session session = new Session())
                {
                    session.Open(_sessionOptions);

                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    SynchronizationResult synchronizationResult = session.SynchronizeDirectories(SynchronizationMode.Local, localPath,
                            remotePath, false, false, SynchronizationCriteria.Time, transferOptions);
                    synchronizationResult.Check();
                    TransferEventArgsCollection transferCollection = synchronizationResult.Downloads;

                    List<string> downloadList = new List<string>();
                    foreach (TransferEventArgs transfer in transferCollection)
                    {
                        downloadList.Add(transfer.Destination);
                        NLogHelper.Info("Successful " + transfer.Destination);
                    }
                    return downloadList;
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
                return null;
            }
        }
        /// <summary>
        /// hàm download file
        /// </summary>
        /// <param name="localPath">đường dẫn local</param>
        /// <param name="remotePath">đường dẫn server</param>
        /// <returns>trả về địa chỉ lưu file ở local</returns>
        public string DownloadFile(string localPath, string remotePath)
        {
            try
            {
                using (Session session = new Session())
                {
                    session.Open(_sessionOptions);

                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.GetFiles(remotePath, localPath, false, transferOptions);
                    transferResult.Check();
                    return localPath;
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
                return null;
            }
        }
        #endregion
        #region Upload
        /// <summary>
        /// hàm upload thư mục
        /// </summary>
        /// <param name="localPath">đường dẫn local</param>
        /// <param name="remotePath">đường dẫn server</param>
        /// <returns>list string lưu danh sách các file đã upload</returns>
        public List<string> UploadDirectory(string localPath, string remotePath)
        {
            try
            {
                using (Session session = new Session())
                {
                    session.Open(_sessionOptions);

                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                    if (!session.FileExists(remotePath))
                    {
                        session.CreateDirectory(remotePath);
                    }
                    SynchronizationResult synchronizationResult = session.SynchronizeDirectories(SynchronizationMode.Remote, localPath,
                            remotePath, false, false, SynchronizationCriteria.Time, transferOptions);
                    synchronizationResult.Check();
                    TransferEventArgsCollection transferCollection = synchronizationResult.Uploads;
                    List<string> uploadList = new List<string>();
                    foreach (TransferEventArgs transfer in transferCollection)
                    {
                        uploadList.Add(transfer.FileName);
                        NLogHelper.Info("Successful " + transfer.FileName);
                    }
                    return uploadList;
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
                return null;
            }
        }
        /// <summary>
        /// hàm upload file
        /// </summary>
        /// <param name="localPath">đường dẫn local</param>
        /// <param name="remotePath">đường dẫn trên server</param>
        /// <returns>địa chỉ file đã up trên server</returns>
        public string UploadFile(string localPath, string remotePath)
        {
            try
            {
                using (Session session = new Session())
                {
                    session.Open(_sessionOptions);

                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;
                    TransferOperationResult transferResult = session.PutFiles(localPath, remotePath, false, transferOptions);
                    transferResult.Check();
                    return remotePath;
                }
            }
            catch(Exception ex)
            {
                NLogHelper.Error(ex);
                return null;
            }
        }

        public bool UploadFile(string localPath, string remotePath, string fileName)
        {
            bool result = false;
            try
            {
                using (Session session = new Session())
                {
                    session.Open(_sessionOptions);

                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    // Check Exits
                    if (!session.FileExists(remotePath))
                    {
                        session.CreateDirectory(remotePath);
                    }

                    localPath += "/" + fileName;
                    remotePath += "/" + fileName;

                    TransferOperationResult transferResult = session.PutFiles(localPath, remotePath, false, transferOptions);
                    transferResult.Check();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);

            }

            return result;
        }

        #endregion
        /// <summary>
        /// Hàm kiểm tra thư mục trên server có tồn tại hay không
        /// </summary>
        /// <param name="path">địa chỉ cần kiểm tra</param>
        /// <returns>true nếu tồn tại, false không tồn tại</returns>
        //public bool RemoteDirectoryExist(string path)
        //{
        //    try
        //    {
        //        using (Session session = new Session())
        //        {
        //            session.Open(_sessionOptions);
        //            if (session.FileExists(path))
        //                return true;
        //            else
        //                return false;
        //            //RemoteDirectoryInfo directory = session.ListDirectory(remotePath);
        //            //var dr = directory.Files.FirstOrDefault(f => f.IsDirectory == true && f.Name == subPath);
        //            //if (dr != null)
        //            //    return true;
        //            //else
        //            //    return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogHelper.Error(ex);
        //        return false;
        //    }
        //}
        /// <summary>
        /// Hàm tạo thư mục trên server
        /// </summary>
        /// <param name="path">địa chỉ cần tạo</param>
        //public void CreateRemoteDirectory(string path)
        //{
        //    try
        //    {
        //        using (Session session = new Session())
        //        {
        //            session.Open(_sessionOptions);
        //            session.CreateDirectory(path);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        NLogHelper.Error(ex);
        //    }
        //}
    }
}
