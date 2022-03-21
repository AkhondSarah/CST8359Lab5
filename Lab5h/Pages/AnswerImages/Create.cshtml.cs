using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab5h.Data;
using Lab5h.Models;
using Azure.Storage.Blobs;
using Azure;

namespace Lab5h.Pages.AnswerImages
{
    public class CreateModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        private readonly Lab5h.Data.AnswerImageDataContext _context;

        public CreateModel(Lab5h.Data.AnswerImageDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AnswerImage AnswerImage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            var ContainerName = "";
            BlobContainerClient ContainerClient;

            /*if(ContainerName == "Earth")
                  {
                      ContainerName = earthContainerName; AnswerImage.Question = Question.Earth;
                  }
                  if (ContainerName == "Computer")
                  {
                      ContainerName = computerContainerName; AnswerImage.Question = Question.Computer;
                  }
            */
            var a = Request.Form["question"].ToString();

            switch (a)
             { case "Earth":
                    {
                        ContainerName = earthContainerName; AnswerImage.Question = Question.Earth;
                    }
                     break;
               case "Computer":
                     {
                        ContainerName = computerContainerName; AnswerImage.Question = Question.Computer;
                    }
                     break;

             }
            try
            {
                ContainerClient = await _blobServiceClient.CreateBlobContainerAsync(ContainerName); ContainerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                ContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            }
            try
            {
                string File = Path.GetRandomFileName();
               var blockBlob = ContainerClient.GetBlobClient(File);
                AnswerImage.FileName = File;
                AnswerImage.Url = ContainerClient.GetBlobClient(blockBlob.Name).Uri.AbsoluteUri;
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await file.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                    //
                   // 
                }
            }
            catch (RequestFailedException)
            {
                RedirectToPage("Error");
            }
            _context.AnswerImages.Add(AnswerImage);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

    }
}
