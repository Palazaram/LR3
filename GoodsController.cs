using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewWebShopApp.Areas.Identity.Data;
using NewWebShopApp.Models;


namespace NewWebShopApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        AppDBContext db;

        public GoodsController(AppDBContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            return await db.Products.ToListAsync();
        }

        // GET api/goods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            Product? prod = await db.Products.FirstOrDefaultAsync(x => x.id == id);
            if (prod == null)
                return NotFound();
            return new ObjectResult(prod);
        }

        // POST api/goods
        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product prod)
        {
            if (prod == null)
            {
                return BadRequest();
            }

            db.Products.Add(prod);
            await db.SaveChangesAsync();
            return Ok(prod);
        }

        // PUT api/goods/
        [HttpPut]
        public async Task<ActionResult<Product>> Put(Product prod)
        {
            if (prod == null)
            {
                return BadRequest();
            }
            if (!db.Products.Any(x => x.id == prod.id))
            {
                return NotFound();
            }

            db.Update(prod);
            await db.SaveChangesAsync();
            return Ok(prod);
        }

        // DELETE api/goods/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(int id)
        {
            Product? prod = db.Products.FirstOrDefault(x => x.id == id);
            if (prod == null)
            {
                return NotFound();
            }
            db.Products.Remove(prod);
            await db.SaveChangesAsync();
            return Ok(prod);
        }
    }
}
