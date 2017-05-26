using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Windows.Documents;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.ViewModel;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Model
{
    [DataContract(Namespace = "")]
    public class JobInformationModel: ViewModelBase
    {

        #region Fields
        private string _sourcePath;
        private string _destinationPath;
        private string _fullSourcePath;
        private string _fullDesticationPath;
        private int _intervalSynchrozation;
        private bool _isRun;
        private string _jobName;

        #endregion

        #region Property

        [DataMember(EmitDefaultValue = false)]
        public string JobName
        {
            get { return _jobName; }
            set
            {
                _jobName = value;
                OnPropertyChanged("JobName");
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                _sourcePath = value;
                OnPropertyChanged("SourcePath");
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public string DestinationPath
        {
            get { return _destinationPath; }
            set
            {
                _destinationPath = value;
                OnPropertyChanged("DestinationPath");
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public string FullSourcePath
        {
            get { return _fullSourcePath; }
            set
            {
                _fullSourcePath = value;
                OnPropertyChanged("FullSourcePath");
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public string FullDesticationPath
        {
            get { return _fullDesticationPath; }
            set
            {
                _fullDesticationPath = value;
                OnPropertyChanged("FullDesticationPath");
            }
        }

        /// <summary>
        /// Interval Time run the Job in Minute
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int IntervalSynchrozation
        {
            get { return _intervalSynchrozation; }
            set
            {
                _intervalSynchrozation = value;
                OnPropertyChanged("IntervalSynchrozation");
            }
        }

        /// <summary>
        /// Whether or not the Job run
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public bool IsRun
        {
            get { return _isRun; }
            set
            {
                _isRun = value;
                OnPropertyChanged("IsRun");
            }
        }

        #endregion
    }

    //DataContract(Namespace = "")]
    //public class JobList : List<JobInformationModel>
    //{
        
    //}

    [DataContract(Namespace = "")]
    
    public class TempJob : JobInformationModel
    {
        public TempJob()
        {
            JobName = "Job Name";
            IntervalSynchrozation = 0;
            IsRun = false;
        }
    }
}
