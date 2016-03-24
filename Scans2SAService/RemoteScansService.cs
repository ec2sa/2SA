

namespace RemoteScansService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;
  

   

    public partial class RemoteScansService : ServiceBase
    {
        private EventLog log;
        private void LogWarning( string message,params object[] messageParams)
        {
            this.log.WriteEntry(string.Format(message,messageParams), EventLogEntryType.Warning);   
        }

        private void LogInfo(string message, params object[] messageParams)
        {
            this.log.WriteEntry(string.Format(message, messageParams), EventLogEntryType.Information);
        }
        
        private void LogError(string message, params object[] messageParams)
        {
            this.log.WriteEntry(string.Format(message, messageParams), EventLogEntryType.Error);
        }

        private static FileSystemWatcher watcher;

        private ServiceConfiguration config { get; set; }

        public RemoteScansService()
        {
            this.AutoLog = false;
          
          
            if (!System.Diagnostics.EventLog.SourceExists("RemoteScans"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                   "RemoteScans", "2SA Service Log");
            }
            log = new EventLog();
           log.Source = "RemoteScans";
        }

        protected override void OnStart(string[] args)
        {
           
            ServiceConfiguration config = ServiceConfiguration.Create(args);
            if (config == null)
            {
                LogError("Service configuration is missing or incomplete");
                return;
            }

            this.config = config;
            try
            {
                watcher = new FileSystemWatcher(config.WatchDirectory);
                watcher.IncludeSubdirectories = false;
                
                watcher.Created += new FileSystemEventHandler(watcher_FileCreated);
                watcher.EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                LogError("Service configuration is invalid. Exception message:{0}", ex.Message);
            }
        }

        void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            FileInfo fi = new FileInfo(e.FullPath);
            if ((fi.Attributes & FileAttributes.Hidden) > 0)
                return;
            processFile(e.FullPath);
        }

        private void processFile(string filePath)
        {
            bool isFree = false;
            while (!isFree)
            {
                try
                {
                    using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
                    {
                        isFree = true;
                    }
                }
                catch
                {
                    System.Threading.Thread.Sleep(100);
                    isFree = false;
                }
            }

            byte[] fileContent = File.ReadAllBytes(filePath);
            if (fileContent.Length == 0)
            {
                LogWarning("Empty scan [{0}] was not sent to 2SA Application. File was deleted.", Path.GetFileName(filePath));
                File.Delete(filePath);
                return;
            }

            Service2SARS.rs wsRef = new Service2SARS.rs();
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            wsRef.Url = config.ServiceUrl;
            wsRef.Timeout = 600;
       
            wsRef.Credentials = new NetworkCredential(config.Username, config.Password);
            try
            {
                bool isSaved = wsRef.SaveScan(config.Username, config.Password, Path.GetFileName(filePath), File.ReadAllBytes(filePath));

                if (isSaved)
                {
                    File.Move(filePath, Path.Combine(config.BackupDirectory, Path.GetFileName(filePath)));
                    LogInfo("Scan [{0}] was sent to 2SA Application.", Path.GetFileName(filePath));
                }
                else
                {
                    File.Move(filePath, Path.Combine(config.BackupDirectory, "FAILED_" + Path.GetFileName(filePath)));
                    LogWarning("Unable to  save scan [{0}] in 2SA Application. File was moved to archive.", Path.GetFileName(filePath));
                }
            }
            catch(Exception ex)
            {
                LogError("Scan [{0}] was not saved. Exception details: {1}", Path.GetFileName(filePath), ex.Message);
            }
        }

        protected override void OnStop()
        {
            if (watcher is FileSystemWatcher)
                watcher.EnableRaisingEvents = false;
        }

        private static bool ValidateRemoteCertificate(object sender,  X509Certificate certificate,  X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
    }


    public class ServiceConfiguration
    {
        public string WatchDirectory { get; set; }
        public string BackupDirectory { get; set; }
        public string ServiceUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public static ServiceConfiguration Create(string[] args)
        {

            if (args == null || args.Length != 5)
                return null;
            return new ServiceConfiguration() {WatchDirectory=args[0],BackupDirectory=args[1],ServiceUrl=args[2],Username=args[3],Password=args[4]};
        }
    }

}
