﻿using AutoMapper;
using Azure.Core;
using Fiorella.App.Context;
using Fiorella.App.Dtos.Category;
using Fiorella.App.Dtos.Discount;
using Fiorella.App.Dtos.Product;
using Fiorella.App.Dtos.ProductImage;
using Fiorella.App.Dtos.Tag;
using Fiorella.App.Extensions;
using Fiorella.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fiorella.App.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductController(FiorellaDbContext context, IWebHostEnvironment webEnv, IMapper mapper) : Controller
    {
        private readonly FiorellaDbContext _context = context;
        private readonly IWebHostEnvironment _webEnv = webEnv;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ProductGetDto> products = await _context.Products.Include(p => p.Images).Include(p => p.Tags).Include(p => p.Categories).Select(p => _mapper.Map<ProductGetDto>(p)).ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateViewBags();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductPostDto productDto)
        {

            if (!ModelState.IsValid)
            {
                await PopulateViewBags();

                return View(productDto);
            }

            if (productDto.FormFiles != null)
            {
                int fileIndex = 0;
                ICollection<string> fileNames = await productDto.FormFiles.SaveMultipleFileAsync(_webEnv.WebRootPath, "assets/images/product");

                foreach (var fileName in fileNames)
                {
                    productDto.Images.Add(new() { Url = fileName, IsMain = fileIndex == 0 });
                    fileIndex++;
                }
            }

            if (productDto.CategoryIds.Count != 0)
            {
                foreach (int categoryId in productDto.CategoryIds)
                {
                    productDto.ProductCategories.Add(new ProductCategory() { CategoryId = categoryId });
                }
            }

            if (productDto.TagIds.Count != 0)
            {
                foreach (int tagId in productDto.TagIds)
                {
                    productDto.ProductTags.Add(new ProductTag() { TagId = tagId });
                }
            }

            Product product = _mapper.Map<Product>(productDto);

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Product? product = await _context.Products
                 .Include(p => p.ProductCategories)
                 .Include(p => p.ProductTags)
                 .Include(p => p.Images)
                 .Include(p => p.Discount)
                 .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            await PopulateViewBags();

            ProductUpdateDto productDto = _mapper.Map<ProductUpdateDto>(product);

            if (product.Categories.Count > 0)
            {
                foreach (var category in product.Categories)
                {
                    productDto.CategoryIds.Add(category.Id);
                }
            }

            if (product.Tags.Count > 0)
            {
                foreach (var tag in product.Tags)
                {
                    productDto.TagIds.Add(tag.Id);
                }
            }

            return View(productDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, ProductUpdateDto updatedProduct)
        {
            Product? product = await _context.Products
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductTags)
                .Include(p => p.Images)
                .Include(p => p.Discount)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateViewBags();
                // If validation error happened, show images nontheless
                updatedProduct.Images = await _context.ProductImages.Where(i => i.ProductId == product.Id).Select(i => _mapper.Map<ProductImageUpdateDto>(i)).ToListAsync();

                return View(updatedProduct);
            }

            // Map updatedProduct to product entity
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.Stock = updatedProduct.Stock;
            product.Info = updatedProduct.Info;
            product.TitleDescription = updatedProduct.TitleDescription;
            product.Description = updatedProduct.Description;
            product.Weight = updatedProduct.Weight;
            product.Dimensions = updatedProduct.Dimensions;
            product.DiscountId = updatedProduct.DiscountId;

            // Handle images
            if (updatedProduct.FormFiles != null)
            {

                ICollection<string> fileNames = await updatedProduct.FormFiles.SaveMultipleFileAsync(_webEnv.WebRootPath, "assets/images/product");

                // Clear old images or not if you'd like to add new ones to the olds
                //_context.ProductImages.RemoveRange(product.Images);

                if (product.Images.Count == 0)
                {
                    int fileIndex = 0;
                    foreach (var fileName in fileNames)
                    {
                        product.Images.Add(new ProductImage { Url = fileName, IsMain = fileIndex == 0 });
                        fileIndex++;
                    }
                }
                else
                {
                    foreach (var fileName in fileNames)
                    {
                        product.Images.Add(new ProductImage { Url = fileName });
                    }
                }
            }

            // Clear existing product categories and tags
            _context.ProductCategory.RemoveRange(product.ProductCategories);
            _context.ProductTag.RemoveRange(product.ProductTags);

            // Update categories
            if (updatedProduct.CategoryIds.Count != 0)
            {
                foreach (int categoryId in updatedProduct.CategoryIds)
                {
                    product.ProductCategories.Add(new ProductCategory { CategoryId = categoryId, ProductId = product.Id });
                }
            }

            // Update tags
            if (updatedProduct.TagIds.Count != 0)
            {
                foreach (int tagId in updatedProduct.TagIds)
                {
                    product.ProductTags.Add(new ProductTag { TagId = tagId, ProductId = product.Id });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMainImage([FromBody] ProductImageUpdateRequestDto request)
        {
            // Validate the request
            if (request.ProductId <= 0 || request.ClickedImageId <= 0)
            {
                return BadRequest(new { message = "Invalid product or image ID.", productId = request.ProductId, imageId = request.ClickedImageId });
            }

            // Fetch the current main image for the product
            ProductImage? mainImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.ProductId == request.ProductId && i.IsMain == true);

            if (mainImage == null)
            {
                return NotFound(new { message = "Main image not found for the product.", productId = request.ProductId });
            }

            // Fetch the new main image by id and product id
            ProductImage? newMainImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.ProductId == request.ProductId && i.Id == request.ClickedImageId);

            if (newMainImage == null)
            {
                return NotFound(new { message = "Image to be set as main not found.", productId = request.ProductId, imageId = request.ClickedImageId });
            }

            // Update the images
            mainImage.IsMain = false;
            newMainImage.IsMain = true;

            // Save changes
            await _context.SaveChangesAsync();

            // Return a success response
            return Ok(new { message = "Main image updated successfully.", productId = request.ProductId });
        }


        [HttpDelete]
        public async Task<IActionResult> RemoveImage(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = $"{nameof(id)} is invalid.", id });
            }

            ProductImage? image = await _context.ProductImages.FindAsync(id);

            if (image == null)
            {
                return NotFound("Image not found to be deleted");
            }

            image.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product image removed successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        private async Task PopulateViewBags()
        {
            ViewBag.Categories = await _context.Categories
                .Select(c => _mapper.Map<CategoryGetDto>(c))
                .ToListAsync();

            ViewBag.Tags = await _context.Tags
                .Select(t => _mapper.Map<TagGetDto>(t))
                .ToListAsync();

            ViewBag.Discounts = await _context.Discounts
                .Select(d => _mapper.Map<DiscountGetDto>(d))
                .ToListAsync();
        }
    }
}
