using System;
using System.IO;
using System.Security.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenameJpgFilesInDirectory
{
    class Program
    {

        static void Main(string[] args)
        {
            Run();
            Console.Read();
        }


        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Directory.GetCurrentDirectory();
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.jpg";

            // Add event handlers.
            //watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            string newFileName = Guid.NewGuid().ToString() + ".jpg";

            bool flag = true;
            while (flag)
            {
                flag = IsLocked(e.Name);
                if (! flag)
                System.IO.File.Move(e.Name, newFileName);
            }
        }

        public static bool IsLocked(string fpath)
        {
            try
            {
                FileStream fs = File.OpenWrite(fpath);
                fs.Close();
                return false;
            }

            catch (Exception) { return true; }
        }
    }
}
