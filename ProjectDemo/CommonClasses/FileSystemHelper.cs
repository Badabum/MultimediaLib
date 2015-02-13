using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonClasses
{
    public sealed  class FileSystemHelper
    {
        private readonly string firstPattern = @"^\.\w+"; //for .mp3 like extensions
        private readonly string secondPattern = @"^\*.\w+";
        private string _directory;
        private List<string> _matchedFiles = null;
        private List<string> _fileExtensions = null;
        public FileSystemHelper(string directory,List<string> fileExtension)
        {
            if (!String.IsNullOrWhiteSpace(directory)||fileExtension!=null)
            {
                _directory = directory;
                _fileExtensions = new List<string>(fileExtension);
            }
            else
            {
                throw new ArgumentException("Directory name length can`t be 0");
            }
        }
        public FileSystemHelper(string directory)
        {
            if (!String.IsNullOrWhiteSpace(directory) )
            {
                _directory = directory;
              
            }
            else
            {
                throw new ArgumentException("Directory name length can`t be 0");
            }
        }
        public List<string> GetFoundedFiles()
        {
            _matchedFiles = new List<string>();
            foreach (var extension in _fileExtensions)
            {
                if (Regex.IsMatch(extension, firstPattern))
                {
                    _matchedFiles.AddRange(Directory.GetFiles(_directory, "*"+extension));
                }
                else if(Regex.IsMatch(extension,secondPattern))
                {
                    _matchedFiles.AddRange(Directory.GetFiles(_directory,extension));
                }
                
            }
            return _matchedFiles;
        }

        public List<string> RemovePath(ICollection<string> fileWithPaths)
        {
            List<string> filesWithoutPaths = new List<string>();
            foreach (var file in fileWithPaths)
            {
                filesWithoutPaths.Add(Path.GetFileName(file));
            }
            return filesWithoutPaths;
        }

       
    }
}
