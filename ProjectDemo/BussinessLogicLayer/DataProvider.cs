using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using DataBaseAccess.Data;
using FileType = DataBaseAccess.Data.FileType;

namespace BussinessLogicLayer
{
    /// <summary>
    /// представляет бизнес логику для связи с базой данных
    /// </summary>
    public sealed class DataProvider : IDisposable
    {
        private MultimediaModel _dataContext;
        private bool _disposed;
        // private string _connectionString = "name=LibraryModel";
        public DataProvider()
        {
            // _connectionString = ConnectionString;
            _dataContext = new MultimediaModel();
        }

        #region SaveToDb Members

        public void AddAudioType(FileType type)
        {
            _dataContext.FileTypes.Add(type);
            _dataContext.SaveChanges();
        }
        public void AddVideoType(FileType fileType)
        {
            _dataContext.FileTypes.Add(fileType);
            _dataContext.SaveChanges();
        }

        public void DeleteType(FileType TypeToDelete)
        {
            var item = _dataContext.FileTypes.First(p => p.Name == TypeToDelete.Name && p.Decription == TypeToDelete.Decription);
            _dataContext.FileTypes.Remove(item);
            _dataContext.SaveChanges();
        }

        public void DeleteAudioFile(Audio audio)
        {
            var item = _dataContext.Audios.First(p => p.Title == audio.Title && p.Artist == audio.Artist);
            _dataContext.Audios.Remove(item);
            _dataContext.SaveChanges();
        }
        public void DeleteVideoFile(Video video)
        {
            var item = _dataContext.Videos.First(p => p.Title == video.Title);
            _dataContext.Videos.Remove(item);
            _dataContext.SaveChanges();
        }

        public void AddVideoFiles(IEnumerable<Video> files)
        {
            _dataContext.Videos.AddRange(files);
            _dataContext.SaveChanges();
        }

        public void AddVideoFile(Video video)
        {
            _dataContext.Videos.Add(video);
            _dataContext.SaveChanges();
        }
        public void AddAudioFiles(IEnumerable<Audio> files)
        {
            _dataContext.Audios.AddRange(files);
            _dataContext.SaveChanges();
        }

        public void AddAudioFile(Audio audio)
        {
            _dataContext.Audios.Add(audio);
            _dataContext.SaveChanges();


        }


        #endregion

        #region Getting data
        public IEnumerable<Video> GetVideoFiles()
        {
            return _dataContext.Videos.ToList();
        }
   

        public ICollection<Audio> GetAudioFiles()
        {
            return _dataContext.Audios.ToList();
        }

        public ICollection<FileType> GetAudioTypes()
        {
            var query = from type in _dataContext.FileTypes
                where type.Decription == "Audio"
                select type;
            return query.ToList();
        }

        public ICollection<FileType> GetVideoTypes()
        {
            var query = from type in _dataContext.FileTypes
                where type.Decription == "Video"
                select type;
            return query.ToList();
        }

        #endregion


        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;
            if (_dataContext != null)
            {
                _dataContext.Dispose();
                _disposed = true;
            }

        }

        #endregion







    


    }
}
