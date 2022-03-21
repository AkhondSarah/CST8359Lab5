using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab5h.Data;
using Lab5h.Models;
using Azure.Storage.Blobs;
using Azure;

namespace Lab5h.Pages.AnswerImages
{
    public class DeleteModel : PageModel
    {
        private readonly Lab5h.Data.AnswerImageDataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

       

        public DeleteModel(Lab5h.Data.AnswerImageDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        [BindProperty]
        public AnswerImage AnswerImage { get; set; }

        //From razorpageentity frameworks/delete
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AnswerImage = await _context.AnswerImages.FirstOrDefaultAsync(m => m.AnswerImageId == id);

            if (AnswerImage == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            BlobContainerClient ContainerClient;
            // Get the container and return a container client object
            try
            {
                ContainerClient = _blobServiceClient.GetBlobContainerClient("Earth");
            }
            catch (RequestFailedException)
            {
                return RedirectToPage("Error");
            }

           try
                {
                    // Get the blob that holds the data
                    var blockBlob = ContainerClient.GetBlobClient(AnswerImage.FileName);
                    if (await blockBlob.ExistsAsync())
                    {
                        await blockBlob.DeleteAsync();
                    }
                }
                catch (RequestFailedException)
                {
                    return RedirectToPage("Error");
                }
            



            /* if (id == null)
             {
                 return NotFound();
             }*/

            AnswerImage = await _context.AnswerImages.FindAsync(id);

            if (AnswerImage != null)
            {
                _context.AnswerImages.Remove(AnswerImage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
