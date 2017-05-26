using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using ITD.ETC.VETC.Synchonization.Controller;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Model
{
    [DataContract(Namespace = "")]
    public class ConfigModel
    {
        private static string configFile = "config.xml";

        #region Property
        
        #region Database

        [DataMember(EmitDefaultValue = false)]
        public string DataBaseServer { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DatabaseName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DatabaseUser { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DatabaseUserPassword { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int DatabaseTimeOut { get; set; }

        #endregion

        #region FTP Server
        [DataMember(EmitDefaultValue = false)]
        public string FtpServerAddress { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int FtpServerPort { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FtpUserName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FtpPassword { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int FtpTimeout { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ServerPath { get; set; }

        #endregion

        #region MTC Job

        /// <summary>
        /// Main folder of MTC 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string MtcMainLoaclFolder { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string MtcMainFtpFolder { get; set; }
        /// <summary>
        /// Whether or not transfer file to FTP or local
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool MtcTranferMode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public ObservableCollection<JobInformationModel> MtcJobList { get; set; }

        #endregion

        #region ETC Job
        /// <summary>
        /// Main folder of ETC 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string EtcMainLocalFolder { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string EtcMainFtpFolder { get; set; }
        /// <summary>
        /// Whether or not transfer file to FTP or local
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool EtcTranferMode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        //public List<JobInformationModel> EtcJobList { get; set; }
        public ObservableCollection<JobInformationModel> EtcJobList { get; set; }
        #endregion

        #region Image
        [DataMember(EmitDefaultValue = false)]
        public string ImageLocalFolder { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ImageFtpFolder { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ImageLocalFolderFormat { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ImageFtpFolderFormat { get; set; }
        #endregion

        #region Server Mode
        [DataMember(EmitDefaultValue = false)]
        public bool IsAutoStart { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsManualStart { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsRunOnMtcServer { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsRunOnEtcServer { get; set; }
        #endregion

        #endregion

        #region Method

        /// <summary>
        /// Load config infor from XML file
        /// </summary>
        /// <returns></returns>
        public static ConfigModel LoadConfig()
        {

            ConfigModel _mConfig = null;
            try
            {
                if (File.Exists(configFile))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(configFile);
                    string xml = xmlDoc.InnerXml;
                    _mConfig = Utility.DeserialiseXmlLocal<ConfigModel>(xml);
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
            return _mConfig;
        }

        /// <summary>
        /// Save config infor to XML file
        /// </summary>
        /// <param name="pConfigModel"></param>
        /// <returns></returns>
        public static bool SaveConfig(ConfigModel pConfigModel)
        {
            try
            {
                // Save Xml file
                //encryptionConfig(pConfigModel);
                string xml = Utility.SerializeXml<ConfigModel>(pConfigModel);
                XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.CreateComment(xml);
                xmlDoc.LoadXml(xml);
                xmlDoc.Save(configFile);
                //_mGlobal.MyConfig = _mConfig;
                //MessageBox.Show("Lưu file thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Lưu file không thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        #endregion
    }
}
