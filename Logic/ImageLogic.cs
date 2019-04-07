

using FactFluxV3.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http.Headers;

namespace FactFluxV3.Logic
{
    public class ImageLogic
    {
        public string CreateImage(string contentType, int contentId, IFormFileCollection filesUploaded, Images foundImage = null)
        {
            var fileName = string.Empty;

            string newFileName = string.Empty;

            foreach (var file in filesUploaded)
            {
                if (file.Length > 0)
                {
                    //Getting FileName
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    //Assigning Unique Filename (Guid)
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Getting file Extension
                    var FileExtension = Path.GetExtension(fileName);

                    string startupPath = Environment.CurrentDirectory;

                    // concating  FileName + FileExtension
                    newFileName = myUniqueFileName + FileExtension;

                    // Combines two strings into a path.
                    fileName = Path.Combine(startupPath, "ClientApp\\src\\assets\\images") + $@"\{newFileName}";

                    var savePath = "/assets/images/" + newFileName;

                    using (FileStream fs = File.Create(fileName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    using (var db = new FactFluxV3Context())
                    {
                        if (foundImage == null)
                        {
                            var newImage = new Images()
                            {
                                ContentType = contentType,
                                ContentId = contentId,
                                ImageLocation = savePath
                            };

                            db.Images.Add(newImage);
                        }
                        else
                        {
                            foundImage.ImageLocation = savePath;
                        }
                        db.SaveChanges();
                    }
                }
            }
            return newFileName;
        }
    }
}
