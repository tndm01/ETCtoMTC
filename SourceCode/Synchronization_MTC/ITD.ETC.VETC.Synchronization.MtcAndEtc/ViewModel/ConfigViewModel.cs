using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using Quartz.Util;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.ViewModel
{
    public class ConfigViewModel : ViewModelBase
    {
        #region Fields
        private ConfigModel _mConfig;
        #endregion

        #region Property
        #region Database

        private string _databaseServerName = string.Empty;
        public string DatabaseServerName
        {
            get { return _databaseServerName; }
            set
            {
                if (_databaseServerName != value)
                {
                    _databaseServerName = value;
                    OnPropertyChanged("DatabaseServerName");
                }
            }
        }

        private string _databaseName = string.Empty;
        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {
                if (_databaseName != value)
                {
                    _databaseName = value;
                    OnPropertyChanged("DatabaseName");
                }
            }
        }

        private string _databaseUser = string.Empty;
        public string DatabaseUser
        {
            get { return _databaseUser; }
            set
            {
                if (_databaseUser != value)
                {
                    _databaseUser = value;
                    OnPropertyChanged("DatabaseUser");
                }
            }
        }

        private string _databaseUserPassword = string.Empty;
        public string DatabaseUserPassword
        {
            get { return _databaseUserPassword; }
            set
            {
                if (_databaseUserPassword != value)
                {
                    _databaseUserPassword = value;
                    OnPropertyChanged("DatabaseUserPassword");
                }
            }
        }

        private int _databaseTimeout;
        public int DatabaseTimeout
        {
            get { return _databaseTimeout; }
            set
            {
                if (_databaseTimeout != value)
                {
                    _databaseTimeout = value;
                    OnPropertyChanged("DatabaseTimeout");
                }
            }
        }
        #endregion

        #region FTP Server
        private string _serverAddress = string.Empty;
        public string ServerAddress
        {
            get { return _serverAddress; }
            set
            {
                if (_serverAddress != value)
                {
                    _serverAddress = value;
                    OnPropertyChanged("ServerAddress");
                }
            }
        }

        private int _serverPort;
        public int ServerPort
        {
            get { return _serverPort; }
            set
            {
                if (_serverPort != value)
                {
                    _serverPort = value;
                    OnPropertyChanged("ServerPort");
                }
            }
        }

        private string _ftpUserName = string.Empty;
        public string FtpUserName
        {
            get { return _ftpUserName; }
            set
            {
                if (_ftpUserName != value)
                {
                    _ftpUserName = value;
                    OnPropertyChanged("FtpUserName");
                }
            }
        }

        private string _ftpPassword = string.Empty;
        public string FtpPassword
        {
            get { return _ftpPassword; }
            set
            {
                if (_ftpPassword != value)
                {
                    _ftpPassword = value;
                    OnPropertyChanged("FtpPassword");
                }
            }
        }

        private int _ftpTimeout = 40000;
        public int FtpTimeOut
        {
            get { return _ftpTimeout; }
            set
            {
                if (_ftpTimeout != value)
                {
                    _ftpTimeout = value;
                    OnPropertyChanged("FtpTimeOut");
                }
            }
        }

        private string _serverPath = string.Empty;
        public string ServerPath
        {
            get { return _serverPath; }
            set
            {
                if (_serverPath != value)
                {
                    _serverPath = value;
                    OnPropertyChanged("ServerPath");
                }
            }
        }

        //private bool _isAutoRun = true;

        //public bool IsAutoRun
        //{
        //    get { return _isAutoRun; }
        //    set
        //    {
        //        if (_isAutoRun != value)
        //        {
        //            _isAutoRun = value;
        //            OnPropertyChanged("IsAutoRun");
        //        }
        //    }
        //}
        #endregion

        #region MTC
        private string _mtcMainLoaclFolder;

        public string MtcMainLocalFolder
        {
            get { return _mtcMainLoaclFolder; }
            set
            {
                if (_mtcMainLoaclFolder != value)
                {
                    _mtcMainLoaclFolder = value;
                    OnPropertyChanged("MtcMainLocalFolder");
                    updateMtcJobLocalPath();
                }
            }
        }

        private string _mtcMainFtpFolder;

        public string MtcMainFtpFolder
        {
            get { return _mtcMainFtpFolder; }
            set
            {

                if (_mtcMainFtpFolder != value)
                {
                    _mtcMainFtpFolder = value;
                    OnPropertyChanged("MtcMainFtpFolder");
                    updateMtcJobFtpPath();
                }
            }
        }
        private bool _mtcTranferMode;

        public bool MtcTranferMode
        {
            get { return _mtcTranferMode; }
            set
            {
                
                if (_mtcTranferMode != value)
                {
                    _mtcTranferMode = value;
                    OnPropertyChanged("MtcTranferMode");
                }
            }
        }
        private ObservableCollection<JobInformationModel> _mtcJobList;
        public ObservableCollection<JobInformationModel> MtcJobList
        {
            get { return _mtcJobList; }
            set
            {

                if (_mtcJobList != value)
                { 
                    _mtcJobList = value;
                    OnPropertyChanged("MtcJobList");
                }
            }
        }
        private JobInformationModel _selectedMtcJob;
        public JobInformationModel SelectedMtcJob
        {
            get { return _selectedMtcJob; }
            set
            {
                if (_selectedMtcJob != value)
                {
                    _selectedMtcJob = value;
                    OnPropertyChanged("SelectedMtcJob");
                }
            }
        }

        #endregion

        #region ETC Job

        private string _etcMainLocalFolder;

        public string EtcMainLocalFolder
        {
            get { return _etcMainLocalFolder; }
            set
            {

                if (_etcMainLocalFolder != value)
                {
                    _etcMainLocalFolder = value;
                    OnPropertyChanged("EtcMainLocalFolder");
                    updateEtcJobLocalPath();
                }
            }
        }
        

        private string _etcMainFtpFolder;
        public string EtcMainFtpFolder
        {
            get { return _etcMainFtpFolder; }
            set
            {
                
                if (_etcMainFtpFolder != value)
                {
                    _etcMainFtpFolder = value;
                    OnPropertyChanged("EtcMainFtpFolder");
                    updateEtcJobFtpPath();
                }
            }
        }
        

        private bool _etcTranferMode;
        public bool EtcTranferMode
        {
            get { return _etcTranferMode; }
            set
            {
               
                if (_etcTranferMode != value)
                { 
                    _etcTranferMode = value;
                    OnPropertyChanged("EtcTranferMode");
                }
            }
        }

        //private List<JobInformationModel> _etcJobList;
        private ObservableCollection<JobInformationModel> _etcJobList;
        //public List<JobInformationModel> EtcJobList
        public ObservableCollection<JobInformationModel> EtcJobList
        {
            get { return _etcJobList; }
            set
            {
                
                if (_etcJobList != value)
                {
                    _etcJobList = value;
                    OnPropertyChanged("EtcJobList");
                }
            }
        }

        private JobInformationModel _selectedEtcJob;
        public JobInformationModel SelectedEtcJob
        {
            get { return _selectedEtcJob; }
            set
            {
                if (_selectedEtcJob != value)
                {
                    _selectedEtcJob = value;
                    OnPropertyChanged("SelectedEtcJob");
                }
            }
        }
        #endregion

        #region Image
        private string _imageLocalFolder;
        public string ImageLocalFolder
        {
            get { return _imageLocalFolder; }
            set
            {
                if (_imageLocalFolder != value)
                {
                    _imageLocalFolder = value;
                    OnPropertyChanged("ImageLocalFolder");
                }
            }
        }

        private string _imageFtpFolder;
        public string ImageFtpFolder
        {
            get { return _imageFtpFolder; }
            set
            {
                if (_imageFtpFolder != value)
                {
                    _imageFtpFolder = value;
                    OnPropertyChanged("ImageFtpFolder");
                }
            }
        }

        private string _imageLocalFolderFormat;
        public string ImageLocalFolderFormat
        {
            get { return _imageLocalFolderFormat; }
            set
            {
                if (_imageLocalFolderFormat != value)
                {
                    _imageLocalFolderFormat = value;
                    OnPropertyChanged("ImageLocalFolderFormat");
                }

            }
        }

        private string _imageFtpFolderFormat;
        public string ImageFtpFolderFormat
        {
            get { return _imageFtpFolderFormat; }
            set
            {
                if (_imageFtpFolderFormat != value)
                {
                    _imageFtpFolderFormat = value;
                    OnPropertyChanged("ImageFtpFolderFormat");
                }
            }
        }

        #endregion

        #region ServerMode

        private bool _isAutoStart;

        public bool IsAutoStart
        {
            get { return _isAutoStart; }
            set
            {
                _isAutoStart = value;
                if (_isAutoStart != value)
                {
                    _isAutoStart = value;
                    OnPropertyChanged("IsAutoStart");
                }
            }
        }
        private bool _isManualStart;

        public bool IsManualStart
        {
            get { return _isManualStart; }
            set
            {
                if (_isManualStart != value)
                {
                    _isManualStart = value;
                    OnPropertyChanged("IsManualStart");
                }
            }
        }

        private bool _isRunMtcServer;

        public bool IsRunMtcServer
        {
            get { return _isRunMtcServer; }
            set
            {
                if (_isRunMtcServer != value)
                {
                    _isRunMtcServer = value;
                    OnPropertyChanged("IsRunMtcServer");
                }
            }
        }
        private bool _isRunEtcServer;
        public bool IsRunEtcServer
        {
            get { return _isRunEtcServer; }
            set
            {
                if (_isRunEtcServer != value)
                {
                    _isRunEtcServer = value;
                    OnPropertyChanged("IsRunEtcServer");
                }
            }
        }

        #endregion

        #endregion

        #region Constructor

        public ConfigViewModel()
        {
            if (_mtcJobList == null)
            {
                MtcJobList = new ObservableCollection<JobInformationModel>();
            }
            if (_etcJobList == null)
            {
                EtcJobList = new ObservableCollection<JobInformationModel>();
            }

            MtcJobList.CollectionChanged += MtcJobList_CollectionChanged;
            EtcJobList.CollectionChanged += EtcJobList_CollectionChanged;

            LoadConfigValue();
        }

        
        #endregion

        #region Command
        private ICommand _clickCheckDatabase;

        public ICommand ClickCheckDatabase
        {
            get
            {
                return _clickCheckDatabase ?? (_clickCheckDatabase = new RelayCommand(param => { CheckDatabase(); }, param => CanCheckDatabase()));
            }
        }

        private bool CanCheckDatabase()
        {
            return true;
        }

        private void CheckDatabase()
        {
            try
            {
                string connectionString = DataBaseHelper.GetConnectionString(_databaseServerName, DatabaseName, DatabaseUser,
                     _databaseUserPassword, _databaseTimeout.ToString());

                DataBaseHelper data = new DataBaseHelper(connectionString);
                bool ret = data.CheckOpenConnection();

                if (ret)
                {
                    MessageBox.Show("Kết nối thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Kết nối không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private ICommand _clickSaveButton;
        public ICommand ClickSaveButton
        {
            get
            {
                return _clickSaveButton ?? (_clickSaveButton = new RelayCommand(param => { SaveChangeConfig(); }, param => CanSaveConfig()));

            }
        }

        private bool CanSaveConfig()
        {
            return true;
        }

        private void SaveChangeConfig()
        {
            SaveConfigValue();
            //MessageBox.Show("Lưu thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private ICommand _cancelChangeConfig;

        public ICommand CancelChangeConfigCommand
        {
            get
            {
                return _cancelChangeConfig ?? (_cancelChangeConfig = new RelayCommandP<Window>(CancelChangeConfig, param => CanCancelConfig()));

            }
        }

        private bool CanCancelConfig()
        {
            return true;
        }

        private void CancelChangeConfig(Window configView)
        {
            // Close View
            if (configView != null)
            {
                configView.Close();
            }
        }

        // Add Mtc Job
        private ICommand _addMtcJob;

        public ICommand AddMtcJobCommand
        {
            get
            {
                return _addMtcJob ?? (_addMtcJob = new RelayCommand(param => { AddMtcJob(); }, param => CanAddMtcJob()));
            }
        }
        

        private bool CanAddMtcJob()
        {
            return true;
        }

        private void AddMtcJob()
        {
            try
            {

                if (string.IsNullOrEmpty(_mtcMainLoaclFolder))
                {
                    MessageBox.Show("Please config main local folder!", "Synchronization", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrEmpty(_mtcMainFtpFolder))
                {
                    MessageBox.Show("Please config main Ftp folder!", "Synchronization", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                JobInformationModel tempJob = new JobInformationModel();

                //List<JobInformationModel> jobList = new List<JobInformationModel>();

                //if (_mtcJobList != null)
                //{
                //    jobList.AddRange(_mtcJobList);
                //    _mtcJobList = null;
                //}
                //jobList.Add(tempJob);
                //MtcJobList = jobList;
                if (_mtcJobList == null)
                {
                    MtcJobList = new ObservableCollection<JobInformationModel>();
                }
                MtcJobList.Add(tempJob);
                SelectedMtcJob = tempJob;
                
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        // Delete Mtc Job
        private ICommand _deleteMtcJob;

        public ICommand DeleteMtcJobCommand
        {
            get
            {
                return _deleteMtcJob ?? (_deleteMtcJob = new RelayCommand(param => { DeleteMtcJob(); }, param => CanDelMtcJob()));
            }
        }


        private bool CanDelMtcJob()
        {
            return true;
        }

        private void DeleteMtcJob()
        {
            try
            {
                if (_mtcJobList != null && _selectedMtcJob != null && _mtcJobList.Contains(_selectedMtcJob))
                {
                    //_mtcJobList.Remove(_selectedMtcJob);
                    MtcJobList.Remove(_selectedMtcJob);
                }

                //List<JobInformationModel> jobList = new List<JobInformationModel>();

                //if (_mtcJobList != null)
                //{
                //    jobList.AddRange(_mtcJobList);
                //    _mtcJobList = null;
                //}
                //MtcJobList = jobList;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        // Add Etc Job
        private ICommand _addEtcJob;

        public ICommand AddEtcJobCommand
        {
            get
            {
                return _addEtcJob ?? (_addEtcJob = new RelayCommand(param => { AddEtcJob(); }, param => CanAddEtcJob()));
            }
        }


        private bool CanAddEtcJob()
        {
            return true;
        }

        private void AddEtcJob()
        {
            try
            {
                if (string.IsNullOrEmpty(_etcMainLocalFolder))
                {
                    MessageBox.Show("Please config main local folder!", "Synchronization", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (string.IsNullOrEmpty(_etcMainFtpFolder))
                {
                    MessageBox.Show("Please config main Ftp folder!", "Synchronization", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }


                JobInformationModel tempJob = new JobInformationModel();

                List<JobInformationModel> jobList = new List<JobInformationModel>();

                //if (_etcJobList != null)
                //{
                //    jobList.AddRange(_etcJobList);
                //    _etcJobList = null;
                //}
                //jobList.Add(tempJob);
                //EtcJobList = jobList;
                if (_etcJobList == null)
                {
                    EtcJobList = new ObservableCollection<JobInformationModel>();
                }
                EtcJobList.Add(tempJob);
                SelectedEtcJob = tempJob;

            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        // Delete Etc Job
        private ICommand _deleteEtcJob;

        public ICommand DeleteEtcJobCommand
        {
            get
            {
                return _deleteEtcJob ?? (_deleteEtcJob = new RelayCommand(param => { DeleteEtcJob(); }, param => CanDelEtcJob()));
            }
        }


        private bool CanDelEtcJob()
        {
            return true;
        }

        private void DeleteEtcJob()
        {
            try
            {
                if (_etcJobList != null && _selectedEtcJob != null && _etcJobList.Contains(_selectedEtcJob))
                {
                    //_etcJobList.Remove(_selectedEtcJob);
                    EtcJobList.Remove(_selectedEtcJob);
                }

                //List<JobInformationModel> jobList = new List<JobInformationModel>();

                //if (_etcJobList != null)
                //{
                //    jobList.AddRange(_etcJobList);
                //    _etcJobList = null;
                //}
                //EtcJobList = jobList;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private ICommand _browserMtcMainFolder;

        public ICommand BrowserMtcMainFolder
        {
            get
            {
                return _browserMtcMainFolder ?? (_browserMtcMainFolder = new RelayCommand(param => { BrowserMtcMainFolderPath(); }, param => CanBrowser()));

            }
        }

        private bool CanBrowser()
        {
            return true;
        }

        private void BrowserMtcMainFolderPath()
        {
            string folderPath = _mtcMainLoaclFolder;
            browserFolderPath(ref folderPath);
            MtcMainLocalFolder = folderPath;
        }

        private ICommand _browserImageFolder;

        public ICommand BrowserImageFolder
        {
            get
            {
                return _browserImageFolder ?? (_browserImageFolder = new RelayCommand(param => { BrowserImageFolderPath(); }, param => CanBrowser()));

            }
        }

        private void BrowserImageFolderPath()
        {
            string folderPath = _imageLocalFolder;
            browserFolderPath(ref folderPath);
            ImageLocalFolder = folderPath;
        }

        private ICommand _browserEtcMainFolder;

        public ICommand BrowserEtcMainFolder
        {
            get
            {
                return _browserEtcMainFolder ?? (_browserEtcMainFolder = new RelayCommand(param => { BrowserEtcMainFolderPath(); }, param => CanBrowser()));

            }
        }

        private void BrowserEtcMainFolderPath()
        {
            string folderPath = _etcMainLocalFolder;
            browserFolderPath(ref folderPath);
            EtcMainLocalFolder = folderPath;
        }
        #endregion

        #region Method
        private void LoadConfigValue()
        {
            _mConfig = ConfigModel.LoadConfig();
            
            getConfigValue();
        }

        private void getConfigValue()
        {
            if (_mConfig != null)
            {
                // Database
                _databaseServerName = _mConfig.DataBaseServer;
                _databaseName = _mConfig.DatabaseName;
                _databaseUser = _mConfig.DatabaseUser;
                _databaseUserPassword = _mConfig.DatabaseUserPassword;
                _databaseTimeout = _mConfig.DatabaseTimeOut;

                // FTP
                _serverAddress = _mConfig.FtpServerAddress;
                _serverPort = _mConfig.FtpServerPort;
                _ftpUserName = _mConfig.FtpUserName;
                _ftpPassword = _mConfig.FtpPassword;
                _ftpTimeout = _mConfig.FtpTimeout;
                _serverPath = _mConfig.ServerPath;
                //_isAutoStart = _mConfig.IsAutoStart;

                // MTC
               // _mtcJobList = _mConfig.MtcJobList;
                _mtcMainLoaclFolder = _mConfig.MtcMainLoaclFolder;
                _mtcMainFtpFolder = _mConfig.MtcMainFtpFolder;
                _mtcTranferMode = _mConfig.MtcTranferMode;
                //_mtcJobList = _mConfig.MtcJobList;
                if (_mConfig.MtcJobList != null)
                {
                    foreach (var lt in _mConfig.MtcJobList)
                    {
                        MtcJobList.Add(lt);
                    }
                }

                // ETC                                             
                //_etcJobList = _mConfig.EtcJobList;
                _etcMainLocalFolder = _mConfig.EtcMainLocalFolder;
                _etcMainFtpFolder = _mConfig.EtcMainFtpFolder;
                _etcTranferMode = _mConfig.EtcTranferMode;
                //_etcJobList = _mConfig.EtcJobList;
                if (_mConfig.EtcJobList != null)
                {
                    foreach (var lt in _mConfig.EtcJobList)
                    {
                        EtcJobList.Add(lt);
                    }
                }

                // Image
                _imageLocalFolder = _mConfig.ImageLocalFolder;
                _imageFtpFolder = _mConfig.ImageFtpFolder;
                _imageLocalFolderFormat = _mConfig.ImageLocalFolderFormat;
                _imageFtpFolderFormat = _mConfig.ImageFtpFolderFormat;

                // Server
                _isAutoStart = _mConfig.IsAutoStart;
                _isManualStart = _mConfig.IsManualStart;
                _isRunEtcServer = _mConfig.IsRunOnEtcServer;
                _isRunMtcServer = _mConfig.IsRunOnMtcServer;
            }
        }

        private void SaveConfigValue()
        {
            if (_mConfig == null)
            {
                _mConfig = new ConfigModel();
            }

            updateConfigValue();
           
            

            if (ConfigModel.SaveConfig(_mConfig))
            {
                MessageBox.Show("Lưu file thành công!", "Sync FE", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Lưu file không thành công!", "Sync FE", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void updateConfigValue()
        {
            _mConfig.DataBaseServer = _databaseServerName;
            _mConfig.DatabaseName = _databaseName;
            _mConfig.DatabaseUser = _databaseUser;
            _mConfig.DatabaseUserPassword = _databaseUserPassword;
            _mConfig.DatabaseTimeOut = _databaseTimeout;


            _mConfig.FtpServerAddress = _serverAddress;
            _mConfig.FtpServerPort = _serverPort;
            _mConfig.FtpUserName = _ftpUserName;
            _mConfig.FtpPassword = _ftpPassword;
            _mConfig.FtpTimeout = _ftpTimeout;
            _mConfig.ServerPath = _serverPath;
            //_mConfig.IsAutoStart = _isAutoStart;

            // MTC
            _mConfig.MtcJobList = _mtcJobList;
            _mConfig.MtcMainLoaclFolder = _mtcMainLoaclFolder;
            _mConfig.MtcMainFtpFolder = _mtcMainFtpFolder;
            _mConfig.MtcTranferMode = _mtcTranferMode;

            // ETC
            _mConfig.EtcJobList = _etcJobList;
            _mConfig.EtcMainLocalFolder = _etcMainLocalFolder;
            _mConfig.EtcMainFtpFolder = _etcMainFtpFolder;
            _mConfig.EtcTranferMode = _etcTranferMode;

            // Image
            _mConfig.ImageLocalFolder = _imageLocalFolder;
            _mConfig.ImageFtpFolder = _imageFtpFolder;
            _mConfig.ImageLocalFolderFormat = _imageLocalFolderFormat;
            _mConfig.ImageFtpFolderFormat = _imageFtpFolderFormat;

            // Server mode
            _mConfig.IsAutoStart = _isAutoStart;
            _mConfig.IsManualStart = _isManualStart;
            _mConfig.IsRunOnEtcServer = _isRunEtcServer;
            _mConfig.IsRunOnMtcServer = _isRunMtcServer;

        }

        private void EtcJobList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {

                if (e.OldItems != null)
                    foreach (JobInformationModel oldItem in e.OldItems)
                        oldItem.PropertyChanged -= EtcJob_PropertyChanged;

                if (e.NewItems != null)
                    foreach (JobInformationModel newItem in e.NewItems)
                        newItem.PropertyChanged += EtcJob_PropertyChanged;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void EtcJob_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "SourcePath")
                {
                    if (string.IsNullOrEmpty(_etcMainLocalFolder))
                    {
                        MessageBox.Show("Please config main local folder!", "Synchronization", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        return;
                    }

                    if (string.IsNullOrEmpty(_etcMainFtpFolder))
                    {
                        MessageBox.Show("Please config main Ftp folder!", "Synchronization", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                        return;
                    }

                    JobInformationModel job = sender as JobInformationModel;
                    if (job != null)
                    {
                        job.FullSourcePath = _etcMainLocalFolder + "/" + job.SourcePath;
                        job.FullDesticationPath = _etcMainFtpFolder + "/" + job.SourcePath;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void MtcJobList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                try
                {
                    if (e.OldItems != null)
                        foreach (JobInformationModel oldItem in e.OldItems)
                            oldItem.PropertyChanged -= MtcJob_PropertyChanged;

                    if (e.NewItems != null)
                        foreach (JobInformationModel newItem in e.NewItems)
                            newItem.PropertyChanged += MtcJob_PropertyChanged;
                }
                catch (Exception ex)
                {
                    NLogHelper.Error(ex);
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void MtcJob_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == "SourcePath")
                {
                    if (string.IsNullOrEmpty(_mtcMainLoaclFolder))
                    {
                        MessageBox.Show("Please config main local folder!", "Synchronization", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (string.IsNullOrEmpty(_mtcMainFtpFolder))
                    {
                        MessageBox.Show("Please config main Ftp folder!", "Synchronization", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    JobInformationModel job = sender as JobInformationModel;
                    if (job != null)
                    {
                        job.FullSourcePath = _mtcMainLoaclFolder + "/" + job.SourcePath;
                        job.FullDesticationPath = _mtcMainFtpFolder + "/" + job.SourcePath;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void updateEtcJobFtpPath()
        {
            try
            {
                if (!String.IsNullOrEmpty(_etcMainFtpFolder))
                {
                    foreach (var job in _etcJobList)
                    {
                        job.FullDesticationPath = _etcMainFtpFolder + "/" + job.SourcePath;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        private void updateEtcJobLocalPath()
        {
            try
            {
                if (!String.IsNullOrEmpty(_etcMainLocalFolder))
                {
                    foreach (var job in _etcJobList)
                    {
                        job.FullSourcePath = _etcMainLocalFolder + "/" + job.SourcePath;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void updateMtcJobFtpPath()
        {
            try
            {
                if (!String.IsNullOrEmpty(_mtcMainFtpFolder))
                {
                    foreach (var job in _mtcJobList)
                    {
                        job.FullDesticationPath = _mtcMainFtpFolder + "/" + job.SourcePath;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        private void updateMtcJobLocalPath()
        {
            try
            {
                if (!String.IsNullOrEmpty(_mtcMainLoaclFolder))
                {
                    foreach (var job in _mtcJobList)
                    {
                        job.FullSourcePath = _mtcMainLoaclFolder + "/" + job.SourcePath;
                    }
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        private void browserFolderPath(ref string folderPath)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (!string.IsNullOrEmpty(folderPath))
            {
                if (Directory.Exists(folderPath))
                {
                    dialog.SelectedPath = folderPath;
                }
                else
                {
                    MessageBox.Show("Thư mục không tồn tại!", "Thông Báo", MessageBoxButton.OKCancel);
                }
            }
            dialog.ShowDialog();
            folderPath = dialog.SelectedPath;
            if (String.IsNullOrEmpty(folderPath))
            {
                System.Windows.MessageBox.Show("Chọn Folder dữ liệu!", "Thông Báo", MessageBoxButton.OK);
            }

        }
        #endregion
    }
}
