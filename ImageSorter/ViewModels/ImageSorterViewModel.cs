﻿using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using ImageSorter.Models;

namespace ImageSorter.ViewModels
{
    public class ImageSorterViewModel: NotifyPropertyChanged
    {
        public SelectedDirectory SelectedDirectory { get; set; }
        public TaskManager TaskManager { get; set; }
        private Task MainTask { get; set; }

        private string _sourceFolderPath = "";
        private string _destFolderPath = "";
        private bool _includeSubDirectories = false;
        private int _maxThreads = 1;

        public string SourceDirectoryPath
        {
            get
            {
                return _sourceFolderPath;
            }
            set
            {
                _sourceFolderPath = value;
                SelectedDirectory.DirectoryPath = value;
            }
        }

        public string DestinationDirectoryPath
        {
            get
            {
                return _destFolderPath;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    var dir = new DirectoryInfo(value);

                    if (dir.Exists)
                    {
                        _destFolderPath = value;
                        TaskManager.DestinationDirectory = dir;
                    }
                }
                

            }
        }

        public bool IncludeSubDirectories
        {
            get
            {
                return _includeSubDirectories;
            }
            set
            {
                _includeSubDirectories = value;
                SelectedDirectory.IncludeSubDirectories = value;
            }
        }

        public int MaxThreads
        {
            get
            {
                return _maxThreads;
            }
            set
            {
                _maxThreads = value;
                TaskManager.MaxThreads = value;
            }
        }

        public int NumberOfImages { get; private set; }
        public long SizeOfImages { get; private set; }
        public int TotalFiles { get; private set; }

        public ImageSorterViewModel()
        {
            SelectedDirectory = new SelectedDirectory();
            TaskManager = new TaskManager(SelectedDirectory);
            SelectedDirectory.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "NumberOfImageFiles")
                {
                    NumberOfImages = SelectedDirectory.NumberOfImageFiles;
                    RaisePropertyChangedEvent("NumberOfImages");

                }
                if (args.PropertyName == "NumberOfImageBytes")
                {
                    SizeOfImages = SelectedDirectory.NumberOfImageBytes;
                    RaisePropertyChangedEvent("SizeOfImages");

                }
            };

            TaskManager.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "TotalFiles")
                {
                    TotalFiles = TaskManager.TotalFiles;
                    RaisePropertyChangedEvent("TotalFiles");

                }
            };
        }

        public void Start()
        {
            MainTask = new Task(() => TaskManager.Start());
            MainTask.Start();
        }

        public void Stop()
        {
            TaskManager.Stop();
        }


    }
}