﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ITD.ETC.VETC.Synchonization.Controller.Database;
using ITD.ETC.VETC.Synchonization.Controller.Ftp;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Provider;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Quartz;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.View;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.ViewModel
{
    public class SyncMainViewModel: ViewModelBase
    {
        #region Field
        private MainProvider _mGlobal;
        private ConfigModel _clientConfig;
        private MainScheduler _mainScheduler;

        private bool _isStarted = false;
        #endregion

        #region Property
        private bool _canConfig = true;
        public bool CanConfigView
        {
            get { return _canConfig; }
            set
            {
                if (_canConfig != value)
                {
                    _canConfig = value;
                    OnPropertyChanged("CanConfigView");
                }
            }
        }
        #endregion

        #region Command
        private ICommand _clickConfigCommand;
        public ICommand ConfigCommand
        {
            get
            {
                return _clickConfigCommand ?? (_clickConfigCommand = new RelayCommand(param => { ConfigDatabaseServer(); }, param => CanConfig()));
            }
        }
        private bool CanConfig()
        
        {
            //return _canConfig;
            return !_isStarted;
        }
        private void ConfigDatabaseServer()
        {

            ConfigView _clientConfigView = new ConfigView();
            _clientConfigView.ShowDialog();
            
        }

        private ICommand _clickSyncCommand;

        public ICommand StartSyncCommand
        {
            get
            {
                return _clickSyncCommand ?? (_clickSyncCommand = new RelayCommand(param => { StartSyncProcess(); }, param => CanSync()));
            }
        }
        private bool CanSync()
        {
            //return true;
            return !_isStarted;
        }
        private void StartSyncProcess()
        {
            try
            {
                // Load config
                _clientConfig = ConfigModel.LoadConfig();
                string connectionString = DataBaseHelper.GetConnectionString(_clientConfig.DataBaseServer,
                    _clientConfig.DatabaseName, _clientConfig.DatabaseUser, _clientConfig.DatabaseUserPassword,
                    _clientConfig.DatabaseTimeOut.ToString());

                // Check Database from Config
                DataBaseHelper dbHelper = new DataBaseHelper(connectionString);
                bool isDataConnected = dbHelper.CheckOpenConnection();

                if (!isDataConnected)
                {
                    MessageBox.Show(Resources.NoDatabaseConnection, "Synchronization", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }

                // Init Ftp information
                FileTransferFtp ftpTransfer = new FileTransferFtp(_clientConfig.FtpServerAddress, _clientConfig.FtpServerPort,_clientConfig.FtpUserName, _clientConfig.FtpPassword, _clientConfig.FtpTimeout);
                
                MainProvider mainProvider = new MainProvider(dbHelper, _clientConfig, ftpTransfer);

                // Start schedule
                _mainScheduler = new MainScheduler(_clientConfig);
                _mainScheduler.StartBackgroudWorker();

                _isStarted = true;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
            
        }

        private ICommand _stopSyncCommand;

        public ICommand StopSyncCommand
        {
            get
            {
                return _stopSyncCommand ?? (_stopSyncCommand = new RelayCommand(param => { StopSyncProcess(); }, param => CanStopSync()));
            }
        }
        private bool CanStopSync()
        {
            //return true;
            return _isStarted;
        }
        private void StopSyncProcess()
        {
            try
            {
                
                if (_mainScheduler != null)
                {
                    // Stop schedule
                    _mainScheduler.StopBackgroundWorker();

                    // Release resource
                    _mainScheduler.Dispose();
                    _mainScheduler = null;
                }
                
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }

            _isStarted = false;

        }
        #endregion
    }
}
