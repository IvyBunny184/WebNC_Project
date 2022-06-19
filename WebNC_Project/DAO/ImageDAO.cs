using Firebase.Auth;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebNC_Project.Models;

namespace WebNC_Project.DAO
{
    public class ImageDAO
    {
        private static string Bucket = "web-nc-a26ca.appspot.com";
        private static string ApiKey = "AIzaSyDGg0Tz9d6ClyQyh-t1ghRLSrP-cRX55pg";
        private static string email = "resortmanagement@gmail.com";
        private static string pass = "resortmanagementproject";

        public static async Task<IEnumerable<Image>> GetByIDEnti(string id)
        {
            using(ResortContext db = new ResortContext())
            {
                return await db.Images.Where(i => i.RoomID == id).ToListAsync();
            }
        }

        public static async Task<Image> GetByURL(string url)
        {
            using (ResortContext db = new ResortContext())
            {
                return await db.Images.FindAsync(url);
            }
        }

        public static async Task<int> Create(Image img)
        {
            using (ResortContext db = new ResortContext())
            {
                db.Images.Add(img);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<int> Remove(string url)
        {
            using (ResortContext db = new ResortContext())
            {
                Image img = await db.Images.FindAsync(url);
                db.Images.Remove(img);
                return await db.SaveChangesAsync();
            }
        }

        public static async Task<string> Upload(FileStream stream, string fileName, string path, string idroom)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, pass);
            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(
                Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }).Child("imgsroom").Child(idroom).Child(DateTime.Now.ToLongTimeString()).PutAsync(stream, cancellation.Token);
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}