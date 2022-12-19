using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text.Json;
using System.Security.Cryptography;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using System.Data;
using System.ComponentModel;
using FirebaseAdmin.Auth;
using Firebase.Storage;

namespace APIMyMyStore
{
    public class CommonFileStore
    {
        public static async Task<string> Upload(IFormFile file, long id)
        {

            var ProjectId = FirebaseAdmin.FirebaseApp.DefaultInstance.Options.ProjectId;
            //authentication
            string customToken =  await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync("1");
            var cancellation = new CancellationTokenSource();

            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = new FirebaseStorage(
                 ProjectId+".appspot.com",
                 new FirebaseStorageOptions
                 {
                     AuthTokenAsyncFactory = () => Task.FromResult(customToken),
                     ThrowOnCancel = true,
                     
                 })
                .Child("image")
                .Child(id.ToString())
                .PutAsync(file.OpenReadStream());
            // Track progress of the upload
            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

            var downloadUrl = await task;
            return downloadUrl;
        }

    }
}