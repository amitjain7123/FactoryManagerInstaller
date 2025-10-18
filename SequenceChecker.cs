using Microsoft.Win32;

namespace FactoryManagerInstaller
{
    class SequenceChecker
    {

        public static void SetSequence(int sequence)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"FMInstaller");
            Registry.CurrentUser.CreateSubKey(@"FMInstaller").SetValue("Sequence", sequence.ToString());
        }

        public static int GetSequence()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"FMInstaller");
            if (key == null)
            {
                return 0;
            }
            else
            {
                return (int.Parse(key.GetValue("Sequence").ToString()));
            }
        }

        public static void RemoveSequence()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"FMInstaller");
            if (key != null)
            {
                Registry.CurrentUser.DeleteSubKey(@"FMInstaller");
            }
        }
    }
}
