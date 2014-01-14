using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Mnk.Library.Common.OS
{
    public sealed class ShellLink : IDisposable
    {
        [ComImport()]
        [Guid("0000010B-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IPersistFile
        {
            [PreserveSig]
            void GetClassID(out Guid pClassID);
            void IsDirty();
            void Load(
                [MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
                uint dwMode);
            void Save(
                [MarshalAs(UnmanagedType.LPWStr)] string pszFileName,
                [MarshalAs(UnmanagedType.Bool)] bool fRemember);
            void SaveCompleted(
                [MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
            void GetCurFile(
                [MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0,
            CharSet = CharSet.Unicode)]
        private struct _WIN32_FIND_DATAW
        {
            public uint dwFileAttributes;
            public _FILETIME ftCreationTime;
            public _FILETIME ftLastAccessTime;
            public _FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] // MAX_PATH
            public string cFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 0)]
        private struct _FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        }

        [ComImport()]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellLinkW
        {
            void GetPath(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
                int cchMaxPath,
                ref _WIN32_FIND_DATAW pfd,
                uint fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
                int cchMaxName);
            void SetDescription(
                [MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir,
                int cchMaxPath);
            void SetWorkingDirectory(
                [MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs,
                int cchMaxPath);
            void SetArguments(
                [MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short pwHotkey);
            void GetShowCmd(out uint piShowCmd);
            void SetShowCmd(uint piShowCmd);
            void GetIconLocation(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath,
                int cchIconPath,
                out int piIcon);
            void SetIconLocation(
                [MarshalAs(UnmanagedType.LPWStr)] string pszIconPath,
                int iIcon);
            void SetRelativePath(
                [MarshalAs(UnmanagedType.LPWStr)] string pszPathRel,
                uint dwReserved);
            void Resolve(
                IntPtr hWnd,
                uint fFlags);
            void SetPath(
                [MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        [Guid("00021401-0000-0000-C000-000000000046")]
        [ClassInterface(ClassInterfaceType.None)]
        [ComImport()]
        private class CShellLink
        {
        }

        private IShellLinkW link;

        public ShellLink()
        {
            link = (IShellLinkW) new CShellLink();
        }

        public void Dispose()
        {
            if (link == null) return;
            Marshal.ReleaseComObject(link);
            link = null;
        }

        public void Save(string linkFile, string targetPath, string workingDirectory, string iconPath)
        {
            link.SetPath(targetPath);
            link.SetWorkingDirectory(workingDirectory);
            link.SetIconLocation(iconPath, 0);
            ((IPersistFile) link).Save(linkFile, true);
        }
    }
}