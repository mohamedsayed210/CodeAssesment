using CodeAssesment.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAssesment.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly ApplicationContext _auc;
        private readonly IWebHostEnvironment webHostEnvironment;

        public IActionResult Index()
        {
            var _UsersList = _auc.Users.ToList();

            return View(_UsersList);
        }
        public RegistrationController(ApplicationContext auc, IWebHostEnvironment hostEnvironment)
        {  
        
            _auc = auc;
            webHostEnvironment = hostEnvironment;
        }
        public IActionResult Create()
        {
            return View();
        }
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Users uc)
        {
            _auc.Add(uc);
            _auc.SaveChanges();
            ViewBag.message = "The user " + uc.UserFirstName + " is saved successfully";
            return RedirectToAction("Index");
        }
        //
        public IActionResult Edit(int ?UserId)
        {
            if (UserId == null)
            {
                return StatusCode(400); 
              
            }

            Users Edited_user = _auc.Users.Find(UserId);
            if (Edited_user == null)
            {
                return StatusCode(404);
            }
            if (Edited_user.UserImgProfile != "")
            {
                string ImgaPath = Edited_user.UserImgProfile != null ? Edited_user.UserImgProfile.ToString() : "";
                Edited_user.UserImgProfile = "/Content/Images_Doc/" + ImgaPath;
            }
             return View(Edited_user);
      
        }
     

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Users uc)
        {
            var Userfound =  _auc.Users.Where(w => w.UserId == uc.UserId && w.UserEmail == uc.UserEmail).FirstOrDefault();
            if (Userfound != null)
            {
                Userfound.UserMobile = uc.UserMobile;
                Userfound.UserFirstName = uc.UserFirstName;
                Userfound.UserLastName = uc.UserLastName;
                Userfound.UserImgProfile = uc.UserImgProfile;
                //here for update image Profile And other Documnets
              // get files loades on form Post
                var files = HttpContext.Request.Form.Files;
                // this path of Images ==>>>//wwwroot //content/imges_Doc
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Content\\Images_Doc\\");

            if (files.Count != 0)
                {
                    foreach (var item in files)
                    {
                       // to update user Image Profile 
                        if (item.Name =="UserImgProfile")
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "").ToString() + Path.GetExtension(item.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploadsFolder, fileName.ToString()), FileMode.Create))
                            {
                                //await file.CopyToAsync(fileStream);
                                item.CopyToAsync(fileStream);
                                //      Userfound.UserImgProfileLoc = uploadsFolder+ fileName.ToString();
                                Userfound.UserImgProfile = fileName.ToString();
                            }

                            FileInfo fi = new FileInfo(item.FileName);
                            Userfound.UserImgExt = fi.Extension;
                      
                    }
                        // to update user Documnet
                        if (item.Name == "UserDocument")
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "").ToString() + Path.GetExtension(item.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploadsFolder, fileName.ToString()), FileMode.Create))
                            {
                                //await file.CopyToAsync(fileStream);
                                item.CopyToAsync(fileStream);
                                Userfound.UserDocument = fileName.ToString();
                            }

                            FileInfo fi = new FileInfo(files[0].FileName);
                            Userfound.UserDoc_Ext = fi.Extension;
                      
                        }
                    }
             }
                    _auc.SaveChanges();
                    return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        // download Document if found

        [HttpGet]
        public IActionResult DownloadFile(string FileName)
        {
            // Since this is just and example, I am using a local file located inside wwwroot
            // Afterwards file is converted into a stream
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Content\\Images_Doc\\");
            path = path +"\\"+ FileName;
            
            var fs = new FileStream(path, FileMode.Open);
            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", FileName);
        }

        // delete  Document if found
        public IActionResult DeleteDocument(string FileName, int user_Id)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Content\\Images_Doc\\");
            path = path + "\\" + FileName;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            // update User Profile in database
            var Userfound = _auc.Users.Where(w => w.UserId == user_Id).FirstOrDefault();
            if (Userfound != null)
            {
                Userfound.UserDocument = "";
                Userfound.UserDoc_Ext = "";

            }
            _auc.SaveChanges();
            return RedirectToAction("Index");
        }
        // delete  image if found
        public IActionResult DeleteImageProfile(string FileName,int user_Id)
        {
            // Since this is using a local file located inside wwwroot
            // Afterwards file is converted into a stream
            FileName = Path.GetFileName(FileName);
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Content\\Images_Doc\\");
            path = path + "\\" + FileName;
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            // update User Profile in database
            var Userfound = _auc.Users.Where(w => w.UserId == user_Id).FirstOrDefault();
            if (Userfound != null)
            {
                Userfound.UserImgProfile = "";
                Userfound.UserImgExt = "";

            }
            _auc.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
