 
using Microsoft.AspNetCore.Mvc;
using System; 
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.Collections.Generic;
using System.Threading;
using Google.Apis.Util.Store;
using AspNetCoreAPI.Models.Error;

namespace AspNetCoreAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OtherController : ControllerBase
    {
        #region
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "Application_UploadFile_GoogleDrive";

        //[HttpGet]
        //public List<Error_Model> lisfile(DriveService service, List<Error_Model> errorModel)
        //{
        //    FilesResource.ListRequest listRequest = service.Files.List();
        //    var request = listRequest.Execute();
        //    if (request != null && request.Files.Count > 0)
        //    {
        //        return errorModel;
        //    }
        //    else
        //    {
        //        return errorModel;
        //    }
        //}
        public void upLoad(string path, DriveService service, string folder)
        {
            var file = new Google.Apis.Drive.v3.Data.File();
            file.Name = Path.GetFileName(path);
            file.MimeType = "image/*";
            file.Parents = new List<string>
            {
                folder
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
            {
                request = service.Files.Create(file, stream, "image/*");
                request.Fields = "id";
                request.Upload();
            }
        }
        private UserCredential UserCredentials()
        {
            UserCredential userCredentials;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string crepath = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                crepath = Path.Combine(crepath, "credentials.json");
                userCredentials = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None, new FileDataStore(crepath, true)).Result;
            }
            return userCredentials;
        }
        [HttpGet]
        public string upload_click()
        {
            UserCredential userCredential;
            userCredential = UserCredentials();
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = ApplicationName
            });
          //  string folderid;
            var fileMetadatas = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "Nhanlh6",
                MimeType = "application/vnd.Google-apps.folder"
            };
            var requests = service.Files.Create(fileMetadatas);
            requests.Fields = "id";
            var files = requests.Execute();
            return "OK";
        }
        #endregion


        // Class to demonstrate use of Drive insert file API
        public class UploadBasic
        {
            /// <summary>
            /// Upload new file.
            /// </summary>
            /// <param name="filePath">Image path to upload.</param>
            /// <returns>Inserted file metadata if successful, null otherwise.</returns>
            /// 
            [HttpGet]
            public static string DriveUploadBasic(string filePath)
            {
                try
                {
                    /* Load pre-authorized user credentials from the environment.
                     TODO(developer) - See https://developers.google.com/identity for
                     guides on implementing OAuth2 for your application. */
                    GoogleCredential credential = GoogleCredential.GetApplicationDefault().CreateScoped(DriveService.Scope.Drive);

                    // Create Drive API service.
                    var service = new DriveService(new BaseClientService.Initializer
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Drive API Snippets"
                    });

                    // Upload file photo.jpg on drive.
                    var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                    {
                        Name = "photo.jpg"
                    };
                    FilesResource.CreateMediaUpload request;
                    // Create a new file on drive.
                    using (var stream = new FileStream(filePath,FileMode.Open))
                    {
                        // Create a new file, with metadata and stream.
                        request = service.Files.Create(fileMetadata, stream, "image/jpeg");
                        request.Fields = "id";
                        request.Upload();
                    }

                    var file = request.ResponseBody;
                    // Prints the uploaded file id.
                    Console.WriteLine("File ID: " + file.Id);
                    return file.Id;
                }
                catch (Exception e)
                {
                    // TODO(developer) - handle error appropriately
                    if (e is AggregateException)
                    {
                        Console.WriteLine("Credential Not found");
                    }
                    else if (e is FileNotFoundException)
                    {
                        Console.WriteLine("File not found");
                    }
                    else
                    {
                        throw;
                    }
                }
                return null;
            }
        }
    } 
}
   
