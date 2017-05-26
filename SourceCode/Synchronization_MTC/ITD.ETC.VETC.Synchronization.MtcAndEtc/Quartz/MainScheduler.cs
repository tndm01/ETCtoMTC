using System;
using System.ComponentModel;
using System.Linq;
using Quartz;
using Quartz.Impl;
using System.Threading;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Job;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Model;
using ITD.ETC.VETC.Synchronization.MtcAndEtc.Properties;

namespace ITD.ETC.VETC.Synchronization.MtcAndEtc.Quartz
{
    public class MainScheduler: IDisposable
    {
        #region Field
        private ISchedulerFactory schedulerFactory;
        private IScheduler scheduler;

        protected Thread m_thread;
        protected ManualResetEvent m_shutdownEvent;
        protected TimeSpan m_delay;

        private BackgroundWorker myWorker;

        private ConfigModel _config;
        #endregion

        #region Property

        #endregion

        #region Constructor

        public MainScheduler(ConfigModel config)
        {
            try
            {
                _config = config;

                schedulerFactory = new StdSchedulerFactory();
                scheduler = schedulerFactory.GetScheduler();
                myWorker = new BackgroundWorker();
                //myWorker.DoWork += myWorker_DoWork;
                //myWorker.RunWorkerCompleted += myWorker_RunWorkerCompleted;
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// Managerment Schedule
        /// </summary>
        public void JobManagerment()
        {
            try
            {
                if (_config == null)
                {
                    NLogHelper.Info("The Config should be loaded before run schedule!");
                    return;
                }

                //int time = 2;

                // luồng TollTicket MTC->ETC
                var tollTicketJob = _config.MtcJobList.FirstOrDefault(j => j.JobName == Resources.TollTicketDataJobName);
                if(tollTicketJob != null && tollTicketJob.IntervalSynchrozation > 0 && tollTicketJob.IsRun)
                {
                     IJobDetail jobTollTicket = JobBuilder.Create<TollTicketMTCtoETC>()
                    .WithIdentity("TollTicket", "MTCtoETC")
                    .Build();

                    ITrigger trigTollTicket = TriggerBuilder.Create()
                    .WithSimpleSchedule(s => s.WithIntervalInSeconds(tollTicketJob.IntervalSynchrozation).RepeatForever())
                    .StartNow()
                    .Build();
                scheduler.ScheduleJob(jobTollTicket, trigTollTicket);
                }


                //luồng CommuterTicket MTC->ETC
                var commuterJob = _config.MtcJobList.FirstOrDefault(j => j.JobName == Resources.ComuterTicketDataJobName);
                if(commuterJob != null && commuterJob.IntervalSynchrozation > 0 && commuterJob.IsRun)
                {
                    IJobDetail jobCommuterTicket = JobBuilder.Create<CommuterTicketMTCtoETC>()
                        .WithIdentity("CommuterTicket", "MTCtoETC")
                        .Build();
                    ITrigger trigCommuterTicket = TriggerBuilder.Create()
                    .WithSimpleSchedule(s => s.WithIntervalInSeconds(commuterJob.IntervalSynchrozation).RepeatForever())
                    .StartNow()
                    .Build();
                }


                //scheduler.ScheduleJob(jobCommuterTicket, trigCommuterTicket);

                //luồng EmployeeData MTC->ETC
                var employeeJob = _config.MtcJobList.FirstOrDefault(j => j.JobName == Resources.EmployeeDataJobName);
                if (employeeJob != null && employeeJob.IntervalSynchrozation > 0 && employeeJob.IsRun)
                {
                    IJobDetail jobEmployeeData = JobBuilder.Create<EmployeeDataMTCtoETC>()
                        .WithIdentity(" EmployeeData", "MTCtoETC")
                        .Build();

                    ITrigger trigEmployeeData = TriggerBuilder.Create()
                        .WithSimpleSchedule(s => s.WithIntervalInSeconds(employeeJob.IntervalSynchrozation).RepeatForever())
                        .StartNow()
                        .Build();
                    scheduler.ScheduleJob(jobEmployeeData, trigEmployeeData);
                }

                //luồng VehiclePlateData    MTC->ETC 
                var vehicleJob = _config.MtcJobList.FirstOrDefault(j => j.JobName == Resources.VehiclePlateJobName);
                if(vehicleJob != null && vehicleJob.IntervalSynchrozation > 0 && vehicleJob.IsRun)
                {
                    IJobDetail jobVehiclePlateData = JobBuilder.Create<VehiclePlateDataMTCtoETC>()
                    .WithIdentity("VehiclePlateData", "MTCtoETC")
                    .Build();
                    ITrigger trigVehiclePlateData = TriggerBuilder.Create()
                    .WithSimpleSchedule(s => s.WithIntervalInSeconds(vehicleJob.IntervalSynchrozation).RepeatForever())
                    .StartNow()
                    .Build();
                scheduler.ScheduleJob(jobVehiclePlateData, trigVehiclePlateData);
                }


                ////luồng EtagRegisterData ETC->MTC
                var eregisJob = _config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.EtagRegisterDataJobName);
                if (eregisJob != null && eregisJob.IntervalSynchrozation > 0 && eregisJob.IsRun)
                {
                    IJobDetail jobEtagRegisterData = JobBuilder.Create<EtagRegisterDataETCtoMTC>()
                        .WithIdentity("EtagRegisterData", "ETC->MTC")
                        .Build();
                    ITrigger trigEtagRegisterData = TriggerBuilder.Create()
                        .WithSimpleSchedule(s => s.WithIntervalInMinutes(eregisJob.IntervalSynchrozation).RepeatForever())
                        .StartNow()
                        .Build();
                    scheduler.ScheduleJob(jobEtagRegisterData, trigEtagRegisterData);
                }
                ////luồng TollTicket Transaction ETC->MTC
                var tollJob = _config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.TollTicketTransactionDataJobName);
                if (tollJob != null && tollJob.IntervalSynchrozation > 0 && tollJob.IsRun)
                {
                    IJobDetail jobTollTicketTransaction = JobBuilder.Create<TollTicketTransactionETCtoMTC>()
                        .WithIdentity("TollTicketTransaction", "ETC->MTC")
                        .Build();
                    ITrigger trigTollTicketTransaction = TriggerBuilder.Create()
                        .WithSimpleSchedule(s => s.WithIntervalInMinutes(tollJob.IntervalSynchrozation).RepeatForever())
                        .StartNow()
                        .Build();
                    scheduler.ScheduleJob(jobTollTicketTransaction, trigTollTicketTransaction);
                }

                ////luồng CommuterTransactionData ETC->MTC
                var comJob = _config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.CommuterTicketTransactionDataJobName);
                if (comJob != null && comJob.IntervalSynchrozation > 0 && comJob.IsRun)
                {
                    IJobDetail jobCommuterTransactionData = JobBuilder.Create<CommuterTransactionDataETCtoMTC>()
                        .WithIdentity("CommuterTransactionData", "ETC->MTC")
                        .Build();
                    ITrigger trigCommuterTransactionData = TriggerBuilder.Create()
                        .WithSimpleSchedule(s => s.WithIntervalInMinutes(comJob.IntervalSynchrozation).RepeatForever())
                        .StartNow()
                        .Build();
                    scheduler.ScheduleJob(jobCommuterTransactionData, trigCommuterTransactionData);
                }

                //luồng SpecicalTransactionData ETC-MTC
                var specialJob = _config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.SpecialTransactionDataJobName);
                if (specialJob != null && specialJob.IntervalSynchrozation > 0 && specialJob.IsRun)
                {
                    IJobDetail jobSpecicalTransaction = JobBuilder.Create<SpecicalTransactionETCtoMTC>()
                    .WithIdentity("SpecialTransactionData", "ETC->MTC")
                    .Build();
                    ITrigger trigSpecicalTransaction = TriggerBuilder.Create()
                        .WithSimpleSchedule(s => s.WithIntervalInSeconds(specialJob.IntervalSynchrozation).RepeatForever())
                        .StartNow()
                        .Build();
                    scheduler.ScheduleJob(jobSpecicalTransaction, trigSpecicalTransaction);
                }


                //luồng ETAGTransactionData ETC->MTC
                // lay thoi gian tu config
                var etagJob = _config.EtcJobList.FirstOrDefault(j => j.JobName == Resources.EtagTransactionDataJobName);
                if (etagJob != null && etagJob.IntervalSynchrozation > 0 && etagJob.IsRun)
                {
                    IJobDetail jobETAGTransactionData = JobBuilder.Create<ETAGTransactionDataETCtoMTC>()
                        .WithIdentity("ETAGTransactionData", "ETC->MTC")
                        .Build();
                    ITrigger trigETAGTransactionData = TriggerBuilder.Create()
                        .WithSimpleSchedule(s => s.WithIntervalInMinutes(etagJob.IntervalSynchrozation).RepeatForever())
                        .StartNow()
                        .Build();
                    scheduler.ScheduleJob(jobETAGTransactionData, trigETAGTransactionData);
                }

                ////luồng LaneImage ETC-MTC

                //IJobDetail jobLaneImage = JobBuilder.Create<LaneImageETCtoMTC>()
                //    .WithIdentity("LaneImage", "ETC->MTC")
                //    .Build();
                //ITrigger trigLaneImage = TriggerBuilder.Create()
                //    .WithSimpleSchedule(s => s.WithIntervalInSeconds(time).RepeatForever())
                //    .StartNow()
                //    .Build();
                //scheduler.ScheduleJob(jobLaneImage, trigLaneImage);

                ////luồng Recognization Image ETC-MTC

                //IJobDetail jobRecognizationImage = JobBuilder.Create<RecognizationImageETCtoMTC>()
                //    .WithIdentity("RecognizationImage", "ETC->MTC")
                //    .Build();
                //ITrigger trigRecognizationImage = TriggerBuilder.Create()
                //    .WithSimpleSchedule(s => s.WithIntervalInSeconds(time).RepeatForever())
                //    .StartNow()
                //    .Build();
                //scheduler.ScheduleJob(jobRecognizationImage, trigRecognizationImage);



                scheduler.Start();
                //Thread.Sleep(TimeSpan.FromSeconds(10));
                //scheduler.Shutdown();

            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
            }
        }

        #region Timer

        private void myWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // signal the event to shutdown
            m_shutdownEvent.Set();
            // wait for the thread to stop giving it 5 seconds
            m_thread.Join(5000);

            // call the base class
            NLogHelper.Info("Shutting Down");

            scheduler.Shutdown(true);

            NLogHelper.Info("------------- Shutdown Complete -------------");
        }

        private void myWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // create our threadstart object to wrap our delegate method
            //ThreadStart ts = new ThreadStart(this.mainprocess);
            this.JobManagerment();
            // create the manual reset event and
            // set it to an initial state of unsignaled
            m_shutdownEvent = new ManualResetEvent(false);

            // create the worker thread
            //m_thread = new Thread(ts);

            // go ahead and start the worker thread
            // m_thread.Start();

            NLogHelper.Info("Service started");
        }

        /// <summary>
        /// Start Scheduler
        /// </summary>
        public void StartBackgroudWorker()
        {
            ThreadStart ts = new ThreadStart(this.JobManagerment);
            m_shutdownEvent = new ManualResetEvent(false);
            m_thread = new Thread(ts);
            m_thread.Start();

            NLogHelper.Info("Service started");
        }

        /// <summary>
        /// Stop Scheduler
        /// </summary>
        public void StopBackgroundWorker()
        {
            //m_shutdownEvent.Set();
            // wait for the thread to stop giving it 5 seconds
            //m_thread.Join(5000);

            // call the base class
            NLogHelper.Info("Shutting Down");

            if (scheduler != null && scheduler.IsStarted)
            {
                scheduler.Shutdown(true);
            }
            NLogHelper.Info("------------- Shutdown Complete -------------");
        }
        #endregion


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                m_shutdownEvent = null;
                m_thread = null;
                if (scheduler != null && scheduler.IsStarted)
                {
                    scheduler.Shutdown(true);
                }
                schedulerFactory = null;
                scheduler = null;
                //_dbLibrary.CloseConnection();
            }
            // free native resources if there are any.

        }

        #endregion
    }
}
