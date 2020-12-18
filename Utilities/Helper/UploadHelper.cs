using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Utilities.Helper
{
    public class UploadHelper
    {
        /// <summary>
        /// Creates the folder if needed.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
        /// <summary>
        /// Ups the load file.
        /// </summary>
        /// <param name="fileinput">The fileinput.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static string UpLoadFile(HttpPostedFileBase fileinput, string folder)
        {
            HttpPostedFileBase myFile = fileinput;
            DateTime today = DateTime.Now;
            string date = string.Empty;
            string prefixTick = string.Empty;
            string folderPath = string.Empty;
            string _fileName = string.Empty;
            string fullPath = string.Empty;
            var _imagePath = string.Empty;
            if (myFile != null && myFile.ContentLength != 0)
            {
                date = today.Year + "_" + today.Month + "_" + today.Day;
                var tick = today.Ticks;
                prefixTick = "_" + tick.ToString().Substring(tick.ToString().Length - 6);
                folderPath = HttpContext.Current.Server.MapPath("~/" + folder + date);
                if (CreateFolderIfNeeded(folderPath))
                {
                    try
                    {
                        _fileName = HttpContext.Current.User.Identity.Name.Replace(" ", "") + Guid.NewGuid() + Path.GetExtension(myFile.FileName); //myFile.FileName.Replace(".", prefixTick + ".");

                        fullPath = Path.Combine(folderPath, _fileName);
                        myFile.SaveAs(fullPath);
                        _imagePath = "/" + folder + date + "/" + _fileName;
                    }
                    catch
                    {
                    }
                }
            }
            return _imagePath;
        }
        /// <summary>
        /// Uploadses the specified files.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static List<string> Uploads(HttpFileCollectionBase files, string folder)
        {
            List<string> outvalue = new List<string>();

            if (files != null && files.Count > 0)
            {
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string process = UpLoadFile(file, folder);
                    if (!string.IsNullOrEmpty(process))
                        outvalue.Add(process);
                }
            }
            return outvalue;
        }
        /// <summary>
        /// Ups the load file information.
        /// </summary>
        /// <param name="fileinput">The fileinput.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        //public static EtsFileInfo UpLoadFileInfo(HttpPostedFileBase fileinput, string folder)
        //{
        //    EtsFileInfo _EtsFileInfo = new EtsFileInfo();
        //    HttpPostedFileBase myFile = fileinput;
        //    DateTime today = DateTime.Now;
        //    string date = string.Empty;
        //    string prefixTick = string.Empty;
        //    string folderPath = string.Empty;
        //    string _fileName = string.Empty;
        //    string fullPath = string.Empty;
        //    var _imagePath = string.Empty;
        //    if (myFile != null && myFile.ContentLength != 0)
        //    {
        //        date = today.Year + "_" + today.Month + "_" + today.Day;
        //        var tick = today.Ticks;
        //        prefixTick = "_" + tick.ToString().Substring(tick.ToString().Length - 6);
        //        folderPath = HttpContext.Current.Server.MapPath("~/" + folder + date);
        //        if (CreateFolderIfNeeded(folderPath))
        //        {
        //            try
        //            {
        //                _fileName = HttpContext.Current.User.Identity.Name.Replace(" ", "") + Guid.NewGuid() + Path.GetExtension(myFile.FileName); //myFile.FileName.Replace(".", prefixTick + ".");

        //                fullPath = Path.Combine(folderPath, _fileName);
        //                myFile.SaveAs(fullPath);
        //                _imagePath = "/" + folder + date + "/" + _fileName;
        //                string ext = System.IO.Path.GetExtension(myFile.FileName);
        //                int fileSize = myFile.ContentLength;
        //                _EtsFileInfo.FileName = _fileName;
        //                _EtsFileInfo.Path = _imagePath;
        //                _EtsFileInfo.FileExtension = ext;
        //                _EtsFileInfo.FileLength = fileSize;
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    return _EtsFileInfo;
        //}
        ///// <summary>
        ///// Ups the load file infos.
        ///// </summary>
        ///// <param name="files">The files.</param>
        ///// <param name="folder">The folder.</param>
        ///// <returns></returns>
        //public static List<EtsFileInfo> UpLoadFileInfos(HttpFileCollectionBase files, string folder)
        //{
        //    List<EtsFileInfo> outvalue = new List<EtsFileInfo>();
        //    if (files != null && files.Count > 0)
        //    {
        //        for (int i = 0; i < files.Count; i++)
        //        {
        //            HttpPostedFileBase file = files[i];
        //            var fileInfo = UpLoadFileInfo(file, folder);
        //            if (!string.IsNullOrEmpty(fileInfo.Path))
        //                outvalue.Add(fileInfo);
        //        }
        //    }
        //    return outvalue;
        //}
    }
}