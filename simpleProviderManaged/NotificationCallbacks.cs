﻿using Microsoft.Windows.ProjFS;
using System;
using System.Collections.Generic;

namespace SimpleProviderManaged
{
    class NotificationCallbacks
    {
        private bool testMode;

        private readonly SimpleProvider provider;

        public NotificationCallbacks(
            SimpleProvider provider,
            bool testMode,
            VirtualizationInstance virtInstance,
            IReadOnlyCollection<NotificationMapping> notificationMappings)
        {
            TestMode = testMode;

            this.provider = provider;

            // Look through notificationMappings for all the set notification bits.  Supply a callback
            // for each set bit.
            NotificationType notification = NotificationType.None;
            foreach (NotificationMapping mapping in notificationMappings)
            {
                notification |= mapping.NotificationMask;
            }

            if ((notification & NotificationType.FileOpened) == NotificationType.FileOpened)
            {
                virtInstance.OnNotifyFileOpened = NotifyFileOpenedCallback;
            }

            if ((notification & NotificationType.NewFileCreated) == NotificationType.NewFileCreated)
            {
                virtInstance.OnNotifyNewFileCreated = NotifyNewFileCreatedCallback;
            }

            if ((notification & NotificationType.FileOverwritten) == NotificationType.FileOverwritten)
            {
                virtInstance.OnNotifyFileOverwritten = NotifyFileOverwrittenCallback;
            }

            if ((notification & NotificationType.PreDelete) == NotificationType.PreDelete)
            {
                virtInstance.OnNotifyPreDelete = NotifyPreDeleteCallback;
            }

            if ((notification & NotificationType.PreRename) == NotificationType.PreRename)
            {
                virtInstance.OnNotifyPreRename = NotifyPreRenameCallback;
            }

            if ((notification & NotificationType.PreCreateHardlink) == NotificationType.PreCreateHardlink)
            {
                virtInstance.OnNotifyPreCreateHardlink = NotifyPreCreateHardlinkCallback;
            }

            if ((notification & NotificationType.FileRenamed) == NotificationType.FileRenamed)
            {
                virtInstance.OnNotifyFileRenamed = NotifyFileRenamedCallback;
            }

            if ((notification & NotificationType.HardlinkCreated) == NotificationType.HardlinkCreated)
            {
                virtInstance.OnNotifyHardlinkCreated = NotifyHardlinkCreatedCallback;
            }

            if ((notification & NotificationType.FileHandleClosedNoModification) == NotificationType.FileHandleClosedNoModification)
            {
                virtInstance.OnNotifyFileHandleClosedNoModification = NotifyFileHandleClosedNoModificationCallback;
            }

            if (((notification & NotificationType.FileHandleClosedFileModified) == NotificationType.FileHandleClosedFileModified) ||
                ((notification & NotificationType.FileHandleClosedFileDeleted) == NotificationType.FileHandleClosedFileDeleted))
            {
                virtInstance.OnNotifyFileHandleClosedFileModifiedOrDeleted = NotifyFileHandleClosedFileModifiedOrDeletedCallback;
            }

            if ((notification & NotificationType.FilePreConvertToFull) == NotificationType.FilePreConvertToFull)
            {
                virtInstance.OnNotifyFilePreConvertToFull = NotifyFilePreConvertToFullCallback;
            }
        }

        public bool TestMode { get => testMode; set => testMode = value; }

        public bool NotifyFileOpenedCallback(
            string relativePath,
            bool isDirectory,
            uint triggeringProcessId,
            string triggeringProcessImageFileName,
            out NotificationType notificationMask)
        {
            Console.WriteLine($"NotifyFileOpenedCallback [{relativePath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            notificationMask = NotificationType.UseExistingMask;
            provider.SignalIfTestMode("FileOpened");
            return true;
        }


        public void NotifyNewFileCreatedCallback(
            string relativePath,
            bool isDirectory,
            uint triggeringProcessId,
            string triggeringProcessImageFileName,
            out NotificationType notificationMask)
        {
            Console.WriteLine($"NotifyNewFileCreatedCallback [{relativePath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            notificationMask = NotificationType.UseExistingMask;
            provider.SignalIfTestMode("NewFileCreated");
        }

        public void NotifyFileOverwrittenCallback(
            string relativePath,
            bool isDirectory,
            uint triggeringProcessId,
            string triggeringProcessImageFileName,
            out NotificationType notificationMask)
        {
            Console.WriteLine($"NotifyFileOverwrittenCallback [{relativePath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            notificationMask = NotificationType.UseExistingMask;
            provider.SignalIfTestMode("FileOverwritten");
        }

        public bool NotifyPreDeleteCallback(
            string relativePath,
            bool isDirectory,
            uint triggeringProcessId,
            string triggeringProcessImageFileName)
        {
            Console.WriteLine($"NotifyPreDeleteCallback [{relativePath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            provider.SignalIfTestMode("PreDelete");
            return true;
        }

        public bool NotifyPreRenameCallback(
            string relativePath,
            string destinationPath,
            uint triggeringProcessId,
            string triggeringProcessImageFileName)
        {
            Console.WriteLine($"NotifyPreRenameCallback [{relativePath}] [{destinationPath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            provider.SignalIfTestMode("PreRename");
            return true;
        }

        public bool NotifyPreCreateHardlinkCallback(
            string relativePath,
            string destinationPath,
            uint triggeringProcessId,
            string triggeringProcessImageFileName)
        {
            Console.WriteLine($"NotifyPreCreateHardlinkCallback [{relativePath}] [{destinationPath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            provider.SignalIfTestMode("PreCreateHardlink");
            return true;
        }

        public void NotifyFileRenamedCallback(
            string relativePath,
            string destinationPath,
            bool isDirectory,
            uint triggeringProcessId,
            string triggeringProcessImageFileName,
            out NotificationType notificationMask)
        {
            Console.WriteLine($"NotifyFileRenamedCallback [{relativePath}] [{destinationPath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            notificationMask = NotificationType.UseExistingMask;
            provider.SignalIfTestMode("FileRenamed");
        }

        public void NotifyHardlinkCreatedCallback(
            string relativePath,
            string destinationPath,
            uint triggeringProcessId,
            string triggeringProcessImageFileName)
        {
            Console.WriteLine($"NotifyHardlinkCreatedCallback [{relativePath}] [{destinationPath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            provider.SignalIfTestMode("HardlinkCreated");
        }

        public void NotifyFileHandleClosedNoModificationCallback(
            string relativePath,
            bool isDirectory,
            uint triggeringProcessId,
            string triggeringProcessImageFileName)
        {
            Console.WriteLine($"NotifyFileHandleClosedNoModificationCallback [{relativePath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            provider.SignalIfTestMode("FileHandleClosedNoModification");
        }

        public void NotifyFileHandleClosedFileModifiedOrDeletedCallback(
            string relativePath,
            bool isDirectory,
            bool isFileModified,
            bool isFileDeleted,
            uint triggeringProcessId,
            string triggeringProcessImageFileName)
        {
            Console.WriteLine($"NotifyFileHandleClosedFileModifiedOrDeletedCallback [{relativePath}]");
            Console.WriteLine($"  Modified: {isFileModified}, Deleted: {isFileDeleted} ");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            provider.SignalIfTestMode("FileHandleClosedFileModifiedOrDeleted");
        }

        public bool NotifyFilePreConvertToFullCallback(
            string relativePath,
            uint triggeringProcessId,
            string triggeringProcessImageFileName)
        {
            Console.WriteLine($"NotifyFilePreConvertToFullCallback [{relativePath}]");
            Console.WriteLine($"  Notification triggered by [{triggeringProcessImageFileName} {triggeringProcessId}]");

            provider.SignalIfTestMode("FilePreConvertToFull");
            return true;
        }

    }
}