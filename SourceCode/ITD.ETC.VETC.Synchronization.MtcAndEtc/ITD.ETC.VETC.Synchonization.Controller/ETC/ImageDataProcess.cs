using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchonization.Controller.Objects;
using System.IO;

namespace ITD.ETC.VETC.Synchonization.Controller.ETC
{
    public class ImageDataProcess
    {
        private const string RECOG_FOLDER = "HinhND";
        private const string LANE_FOLDER = "HinhLan";
        private const string IMAGE_FORMAT = ".JPG";
        private const string FORMAT = "{0}/{1}/{2}/{3}";
        private const string LOCAL_FORMAT = "{0}/{1}/{2}/{3}";
        private const string DOWNLOAD_FORMAT = "{0}/{1}/{2}";
        private const string DOWNLOAD_LOCAL_FORMAT = "{0}/{1}/{2}";
        #region Field
        private string _databaseTableName;

        // object data base
        private DataBaseHelper _mydatabaseHelper;

        // object fileTransferFtp
        private FileTransferFtp _fileTransferFtp;

        // object eTag
        //private ETagTransactionModel _oETag;
        // local path
        private string _localPath;

        // remotepath
        private string _remotePath;

        // local datestring format
        private string _dateStringFormat;
        /// <summary>
        /// format for subfolder as date string
        /// </summary>
        string _serverDateStringFormat;

        private string _localRecogImagePath;
        private string _serverRecogImagePath;
        private string _localLaneImagePath;
        private string _serverLaneImagePath;

        public ImageDataProcess(string localPath, string remotePath, string dateStringFormat, string serverDateStringFormat)
        {
            _localPath = localPath;
            _remotePath = remotePath;
            _dateStringFormat = dateStringFormat;
            _serverDateStringFormat = serverDateStringFormat;
            _mydatabaseHelper = DataBaseHelper.GetInstance();
            _fileTransferFtp = FileTransferFtp.GetInstance();
        }

        #endregion
        public void ImageDownloadProcess()
        {
            try
            {
                DateTime now = DateTime.Now;
                string localFullPath = String.Format(DOWNLOAD_LOCAL_FORMAT, _localPath, RECOG_FOLDER, now.ToString(_dateStringFormat));
                string serverFullPath = String.Format(DOWNLOAD_FORMAT, _remotePath, RECOG_FOLDER, now.ToString(_dateStringFormat));
                //localFullPath += "\\";
                //localFullPath.Replace("/","\\");
                if (!Directory.Exists(localFullPath))
                {
                    Directory.CreateDirectory(localFullPath);
                }
                _fileTransferFtp.DownloadDirectory(localFullPath, serverFullPath);

                localFullPath = String.Format(DOWNLOAD_LOCAL_FORMAT, _localPath, LANE_FOLDER, now.ToString(_dateStringFormat));
                serverFullPath = String.Format(DOWNLOAD_FORMAT, _remotePath, LANE_FOLDER, now.ToString(_serverDateStringFormat));
                if (!Directory.Exists(localFullPath))
                {
                    Directory.CreateDirectory(localFullPath);
                }
                _fileTransferFtp.DownloadDirectory(localFullPath, serverFullPath);
            }
            catch(Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        public void ImageUploadProcess()
        {
            try
            {
                // Get image database tracking
                List<ImageDataTracking> listImages = getImageDataTracking();
                // find image on local
                
                foreach (var im in listImages)
                {
                    if (!string.IsNullOrEmpty(im.ImageID))
                    {
                        //DateTime date = Utility.GetDateTimefromTranID(im.ImageID);
                        //string localPath = _localPath + "/" + date.ToString(_dateStringFormat);
                        //string serverPath = _remotePath + "/" + date.ToString(_serverDateStringFormat);

                        // upload Image
                        bool result = uploadImage(im);

                        // Update status tracking data
                        if (result)
                        {
                            updateStatusData(im, 2);
                        }
                        else
                        {
                            if (im.ImageTrackingStatus == 0)
                                updateStatusData(im, 1);
                            else
                                updateStatusData(im, 2);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void updateStatusData(ImageDataTracking im, int status)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_UPDATE_IMAGE_DATA;
                cmd.Parameters.Add("@TrackingId", SqlDbType.BigInt).Value = im.ImageTrackingID;
                cmd.Parameters.Add("@SyncStatus", SqlDbType.Int).Value = status;
                _mydatabaseHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private bool uploadImage( ImageDataTracking image)
        {
            bool result = false;
            try
            {
                DateTime date = Utility.GetDateTimefromTranID(image.ImageID);
                string imagFileName = image.ImageID + "_" + image.LaneID + IMAGE_FORMAT;
                string localFullPath = String.Format(LOCAL_FORMAT, _localPath, RECOG_FOLDER, date.ToString(_dateStringFormat), image.LaneID);
                string serverFullPath = String.Format(FORMAT,_remotePath , RECOG_FOLDER , date.ToString(_serverDateStringFormat) , image.LaneID);
                
                // Upload recog image
                result = _fileTransferFtp.UploadFile(localFullPath, serverFullPath, imagFileName);
                if (result)
                {
                    // upload LaneImage
                    localFullPath = String.Format(LOCAL_FORMAT,_localPath , LANE_FOLDER , date.ToString(_dateStringFormat) , image.LaneID);
                    serverFullPath =String.Format(FORMAT, _remotePath , LANE_FOLDER , date.ToString(_serverDateStringFormat) , image.LaneID);
                    result = _fileTransferFtp.UploadFile(localFullPath, serverFullPath, imagFileName);
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
            return result;
        }
        private List<ImageDataTracking> getImageDataTracking()
        {
            List<ImageDataTracking> listImage = new List<ImageDataTracking>();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = EnumAndConst.STORE_GET_IMAGE_DATA;
                //cmd.Parameters.Add("@TrackingId", SqlDbType.BigInt).Value = im.ImageTrackingID;

                DataTable dt =  _mydatabaseHelper.GetDataTable(cmd);


                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ImageDataTracking image = new ImageDataTracking();
                        image.ImageID = row["ImageID"].ToString();
                        image.ImageTrackingID = (long)row["TrackingID"];
                        image.LaneID = row["LaneID"].ToString();
                        image.ImageTrackingStatus = (int)row["SyncStatus"];
                        listImage.Add(image);
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }

            return listImage;
        }
    }
}
