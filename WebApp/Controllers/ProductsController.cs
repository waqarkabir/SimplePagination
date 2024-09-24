using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
    {
        var products = await PaginatedList<Product>.CreateAsync
            (
                _context
                .Products
                .Include(p => p.Category), pageNumber, pageSize
            );
            //await _context
            //.Products
            //.Include(p => p.Category)
            //.ToListAsync();

        return View(products);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel product)
    {
        if (ModelState.IsValid)
        {
            Product prod = new Product();
            prod.Name = product.Name;
            prod.Price = product.Price;
            prod.CategoryId = product.CategoryId;
            _context.Add(prod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
        return View(product);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.ProductId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.ProductId)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
        return View(product);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id) => _context.Products.Any(e => e.ProductId == id);
}
