using LinkedIn_Api_Beginner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkedIn_Api_Beginner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ShopContext _shopContext;

        public ProductsController(ShopContext shopContext)
        {
            _shopContext = shopContext;
            _shopContext.Database.EnsureCreated(); // ==> Ensures that the database has been seeded
        }

        //[HttpGet]
        //public IEnumerable<Product> GetAllProducts()
        //{
        //    return _shopContext.Products.ToList();
        //}

        //[HttpGet]
        //public async Task<ActionResult> GetAllProductss([FromQuery] QueryParameters queryParameters)
        //{
           
        //    return Ok(await products.ToListAsync());
        //}

        [HttpGet]
        public async Task<ActionResult> FilteringProducts([FromQuery] ProductQueryParameters productQueryParameters)
        {
            /*
             This is to access the products and since we only want to return part of the products we using IQueryable to access part of the products.
             By doing this we are skipping the products we are not looking for.
             */
            IQueryable<Product> products = _shopContext.Products;

            /*
                Filtering through products between a max and min price 
                Or from a Min Price or from a Max Price 
             */
            if (productQueryParameters.MinPrice != null)
            {
                products = products.Where(
                    p => p.Price >= productQueryParameters.MinPrice.Value);
            }

            if (productQueryParameters.MaxPrice != null)
            {
                products = products.Where(
                    p => p.Price <= productQueryParameters.MaxPrice.Value);
            }

            /*
           .Skip() 'skips' over the first 'n' elemenets in the sequence and returns a new sequence containing the remaining elements after the first 'n' elements.
           .Take() will return the amount of elements that is specified in a sequence of numbers.
            We taking the size and multiplying it with the page and whatever we got is amount of products we are returning and with 1 being our default value for page.
            
             */

            products = products
                .Skip(productQueryParameters.Size * (productQueryParameters.Page - 1))
                .Take(productQueryParameters.Size);



            return Ok(await products.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, Product product)
        {
            if(id != product.Id)
            {
                return BadRequest();
            }

            _shopContext.Entry(product).State = EntityState.Modified;
            try
            {
                await _shopContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
               if(!_shopContext.Products.Any(p=> p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if(product == null) { return NotFound(); }

            _shopContext.Products.Remove(product);
            await _shopContext.SaveChangesAsync();

            return product;
        }


        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> DeleteMultiple([FromQuery] int[] ids)
        {
            var products = new List<Product>();
            foreach (var id in ids)
            {
                var product = await _shopContext.Products.FindAsync(id);
                if (product == null) { return NotFound(); }
                products.Add(product);
            }
           

            _shopContext.Products.RemoveRange(products);
            await _shopContext.SaveChangesAsync();

            return Ok(products);
        }
        
    }
}
