using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BussinessLogicLayer;
using CommonClasses;
using CommonClasses.CommonClasses;
using DataBaseAccess.Data;

using FinalProjectBionic.Views;
using MultimediaFilesInfo;
using FileType = DataBaseAccess.Data.FileType;


namespace FinalProjectBionic.ViewModels
{
    class LibraryViewModel:PropertyChangedNotification
    {
        private FileSystemHelper _helper = null;
        private bool _downloadCompleted = false;
        public LibraryViewModel()
        {
            FoundedFiles=new ObservableCollection<string>();
            AudioSearchResults=new ObservableCollection<Audio>();
            VideoSearchResults = new ObservableCollection<Video>();
            VideoFiles = new ObservableCollection<Video>();
            AudioFiles = new ObservableCollection<Audio>();
            AudioTypes=new ObservableCollection<FileType>();
            VideoTypes= new ObservableCollection<FileType>();
            CustomFile=new Video();
            NewAudioType=new FileType();
            NewVideoType = new FileType();
            AudioTypeToDelete = new FileType();
            VideoTypeToDelete = new FileType();
            GetAudioTypes();
            GetVideoTypes();
            GetAudioFiles();
            GetVideoFiles();
        }

        
        #region Properties
        
        public string AudioSearchString { get; set; }
        public string VideoSearchString { get; set; }
        public bool IsVideoSelected { get; set; }
        /// <summary>
        /// Gets result of search by title in all library
        /// </summary>
        public ICollection<Audio> AudioSearchResults { get; private set; }
        public ICollection<Video> VideoSearchResults { get; private set; }

        /// <summary>
        /// Gets video files from library
        /// </summary>
        public ICollection<Video> VideoFiles { get; private set; }
        /// <summary>
        /// Gets audio files from library
        /// </summary>
        public ICollection<Audio> AudioFiles { get; private set; }
        /// <summary>
        /// Gets valid audio file types
        /// </summary>
        public ICollection<FileType> AudioTypes { get; private set; }
        /// <summary>
        /// Gets valid video file types
        /// </summary>
        public ICollection<FileType> VideoTypes { get; private set; } 
        
        public ICollection<string> FoundedFiles { get; set; }
        public ICollection<Audio> ToRemoveListAudios { get; set; }
        public ICollection<Video> ToRemoveVideos { get; set; }
        public Video CustomFile { get; set; }
        public Video VideoFromImdb { get; set; }
        public string ImdbSearchRequest { get; set; }
        public FileType NewAudioType { get;  set; }
        public FileType NewVideoType { get; set; }
        public FileType AudioTypeToDelete { get; set; }
        public FileType VideoTypeToDelete { get; set; }
        #endregion


        #region Commands
        public ActionCommand AudioShowFolderBrowserCommand
        {
            get { return new ActionCommand(p => AudioShowFolderBrowser()); }
        }

        private void AudioShowFolderBrowser()
        {
            string path = "";
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                path = folderDialog.SelectedPath;

                _helper = new FileSystemHelper(path, AudioTypes.Select(extension => extension.Name).ToList());
                FoundedFiles = _helper.GetFoundedFiles();
                if (FoundedFiles.Count != 0)
                {
                    ScanResultWindow view = new ScanResultWindow(FoundedFiles);
                    if (view.ShowDialog() == true)
                    {

                        List<Audio> dump = new List<Audio>();
                        foreach (var file in view.SelectedFiles)
                        {
                            AudioFile af = new AudioFile(file);

                            var dbAudio = new Audio()
                            {
                                Album = af.Album,
                                Artist = af.Artist,
                                Duration = af.Duration,
                                Title = af.Title,
                                Location = af.Location,
                                CreationDate = DateTime.Now
                            };
                            if (!AudioFiles.ToList().Exists(p => p.Artist == dbAudio.Artist && p.Title == dbAudio.Title))
                                dump.Add(dbAudio);

                            //AddAudioFile(dbAudio); //second approach works faster
                        }
                        AddAudios(dump);
                    }
                    FoundedFiles.Clear();
                }

            }
                
                
        }

        public ActionCommand VideoShowFolderBrowserCommand
        {
            get { return new ActionCommand(p => VideoShowFolderBrowser()); }
        }

        private void VideoShowFolderBrowser()
        {
            string path = "";
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                path = folderDialog.SelectedPath;
                _helper = new FileSystemHelper(path, VideoTypes.Select(extension => extension.Name).ToList());
                FoundedFiles = _helper.GetFoundedFiles();
                if (FoundedFiles.Count != 0)
                {
                    ScanResultWindow view = new ScanResultWindow(FoundedFiles);
                    if (view.ShowDialog() == true)
                    {
                        List<Video> dump = new List<Video>();
                        foreach (var file in view.SelectedFiles)
                        {
                            VideoFile af = new VideoFile(file);
                            Video dbVideo = new Video()
                            {
                                Duration = af.Duration,
                                Title = af.Title,
                                Location = af.Location,
                                CreationDate = DateTime.Now
                            };
                            if (!VideoFiles.ToList().Exists(p => p.Title == dbVideo.Title))
                            {
                                dump.Add(dbVideo);
                                VideoFiles.Add(dbVideo);
                            }

                            // AddVideoFile(dbVideo);



                        }
                        AddVideos(dump);
                    }
                    FoundedFiles.Clear();
                }
            }
            
        }

        public ActionCommand DeleteAudioCommand
        {
            get
            {
                return  new ActionCommand(p=>DeleteAudioFiles(ToRemoveListAudios));
            }
        }

        public ActionCommand DeleteVideoCommand
        {
            get
            {
                return new ActionCommand(p=>DeleteVideoFiles(ToRemoveVideos));
            }
        }
        private void DeleteAudioFiles(ICollection<Audio> audios)
        {
            using (var api = new DataProvider())
            {
                foreach (var audio in audios)
                {
                    try
                    {
                        api.DeleteAudioFile(audio);
                        var item = AudioFiles.First(p => p.Title == audio.Title&&p.Artist==audio.Artist);
                        AudioFiles.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }
               
            }
        }

        private void DeleteVideoFiles(ICollection<Video> videos)
        {
            using (var api = new DataProvider())
            {
                foreach (var video in videos)
                {
                    try
                    {
                        api.DeleteVideoFile(video);
                        var item = VideoFiles.First(p => p.Title == video.Title);
                        VideoFiles.Remove(item);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }

            }
        }

        public ActionCommand AddCustomVideoFileCommand
        {
            get
            {
                return new ActionCommand(p=>AddCustomVideoFile(),
                                          p=>CustomFileIsValid);
            }
        }

        private void AddCustomVideoFile()
        {
            using (var api = new DataProvider())
            {     
                if (!VideoFiles.ToList().Exists(p => p.Title == CustomFile.Title&&p.YearOfRelease==CustomFile.YearOfRelease))
                    try
                    {
                        CustomFile.CreationDate = DateTime.Now;
                        api.AddVideoFile(CustomFile);
                        VideoFiles.Add(CustomFile);
                        MessageBox.Show("Information added");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                    
                
            }  
        }

        public ActionCommand SearchAudioCommand
        {
            get
            {
                return new ActionCommand(p=>AudioSearch(AudioSearchString),
                                            p=>AudioSearchStringIsValid);
            }
        }

        public ActionCommand SearchVideoCommand
        {
            get { return new ActionCommand(p => VideoSearch(VideoSearchString), p => VideoSearchStringIsValid); }
        }
        private void AudioSearch(string searchString)
        {
                AudioSearchResults.Clear();
                foreach (var file in AudioFiles.Where(item => item.Title.Contains(searchString)).ToList())
                {
                    AudioSearchResults.Add(file);
                }    
        }

        private void VideoSearch(string searchString)
        {
            VideoSearchResults.Clear();
            foreach (var item in VideoFiles.Where(item => item.Title.Contains(searchString)).ToList())
            {
                VideoSearchResults.Add(item);
            }
        }


        public ActionCommand AddAudioTypeCommand
        {
            get
            {
                return new ActionCommand(p => AddAudioType(),p=>AudioTypeIsValid);
            }
        }
        private void AddAudioType()
        {
            NewAudioType.Decription = "Audio";
            using (var api = new DataProvider())
            {
                try
                {
                    api.AddAudioType(NewAudioType);
                    AudioTypes.Add(new FileType()
                    {
                        Decription = "Audio",
                        Name = NewAudioType.Name
                    });
                    MessageBox.Show("New video files enxtension added");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to add such extension. It's allready exists");
                }
                NewAudioType.Name = String.Empty;
            }
            
            
           
           
        }
        public ActionCommand AddVideoTypeCommand
        {
            get
            {
                return new ActionCommand(p => AddVideoType(),p=>VideoTypeIsValid);
            }
        }

        private void AddVideoType()
        {
            NewVideoType.Decription = "Video";
            using (var api = new DataProvider())
            {
                try
                {
                    api.AddVideoType(NewVideoType);
                    VideoTypes.Add(new FileType()
                    {
                        Name = NewVideoType.Name,
                        Decription = NewVideoType.Decription
                    });
                    MessageBox.Show("New video files enxtension added");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to add such extension. It's allready exists");
                }
                NewVideoType.Name = String.Empty;
            }
            
            
        }

        public ActionCommand GetImbdInfoCommand
        {
            get
            {
                return new ActionCommand(p=>GetInfoFromImdb());
            }
        }

        public ActionCommand SaveFromImbdCommand
        {
            get { return new ActionCommand(p=>SaveFromImdb(),p=>ImdbFileIsValid);}
        }

        private void SaveFromImdb()
        {
            using (var api = new DataProvider())
            {
                try
                {
                    api.AddVideoFile(VideoFromImdb);
                    VideoFiles.Add(new Video()
                    {
                        Poster = VideoFromImdb.Poster,
                        Title = VideoFromImdb.Title,
                        YearOfRelease = VideoFromImdb.YearOfRelease,
                        Comments = VideoFromImdb.Comments,
                        CreationDate = DateTime.Now
                        
                    });;
                    MessageBox.Show("New movie added!");
                    VideoFromImdb.Poster = null;
                    VideoFromImdb.Title = String.Empty;
                    VideoFromImdb.YearOfRelease = String.Empty;
                    VideoFromImdb.Comments = String.Empty;
                    _downloadCompleted = false;
                    NotifyPropertyChanged("VideoFromImdb");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(
                        "Can`t save information about this movie. Maybe such movie already exists in your list");
                }
            }
        }
        private void GetInfoFromImdb()
        {
            try
            {
                _downloadCompleted = false;
                ImdbAPI api = new ImdbAPI(ImdbSearchRequest);
                if (api.Information.Response == "True")
                {
                    VideoFromImdb = new Video()
                    {
                        Comments = (api.Information.Plot=="N/A")?"There are no comments for this video yet...":api.Information.Plot,
                        CreationDate = DateTime.Now,
                        Title = api.Information.Title,
                        ProducedBy = api.Information.Director
                    };
                    if (api.Information.Released != null&&api.Information.Released!="N/A")
                    {
                        VideoFromImdb.YearOfRelease = api.Information.YearOfRelease;
                    }
                    //download poster
                    if (api.Information.PosterUrl != null && api.Information.PosterUrl != "N/A")
                    {
                        using (var client = new WebClient())
                        {
                            //subscribe for event download data complete which nitifies when poster will be downloaded
                            client.DownloadDataCompleted += delegate(object sender, DownloadDataCompletedEventArgs e)
                            {
                                VideoFromImdb.Poster = e.Result;
                                NotifyPropertyChanged("VideoFromImdb");
                            };
                            client.DownloadDataAsync(new Uri(api.Information.PosterUrl));
                            
                        }
                    }
                    NotifyPropertyChanged("VideoFromImdb");
                    _downloadCompleted = true;
                }
                else
                {
                    MessageBox.Show("Cant get any information");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Bad connection");
                Logger.LogException(ex);
            }
            
        }

        public ActionCommand DeleteAudioTypeCommand
        {
            get { return new ActionCommand(p=>DeleteAudioType());}
        }

        public void DeleteAudioType()
        {
            using (var api = new DataProvider())
            {
                try
                {
                    var item = AudioTypes.First(p => p.Name == AudioTypeToDelete.Name);
                    api.DeleteType(AudioTypeToDelete);
                    AudioTypes.Remove(item);
                    AudioTypeToDelete.Name = String.Empty;
                }
                catch (Exception ex)
                {
                    //TODO:MessageBox
                }
            }
        }

        public ActionCommand DeleteVideoTypeCommand
        {
            get { return new ActionCommand(p => DeleteVideoType()); }
        }

        public void DeleteVideoType()
        {
            using (var api = new DataProvider())
            {
                try
                {
                    var item = VideoTypes.First(p => p.Name == VideoTypeToDelete.Name);
                    api.DeleteType(VideoTypeToDelete);
                    VideoTypes.Remove(item);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Can't delete file type");
                }
            }
        }
        #endregion




        #region Get content from db
        private void AddVideoFile(Video dbVideo)
        {
            using (var api = new DataProvider())
            {
                try
                {
                    api.AddVideoFile(dbVideo);
                    VideoFiles.Add(dbVideo);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.StartsWith("Violation of UNIQUE KEY constraint"))
                    {
                        Logger.LogException(ex);
                    }
                }
            }
        }
        private void AddAudioFile(Audio audio)
        {

            using (var api = new DataProvider())
            {
                try
                {
                    api.AddAudioFile(audio);
                    
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.StartsWith("Violation of UNIQUE KEY constraint"))
                    {
                        Logger.LogException(ex);
                    }
                }
            }
            AudioFiles.Add(audio);
        }

        private void AddVideos(IEnumerable<Video> files)
        {
            using (var api = new DataProvider())
            {
                try
                {
                    api.AddVideoFiles(files);

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.StartsWith("Violation of UNIQUE KEY constraint"))
                    {
                        Logger.LogException(ex);
                    }
                }
            }
        }
        private void AddAudios(IEnumerable<Audio> files)
        {
            using (var api = new DataProvider())
            {
                try
                {
                    api.AddAudioFiles(files);

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.InnerException != null &&
                        ex.InnerException.InnerException.Message.StartsWith("Violation of UNIQUE KEY constraint"))
                    {
                        Logger.LogException(ex);
                    }
                }
            }
            foreach (var audio in files)
            {
                AudioFiles.Add(audio);
            }
        }
        private void GetVideoFiles()
        {
            using (var api = new DataProvider())
            {
                foreach (var file in api.GetVideoFiles())
                {
                    VideoFiles.Add(file);
                }

            }
        }

        private void GetAudioFiles()
        {
            using (var api = new DataProvider())
            {
                foreach (var file in api.GetAudioFiles())
                {
                    AudioFiles.Add(file);
                }
              
            }
        }

        private void GetAudioTypes()
        {
            using (var api = new DataProvider())
            {
                foreach (var type in api.GetAudioTypes())
                {
                    AudioTypes.Add(type);
                }
            }
        }
        private void GetVideoTypes()
        {
            using (var api = new DataProvider())
            {
                foreach (var type in api.GetVideoTypes())
                {
                    VideoTypes.Add(type);
                }
            }
        }
        #endregion

        #region Validation

       
        public bool CustomFileIsValid
        {
            get
            {
                return !String.IsNullOrWhiteSpace(CustomFile.Title) &&
                       !String.IsNullOrWhiteSpace(CustomFile.YearOfRelease) &&
                       Regex.IsMatch(CustomFile.YearOfRelease, @"\d{4}$") &&
                       CustomFile.YearOfRelease.Length == 4;



            }
        }

        public bool AudioSearchStringIsValid
        {
            get { return !String.IsNullOrWhiteSpace(AudioSearchString); }
        }

        public bool VideoSearchStringIsValid
        {
            get { return !String.IsNullOrWhiteSpace(VideoSearchString); }
        }
        public bool CanDeleteAudioType
        {
            get { return !String.IsNullOrEmpty(AudioTypeToDelete.Name); }
        }

        public bool CanDeleteVideoType
        {
            get { return !String.IsNullOrEmpty(VideoTypeToDelete.Name); }
        }
        public bool ImdbFileIsValid
        {
            get { return _downloadCompleted; }
        }

        public bool VideoTypeIsValid
        {
            get
            {
                return !String.IsNullOrEmpty(NewVideoType.Name) &&
                       Regex.IsMatch(NewVideoType.Name, @"^\.\w+") &&
                       !Regex.IsMatch(NewVideoType.Name, @"^\.\w+\W");
            }
        }
        public bool AudioTypeIsValid
        {
            get
            {
                return !String.IsNullOrEmpty(NewAudioType.Name) &&
                       Regex.IsMatch(NewAudioType.Name, @"^\.\w+") &&
                       !Regex.IsMatch(NewAudioType.Name, @"^\.\w+\W");
            }
        }
        #endregion
    }
}
